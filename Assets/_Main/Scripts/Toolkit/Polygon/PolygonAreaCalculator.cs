using System.Collections.Generic;
using Clipper2Lib;
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
        
        public double CalculateArea(List<Vector3> vertices, FillRule fillRule = FillRule.NonZero)
        {
            _clipper.Clear();
            CombineByExternalOffsets(vertices, Vector3.zero);
            return CalculateUnionArea(fillRule);
        }

        public double CalculateUnionArea(List<Vector3> worldPositions, List<List<Vector3>> shapesOfExternalOffsets, FillRule fillRule = FillRule.NonZero)
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

        private void CombineByExternalOffsets(List<Vector3> externalOffsets, Vector3 worldPosition)
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
        
    }
}