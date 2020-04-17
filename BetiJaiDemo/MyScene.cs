using BetiJaiDemo.Behaviors;
using BetiJaiDemo.Common;
using BetiJaiDemo.Models;
using FastExpressionCompiler.LightExpression;
using System.Collections.Generic;
using System.Linq;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Graphics.Batchers;
using WaveEngine.Framework.Services;
using WaveEngine.Mathematics;

namespace BetiJaiDemo
{
    public class MyScene : Scene
    {
        private const string HotspotTag = "hotspot";
        private IEnumerable<Zone> _zones;

        protected override void CreateScene()
        {
            DisableMultithreadingStuff();
            CreateZones();
            CreateHotspots();
        }

        private void CreateZones()
        {
            var zonesList = JsonHelper.Deserialize<ZoneList>("Content/Raw/zones.json");
            _zones = zonesList.Zones;
        }

        private void CreateHotspots()
        {
            var hotspotsList = JsonHelper.Deserialize<HotspotList>("Content/Raw/hotspots.json");
            var hotspots = hotspotsList.Hotspots;
            var assetsService = Application.Current.Container.Resolve<AssetsService>();
            var material = assetsService.Load<Material>(WaveContent.Materials.HotspotMaterial);

            foreach (var hotspot in hotspots)
            {
                var position = JsonHelper.ParseVector3(hotspot.Location);
                position.Z *= -1;   // Invert axis Z

                var hotspotEntity = new Entity($"hotspot{hotspot.Id}-{hotspot.ZoneId}") {Tag = HotspotTag}
                    .AddComponent(new MaterialComponent {Material = material})
                    .AddComponent(new MeshRenderer())
                    .AddComponent(new SphereMesh())
                    .AddComponent(new Spinner {AxisIncrease = new Vector3(0.25f, 0.75f, 0.5f)})
                    .AddComponent(
                        new Transform3D
                        {
                            Orientation = Quaternion.CreateFromEuler(new Vector3(0, MathHelper.Pi, -MathHelper.PiOver2)),
                            Scale = new Vector3(Constants.HotspotSideMeters, Constants.HotspotSideMeters, Constants.HotspotSideMeters),
                            Position = position
                        })
                    .AddComponent(new HotspotBehavior(hotspot.Name));

                Managers.EntityManager.Add(hotspotEntity);
            }
        }

        private void DisableMultithreadingStuff()
        {
            var meshRenderFeature = this.Managers.RenderManager.FindRenderFeature<MeshRenderFeature>();
            var dynamicBatchMeshProcessor = meshRenderFeature.FindMeshProcessor<DynamicBatchMeshProcessor>();
            dynamicBatchMeshProcessor.IsActivated = false;
        }

        public void DisplayZone(int zoneId)
        {
            var zone = _zones?.FirstOrDefault(z => z.Id == zoneId);
            if (zone != null)
            {
                DisplayZoneWithItsHotspots(zone);
            }
        }

        private void DisplayZoneWithItsHotspots(Zone zone)
        {
            const float scaleFactor = 1 / 100f;

            var camera = Managers.EntityManager.Find("camera");
            var cameraTravelling = camera.FindComponent<CameraTravellingBehavior>();


            var position = JsonHelper.ParseVector3(zone.Location);
            position.Z *= -1;   // Invert axis Z
            position *= scaleFactor;

            var rotation = JsonHelper.ParseVector3(zone.Rotate);
            rotation *= -Vector3.One;

            cameraTravelling.AnimateTo(position, rotation);

            //ShowAndHideHotspotsByZone(zone);
        }

        private void ShowAndHideHotspotsByZone(Zone zone)
        {
            var hotspots = Managers.EntityManager.FindAllByTag(HotspotTag).ToList();
            foreach (var hotspot in hotspots)
            {
                hotspot.IsEnabled = false;
            }

            var currentZoneHotSpots = hotspots.Where(h => h.Name.EndsWith(zone.Id.ToString()));
            foreach (var hotspot in currentZoneHotSpots)
            {
                hotspot.IsEnabled = true;
            }
        }
    }
}