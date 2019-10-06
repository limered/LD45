using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBase;
using Systems.Drop.Actions;
using Systems.Health.Events;
using Systems.InteractableObjects;
using UniRx;
using Object = UnityEngine.Object;

namespace Assets.Systems.InteractableObjects
{
    [GameSystem]
    public class InteractableObjectsSystem : GameSystem<InteractableObjectsComponent>
    {
        public override void Init()
        {
            MessageBroker.Default.Receive<HealthEvtReachedZero>()
                .Where(msg => msg.ObjectToKill.GetComponent<InteractableObjectsComponent>())
                .Select(msg => msg.ObjectToKill.GetComponent<InteractableObjectsComponent>())
                .Subscribe(KillSceletor);
        }

        public override void Register(InteractableObjectsComponent component)
        {
        }

        private static void KillSceletor(InteractableObjectsComponent component)
        {
            MessageBroker.Default.Publish(new ActDropSpawnKeys
            {
                Position = component.gameObject.transform.position,
                KeysToDrop = component.WordToDrop.ToCharArray()
            });
            Object.Destroy(component.gameObject);
        }
    }
}
