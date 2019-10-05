using Systems.InputHandling;
using UnityEngine;

namespace Systems.Attac.Actions
{
    public class ActAttackSpawn
    {
        public InputWordType Word { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public GameObject Originator { get; set; }
    }
}
