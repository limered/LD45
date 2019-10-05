using SystemBase;
using UnityEngine;

namespace Systems.Movement
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class MovementComponent : GameComponent
    {
        public float Speed;
        public float Friction;
        public float MaxSpeed;
        public Collider Collider;

        public Vector2 Direction { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
    }
}