using System;
using SystemBase;
using Systems.Dog.Events;
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

            MessageBroker.Default.Receive<EvtDogExitsDoor>()
                .Subscribe(message => ShowBulbWithTime(component, InputWordType.Nothing, 1200))
                .AddTo(component);

            MessageBroker.Default.Receive<EvtDogEndsGame>()
                .Subscribe(message => ShowBulbWithTime(component, InputWordType.Nothing, 1500))
                .AddTo(component);
        }

        private void ShowBulbWithAttack(BulbComponent component, InputWordType word)
        {
            ShowBulbWithTime(component, word, component.Lifetime);
        }

        private void ShowBulbWithTime(BulbComponent component, InputWordType word, float time)
        {
            component.CurrentImage.gameObject.SetActive(true);
            component.CurrentImage.sprite = component.BulbSprites[(int)word];

            Observable.Timer(TimeSpan.FromMilliseconds(time))
                .Subscribe(_ => component.CurrentImage.gameObject.SetActive(false))
                .AddTo(component);
        }
    }
}
