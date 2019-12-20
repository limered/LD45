using SystemBase;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Movement
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class MovementComponent : GameComponent
    {
        [FormerlySerializedAs("Speed")] public float speed;
        [FormerlySerializedAs("Friction")] public float friction;
        [FormerlySerializedAs("MaxSpeed")] public float maxSpeed;
        public Collider Collider;

        [FormerlySerializedAs("Direction")] public Vector2ReactiveProperty direction  = new Vector2ReactiveProperty(Vector2.zero);
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
    }
}