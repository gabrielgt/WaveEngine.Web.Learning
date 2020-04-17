using System;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;

namespace BetiJaiDemo.Behaviors
{
    public class LookAtCameraBehavior : Behavior
    {
        private Camera3D camera;

        [BindComponent] public Transform3D transform;

        protected override void OnActivated()
        {
            base.OnActivated();

            camera = Owner.Scene.Managers.RenderManager.ActiveCamera3D;
        }

        protected override void Update(TimeSpan gameTime)
        {
            transform.LookAt(camera.Transform.Position);
        }
    }
}