using SystemBase;
using UnityEngine;

namespace Systems.Nomes
{
    public class HitComponent : GameComponent
    {
        public GameObject Originator { set; get; }
        public float Lifetime;
    }
}