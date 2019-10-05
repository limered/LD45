using SystemBase;
using Systems.Attac.Actions;
using Systems.InputHandling.Events;
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
            component.GetComponent<MovementComponent>().Direction
                .Subscribe(dir => component.IsMoving = dir.magnitude > 0);

            MessageBroker.Default.Receive<InputWordCompleted>()
                .Select(completed => new { word = completed, player = component })
                .Subscribe(obj => Attack(obj.player, obj.word))
                .AddTo(component);
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

            comp.Direction.Value = movementDirection;
        }

        private void Attack(PlayerComponent player, InputWordCompleted word)
        {
            MessageBroker.Default.Publish(new ActAttackSpawn
            {
                Originator = player.gameObject,
                Word = word.InputWord,
                Position = player.transform.position.XZ(),
                Direction = player.GetComponent<MovementComponent>().Velocity.normalized
            });
        }
    }
}