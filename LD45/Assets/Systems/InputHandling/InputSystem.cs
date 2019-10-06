using SystemBase;
using Systems.InputHandling.Events;
using Systems.Inventory;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.InputHandling
{
    [GameSystem]
    public class InputSystem : GameSystem<InputComponent, InventoryComponent>
    {
        private readonly InputConfig _inputConfig = new InputConfig();
        private InventoryComponent _inventoryComponent;

        public override void Register(InventoryComponent component)
        {
            _inventoryComponent = component;
        }

        public override void Register(InputComponent inputComponent)
        {
            inputComponent.UpdateAsObservable()
                .Subscribe(_ => HandleInput(inputComponent));
        }

        private void HandleInput(InputComponent inputComponent)
        {
            CheckForNouns(inputComponent);
            DoTimerStuff(inputComponent);
        }

        private void CheckForNouns(InputComponent inputComponent)
        {
            foreach (var c in Input.inputString.ToLower())
            {
                inputComponent.StartedTyping.Value = true;
                SaveKeyInput(c, inputComponent);

                HandleKeyInput(c, inputComponent);
            }
        }

        private void DoTimerStuff(InputComponent inputComponent)
        {
            if (inputComponent.StartedTyping.Value)
            {
                inputComponent.TimeLeft.Value -= Time.deltaTime;
                if (inputComponent.TimeLeft.Value < 0)
                {
                    ClearCurrentWord(inputComponent);
                }
            }
        }

        private void ClearCurrentWord(InputComponent inputComponent)
        {
            foreach(char c in inputComponent.CurrentWord.Value)
            {
                MessageBroker.Default.Publish(new EvtInputFinished { CharacterInput = c }); //TODO check for last char
            }

            inputComponent.CurrentWord.Value = string.Empty;
            inputComponent.TimeLeft.Value = 0;
            inputComponent.StartedTyping.SetValueAndForceNotify(false);
        }

        private void NotifyWordCompleted(InputWordType inputWordType)
        {
            MessageBroker.Default.Publish(new EvtInputWordCompleted { InputWord = inputWordType });
        }

        private void NotifyValidKeyInput(char c)
        {
            MessageBroker.Default.Publish(new EvtInputValidKey { CharacterInput = c });
        }

        private void HandleKeyInput(char c, InputComponent component)
        {
            if (_inventoryComponent.CollectedKeys.Value.Contains(c))
            {
                component.TimeLeft.Value = component.MaxTime;
                CheckForCompletedWord(component);
                NotifyValidKeyInput(c);
                // TODO: Play successful input sound
            }
            else
            {
                // TODO: Play error input sound
            }
        }

        private void CheckForCompletedWord(InputComponent component)
        {
            switch (component.CurrentWord.Value.ToLower()) //TODO check if word starts with else error sound
            {
                case "fire":
                    ClearCurrentWord(component);
                    NotifyWordCompleted(InputWordType.Fire);
                    break;
                case "hit":
                    ClearCurrentWord(component);
                    NotifyWordCompleted(InputWordType.Hit);
                    break;
                case "key":
                    ClearCurrentWord(component);
                    NotifyWordCompleted(InputWordType.Key);
                    break;
                case "magic":
                    ClearCurrentWord(component);
                    NotifyWordCompleted(InputWordType.Magic);
                    break;
                case "megahit":
                    ClearCurrentWord(component);
                    NotifyWordCompleted(InputWordType.Megahit);
                    break;
                case "nothing":
                    ClearCurrentWord(component);
                    NotifyWordCompleted(InputWordType.Nothing);
                    break;
                case "parry":
                    ClearCurrentWord(component);
                    NotifyWordCompleted(InputWordType.Parry);
                    break;
            }
        }

        private void SaveKeyInput(char c, InputComponent component)
        {
            component.CurrentWord.Value += c;
            component.TimeLeft.Value = component.MaxTime;
        }
    }
}
