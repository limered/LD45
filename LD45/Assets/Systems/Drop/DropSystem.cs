using SystemBase;
using Systems.Drop.Actions;
using Systems.Interaction.Collectable;
using Systems.Inventory.Actions;
using UniRx;
using UnityEngine;
using Utils.Math;

namespace Systems.Drop
{
    [GameSystem(typeof(CollectableSystem))]
    public class DropSystem : GameSystem<DropConfigComponent, DroppedKeyComponent>
    {
        private readonly ReactiveProperty<DropConfigComponent> _config = new ReactiveProperty<DropConfigComponent>();

        public override void Register(DropConfigComponent component)
        {
            _config.Value = component;

            MessageBroker.Default.Receive<ActDropSpawnKeys>()
                .Subscribe(CreateKeyDrop);
        }

        public override void Register(DroppedKeyComponent component)
        {
            component.GetComponent<CollectableComponent>().OnCollect.Subscribe(b =>
            {
                MessageBroker.Default.Publish(new ActKeyCollected
                {
                    Key = component.Value
                });
            });
        }

        private void CreateKeyDrop(ActDropSpawnKeys msg)
        {
            foreach (var c in msg.KeysToDrop)
            {
                var keyInstance = Object.Instantiate(_config.Value.KeyPrefab,
                    new Vector3(msg.Position.x, 1, msg.Position.y),
                    Quaternion.Euler(new Vector3().RandomVector()));
                keyInstance.GetComponent<DroppedKeyComponent>().Value = c;
            }
        }
    }
}