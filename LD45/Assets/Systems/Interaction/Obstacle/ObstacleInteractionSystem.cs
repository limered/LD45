using System.Linq;
using System.Net.Http.Headers;
using SystemBase;
using Systems.Movement;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Math;

namespace Systems.Interaction.Obstacle
{
    [GameSystem(typeof(MovementSystem))]
    public class ObstacleInteractionSystem: GameSystem<ObstacleComponent>
    {
        public override void Register(ObstacleComponent component)
        {
            component.OnCollisionEnterAsObservable()
                .Select(collision => new {collision, obstacle = component})
                .Subscribe(arg => HandleCollision(arg.collision, arg.obstacle))
                .AddTo(component);
        }

        private static void HandleCollision(Collision collision, ObstacleComponent obstacle)
        {
            var center = collision.contacts.Select(c => c.point).Aggregate((one, two) => one + two);
            var inverseCount = 1 / collision.contactCount;
            center = center * inverseCount;
            var dir = collision.gameObject.transform.position.DirectionTo(center);
            var vel = collision.gameObject.GetComponent<MovementComponent>().Velocity;
            collision.gameObject.GetComponent<MovementComponent>().Velocity = vel + new Vector2(dir.x, dir.z);
        }
    }
}
