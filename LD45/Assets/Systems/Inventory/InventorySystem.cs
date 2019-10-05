using System.Collections.Generic;
using SystemBase;
using UniRx;
using Systems.InputHandling.Events;

namespace Systems.Inventory
{
    [GameSystem()]
    public class InventorySystem : GameSystem<InventoryComponent, InputComponent>
    {
        public List<InventoryComponent> _availableKeys = new List<InventoryComponent>();
        public float highlightValue = .0f;

        public override void Init()
        {
            base.Init();

            RegisterValidInputEventHandler();
            RegisterWordCompletedEventHandler();
        }

        public override void Register(InventoryComponent component)
        {
            _availableKeys.Add(component);
        }

        public override void Register(InputComponent component)
        {
            component.TimeLeft.Subscribe(f => UpdateHighlightedKeys(f));
        }

        private void RegisterValidInputEventHandler()
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
            var element = _availableKeys.Find(d => d.KeyValue == validKeyEvent.CharacterInput);

            if (element != null)
            {
                element.KeyIsActive = true;
            }
        }

        private void UpdateHighlightedKeys(float newHighlightValue)
        {
            foreach (InventoryComponent component in _availableKeys)
            {
                if (component.KeyIsActive)
                {
                    component.KeyHighlightValue = newHighlightValue;
                }
            }
        }

        private void ResetKeyHighlighting()
        {
            foreach (InventoryComponent component in _availableKeys)
            {
                component.KeyHighlightValue = .0f;
                component.KeyIsActive = false;
            }
        }
    }
}
