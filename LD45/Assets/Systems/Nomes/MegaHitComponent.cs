using SystemBase;
using UnityEngine;

namespace Systems.Nomes
{
    public class MegaHitComponent : GameComponent
    {
        public GameObject Originator { set; get; }
        public float Lifetime;
    }
}
