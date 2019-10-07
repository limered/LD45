using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Enemy.MegaSkeletor
{
    public class MegaSceletor:GameComponent
    {
        public GameObject PlayerSenseTrigger;
        public float WindUpTimeInMs;
        public float AttackTime;
        public bool CanFire = true;
        public string WordToDrop;
        public GameObject WindUpBulb;
        public GameObject AttackBulb;

        public BoolReactiveProperty IsWindingUp = new BoolReactiveProperty(false);
        public BoolReactiveProperty IsAttacking = new BoolReactiveProperty(false);
    }
}
