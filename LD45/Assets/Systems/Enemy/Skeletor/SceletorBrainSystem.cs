using SystemBase;
using Systems.Drop.Actions;
using Systems.Health.Events;
using Systems.Player;
using UniRx;
using Utils.Unity;
using Object = UnityEngine.Object;

namespace Systems.Enemy.Skeletor
{
    [GameSystem]
    public class SceletorBrainSystem : GameSystem<PlayerComponent, Sceletor>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>();

        public override void Init()
        {
            MessageBroker.Default.Receive<HealthEvtReachedZero>()
                .Where(msg => msg.ObjectToKill.GetComponent<Sceletor>())
                .Select(msg => msg.ObjectToKill.GetComponent<Sceletor>())
                .Subscribe(KillSceletor);
        }

        public override void Register(Sceletor component)
        {
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
        }

        private static void KillSceletor(Sceletor scelletor)
        {
            MessageBroker.Default.Publish(new ActDropSpawnKeys
            {
                Position = scelletor.gameObject.transform.position.XZ(),
                KeysToDrop = scelletor.WordToDrop.ToCharArray()
            });
            Object.Destroy(scelletor.gameObject);
        }
    }
}