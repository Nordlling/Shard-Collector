using System;
using System.Collections.Generic;
using _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Views;
using mattatz.Triangulation2DSystem;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Main.Scripts.Scenes.GameScene.Gameplay.Shape.Components 
{
	[Serializable]
	public struct ShapeComponent : IComponent
	{
		public ShapeView ShapeView;
		public double Area;
		public Vector3[] Points;
		public List<Vector3> ExternalPointOffsets;
		public Triangle2D[] Triangles;
	}
}

