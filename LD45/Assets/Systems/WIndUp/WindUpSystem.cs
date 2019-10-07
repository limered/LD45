using Assets.Systems.Enemy;
using System;
using SystemBase;
using Systems.Player;
using UniRx;
using UniRx.Triggers;

namespace Assets.Systems.WIndUp
{
    [GameSystem]
    public class WindUpSystem : GameSystem<WindUpComponent>
    {
        public override void Register(WindUpComponent component)
        {
            var enmyComp = component.GetComponent<EnemyComponent>();
            enmyComp
                .PlayerSensor
                .OnTriggerStayAsObservable()
                .Where(collider => collider.GetComponent<PlayerComponent>())
                .Subscribe(obj => PlayerIsNearEnemy(component, enmyComp))
                .AddTo(component);

            component.Empty.SetActive(false);
            component.One.SetActive(false);
            component.Two.SetActive(false);
            component.Three.SetActive(false);
        }

        private void PlayerIsNearEnemy(WindUpComponent windUp, EnemyComponent enemy)
        {
            if (!enemy.CanFire) return;

            enemy.CanFire = false;
            windUp.IsWindingUp = true;
            var step = windUp.TimeInMs / 4;
            windUp.Empty.SetActive(true);
            Observable.Timer(TimeSpan.FromMilliseconds(step))
                .Subscribe(l =>
                {
                    windUp.Empty.SetActive(false);
                    windUp.One.SetActive(true);
                }).AddTo(enemy);
            Observable.Timer(TimeSpan.FromMilliseconds(step * 2))
                .Subscribe(l =>
                {
                    windUp.One.SetActive(false);
                    windUp.Two.SetActive(true);
                }).AddTo(enemy);
            Observable.Timer(TimeSpan.FromMilliseconds(step * 3))
                .Subscribe(l =>
                {
                    windUp.Two.SetActive(false);
                    windUp.Three.SetActive(true);
                }).AddTo(enemy);
            Observable.Timer(TimeSpan.FromMilliseconds(step * 4))
                .Subscribe(l =>
                {
                    windUp.Three.SetActive(false);
                    windUp.IsWindingUp = false;
                    windUp.Ready.Execute();
                }).AddTo(enemy);
        }
    }
}
