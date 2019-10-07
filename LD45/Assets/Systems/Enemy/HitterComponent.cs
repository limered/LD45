using SystemBase;
using Assets.Systems.WIndUp;
using UnityEngine;

namespace Assets.Systems.Enemy
{
    [RequireComponent(typeof(WindUpComponent))]
    public class HitterComponent : GameComponent
    {
        public float AttackDurationInMs;
        public GameObject BulbToShow;
    }
}