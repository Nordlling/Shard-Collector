using System.Collections.Generic;
using _Main.Scripts.Global.Pool.Interfaces.Pool;
using _Main.Scripts.Scenes.GameScene.Gameplay.Pattern.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.Render.Configs;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Views;
using _Main.Scripts.Toolkit.Extensions.Geometry;
using _Main.Scripts.Toolkit.Polygon;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Systems
{
    public class ShapeSpawnSystem : ISystem
    {
        private readonly IPool<ShapeView> _pool;
        private readonly RenderConfig _renderConfig;

        private Filter _filter;
        public World World { get; set; }
        
        public ShapeSpawnSystem(IPool<ShapeView> pool, RenderConfig renderConfig)
        {
            _pool = pool;
            _renderConfig = renderConfig;
        }

        public void OnAwake()
        {
            _filter = World.Filter
                .With<ShapeSpawnSignal>()
                .Without<PatternMarker>()
                .Build();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                CreateShape(entity);
            }
        }

        private void CreateShape(Entity entity)
        {
            ref var spawnSignal = ref entity.GetComponent<ShapeSpawnSignal>();
                
            if (spawnSignal.Triangles == null)
            {
                UnityEngine.Debug.LogError("Null triangles");
                return;
            }
                
            Vector3 offset = ShapeUtils.RecalculateCenter(spawnSignal.Triangles);
            Mesh mesh = ShapeUtils.CreateMesh(spawnSignal.Triangles);

            var shapeView = _pool.Get();
            shapeView.Init(entity, _renderConfig.ShapeMaterial, false);
            shapeView.SetupTransformProperties(spawnSignal.Parent, spawnSignal.Position, spawnSignal.Size);
            shapeView.MeshFilter.sharedMesh = mesh;
            shapeView.ShadowMeshFilter.sharedMesh = mesh;
            Vector3 shapePosition = shapeView.transform.position;
            List<Vector3> externalPoints = ShapeUtils.FindExternalPoints(spawnSignal.Triangles, shapePosition);
            
            mesh.FillUV();
            shapePosition -= offset;
            shapeView.transform.position = shapePosition;
            var propertyBlock = GradientUtils.GetChangeColorPropertyBlock(shapeView.MeshRenderer.material, Vector3.zero, spawnSignal.PatternSize, shapePosition, mesh.bounds.size);
            shapeView.MeshRenderer.SetPropertyBlock(propertyBlock);

            ref var shapeComponent = ref entity.AddComponent<ShapeComponent>();
            shapeComponent.ShapeView = shapeView;
            shapeComponent.Triangles = spawnSignal.Triangles;
            shapeComponent.Points = mesh.vertices;
            shapeComponent.ExternalPointOffsets = externalPoints;
        }

        public void Dispose() { }
    }
}