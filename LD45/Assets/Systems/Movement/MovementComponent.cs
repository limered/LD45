using SystemBase;
using UnityEngine;

namespace Systems.Movement
{
    public class MovementComponent : GameComponent
    {
        public float Speed;
        public float Friction;
        public float MaxSpeed;
        public GameObject Collider;

        public Vector2 Direction { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
    }
}