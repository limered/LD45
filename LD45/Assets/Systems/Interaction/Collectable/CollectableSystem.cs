using SystemBase;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Interaction.Collectable
{
    [GameSystem]
    public class CollectableSystem : GameSystem<CollectableComponent>
    {
        public override void Register(CollectableComponent component)
        {
            component.OnTriggerEnterAsObservable()
                .Where(collider => collider.GetComponent<PlayerComponent>())
                .Select(_ => component)
                .Subscribe(Collect)
                .AddTo(component);
        }

        private static void Collect(CollectableComponent collectible)
        {
            collectible.OnCollect.Execute(true);
            Object.Destroy(collectible.gameObject);
        }
    }
}