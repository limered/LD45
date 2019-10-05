using SystemBase;
using Systems.Movement;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Unity;

namespace Systems.Player
{
    [GameSystem(typeof(MovementSystem))]
    public class PlayerSystem : GameSystem<PlayerComponent, MovementComponent>
    {
        public override void Register(PlayerComponent component)
        {
        }

        public override void Register(MovementComponent component)
        {
            component.FixedUpdateAsObservable()
                .Select(_ => component)
                .Where(movementComponent => movementComponent.GetComponent<PlayerComponent>())
                .Subscribe(ControlPlayer)
                .AddTo(component);
        }

        private static void ControlPlayer(MovementComponent comp)
        {
            var movementDirection = Vector2.zero;
            if (KeyCode.UpArrow.IsPressed())
            {
                movementDirection.y += 1;
            }

            if (KeyCode.DownArrow.IsPressed())
            {
                movementDirection.y += -1;
            }

            if (KeyCode.LeftArrow.IsPressed())
            {
                movementDirection.x += -1;
            }

            if (KeyCode.RightArrow.IsPressed())
            {
                movementDirection.x += 1;
            }
            
            comp.Direction = movementDirection;
        }
    }
}