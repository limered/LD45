using SystemBase;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Bulb
{
    public class BulbComponent : GameComponent
    {
        public Image CurrentImage;
        public float Lifetime;
        public Sprite[] BulbSprites;
    }
}
