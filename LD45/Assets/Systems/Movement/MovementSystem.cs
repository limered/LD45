using GameState.States;
using SystemBase;
using Systems.Dog;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;

namespace Systems.Movement
{
    [GameSystem]
    public class MovementSystem : GameSystem<MovementComponent>
    {
        public override void Register(MovementComponent component)
        {
            component.FixedUpdateAsObservable()
                .Select(_ => component)
                .Subscribe(CalculateMovement)
                .AddTo(component);

            component.FixedUpdateAsObservable()
                .Select(_ => component)
                .Where(c => c.GetComponent<StartDogComponent>())
                .Subscribe(CalculateDogMovement)
                .AddTo(component);

            ResetRigidbody(component);
        }

        private static void ResetRigidbody(Component comp)
        {
            var body = comp.GetComponent<Rigidbody>();
            body.drag = 0;
            body.angularDrag = 0;
            body.isKinematic = false;
            body.useGravity = false;
            body.freezeRotation = true;
        }

        private static void FixCollider(MovementComponent component)
        {
            component.Collider.transform.localPosition = Vector3.zero;
        }

        private static void ApplyAnimationToObject(MovementComponent component)
        {
            var positionChange = component.Velocity * Time.fixedDeltaTime;
            var transform = component.transform;
            var position = transform.position;
            position = new Vector3(
                position.x + positionChange.x,
                0.1f,
                position.z + positionChange.y);
            transform.position = position;
        }

        private static void Animate(MovementComponent component)
        {
            var futureVel = component.Velocity + component.Acceleration * Time.fixedDeltaTime;
            var speed = component.Velocity.magnitude;
            if (speed < component.maxSpeed)
            {
                component.Velocity = futureVel;
            }
        }

        private static void ApplyFriction(MovementComponent component)
        {
            var backFriction = component.Velocity * -component.friction;
            component.Velocity += backFriction * Time.fixedDeltaTime;
        }

        private static void ApplyDirection(MovementComponent component)
        {
            component.Acceleration = component.direction.Value * component.speed;
        }

        private void CalculateMovement(MovementComponent component)
        {
            if (!(IoC.Game.GameStateContext.CurrentState.Value is Running)) return;

            StopRigidbodyMovement(component);
            ApplyDirection(component);
            ApplyFriction(component);
            Animate(component);
            ApplyAnimationToObject(component);
            if (component.Collider) FixCollider(component);
        }

        private void CalculateDogMovement(MovementComponent component)
        {
            StopRigidbodyMovement(component);
            ApplyDirection(component);
            ApplyFriction(component);
            Animate(component);
            ApplyAnimationToObject(component);
            if (component.Collider) FixCollider(component);
        }

        private static void StopRigidbodyMovement(MovementComponent component)
        {
            var body = component.GetComponent<Rigidbody>();
            body.angularVelocity = Vector3.zero;
            body.velocity = Vector3.zero;
        }
    }
}