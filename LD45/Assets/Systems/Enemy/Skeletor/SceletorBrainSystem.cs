using SystemBase;
using Systems.Movement;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Math;
using Utils.Plugins;
using Utils.Unity;

namespace Systems.Enemy.Skeletor
{
    [GameSystem]
    public class SceletorBrainSystem : GameSystem<PlayerComponent, Sceletor>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>();

        public override void Register(Sceletor component)
        {
            _player.WhereNotNull()
                .Subscribe(_ => component.FixedUpdateAsObservable()
                    .Select(a => component)
                    .Subscribe(TakeAction)
                    .AddTo(component)
                )
                .AddTo(component);
        }

        private void TakeAction(Sceletor enemy)
        {
            if (_player.Value.IsMoving)
            {
                var directionToPlayer = enemy.transform.position.DirectionTo(_player.Value.transform.position);
                enemy.GetComponent<MovementComponent>().Direction.Value = directionToPlayer.XZ();
            }
            else
            {
                enemy.GetComponent<MovementComponent>().Direction.Value = Vector2.zero;
            }
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }
    }
}
