using SystemBase;
using Systems.Dog;
using Systems.Enemy.Skeletor;
using Systems.Movement;
using Systems.Player;
using Systems.Room;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Math;
using Utils.Plugins;
using Utils.Unity;

namespace Systems.Enemy
{
    [GameSystem(typeof(PlayerSystem), typeof(MovementSystem), typeof(RoomSystem))]
    public class EnemyMovementSystem:GameSystem<PlayerComponent, FollowPlayerComponent, IradicMovementComponent, EndDogComponent>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>();

        public override void Register(FollowPlayerComponent component)
        {
            _player.WhereNotNull()
                .Subscribe(_ => component.FixedUpdateAsObservable()
                    .Select(a => component)
                    .Subscribe(FollowPlayer)
                    .AddTo(component)
                )
                .AddTo(component);
        }

        public override void Register(EndDogComponent component)
        {
            _player.WhereNotNull()
                .Subscribe(_ => component.FixedUpdateAsObservable()
                    .Select(a => component)
                    .Subscribe(FollowPlayer)
                    .AddTo(component)
                )
                .AddTo(component);
        }

        private void FollowPlayer(FollowPlayerComponent enemy)
        {
            if (enemy.GetComponent<RoomEnemyComponent>().TheRoom.PlayerInside && _player.Value.IsMoving)
            {
                var directionToPlayer = enemy.transform.position.DirectionTo(_player.Value.transform.position);
                enemy.GetComponent<MovementComponent>().Direction.Value = directionToPlayer.XZ();
            }
            else
            {
                enemy.GetComponent<MovementComponent>().Direction.Value = Vector2.zero;
            }
        }

        private void FollowPlayer(EndDogComponent dog)
        {
            if (dog.GetComponent<RoomEnemyComponent>().TheRoom.PlayerInside)
            {
                var directionToPlayer = dog.transform.position.DirectionTo(_player.Value.transform.position);
                dog.GetComponent<MovementComponent>().Direction.Value = directionToPlayer.XZ();
            }
            else
            {
                dog.GetComponent<MovementComponent>().Direction.Value = Vector2.zero;
            }
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        public override void Register(IradicMovementComponent component)
        {
            _player.WhereNotNull()
                .Subscribe(_ => component.FixedUpdateAsObservable()
                    .Select(a => component)
                    .Subscribe(MoveIradic)
                    .AddTo(component)
                )
                .AddTo(component);
        }

        private void MoveIradic(IradicMovementComponent enemy)
        {
            if (enemy.GetComponent<RoomEnemyComponent>().TheRoom.PlayerInside && _player.Value.IsMoving)
            {
                var newRandom = new Vector3().RandomVector(new Vector3(-1,-1,-1), new Vector3(1, 1, 1));
                var dir = (enemy.LastrandomVector + newRandom).normalized;
                enemy.GetComponent<MovementComponent>().Direction.Value = dir;
                enemy.LastrandomVector = dir;
            }
            else
            {
                enemy.GetComponent<MovementComponent>().Direction.Value = Vector2.zero;
            }
        }
    }
}
