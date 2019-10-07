using SystemBase;
using Assets.Systems.Enemy;
using UniRx;
using UnityEngine;

namespace Assets.Systems.WIndUp
{
    [RequireComponent(typeof(EnemyComponent))]
    public class WindUpComponent : GameComponent
    {
        public float TimeInMs;
        public GameObject Empty;
        public GameObject One;
        public GameObject Two;
        public GameObject Three;
        public ReactiveCommand Ready = new ReactiveCommand();
        public bool IsWindingUp;
    }
}