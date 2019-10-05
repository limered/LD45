using SystemBase;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.HealthBar
{
    public class HealthBarComponent : GameComponent
    {
        public int MaxHealthShown = 3;
        public int CurrentHealthShown = 3;
        public Image[] HeartImages;
        public Sprite FullHeart;
        public Sprite EmptyHeart;

        public void SetSpriteOfHeart()
        {
            for(int i = 0; i < MaxHealthShown; i++)
            {
                if(i < CurrentHealthShown)
                {
                    HeartImages[i].sprite = FullHeart;
                }
                else
                {
                    HeartImages[i].sprite = EmptyHeart;
                }
            }

        }
    }
}
