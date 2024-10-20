using App.Scripts.Modules.Pool.Abstract;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Main.Scripts 
{
	public class ShapeView : MonoPoolableItem 
	{
		[SerializeField] private MeshFilter meshFilter;
		[SerializeField] private Rigidbody shapeRigidbody;
		[SerializeField] private MeshRenderView meshRenderView;
		[SerializeField] private MeshRenderer meshRenderer;
		[SerializeField] private MeshFilter shadowMeshFilter;
		[SerializeField] private Transform shadowTransform;
		
		[SerializeField] [ReadOnly] private Entity _entity;
		
		public MeshFilter MeshFilter => meshFilter;
		public MeshFilter ShadowMeshFilter => shadowMeshFilter;
		public Transform ShadowTransform => shadowTransform;
		public Rigidbody ShapeRigidbody => shapeRigidbody;

		public void Init(Entity entity, Material meshRendererMaterial, bool renderLines = true)
		{
			_entity = entity;
			meshRenderer.material = meshRendererMaterial;
			meshRenderView.Init(renderLines ? _entity : null);
		}
		
		public override void OnResetItem()
		{
			_entity = null;
			ShadowMeshFilter.mesh = null;
		}

		public void SetupTransformProperties(Transform parent, Vector3 position, Vector3 size)
		{
			transform.SetParent(parent, false);
			transform.localPosition = position;
			transform.localScale = size;
		}
		
	}
}

