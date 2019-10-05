using UniRx;
using UnityEngine;

namespace Assets.Systems.Hud
{
    public class HudKey
    {
        public string KeyValue;
        public Sprite KeyThumbnail;
        public BoolReactiveProperty IsActive = new BoolReactiveProperty(false);
    }
}
