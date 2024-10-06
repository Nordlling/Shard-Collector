using UnityEngine;
using System.Collections.Generic;
using mattatz.Triangulation2DSystem;

namespace _Main.Scripts 
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

		private static void TryAddPoint(Vector3 shapeCenterPosition, Vector3 point, List<Vector3> externalPoints)
		{
			var externalOffset1 = point - shapeCenterPosition;
			if (!externalPoints.Contains(externalOffset1))
			{
				externalPoints.Add(externalOffset1);
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
		}
		
	}
}