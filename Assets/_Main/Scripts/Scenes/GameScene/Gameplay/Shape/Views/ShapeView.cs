using _Main.Scripts.Global.Pool.Abstract;
using _Main.Scripts.Scenes.GameScene.Gameplay.Render.Views;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Views 
{
	public class ShapeView : MonoPoolableItem 
	{
		[SerializeField] private MeshFilter meshFilter;
		[SerializeField] private Rigidbody shapeRigidbody;
		[SerializeField] private MeshRenderView meshRenderView;
		[SerializeField] private MeshRenderer meshRenderer;
		[SerializeField] private MeshFilter shadowMeshFilter;
		[SerializeField] private Transform shadowTransform;
		
		private Entity _entity;
		
		public MeshFilter MeshFilter => meshFilter;
		public MeshFilter ShadowMeshFilter => shadowMeshFilter;
		public Transform ShadowTransform => shadowTransform;
		public MeshRenderer MeshRenderer => meshRenderer;


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
		
		public override void OnResetItem()
		{
			_entity = null;
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

	}
}

