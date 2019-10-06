using SystemBase;
using Systems.Movement;
using UniRx;
using UnityEngine;

namespace Systems.Enemy.Skeletor
{
    [RequireComponent(typeof(MovementComponent))]
    public class Sceletor : GameComponent
    {
        public GameObject PlayerSenseTrigger;
        public float WindUpTimeInMs;
        public float AttackTime;
        public bool CanFire = true;

        public BoolReactiveProperty IsWindingUp = new BoolReactiveProperty(false);
        public BoolReactiveProperty IsAttacking = new BoolReactiveProperty(false);
    }
}