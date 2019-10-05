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
    }
}
