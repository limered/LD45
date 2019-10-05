using SystemBase;
using Systems.Movement;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Interaction.Obstacle
{
    [GameSystem(typeof(MovementSystem))]
    public class ObstacleInteractionSystem : GameSystem<ObstacleComponent>
    {
        public override void Register(ObstacleComponent component)
        {
            component.OnCollisionStayAsObservable()
                .Subscribe(HandleCollision)
                .AddTo(component);
        }

        private static void HandleCollision(Collision collision)
        {
            var dir = collision.impulse;
            var vel = collision.gameObject.GetComponent<MovementComponent>().Velocity;
            collision.gameObject.GetComponent<MovementComponent>().Velocity = vel + new Vector2(dir.x, dir.z);
        }
    }
}