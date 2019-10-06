using SystemBase;
using UnityEngine;

namespace Systems.Nomes
{
    public class FireComponent : GameComponent
    {
        public GameObject Originator { set; get; }
        public float Lifetime;
    }
}
