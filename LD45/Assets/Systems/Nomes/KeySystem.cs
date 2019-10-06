using System;
using System.Linq;
using SystemBase;
using Systems.Health;
using Systems.Health.Actions;
using Systems.InteractableObjects;
using Systems.InteractableObjects.Events;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.Nomes
{
    [GameSystem]
    public class KeySystem : GameSystem<KeyComponent>
    {
        public override void Register(KeyComponent component)
        {
            component.OnTriggerEnterAsObservable()
                .Where(collider => !collider.gameObject.CompareTag(component.Originator.tag))
                .Subscribe(Key)
                .AddTo(component);

            component.OnTriggerEnterAsObservable()
                .Where(collider => collider.GetComponent<KeyDoorComponent>())
                .Subscribe(KillKeyDoor)
                .AddTo(component);

            Observable.Timer(TimeSpan.FromMilliseconds(component.Lifetime))
                .Subscribe(_ => Object.Destroy(component.gameObject))
                .AddTo(component);
        }

        private static void Key(Collider coll)
        {
            HealthComponent hlth;
            if (!coll.TryGetComponent(out hlth)) return;

            MessageBroker.Default.Publish(new HealthActSubtract
            {
                Value = 1,
                ComponentToChange = hlth
            });
        }

        private static void KillKeyDoor(Collider coll)
        {
            MessageBroker.Default.Publish(new EvtKillDoor { ObjectToKill = coll.gameObject });
        }
    }
}
