using System;
using SystemBase;
using Systems.Attac.Actions;
using Systems.Drop.Actions;
using Systems.Health.Events;
using Systems.Movement;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
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
            component.WindUpBulb.SetActive(false);
            component.AttackBulb.SetActive(false);

            component.PlayerSenseTrigger.OnTriggerStayAsObservable()
                .Where(collider => collider.gameObject.CompareTag("Player"))
                .Subscribe(collider => PlayerIsInSenseCollider(component))
                .AddTo(component);

            component.IsWindingUp.Subscribe(b => OnIsWindingChanged(component, b)).AddTo(component);
            component.IsAttacking.Subscribe(b => OnIsAttackingChanged(component, b)).AddTo(component);
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

        private static void PlayerIsInSenseCollider(Sceletor sceletor)
        {
            if (!sceletor.CanFire) return;

            sceletor.CanFire = false;
            sceletor.IsWindingUp.Value = true;

            Observable
                .Timer(TimeSpan.FromMilliseconds(sceletor.WindUpTimeInMs))
                .Subscribe(_ =>
                {
                    MessageBroker.Default.Publish(new ActAttackSpawn
                    {
                        Word = sceletor.AtackWord,
                        Originator = sceletor.gameObject,
                        Position = sceletor.gameObject.transform.position.XZ(),
                        Direction = sceletor.GetComponent<MovementComponent>().Velocity
                    });
                    sceletor.IsWindingUp.Value = false;
                    sceletor.IsAttacking.Value = true;
                })
                .AddTo(sceletor);

            var timeBetweenAttacs = sceletor.WindUpTimeInMs + sceletor.AttackTime;

            Observable
                .Timer(TimeSpan.FromMilliseconds(timeBetweenAttacs))
                .Subscribe(_ =>
                {
                    sceletor.CanFire = true;
                    sceletor.IsAttacking.Value = false;
                }).AddTo(sceletor); ;
        }

        private void SetBulbsToAttack(Sceletor component, bool attack)
        {
            component.WindUpBulb.SetActive(!attack);
            component.AttackBulb.SetActive(attack);
        }

        private void HideBulbs(Sceletor component)
        {
            component.WindUpBulb.SetActive(false);
            component.AttackBulb.SetActive(false);
        }

        private void OnIsAttackingChanged(Sceletor component, bool isAttacking)
        {
            if (isAttacking)
            {
                SetBulbsToAttack(component, true);
                Debug.Log("Attack");
            }
            else
            {
                HideBulbs(component);
            }
        }

        private void OnIsWindingChanged(Sceletor component, bool isWindingUp)
        {
            if (isWindingUp)
            {
                SetBulbsToAttack(component, false);
                Debug.Log("Windup");
            }
            else
            {
                SetBulbsToAttack(component, true);
            }
        }
    }
}