using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBase;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.InventoryBar
{
    public class InventoryBarComponent : GameComponent
    {
        public Image[] KeysImages;
        public Sprite BackgroundSprite;

        public void SetupInventoryBar()
        {
            for(int i = 0; i < KeysImages.Count(); i++)
            {
                //KeysImages[i]
            }
        }
    }
}
