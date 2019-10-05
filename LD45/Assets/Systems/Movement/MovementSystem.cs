using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Movement
{
    [GameSystem]
    public class MovementSystem : GameSystem<MovementComponent>
    {
        public override void Register(MovementComponent component)
        {
            component.FixedUpdateAsObservable()
                .Select(_ => component)
                .Subscribe(CalculatyeMovement)
                .AddTo(component);
        }

        public void CalculatyeMovement(MovementComponent component)
        {
            ApplyDirection(component);
            ApplyFriction(component);
            Animate(component);
            ApplyAnimationToObject(component);
            if(component.Collider) FixCollider(component);
        }

        private static void FixCollider(MovementComponent component)
        {
            component.Collider.transform.localPosition = Vector3.zero;
        }

        private static void ApplyAnimationToObject(MovementComponent component)
        {
            var positionChange = component.Velocity * Time.fixedDeltaTime;
            component.transform.position = new Vector3(
                component.transform.position.x + positionChange.x,
                component.transform.position.y,
                component.transform.position.z + positionChange.y);
        }

        private static void Animate(MovementComponent component)
        {
            var futureVel = component.Velocity + component.Acceleration * Time.fixedDeltaTime;
            var speed = component.Velocity.magnitude;
            if (speed < component.MaxSpeed)
            {
                component.Velocity = futureVel;
            }
        }

        private static void ApplyFriction(MovementComponent component)
        {
            var backFriction = component.Velocity * -component.Friction;
            component.Velocity = component.Velocity + backFriction * Time.fixedDeltaTime;
        }

        private static void ApplyDirection(MovementComponent component)
        {
            component.Acceleration = component.Direction * component.Speed;
        }
    }
}