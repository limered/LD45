using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBase;
using Systems.Inventory;

namespace Systems.InventoryBar
{
    [GameSystem]
    public class InventoryBarSystem : GameSystem<InventoryBarComponent, InventoryComponent>
    {
        private InventoryBarComponent _inventoryBarComponent;
        private InventoryComponent _inventoryComponent;
        public override void Register(InventoryBarComponent component)
        {
            _inventoryBarComponent = component;
            FinishRegistration();
        }

        public override void Register(InventoryComponent component)
        {
            _inventoryComponent = component;
            FinishRegistration();
        }

        private void FinishRegistration()
        {
            if(_inventoryBarComponent == null || _inventoryComponent == null)
            {
                return;
            }
        }
    }
}
