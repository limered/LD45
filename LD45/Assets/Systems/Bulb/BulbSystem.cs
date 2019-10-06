using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBase;
using Systems.InputHandling;
using Systems.InputHandling.Events;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Bulb
{
    public class BulbSystem : GameSystem<BulbComponent>
    {
        public override void Register(BulbComponent component)
        {
            MessageBroker.Default.Receive<EvtInputWordCompleted>()
                .Subscribe(message => ShowBulbWithAttack(component, message.InputWord))
                .AddTo(component);
        }

        private void ShowBulbWithAttack(BulbComponent component, InputWordType word)
        {
            component.Bulb.SetActive(true);
            component.Bulb.GetComponentInChildren<Image>().sprite = component.BulbSprites[(int) word];
        }
    }
}
