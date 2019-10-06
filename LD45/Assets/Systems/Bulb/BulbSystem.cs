using System;
using SystemBase;
using Systems.InputHandling;
using Systems.InputHandling.Events;
using UniRx;

namespace Systems.Bulb
{
    [GameSystem]
    public class BulbSystem : GameSystem<BulbComponent>
    {
        public override void Register(BulbComponent component)
        {
            component.CurrentImage.gameObject.SetActive(false);

            MessageBroker.Default.Receive<EvtInputWordCompleted>()
                .Subscribe(message => ShowBulbWithAttack(component, message.InputWord))
                .AddTo(component);
        }

        private void ShowBulbWithAttack(BulbComponent component, InputWordType word)
        {
            component.CurrentImage.gameObject.SetActive(true);
            component.CurrentImage.sprite = component.BulbSprites[(int) word];

            Observable.Timer(TimeSpan.FromMilliseconds(component.Lifetime))
                .Subscribe(_ => component.CurrentImage.gameObject.SetActive(false))
                .AddTo(component);
        }
    }
}
