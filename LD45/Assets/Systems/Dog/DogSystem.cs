using System;
using System.Linq;
using SystemBase;
using Systems.Dog.Actions;
using Systems.Health;
using Systems.Health.Actions;
using Systems.InteractableObjects;
using Systems.InteractableObjects.Events;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.Dog
{
    [GameSystem]
    public class DogSystem : GameSystem<StartDogComponent, EndDogComponent>
    {
        public override void Register(EndDogComponent component)
        {
            component.OnTriggerEnterAsObservable()
                .Where(collider => collider.GetComponent<PlayerComponent>())
                .Subscribe(OnDogEndsGame)
                .AddTo(component);
        }

        public override void Register(StartDogComponent component)
        {
        }

        private void OnDogEndsGame(Collider coll)
        {
            MessageBroker.Default.Publish(new ActDogHitsPlayer
            {
            });
        }
    }
}
