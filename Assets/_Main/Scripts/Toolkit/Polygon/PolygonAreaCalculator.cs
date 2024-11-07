using System.Collections.Generic;
using Clipper2Lib;
using DG.Tweening.Plugins.Core.PathCore;
using mattatz.Triangulation2DSystem;
using UnityEngine;

namespace _Main.Scripts.Toolkit.Polygon
{
    public class PolygonAreaCalculator
    {
        private readonly ClipperD _clipper = new();
        private readonly PathsD _solutionClosed = new();
        private readonly PathsD _solutionOpen = new();
        private readonly PathD _path = new();
        
        public double CalculateArea(Vector3[] vertices, FillRule fillRule = FillRule.NonZero)
        {
            _clipper.Clear();
            CombineByExternalOffsets(vertices, Vector3.zero);
            return CalculateUnionArea(fillRule);
        }

        public double CalculateUnionArea(List<Vector3> worldPositions, List<Vector3[]> shapesOfExternalOffsets, FillRule fillRule = FillRule.NonZero)
        {
            _clipper.Clear();
            for (int i = 0; i < shapesOfExternalOffsets.Count; i++)
            {
                var worldPosition = i < worldPositions.Count ? worldPositions[i] : Vector3.zero;
                CombineByExternalOffsets(shapesOfExternalOffsets[i], worldPosition);
            }
            
            return CalculateUnionArea(fillRule);
        }

        public double CalculateUnionArea(List<Vector3> worldPositions, List<Triangle2D[]> shapesOfTriangles, FillRule fillRule = FillRule.NonZero)
        {
            _clipper.Clear();
            for (int i = 0; i < worldPositions.Count; i++)
            {
                var worldPosition = i < worldPositions.Count ? worldPositions[i] : Vector3.zero;
                CombineByTriangles(shapesOfTriangles[i], worldPosition);
            }

            return CalculateUnionArea(fillRule);
        }

        private double CalculateUnionArea(FillRule fillRule)
        {
            _clipper.Execute(ClipType.Union, fillRule, _solutionClosed, _solutionOpen);
            double totalArea = Clipper.Area(_solutionClosed);
            return totalArea;
        }

        private void CombineByExternalOffsets(Vector3[] externalOffsets, Vector3 worldPosition)
        {
            _path.Clear();
            foreach (var offset in externalOffsets)
            {
                _path.Add(CreatePoint(offset, worldPosition));
            }
            _clipper.AddSubject(_path);
        }

        private void CombineByTriangles(Triangle2D[] triangles, Vector3 worldPosition)
        {
            foreach (var triangle in triangles)
            {
                _path.Clear();
                _path.Add(CreatePoint(triangle.a.Coordinate, worldPosition));
                _path.Add(CreatePoint(triangle.b.Coordinate, worldPosition));
                _path.Add(CreatePoint(triangle.c.Coordinate, worldPosition));
                _clipper.AddSubject(_path);
            }
        }

        private PointD CreatePoint(Vector2 point, Vector3 worldPosition)
        {
            return new PointD(point.x + worldPosition.x, point.y + worldPosition.y);
        }
        
        
        private static readonly PathsD Paths1 = new() { new PathD() };
        private static readonly PathsD Paths2 = new() { new PathD() };
        
        public static bool DoPolygonsIntersect(Vector3[] poly1, Vector3[] poly2)
        {
            FillPaths(Paths1, poly1);
            FillPaths(Paths2, poly2);

            PathsD solution = Clipper.Intersect(Paths1, Paths2, FillRule.NonZero);
            
            return solution.Count > 0;
        }

        private static void FillPaths(PathsD paths, Vector3[] polygon)
        {
            paths[0].Clear();
            foreach (var point in polygon)
            {
                paths[0].Add(new PointD(point.x, point.y));
            }
        }
        
    }
}