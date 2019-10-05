using SystemBase;
using UnityEngine;

namespace Systems.Hud
{
    public class InventoryComponent : GameComponent
    {
        public char KeyValue;
        public Sprite KeyThumbnail;
        public float KeyHighlightValue = .0f;
        public bool KeyIsActive = false;
    }
}
