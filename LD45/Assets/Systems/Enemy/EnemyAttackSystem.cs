using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemBase;
using Systems.Attac.Actions;
using Systems.InputHandling;
using Systems.Movement;
using Assets.Systems.WIndUp;
using UniRx;
using UnityEngine;
using Utils.Unity;

namespace Assets.Systems.Enemy
{
    [GameSystem]
    public class EnemyAttackSystem:GameSystem<EnemyAttackComponent>
    {
        public override void Register(EnemyAttackComponent component)
        {
            var windUp = component.GetComponent<WindUpComponent>();
            var enemyComponent = component.GetComponent<EnemyComponent>();
            windUp.Ready.Subscribe(_ => AttackWithHit(component, enemyComponent)).AddTo(component);

            component.BulbToShow.SetActive(false);
        }

        private void AttackWithHit(EnemyAttackComponent component, EnemyComponent enemyComponent)
        {
            component.BulbToShow.SetActive(true);
            MessageBroker.Default.Publish(new ActAttackSpawn
            {
                Word = component.AttackType,
                Originator = component.gameObject,
                Position = component.gameObject.transform.position.XZ(),
                Direction = Vector2.one,
                Duration = component.AttackDurationInMs
            });
            Observable.Timer(TimeSpan.FromMilliseconds(component.AttackDurationInMs))
                .Subscribe(l =>
                {
                    component.BulbToShow.SetActive(false);
                    enemyComponent.CanFire = true;
                });
        }
    }
}
