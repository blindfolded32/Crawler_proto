using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "DataBase/VoxelSO", fileName = nameof(VoxelSO))]
    public class VoxelSO : ScriptableObject
    {
        public VoxelTile Prefab;
        public List<VoxelNeighbours> PossibleTiles;

        [Serializable]
        public struct VoxelNeighbours
        {
            public List<VoxelTile> Voxel;
            public Direction Direction;
            public VoxelNeighbours(List<VoxelTile> voxel, Direction direction)
            {
                Voxel = voxel;
                Direction = direction;
            }
        }
        
    }
}