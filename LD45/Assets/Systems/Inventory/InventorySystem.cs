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
        private InventoryComponent _inventoryComponent;

        public override void Init()
        {
            base.Init();

            //RegisterWordCompletedEventHandler();
        }

        public override void Register(InventoryComponent component)
        {
            _inventoryComponent = component;
            MessageBroker.Default
                .Receive<ActKeyCollected>()
                .Subscribe(message => AddKeyToCollectedKeys(component, message.Key));
        }

        private void AddKeyToCollectedKeys(InventoryComponent component, char key)
        {
            component.CollectedKeys.Value.Add(key);
        }
    }
}
