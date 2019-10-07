using GameState.States;
using System.Linq;
using SystemBase;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;

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
            component.OnTriggerEnterAsObservable()
                .Where(collider => collider.GetComponent<ExitDoorComponent>())
                .Subscribe(OnDogExitsDoor)
                .AddTo(component);
        }

        private void OnDogEndsGame(Collider coll)
        {
            IoC.Game.GameStateContext.GoToState(new GameOver());
        }

        private void OnDogExitsDoor(Collider coll)
        {
            IoC.Game.GameStateContext.GoToState(new Running());
            GameObject.Destroy(coll, 1);
        }
    }
}
