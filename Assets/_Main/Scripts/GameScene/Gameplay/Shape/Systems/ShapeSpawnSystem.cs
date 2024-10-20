using System.Collections.Generic;
using App.Scripts.Modules.Pool.Interfaces.Pool;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Spawn
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
                ref var spawnSignal = ref entity.GetComponent<ShapeSpawnSignal>();
                
                if (spawnSignal.Triangles == null)
                {
                    UnityEngine.Debug.LogError("Null triangles");
                    continue;
                }
                
                ShapeUtils.RecalculateCenter(spawnSignal.Triangles);
                Mesh mesh = ShapeUtils.CreateMesh(spawnSignal.Triangles);

                var shapeView = _pool.Get();
                shapeView.Init(entity, _renderConfig.ShapeMaterial, false);
                shapeView.SetupTransformProperties(spawnSignal.Parent, spawnSignal.Position, spawnSignal.Size);
                shapeView.MeshFilter.sharedMesh = mesh;
                shapeView.ShadowMeshFilter.sharedMesh = mesh;

                List<Vector3> externalPoints = ShapeUtils.FindExternalPoints(spawnSignal.Triangles, shapeView.transform.position);

                ref var shapeComponent = ref entity.AddComponent<ShapeComponent>();
                shapeComponent.ShapeView = shapeView;
                shapeComponent.Triangles = spawnSignal.Triangles;
                shapeComponent.Points = mesh.vertices;
                shapeComponent.ExternalPointOffsets = externalPoints;
            }
        }

        public void Dispose() { }
    }
}