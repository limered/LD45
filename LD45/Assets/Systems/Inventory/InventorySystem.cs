using SystemBase;
using Systems.Inventory.Actions;
using Systems.Inventory.Events;
using UniRx;
using UnityEngine;

namespace Systems.Inventory
{
    [GameSystem()]
    public class InventorySystem : GameSystem<InventoryComponent>
    {
        public override void Register(InventoryComponent component)
        {
            MessageBroker.Default
                .Receive<ActKeyCollected>()
                .Subscribe(message => AddKeyToCollectedKeys(component, message.Key));
        }

        private void AddKeyToCollectedKeys(InventoryComponent component, char key)
        {
            if (component.CollectedKeys.Value.Contains(key)) return;

            MessageBroker.Default.Publish(new EvtNewKeyCollected
            {
                NewKey = key
            });
            component.CollectedKeys.Value.Add(key);
        }
    }
}