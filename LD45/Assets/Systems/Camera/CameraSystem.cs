using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Camera
{
    [GameSystem]
    public class CameraSystem : GameSystem<CameraComponent>
    {
        public override void Register(CameraComponent component)
        {
            component.FixedUpdateAsObservable()
                .Select(_ => component)
                .Subscribe(AnimateCamera)
                .AddTo(component);

            MessageBroker.Default.Receive<ActCameraSwitchTarget>()
                .Select(msg => new { target = msg.NewTarget, cam = component })
                .Subscribe(obj => obj.cam.TargetPosition = obj.target)
                .AddTo(component);
        }

        private void AnimateCamera(CameraComponent camComponent)
        {
            if (camComponent.TargetPosition == Vector3.zero) return;

            var newPos = Vector3.Lerp(camComponent.transform.position, camComponent.TargetPosition, 0.1f);
            camComponent.transform.position = newPos;
        }
    }
}