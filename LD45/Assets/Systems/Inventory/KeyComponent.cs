using SystemBase;
using UnityEngine;

namespace Systems.Inventory
{
    public class KeyComponent : GameComponent
    {
        public char KeyValue;
        public Sprite KeyThumbnail;
        public float KeyHighlightValue = .0f;
        public bool KeyIsActive = false;

        public Color32 NormalTextColor = new Color32(0, 0, 0, 255);
        public Color32 NormalBackgroundColor = new Color32(255, 255, 255, 255);
    }
}
