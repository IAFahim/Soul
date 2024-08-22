using System;
using System.Collections.Generic;
using _Root.Scripts.NPC_Ai.Runtime.WayPointGizmoTool;
using Pancake.Common;
using UnityEngine;

namespace Soul.Controller.Runtime.Grids
{
    [Serializable]
    public class GridWayPointLimiter
    {
        [SerializeField] private WayPoints wayPoints;
        public Vector3Int stepOffset = Vector3Int.one;

        public Vector3Int[] levelOffsets =
        {
            new(2, 2, 2),
            new(1, 2, 2),
            new(1, 1, 2),
            new(1, 1, 1)
        };

        public Vector2Int[] cores =
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


        private List<(Vector3 position, Quaternion rotation)> SpawnPoints(int level)
        {
            stepOffset = levelOffsets[level - 1];
            return SpawnPoints(stepOffset);
        }

        private List<(Vector3 position, Quaternion rotation)> SpawnPoints(Vector3Int step)
        {
            List<(Vector3 position, Quaternion rotation)> placeAbleSpots = new();
            var count = 0;
            for (int c = 0; c < cores.Length; c++)
            {
                Vector2Int core = cores[c];
                int lines = core.x;
                int perLine = core.y;
                if (c % 1.Max(step.x) == 0)
                {
                    for (int l = 0; l < lines; l += 1.Max(step.y))
                    {
                        var index = l * perLine + count;
                        for (int p = 0; p < perLine; p += 1.Max(step.z))
                        {
                            placeAbleSpots.Add((wayPoints.positions[index], wayPoints.rotations[index]));
                            index += 1.Max(step.z);
                        }
                    }
                }

                count += lines * perLine;
            }

            return placeAbleSpots;
        }

        public void OnDrawGizmosSelected()
        {
            if (wayPoints == null) return;
            var placeAbleSpots = SpawnPoints(stepOffset);
            foreach (var position in placeAbleSpots) Gizmos.DrawWireSphere(position.position, 1);
        }
    }
}