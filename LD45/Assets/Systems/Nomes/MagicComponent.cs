using SystemBase;
using UnityEngine;

namespace Systems.Nomes
{
    public class MagicComponent : GameComponent
    {
        public GameObject Originator { set; get; }
        public float Lifetime;
    }
}
