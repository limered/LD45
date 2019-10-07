using System;
using SystemBase;
using UniRx;

namespace Systems.Interaction.Collectable
{
    public class CollectableComponent : GameComponent
    {
        public ReactiveCommand<bool> OnCollect = new ReactiveCommand<bool>();
    }
}