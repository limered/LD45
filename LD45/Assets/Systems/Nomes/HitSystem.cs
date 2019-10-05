using System;
using SystemBase;
using Systems.Attac;
using Systems.Health;
using Systems.Health.Actions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.Nomes
{
    [GameSystem]
    public class HitSystem : GameSystem<HitComponent>
    {
        public override void Register(HitComponent component)
        {
            component.OnTriggerEnterAsObservable()
                .Where(collider => !collider.gameObject.CompareTag(component.Originator.tag))
                .Subscribe(Hit)
                .AddTo(component);

            Observable.Timer(TimeSpan.FromMilliseconds(500))
                .Subscribe(_ => Object.Destroy(component.gameObject))
                .AddTo(component);
        }

        private static void Hit(Collider coll)
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