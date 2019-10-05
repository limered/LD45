using SystemBase;
using UniRx;
using Systems.InputHandling.Events;
using Systems.Inventory.Actions;

namespace Systems.Inventory
{
    [GameSystem()]
    public class InventorySystem : GameSystem<InventoryComponent>
    {
        public float highlightValue = .0f;

        public override void Init()
        {
            base.Init();

            //RegisterValidInputEventHandler();
            //RegisterWordCompletedEventHandler();
        }

        public override void Register(InventoryComponent component)
        {
            MessageBroker.Default
                .Receive<ActKeyCollected>()
                .Subscribe(message => AddKeyToCollectedKeys(component, message.Key));
        }

        /*private void RegisterValidInputEventHandler()
        {
            MessageBroker.Default
                .Receive<InputValidKey>()
                .Subscribe(ev => HighlightKey(ev));
        }

        private void RegisterWordCompletedEventHandler()
        {
            MessageBroker.Default
                .Receive<InputWordCompleted>()
                .Subscribe(ev => ResetKeyHighlighting());
        }

        private void HighlightKey(InputValidKey validKeyEvent)
        {
            var element = CollectedKeys.Find(d => d.KeyValue == validKeyEvent.CharacterInput);

            if (element != null)
            {
                element.KeyIsActive = true;
            }
        }

        private void UpdateHighlightedKeys(float newHighlightValue)
        {
            foreach (KeyComponent component in CollectedKeys)
            {
                if (component.KeyIsActive)
                {
                    component.KeyHighlightValue = newHighlightValue;
                }
            }
        }

        private void ResetKeyHighlighting()
        {
            foreach (KeyComponent component in CollectedKeys)
            {
                component.KeyHighlightValue = .0f;
                component.KeyIsActive = false;
            }
        }*/




        private void AddKeyToCollectedKeys(InventoryComponent component, char key)
        {
            component.CollectedKeys.Value.Add(key);
        }
    }
}
