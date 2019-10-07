using SystemBase;
using UnityEngine;

namespace Assets.Systems.Enemy
{
    public class EnemyComponent : GameComponent
    {
        public Collider PlayerSensor;
        public bool CanFire = true;
    }
}