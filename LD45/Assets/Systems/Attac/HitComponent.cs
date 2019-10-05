using SystemBase;
using UnityEngine;

namespace Systems.Attac
{
    public class HitComponent : GameComponent
    {
        public GameObject Originator { set; get; }
        public float Lifetime;
    }
}