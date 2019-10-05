using SystemBase;
using UniRx;

namespace Systems.Health
{
    public class HealthComponent : GameComponent
    {
        public int MaxHealth = 3;
        public IntReactiveProperty CurrentHealth = new IntReactiveProperty(3);
    }
}