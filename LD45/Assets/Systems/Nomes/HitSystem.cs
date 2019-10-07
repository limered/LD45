using StrongSystems.Audio;
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
    public class HitSystem : GameSystem<HitComponent, MegaHitComponent>
    {
        public override void Register(HitComponent component)
        {
            component.OnTriggerEnterAsObservable()
                .Where(collider => !collider.gameObject.CompareTag(component.Originator.tag))
                .Subscribe(Hit)
                .AddTo(component);

            Observable.Timer(TimeSpan.FromMilliseconds(component.Lifetime))
                .Subscribe(_ => Object.Destroy(component.gameObject))
                .AddTo(component);
        }

        public override void Register(MegaHitComponent component)
        {
            component.OnTriggerEnterAsObservable()
                .Where(collider => !collider.gameObject.CompareTag(component.Originator.tag))
                .Subscribe(Megahit)
                .AddTo(component);

            Observable.Timer(TimeSpan.FromMilliseconds(component.Lifetime))
                .Subscribe(_ => Object.Destroy(component.gameObject))
                .AddTo(component);
        }

        private static void Hit(Collider coll)
        {
            "Hit".Play();

            HealthComponent hlth;
            if (!coll.TryGetComponent(out hlth)) return;

            MessageBroker.Default.Publish(new HealthActSubtract
            {
                Value = 1,
                ComponentToChange = hlth
            });
        }

        private static void Megahit(Collider coll)
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