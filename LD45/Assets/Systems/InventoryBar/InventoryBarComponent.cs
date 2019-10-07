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

        public Color32 NormalTextColor = new Color32(0, 0, 0, 255);
        public Color32 NormalBackgroundColor = new Color32(150, 150, 150, 255);

        public Color32 HighlightTextColor = new Color32(160, 160, 160, 255);
        public Color32 HighlightBackgroundColor = new Color32(255, 255, 255, 255);
    }
}
