using System.Linq;
using SystemBase;
using Systems.Drop.Actions;
using Systems.Health.Events;
using Systems.InteractableObjects;
using Systems.InteractableObjects.Events;
using UniRx;
using Object = UnityEngine.Object;

namespace Assets.Systems.InteractableObjects
{
    [GameSystem]
    public class InteractableObjectsSystem : GameSystem<InteractableObjectsComponent, FireDoorComponent>
    {
        public override void Init()
        {
            MessageBroker.Default.Receive<HealthEvtReachedZero>()
                .Where(msg => msg.ObjectToKill.GetComponent<InteractableObjectsComponent>())
                .Select(msg => msg.ObjectToKill.GetComponent<InteractableObjectsComponent>())
                .Subscribe(DestroyObjectWithDrop);

            MessageBroker.Default.Receive<EvtKillDoor>()
                .Where(msg => msg.ObjectToKill.GetComponent<FireDoorComponent>())
                .Select(msg => msg.ObjectToKill.GetComponent<FireDoorComponent>())
                .Subscribe(DestroyObjectWithoutDrop);

            MessageBroker.Default.Receive<EvtKillDoor>()
                .Where(msg => msg.ObjectToKill.GetComponent<KeyDoorComponent>())
                .Select(msg => msg.ObjectToKill.GetComponent<KeyDoorComponent>())
                .Subscribe(DestroyObjectWithoutDrop);
        }

        public override void Register(InteractableObjectsComponent component)
        {
        }

        public override void Register(FireDoorComponent component)
        {
        }

        private static void DestroyObjectWithDrop(InteractableObjectsComponent component)
        {
            MessageBroker.Default.Publish(new ActDropSpawnKeys
            {
                Position = component.gameObject.transform.position,
                KeysToDrop = component.WordToDrop.ToCharArray()
            });
            Object.Destroy(component.gameObject);
        }

        private static void DestroyObjectWithoutDrop(FireDoorComponent component)
        {
            Object.Destroy(component.gameObject);
        }

        private static void DestroyObjectWithoutDrop(KeyDoorComponent component)
        {
            Object.Destroy(component.gameObject);
        }
    }
}
