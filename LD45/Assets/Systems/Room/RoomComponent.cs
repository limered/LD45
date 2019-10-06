using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Room
{
    public class RoomComponent : GameComponent
    {
        public Collider PlayerSensor;
        public GameObject CameraPosition;
        public bool PlayerInside;
    }
}