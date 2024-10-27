using System.Collections.Generic;
using App.Scripts.Modules.Utils.RandomService;
using mattatz.Triangulation2DSystem;

namespace _Main.Scripts.Gameplay.GameBoard
{
    public class ShapeGrouper
    {
        private readonly IRandomService _randomService;
        
        private double _minWeight;
        private double _maxWeight;

        public ShapeGrouper(IRandomService randomService)
        {
            _randomService = randomService;
        }

        public List<List<Triangle2D>> GroupTrianglesIntoShapes(Triangle2D[] triangles, double minWeight, double maxWeight)
        {
            _minWeight = minWeight;
            _maxWeight = maxWeight;
            var activeTriangles = new List<Triangle2D>();
            var allTriangles = new List<Triangle2D>(triangles);
            var shapes = new List<List<Triangle2D>>();

            while (allTriangles.Count != 0)
            {
                List<Triangle2D> usedTriangles = new();

                int firstTriangleIndex = _randomService.Range(0, allTriangles.Count);
                Triangle2D firstTriangle = allTriangles[firstTriangleIndex];

                if (firstTriangle.Area > _maxWeight)
                {
                    shapes.Add(new List<Triangle2D> { firstTriangle });
                    allTriangles.Remove(firstTriangle);
                    continue;
                }

                activeTriangles.Add(firstTriangle);
                GroupShape(allTriangles, usedTriangles, activeTriangles, firstTriangle.GetArea());

                shapes.Add(usedTriangles);
                allTriangles.RemoveAll(el => usedTriangles.Contains(el));
            }

            return shapes;
        }

        private void GroupShape(List<Triangle2D> allTriangles, List<Triangle2D> usedTriangles, List<Triangle2D> activeTriangles, 
            double shapeTotalArea)
        {
            while (true)
            {
                usedTriangles.AddRange(activeTriangles);

                if (activeTriangles.Count == 0)
                {
                    return;
                }

                var newActiveTriangles = IterateActiveTriangles(allTriangles, usedTriangles, activeTriangles, ref shapeTotalArea);

                activeTriangles.Clear();
                activeTriangles.AddRange(newActiveTriangles);
            }
        }

        private List<Triangle2D> IterateActiveTriangles(List<Triangle2D> allTriangles, List<Triangle2D> usedTriangles, List<Triangle2D> activeTriangles,
            ref double shapeTotalArea)
        {
            List<Triangle2D> newActiveTriangles = new();
            activeTriangles.ShuffleRandom(_randomService);

            foreach (Triangle2D activeTriangle in activeTriangles)
            {
                int minAdjacentCountForRandom = shapeTotalArea >= _minWeight ? 0 : 1;

                List<Triangle2D> adjacentTriangles = FindAdjacentTriangles(allTriangles, usedTriangles, activeTriangle, newActiveTriangles, shapeTotalArea);

                if (adjacentTriangles.Count == 0)
                {
                    continue;
                }

                adjacentTriangles.ShuffleRandom(_randomService);
                int adjacentCount = _randomService.Range(minAdjacentCountForRandom, adjacentTriangles.Count + 1);
                for (int i = 0; i < adjacentCount; i++)
                {
                    var adjacentTriangle = adjacentTriangles[i];
                    var newArea = shapeTotalArea + adjacentTriangle.Area;

                    if (newArea <= _maxWeight)
                    {
                        newActiveTriangles.Add(adjacentTriangle);
                        shapeTotalArea = newArea;
                    }
                }
            }

            return newActiveTriangles;
        }

        private List<Triangle2D> FindAdjacentTriangles(List<Triangle2D> allTriangles, List<Triangle2D> usedTriangles,
            Triangle2D selectedTriangle, List<Triangle2D> newActiveTriangles, double shapeTotalArea)
        {
            List<Triangle2D> adjacentTriangles = new();
            foreach (Triangle2D triangle in allTriangles)
            {
                if (usedTriangles.Contains(triangle) || newActiveTriangles.Contains(triangle))
                {
                    continue;
                }

                int commonCount = selectedTriangle.ContactPointsCount(triangle);
                double newShapeTotalArea = shapeTotalArea + triangle.Area;

                if (commonCount == 2 && newShapeTotalArea <= _maxWeight)
                {
                    adjacentTriangles.Add(triangle);
                }
            }

            return adjacentTriangles;
        }

    }
}