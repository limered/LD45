using System;
using SystemBase;
using Systems.Health;
using Systems.Health.Actions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
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

            Observable.Timer(TimeSpan.FromMilliseconds(component.Lifetime))
                .Subscribe(_ => Object.Destroy(component.gameObject))
                .AddTo(component);
        }

        private static void Fire(Collider coll)
        {
            HealthComponent hlth;
            if (!coll.TryGetComponent(out hlth)) return;

            MessageBroker.Default.Publish(new HealthActSubtract
            {
                Value = 1,
                ComponentToChange = hlth
            });
        }
    }
}
