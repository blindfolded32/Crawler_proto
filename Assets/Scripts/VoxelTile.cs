using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VoxelTile : MonoBehaviour
{
    public float VoxelSize = 0.1f;
    public int TileSideVoxels = 8;

    [Range(1, 100)]
    public int Weight = 50;

    [HideInInspector] public byte[] ColorsRight;
    [HideInInspector] public byte[] ColorsForward;
    [HideInInspector] public byte[] ColorsLeft;
    [HideInInspector] public byte[] ColorsBack;

    public void CalculateSidesColors()
    {
        ColorsRight = new byte[TileSideVoxels * TileSideVoxels];
        ColorsForward = new byte[TileSideVoxels * TileSideVoxels];
        ColorsLeft = new byte[TileSideVoxels * TileSideVoxels];
        ColorsBack = new byte[TileSideVoxels * TileSideVoxels];
        
        for (int y = 0; y < TileSideVoxels; y++)
        {
            for (int i = 0; i < TileSideVoxels; i++)
            {
                ColorsRight[y * TileSideVoxels + i] = GetVoxelColor(y, i, Direction.Right);
                ColorsForward[y * TileSideVoxels + i] = GetVoxelColor(y, i, Direction.Forward);
                ColorsLeft[y * TileSideVoxels + i] = GetVoxelColor(y, i, Direction.Left);
                ColorsBack[y * TileSideVoxels + i] = GetVoxelColor(y, i, Direction.Back);
            }
        }
    }

    private byte GetVoxelColor(int verticalLayer, int horizontalOffset, Direction direction)
    {
        var meshCollider = GetComponentInChildren<MeshCollider>();

        float vox = VoxelSize;
        float half = VoxelSize / 2;

        Vector3 rayStart;
        Vector3 rayDir;
        if (direction == Direction.Right)
        {
            rayStart = meshCollider.bounds.min +
                       new Vector3(-half, 0, half + horizontalOffset * vox);
            rayDir = Vector3.right;
        }
        else if (direction == Direction.Forward)
        {
            rayStart = meshCollider.bounds.min +
                       new Vector3(half + horizontalOffset * vox, 0, -half);
            rayDir = Vector3.forward;
        }
        else if (direction == Direction.Left)
        {
            rayStart = meshCollider.bounds.max +
                       new Vector3(half, 0, -half - (TileSideVoxels - horizontalOffset - 1) * vox);
            rayDir = Vector3.left;
        }
        else if (direction == Direction.Back)
        {
            rayStart = meshCollider.bounds.max +
                       new Vector3(-half - (TileSideVoxels - horizontalOffset - 1) * vox, 0, half);
            rayDir = Vector3.back;
        }
        else
        {
            throw new ArgumentException("Wrong direction value, should be Direction.left/right/back/forward",
                nameof(direction));
        }

        rayStart.y = meshCollider.bounds.min.y + half + verticalLayer * vox;

        //Debug.DrawRay(rayStart, direction * .1f, Color.blue, 2);

        if (Physics.Raycast(new Ray(rayStart, rayDir), out RaycastHit hit, vox))
        {
            byte colorIndex = (byte) (hit.textureCoord.x * 256);

            if (colorIndex == 0) Debug.LogWarning("Found color 0 in mesh palette, this can cause conflicts");
            
            return colorIndex;
        }

        return 0;
    }
}