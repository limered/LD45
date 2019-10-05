using SystemBase;
using Systems.InputHandling.Events;
using Systems.InputHandling;
using UniRx;
using UnityEngine;
using UniRx.Triggers;
using System.Collections.Generic;

namespace Systems
{
    [GameSystem]
    public class InputSystem : GameSystem<InputComponent>
    {
        private List<char> _memory = new List<char>();

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
            foreach (char c in Input.inputString)
            {
                inputComponent.StartedTyping.SetValueAndForceNotify(true);
                inputComponent.TimeLeft.SetValueAndForceNotify(inputComponent.MaxTime);

                _memory.Add(c);
                Debug.Log("pressed " + c);
                inputComponent.CurrentWord.SetValueAndForceNotify(string.Join("", _memory.ToArray()));
                switch (inputComponent.CurrentWord.Value.ToLower())
                {
                    case "fire":
                        ClearCurrentWord(inputComponent);
                        NotifyWordCompleted(InputWordType.Fire);
                        break;
                    case "hit":
                        ClearCurrentWord(inputComponent);
                        NotifyWordCompleted(InputWordType.Hit);
                        break;
                    case "key":
                        ClearCurrentWord(inputComponent);
                        NotifyWordCompleted(InputWordType.Key);
                        break;
                    case "magic":
                        ClearCurrentWord(inputComponent);
                        NotifyWordCompleted(InputWordType.Magic);
                        break;
                    case "megahit":
                        ClearCurrentWord(inputComponent);
                        NotifyWordCompleted(InputWordType.Megahit);
                        break;
                    case "nothing":
                        ClearCurrentWord(inputComponent);
                        NotifyWordCompleted(InputWordType.Nothing);
                        break;
                    case "parry":
                        ClearCurrentWord(inputComponent);
                        NotifyWordCompleted(InputWordType.Parry);
                        break;
                }
            }
        }

        private void DoTimerStuff(InputComponent inputComponent)
        {
            if (inputComponent.StartedTyping.Value)
            {
                inputComponent.TimeLeft.SetValueAndForceNotify(inputComponent.TimeLeft.Value - Time.deltaTime);
                if (inputComponent.TimeLeft.Value < 0)
                {
                    ClearCurrentWord(inputComponent);
                }
            }
        }

        private void ClearCurrentWord(InputComponent inputComponent)
        {
            inputComponent.CurrentWord.SetValueAndForceNotify("");
            _memory.Clear();
            inputComponent.TimeLeft.SetValueAndForceNotify(inputComponent.MaxTime);
            inputComponent.StartedTyping.SetValueAndForceNotify(false);
        }

        private void NotifyWordCompleted(InputWordType inputWordType)
        {
            MessageBroker.Default.Publish(new InputWordCompleted { InputWord = inputWordType });
        }
    }
}
