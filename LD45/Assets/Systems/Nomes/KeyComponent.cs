using SystemBase;
using UnityEngine;

namespace Systems.Nomes
{
    public class KeyComponent : GameComponent
    {
        public GameObject Originator { set; get; }
        public float Lifetime;
    }
}
