using System;
using System.Collections.Generic;
using mattatz.Triangulation2DSystem;
using UnityEngine;

namespace _Main.Scripts.Toolkit.Polygon
{
	public static class ShapeUtils 
	{
		public static Mesh CreateMesh(Triangle2D[] triangles)
		{
			Mesh mesh = new Mesh();
			List<Vector3> vertices = new List<Vector3>();
			List<int> indices = new List<int>();

			int vertexIndex = 0;

			foreach (var triangle in triangles)
			{
				vertices.Add(new Vector3(triangle.a.Coordinate.x, triangle.a.Coordinate.y, 0));
				vertices.Add(new Vector3(triangle.b.Coordinate.x, triangle.b.Coordinate.y, 0));
				vertices.Add(new Vector3(triangle.c.Coordinate.x, triangle.c.Coordinate.y, 0));

				indices.Add(vertexIndex);
				indices.Add(vertexIndex + 1);
				indices.Add(vertexIndex + 2);

				vertexIndex += 3;
			}

			mesh.vertices = vertices.ToArray();
			mesh.triangles = indices.ToArray();

			mesh.RecalculateNormals();
			mesh.RecalculateBounds();

			return mesh;
		}
		
		public static Vector2 RecalculateCenter(Triangle2D[] triangles)
		{
			var minXPosition = float.MaxValue;
			var maxXPosition = float.MinValue;
			var minYPosition = float.MaxValue;
			var maxYPosition = float.MinValue;

			foreach (var triangle in triangles)
			{
				minXPosition = Math.Min(triangle.a.Coordinate.x, minXPosition);
				minXPosition = Math.Min(triangle.b.Coordinate.x, minXPosition);
				minXPosition = Math.Min(triangle.c.Coordinate.x, minXPosition);
				
				maxXPosition = Math.Max(triangle.a.Coordinate.x, maxXPosition);
				maxXPosition = Math.Max(triangle.b.Coordinate.x, maxXPosition);
				maxXPosition = Math.Max(triangle.c.Coordinate.x, maxXPosition);
				
				minYPosition = Math.Min(triangle.a.Coordinate.y, minYPosition);
				minYPosition = Math.Min(triangle.b.Coordinate.y, minYPosition);
				minYPosition = Math.Min(triangle.c.Coordinate.y, minYPosition);
				
				maxYPosition = Math.Max(triangle.a.Coordinate.y, maxYPosition);
				maxYPosition = Math.Max(triangle.b.Coordinate.y, maxYPosition);
				maxYPosition = Math.Max(triangle.c.Coordinate.y, maxYPosition);
			}
			
			var centerXPosition = minXPosition + (maxXPosition - minXPosition) / 2;
			var centerYPosition = minYPosition + (maxYPosition - minYPosition) / 2;
			var offset = Vector2.zero - new Vector2(centerXPosition, centerYPosition);

			foreach (var triangle in triangles)
			{
				triangle.a = new Vertex2D(triangle.a.Coordinate + offset);
				triangle.b = new Vertex2D(triangle.b.Coordinate + offset);
				triangle.c = new Vertex2D(triangle.c.Coordinate + offset);
				triangle.Offset = offset;
			}

			return offset;
		}

		public static List<Vector3> FindExternalPoints(Triangle2D[] triangles, Vector3 shapeCenterPosition)
		{
			List<Vector3> externalPoints = new();
			List<Edge> externalEdges = new();
			FillExternalEdges(triangles, externalEdges);
			foreach (Edge externalEdge in externalEdges)
			{
				TryAddPoint(shapeCenterPosition, externalEdge.PointA, externalPoints);
				TryAddPoint(shapeCenterPosition, externalEdge.PointB, externalPoints);
			}

			return externalPoints;
		}

		public static bool ShapeInsidePolygon(Vector3 shapePosition, List<Vector3> shapeExternalOffsets, 
			Vector3 polygonPosition, List<Vector3> polygonExternalOffsets)
		{
			foreach (var externalOffset in shapeExternalOffsets)
			{
				Vector3 externalPoint = shapePosition + externalOffset;
				if (!PointInPolygon(externalPoint, polygonPosition, polygonExternalOffsets))
				{
					return false;
				}
			}
			
			return true;
		}

		public static bool PointInPolygon(Vector3 point, Vector3 polygonPosition, List<Vector3> polygonExternalOffsets) 
		{
			bool result = false;
			var length = polygonExternalOffsets.Count;
			for(int i = 0, j = length - 1; i < length; j = i++)
			{
				var position = polygonPosition + polygonExternalOffsets[i];
				var previousPosition = polygonPosition + polygonExternalOffsets[j];
				
				if (position == point)
				{
					return true;
				}
				if (((position.y >= point.y) != (previousPosition.y >= point.y)) && 
				    (point.x < (previousPosition.x - position.x) * (point.y - position.y) / (previousPosition.y - position.y) + position.x)) 
				{
					result = !result;
				}
			}
			return result;
		}

		private static void TryAddPoint(Vector3 shapeCenterPosition, Vector3 point, List<Vector3> externalPoints)
		{
			var externalOffset = point - shapeCenterPosition;
			if (!externalPoints.Contains(externalOffset))
			{
				externalPoints.Add(externalOffset);
			}
		}

		private static void FillExternalEdges(Triangle2D[] triangles, List<Edge> externalEdges)
		{
			foreach (Triangle2D triangle in triangles)
			{
				externalEdges.Add(new Edge(triangle.a.Coordinate, triangle.b.Coordinate));
				externalEdges.Add(new Edge(triangle.b.Coordinate, triangle.c.Coordinate));
				externalEdges.Add(new Edge(triangle.c.Coordinate, triangle.a.Coordinate));
			}

			for (int i = 0; i < externalEdges.Count - 1; i++)
			{
				bool isDuplicate = false;
				for (int j = i + 1; j < externalEdges.Count; j++)
				{
					if (externalEdges[j].Equals(externalEdges[i]))
					{
						externalEdges.RemoveAt(j);
						isDuplicate = true;
						j--;
					}
				}

				if (isDuplicate)
				{
					externalEdges.RemoveAt(i);
					i--;
				}
			}

			SortEdges(externalEdges);
		}

		private static void SortEdges(List<Edge> externalEdges)
		{
			for (int i = 1; i < externalEdges.Count; i++)
			{
				if (externalEdges[i].PointA == externalEdges[i - 1].PointB)
				{
					continue;
				}

				for (int j = i + 1; j < externalEdges.Count; j++)
				{
					if (externalEdges[j].PointA == externalEdges[i - 1].PointB)
					{
						(externalEdges[i], externalEdges[j]) = (externalEdges[j], externalEdges[i]);
						break;
					}
				}
			}
		}
		
	}
}