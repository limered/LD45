﻿using System;
using SystemBase;
using Systems.Attac.Actions;
using Systems.InputHandling;
using Systems.Movement;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Math;
using Utils.Plugins;
using Utils.Unity;

namespace Systems.Enemy.Skeletor
{
    [GameSystem]
    public class SceletorBrainSystem : GameSystem<PlayerComponent, Sceletor>
    {
        private readonly ReactiveProperty<PlayerComponent> _player = new ReactiveProperty<PlayerComponent>();

        public override void Register(Sceletor component)
        {
            _player.WhereNotNull()
                .Subscribe(_ => component.FixedUpdateAsObservable()
                    .Select(a => component)
                    .Subscribe(TakeAction)
                    .AddTo(component)
                )
                .AddTo(component);

            component.PlayerSenseTrigger.OnTriggerStayAsObservable()
                .Where(collider => collider.gameObject.CompareTag("Player"))
                .Subscribe(collider => PlayerIsInSenseCollider(component))
                .AddTo(component);

            component.IsWindingUp.Subscribe(OnIsWindingChanged).AddTo(component);
            component.IsAttacking.Subscribe(OnIsAttackingChanged).AddTo(component);
        }

        public override void Register(PlayerComponent component)
        {
            _player.Value = component;
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
                        Word = InputWordType.Hit,
                        Originator = sceletor.gameObject,
                        Position = sceletor.gameObject.transform.position.XZ(),
                        Direction = sceletor.GetComponent<MovementComponent>().Velocity
                    });
                    sceletor.IsWindingUp.Value = false;
                    sceletor.IsAttacking.Value = true;
                });

            var timeBetweenAttacs = sceletor.WindUpTimeInMs + sceletor.AttackTime;

            Observable
                .Timer(TimeSpan.FromMilliseconds(timeBetweenAttacs))
                .Subscribe(_ =>
                {
                    sceletor.CanFire = true;
                    sceletor.IsAttacking.Value = false;
                });
        }

        private void OnIsAttackingChanged(bool isAttacking)
        {
            // TODO Show and Hide Attack Bubble
            if (isAttacking) Debug.Log("Attack");
        }

        private void OnIsWindingChanged(bool isWindingUp)
        {
            // TODO Show and Hide WindUp Bubble
            if (isWindingUp) Debug.Log("Windup");
        }

        private void TakeAction(Sceletor enemy)
        {
            if (_player.Value.IsMoving)
            {
                var directionToPlayer = enemy.transform.position.DirectionTo(_player.Value.transform.position);
                enemy.GetComponent<MovementComponent>().Direction.Value = directionToPlayer.XZ();
            }
            else
            {
                enemy.GetComponent<MovementComponent>().Direction.Value = Vector2.zero;
            }
        }
    }
}