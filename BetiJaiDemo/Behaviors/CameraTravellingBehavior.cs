using System;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Mathematics;

namespace BetiJaiDemo.Behaviors
{
    public class CameraTravellingBehavior : Behavior
    {
        private const int CameraSmoothTimeSeconds = 4;

        private Vector3 cameraPositionCurrentVelocity;

        private Vector3 cameraRotationCurrentVelocity;

        private Vector3 cameraTargetPosition;

        private Vector3 cameraTargetRotation;

        [BindComponent] public Transform3D cameraTransform;

        private bool isCameraAnimationInProgress;

        public void AnimateTo(Vector3 position, Vector3 rotation)
        {
            cameraTargetPosition = position;
            cameraTargetRotation = rotation;
            isCameraAnimationInProgress = true;
        }

        public void FixTo(Vector3 position, Vector3 rotation)
        {
            cameraTransform.Position = position;
            cameraTransform.Rotation = rotation;
        }

        protected override void Update(TimeSpan gameTime)
        {
            if (isCameraAnimationInProgress)
            {
                cameraTransform.Position = Vector3.SmoothDamp(
                    cameraTransform.Position,
                    cameraTargetPosition,
                    ref cameraPositionCurrentVelocity,
                    CameraSmoothTimeSeconds,
                    (float) gameTime.TotalSeconds);

                cameraTransform.Rotation = Vector3.SmoothDamp(
                    cameraTransform.Rotation,
                    cameraTargetRotation,
                    ref cameraRotationCurrentVelocity,
                    CameraSmoothTimeSeconds,
                    (float) gameTime.TotalSeconds);

                if ((cameraTransform.Position == cameraTargetPosition) &&
                    (cameraTransform.Rotation == cameraTargetRotation))
                {
                    isCameraAnimationInProgress = false;
                }
            }
        }
    }
}