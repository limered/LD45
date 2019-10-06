using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBase;
using Systems.InputHandling.Events;
using Systems.Inventory;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.InventoryBar
{
    [GameSystem]
    public class InventoryBarSystem : GameSystem<InventoryBarComponent, InventoryComponent, InputComponent, KeyComponent>
    {
        private InventoryBarComponent _inventoryBarComponent;
        private InventoryComponent _inventoryComponent;
        private InputComponent _inputComponent;

        private Color32 _currentHighlightTextColor;
        private Color32 _currentHighlightBackgroundColor;
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

        public override void Register(InputComponent component)
        {
            _inputComponent = component;
        }

        public override void Register(KeyComponent component)
        {
            MessageBroker.Default
                .Receive<EvtInputValidKey>()
                .Where(message => message.CharacterInput == component.KeyValue)
                .Subscribe(message => HighlightKey(component));
        }

        private void SetColorOfPressedButton(KeyComponent keyComponent, float time)
        {
            float changeVal = time / _inputComponent.MaxTime;
            _currentHighlightTextColor = Color32.Lerp(
                _inventoryBarComponent.NormalTextColor,
                _inventoryBarComponent.HighlightTextColor,
                changeVal);
            _currentHighlightBackgroundColor = Color32.Lerp(
                _inventoryBarComponent.NormalBackgroundColor,
                _inventoryBarComponent.HighlightBackgroundColor,
                changeVal
                );
            keyComponent.gameObject.GetComponent<Image>().color = _currentHighlightBackgroundColor;
            keyComponent.gameObject.GetComponentInChildren<Text>().color = _currentHighlightTextColor;
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
                KeyComponent keyComponent = key.GetComponent<KeyComponent>();
                keyComponent.KeyValue = _inventoryComponent.CollectedKeys.Value[i];
                keyComponent.KeyIsActive = true;
                keyComponent.KeyHighlightValue = 255f;
                key.GetComponent<Image>().sprite = keyComponent.KeyThumbnail;

                keyComponent.gameObject.GetComponent<Image>().color = _inventoryBarComponent.NormalBackgroundColor;
                keyComponent.gameObject.GetComponentInChildren<Text>().color = _inventoryBarComponent.NormalTextColor;

                key.GetComponentInChildren<Text>().text = _inventoryComponent.CollectedKeys.Value[i].ToString();
                _inventoryBarComponent.Keys.Add(key);
                key.transform.SetParent(_inventoryBarComponent.KeysPanel.transform);
            }
        }

        private void HighlightKey(KeyComponent keyComponent)
        {
            _inputComponent.TimeLeft.TakeUntil(
                    MessageBroker.Default.Receive<EvtInputFinished>()
                )
                .DoOnTerminate(() => ResetHighlightKey(keyComponent))
                .Subscribe(time => SetColorOfPressedButton(keyComponent, time));
        }

        private void ResetHighlightKey(KeyComponent keyComponent)
        {
            Observable.Timer(TimeSpan.FromMilliseconds(500)).Subscribe(_ =>
            {
                keyComponent.gameObject.GetComponent<Image>().color = _inventoryBarComponent.NormalBackgroundColor;
                keyComponent.gameObject.GetComponentInChildren<Text>().color = _inventoryBarComponent.NormalTextColor;
            });
        }
    }
}
