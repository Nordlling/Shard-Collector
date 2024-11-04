using _Main.Scripts.Global.Pool.Abstract;
using _Main.Scripts.Scenes.GameScene.Gameplay.Render.Views;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components;
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
			Paths.Clear(); // for test view
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

		// for test view
		[SerializeField] private bool show = true;
		public System.Collections.Generic.List<Clipper2Lib.PathD> Paths = new();

		private void OnDrawGizmos()
		{
			if (!show)
			{
				return;
			}
			
			if (_entity?.Has<ShapeComponent>() ?? false)
			{
				Gizmos.color = Color.red;
				var offsets = _entity.GetComponent<ShapeComponent>().ExternalPointOffsets;
				int offsetsCount = offsets.Count;
				var position = transform.position;

				for (int i = 0; i < offsetsCount; i++)
				{
					int nextIndex = (i + 1) % offsetsCount;
					Gizmos.DrawLine(position + offsets[i], position + offsets[nextIndex]);
				}
			}

			Gizmos.color = Color.yellow;
			foreach (var path in Paths)
			{
				int count = path.Count;
				for (int i = 0; i < count; i++)
				{
					int nextIndex = (i + 1) % count;
					Gizmos.DrawLine(new Vector3((float)path[i].x, (float)path[i].y, 0f),
						new Vector3((float)path[nextIndex].x, (float)path[nextIndex].y, 0f));
				}
			}
		}

	}
}

