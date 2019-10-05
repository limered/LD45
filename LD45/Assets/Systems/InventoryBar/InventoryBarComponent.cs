using System.Collections.Generic;
using SystemBase;
using UnityEngine;

namespace Systems.InventoryBar
{
    public class InventoryBarComponent : GameComponent
    {
        public GameObject KeyPrefab;
        public List<GameObject> Keys;
        public GameObject KeysPanel;
    }
}
