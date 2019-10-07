using StrongSystems.Audio;
using System;
using SystemBase;
using Systems.Health;
using Systems.Health.Actions;
using Systems.InteractableObjects;
using Systems.InteractableObjects.Events;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Math;
using Object = UnityEngine.Object;

namespace Systems.Nomes
{
    [GameSystem]
    public class FireSystem : GameSystem<FireComponent>
    {
        public override void Register(FireComponent component)
        {
            component.OnTriggerEnterAsObservable()
                .Where(collider => !collider.gameObject.CompareTag(component.Originator.tag))
                .Subscribe(Fire)
                .AddTo(component);

            component.OnTriggerEnterAsObservable()
                .Where(collider => collider.GetComponent<FireDoorComponent>())
                .Subscribe(KillFireDoor)
                .AddTo(component);

            Observable.Timer(TimeSpan.FromMilliseconds(component.Lifetime))
                .Subscribe(_ => Object.Destroy(component.gameObject))
                .AddTo(component);

            for (var i = 0; i < component.FireCount; i++)
            {
                var d = component.MaxFireDistance;
                var firePosition = new Vector3().RandomVector(new Vector3(-d, -d, -d), new Vector3(d, d, d));
                var fire = Object.Instantiate(component.FirePrefab, component.transform );
                fire.transform.localPosition = firePosition;
            }
        }

        private static void Fire(Collider coll)
        {
            "Fire".Play();

            HealthComponent hlth;
            if (!coll.TryGetComponent(out hlth)) return;

            MessageBroker.Default.Publish(new HealthActSubtract
            {
                Value = 1,
                ComponentToChange = hlth
            });
        }

        private static void KillFireDoor(Collider coll)
        {
            MessageBroker.Default.Publish(new EvtKillDoor{ ObjectToKill = coll.gameObject });
        }
    }
}
