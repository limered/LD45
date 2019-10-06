﻿using SystemBase;
using Systems.Camera;
using Systems.Health.Events;
using Systems.Player;
using UniRx;
using UniRx.Triggers;

namespace Systems.Room
{
    [GameSystem]
    public class RoomSystem : GameSystem<RoomComponent>
    {
        public override void Register(RoomComponent component)
        {
            component.PlayerSensor.OnTriggerEnterAsObservable()
                .Where(collider => collider.GetComponent<PlayerComponent>())
                .Select(_ => component)
                .Subscribe(PlayerEntered)
                .AddTo(component);

            component.PlayerSensor.OnTriggerExitAsObservable()
                .Where(collider => collider.GetComponent<PlayerComponent>())
                .Select(_ => component)
                .Subscribe(PlayerExits)
                .AddTo(component);

            MessageBroker.Default.Receive<HealthEvtReachedZero>()
                .Where(zero => zero.ObjectToKill.GetComponent<PlayerComponent>())
                .Subscribe(zero => component.PlayerInside = false)
                .AddTo(component);
        }

        private void PlayerEntered(RoomComponent roomComponent)
        {
            MessageBroker.Default.Publish(new ActCameraSwitchTarget
            {
                NewTarget = roomComponent.CameraPosition.transform.position
            });

            roomComponent.PlayerInside = true;
        }

        private void PlayerExits(RoomComponent roomComponent)
        {
            roomComponent.PlayerInside = false;
        }
    }
}