using SystemBase;
using Systems.InputHandling;
using Assets.Systems.WIndUp;
using UnityEngine;

namespace Assets.Systems.Enemy
{
    [RequireComponent(typeof(WindUpComponent))]
    public class EnemyAttackComponent : GameComponent
    {
        public float AttackDurationInMs;
        public GameObject BulbToShow;
        public InputWordType AttackType;
    }
}