using _Main.Scripts.Global.Pool.Abstract;
using _Main.Scripts.Scenes.GameScene.Gameplay.Render.Views;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
using _Main.Scripts.Scenes.GameScene.Services.Layer;
using Scellecs.Morpeh;
using UnityEngine;
using Zenject;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Views 
{
	public class ShapeView : MonoPoolableItem 
	{
		[SerializeField] private MeshFilter meshFilter;
		[SerializeField] private MeshRenderView meshRenderView;
		[SerializeField] private MeshRenderer meshRenderer;
		[SerializeField] private MeshRenderer shadowMeshRenderer;
		[SerializeField] private MeshFilter shadowMeshFilter;
		[SerializeField] private Transform shadowTransform;
		
		private Entity _entity;
		private ILayerService _layerService;

		public MeshFilter MeshFilter => meshFilter;
		public MeshFilter ShadowMeshFilter => shadowMeshFilter;
		public Transform ShadowTransform => shadowTransform;
		public MeshRenderer MeshRenderer => meshRenderer;

		[Inject]
		public void Construct(ILayerService layerService)
		{
			_layerService = layerService;
		}

		public override void OnInitializeItem()
		{
			Mesh mesh = new Mesh();
			meshFilter.sharedMesh = mesh;
			shadowMeshFilter.sharedMesh = mesh;
		}
		
		public void Init(Entity entity, Material meshRendererMaterial, bool renderLines = true)
		{
			_entity = entity;
			meshRenderer.material = meshRendererMaterial;
			meshRenderView.Init(renderLines ? _entity : null);
		}

		public override void OnSetupItem()
		{
			meshRenderer.enabled = true;
			_layerService.OnLayerViewChanged += SetViewByLayer;
			_layerService.OnLayersReset += ResetSortingOrder;
		}

		public override void OnResetItem()
		{
			_entity = null;
			_layerService.OnLayerViewChanged -= SetViewByLayer;
			_layerService.OnLayersReset -= ResetSortingOrder;
			meshRenderView.Reset();
		}

		public void SetupTransformProperties(Transform parent, Vector3 position, Vector3 size)
		{
			transform.SetParent(parent, false);
			transform.localPosition = position;
			transform.localScale = size;
		}

		public override void OnDisposeItem()
		{
			if (meshFilter.sharedMesh!= null)
			{
				Destroy(meshFilter.sharedMesh);
			}
		}

		public void UpdateSortingOrder(int orderIndex)
		{
			meshRenderer.sortingOrder = orderIndex;
			shadowMeshRenderer.sortingOrder = orderIndex;
		}

		private void SetViewByLayer(int layerIndex)
		{
			if (_entity.Has<ShapeOnPatternMarker>())
			{
				meshRenderer.enabled = layerIndex == -1 || layerIndex == meshRenderer.sortingOrder;
			}
		}

		private void ResetSortingOrder()
		{
			meshRenderer.enabled = true;
			UpdateSortingOrder(0);
		}
		
	}
}

