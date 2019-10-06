using SystemBase;
using Systems.Health;
using Systems.Player;
using UniRx;

namespace Systems.HealthBar
{
    [GameSystem]
    public class HealthBarSystem : GameSystem<HealthBarComponent, HealthComponent>
    {
        private HealthBarComponent _healthBarComponent;
        private HealthComponent _healthComponent;

        public override void Register(HealthComponent component)
        {
            if (component.GetComponent<PlayerComponent>())
            {
                _healthComponent = component;
                FinishRegistration();
            }
        }

        public override void Register(HealthBarComponent component)
        {
            _healthBarComponent = component;
            FinishRegistration();
        }

        private void FinishRegistration()
        {
            if (_healthComponent == null || _healthBarComponent == null)
            {
                return;
            }

            _healthBarComponent.MaxHealthShown = _healthComponent.MaxHealth;
            _healthBarComponent.CurrentHealthShown = _healthComponent.MaxHealth;
            SetSpriteOfHeart();
            _healthComponent.CurrentHealth.AsObservable().Where(_ => _healthBarComponent != null).Subscribe(OnHealthChanged)
                .AddTo(_healthComponent);
        }

        private void OnHealthChanged(int newHealth)
        {
            _healthBarComponent.CurrentHealthShown = newHealth;

            SetSpriteOfHeart();
        }

        private void SetSpriteOfHeart()
        {
            for (int i = 0; i < _healthBarComponent.MaxHealthShown; i++)
            {
                if (i < _healthBarComponent.CurrentHealthShown)
                {
                    _healthBarComponent.HeartImages[i].sprite = _healthBarComponent.FullHeart;
                }
                else
                {
                    _healthBarComponent.HeartImages[i].sprite = _healthBarComponent.EmptyHeart;
                }
            }
        }
    }
}
