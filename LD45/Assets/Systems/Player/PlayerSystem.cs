using System;
using SystemBase;
using Systems.Attac.Actions;
using Systems.Health.Events;
using Systems.InputHandling.Events;
using Systems.Movement;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Unity;
using Object = UnityEngine.Object;

namespace Systems.Player
{
    [GameSystem(typeof(MovementSystem))]
    public class PlayerSystem : GameSystem<PlayerComponent, MovementComponent, PlayerSpawnerComponent>
    {
        private GameObject _currentPlayer;

        public override void Register(PlayerComponent component)
        {
            if (_currentPlayer)
            {
                Object.Destroy(component.gameObject);
            }
            _currentPlayer = component.gameObject;

            component.GetComponent<MovementComponent>().Direction
                .Subscribe(dir => component.IsMoving = dir.magnitude > 0);

            MessageBroker.Default.Receive<EvtInputWordCompleted>()
                .Select(completed => new { word = completed, player = component })
                .Subscribe(obj => Attack(obj.player, obj.word))
                .AddTo(component);

            MessageBroker.Default.Receive<HealthEvtReachedZero>()
                .Where(msg => msg.ObjectToKill.GetComponent<PlayerComponent>())
                .Subscribe(PlayerDies)
                .AddTo(component);
        }

        private void PlayerDies(HealthEvtReachedZero obj)
        {
            Object.Destroy(obj.ObjectToKill);
            _currentPlayer = null;

            Observable.Timer(TimeSpan.FromMilliseconds(1500))
                .Subscribe(l => MessageBroker.Default.Publish(new ActPlayerRespawn()));
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

        private void Attack(PlayerComponent player, EvtInputWordCompleted word)
        {
            MessageBroker.Default.Publish(new ActAttackSpawn
            {
                Originator = player.gameObject,
                Word = word.InputWord,
                Position = player.transform.position.XZ(),
                Direction = player.GetComponent<MovementComponent>().Velocity.normalized
            });
        }

        public override void Register(PlayerSpawnerComponent component)
        {
            MessageBroker.Default.Receive<ActPlayerRespawn>()
                .Subscribe(respawn =>
                    Object.Instantiate(component.PlayerPrefab, component.transform.position, Quaternion.identity));
        }
    }
}