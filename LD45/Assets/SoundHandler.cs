using StrongSystems.Audio;
using Systems.Dog.Events;
using Systems.Health.Actions;
using Systems.InteractableObjects.Events;
using UniRx;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    private void Awake()
    {
        MessageBroker.Default.Receive<EvtDogExitsDoor>().First().Subscribe(_ => "Lizzi_Nothing_Question".Play()).AddTo(this);
        MessageBroker.Default.Receive<EvtDogEndsGame>().First().Subscribe(_ => "Lizzi_Nothing_Happy".Play()).AddTo(this);
        MessageBroker.Default.Receive<HealthActSubtract>().Subscribe(_ => PlayOuchSound(_)).AddTo(this);
        MessageBroker.Default.Receive<EvtKillDoor>().Subscribe(_ => "Door_Opened".Play()).AddTo(this);
    }

    private void PlayOuchSound(HealthActSubtract healthactSubtract)
    {
        var soundName = healthactSubtract.ComponentToChange.gameObject.tag == "Player" ? "Lizzi" : "Enemy";
        $"{soundName}_Ouch".Play();
    }
}
