using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBase;
using Systems.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.InventoryBar
{
    [GameSystem]
    public class InventoryBarSystem : GameSystem<InventoryBarComponent, InventoryComponent>
    {
        private InventoryBarComponent _inventoryBarComponent;
        private InventoryComponent _inventoryComponent;
        public override void Register(InventoryComponent component)
        {
            _inventoryComponent = component;
            FinishRegistration();
        }

        public override void Register(InventoryBarComponent component)
        {
            _inventoryBarComponent = component;
            FinishRegistration();
        }

        private void FinishRegistration()
        {
            if(_inventoryBarComponent == null || _inventoryComponent == null)
            {
                return;
            }

            InitializeInventoryBar();
        }

        public void InitializeInventoryBar()
        {
            for (int i = 0; i < _inventoryComponent.CollectedKeys.Value.Count(); i++)
            {
                GameObject key = GameObject.Instantiate(_inventoryBarComponent.KeyPrefab);
                key.GetComponentInChildren<Text>().text = _inventoryComponent.CollectedKeys.Value[i].ToString();
                _inventoryBarComponent.Keys.Add(key);
                key.transform.SetParent(_inventoryBarComponent.KeysPanel.transform);
            }
        }
    }
}
