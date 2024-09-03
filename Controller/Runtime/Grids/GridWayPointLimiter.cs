using System;
using System.Collections.Generic;
using _Root.Scripts.NPC_Ai.Runtime.WayPointGizmoTool;
using UnityEngine;

namespace Soul.Controller.Runtime.Grids
{
    [Serializable]
    public class GridWayPointLimiter
    {
        [SerializeField] private WayPoints wayPoints;
        [SerializeField] private Vector3Int stepOffset = Vector3Int.one;

        public WayPoints WayPoints
        {
            set => wayPoints = value;
        }

        [SerializeField] private Vector3Int[] levelOffsets =
        {
            new(2, 2, 2),
            new(1, 2, 2),
            new(1, 1, 2),
            new(1, 1, 1)
        };

        [SerializeField] private Vector2Int[] cores =
        {
            new(5, 10),
            new(5, 10),
            new(5, 10),
            new(5, 10),
            new(5, 10),
            new(5, 10),
            new(5, 10),
            new(5, 10),
        };

        // Cache for the last calculated spawn points
        private List<List<(Vector3 position, Quaternion rotation)>> _cachedSpawnPoints;
        private Vector3Int _cachedStepOffset;

        public List<(Vector3 position, Quaternion rotation)> PointList(int level)
        {
            stepOffset = levelOffsets[level - 1];
            return PointList(stepOffset);
        }
        
        public List<List<(Vector3 position, Quaternion rotation)>> CoredPoints(int level)
        {
            stepOffset = levelOffsets[level - 1];
            return CoredPoints(stepOffset);
        }

        private List<(Vector3 position, Quaternion rotation)> PointList(Vector3Int step)
        {
            var points = CoredPoints(step);
            List<(Vector3 position, Quaternion rotation)> spawnPoints = new();
            foreach (var point in points)
            {
                spawnPoints.AddRange(point);
            }

            return spawnPoints;
        }
        
        


        public List<List<(Vector3 position, Quaternion rotation)>> CoredPoints(Vector3Int step)
        {
            // Check if the stepOffset has changed
            if (_cachedStepOffset == step && _cachedSpawnPoints != null)
            {
                return _cachedSpawnPoints;
            }

            // Precalculate step values for efficiency
            int stepX = Mathf.Max(1, step.x);
            int stepY = Mathf.Max(1, step.y);
            int stepZ = Mathf.Max(1, step.z);

            // Recalculate spawn points since stepOffset is different
            int waypointCount = 0;
            int coreToBeAdded = Mathf.CeilToInt(cores.Length / (float)stepX);
            List<List<(Vector3 position, Quaternion rotation)>> placeAbleSpots = new();

            // Iterate over each core (group of waypoints)
            for (int coreIndex = 0; coreIndex < cores.Length; coreIndex++)
            {
                // Get the dimensions of the current core
                Vector2Int core = cores[coreIndex];
                int numLines = core.x;
                int waypointsPerLine = core.y;

                // Check if this core should be processed based on the step offset
                if (coreIndex % stepX == 0)
                {
                    List<(Vector3 position, Quaternion rotation)> coreSpots = new();
                    // Iterate over each line in the core, skipping lines based on the step offset
                    for (int lineIndex = 0; lineIndex < numLines; lineIndex += stepY)
                    {
                        // Calculate the starting index for the current line
                        int startIndex = lineIndex * waypointsPerLine + waypointCount;

                        // Iterate over waypoints in the line, skipping waypoints based on the step offset
                        for (int waypointIndex = 0; waypointIndex < waypointsPerLine; waypointIndex += stepZ)
                        {
                            // Calculate the index of the current waypoint
                            int currentIndex = startIndex + waypointIndex;

                            // Add the waypoint's position and rotation to the list
                            coreSpots.Add((wayPoints.positions[currentIndex], wayPoints.rotations[currentIndex]));
                        }
                    }

                    if (coreSpots.Count > 0)
                    {
                        placeAbleSpots.Add(coreSpots);
                    }
                }


                // Update the total waypoint count
                waypointCount += numLines * waypointsPerLine;
            }

            // Update the cache
            _cachedSpawnPoints = placeAbleSpots;
            _cachedStepOffset = step;

            return placeAbleSpots;
        }

#if UNITY_EDITOR
        internal void OnDrawGizmosSelected()
        {
            if (wayPoints == null) return;
            foreach (var position in PointList(stepOffset)) Gizmos.DrawWireSphere(position.position, 1);
        }
#endif
    }
}