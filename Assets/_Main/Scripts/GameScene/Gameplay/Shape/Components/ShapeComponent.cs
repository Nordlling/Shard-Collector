using System;
using UnityEngine;
using System.Collections.Generic;
using mattatz.Triangulation2DSystem;
using Scellecs.Morpeh;

namespace _Main.Scripts 
{
	[Serializable]
	public struct ShapeComponent : IComponent
	{
		public ShapeView ShapeView;
		public Triangle2D[] Triangles;
		public Vector3[] Points;
		public List<Vector3> ExternalPoints;
	}
}

