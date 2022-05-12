using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class VoxelWFC : MonoBehaviour
    {

        private struct VoxelPosition
        {
            public Vector2Int Position;
            public VoxelTile Tile;

            public VoxelPosition(Vector2Int position, VoxelTile tile)
            {
                Position = position;
                Tile = tile;
            }
        }
        
        
        
        public List<VoxelTile> TilePrefabs;
        public Vector2Int MapSize = new Vector2Int(10, 10);

        private List<VoxelPosition> _voxelGrid;
        private Queue<Vector2Int> recalcPossibleTilesQueue = new Queue<Vector2Int>();
        private List<float> _chances;
        

        private void Start()
        {
            DateTime startTime = DateTime.Now;
            
            foreach (VoxelTile tilePrefab in TilePrefabs)
            {
                tilePrefab.CalculateSidesColors();
            }
            
            _chances = new List<float>();
            _voxelGrid = new List<VoxelPosition>();
            Generate();
            PlaceAllTiles();
            Debug.Log(DateTime.Now - startTime);
        }

      /*  private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                foreach (VoxelTile spawnedTile in spawnedTiles)
                {
                    if (spawnedTile != null) Destroy(spawnedTile.gameObject);
                }

                Generate();
            }
        }*/

      private void Generate()
      {
          //запоняем карту списком всех возможных префабов из СО
          for (int x = 0; x < MapSize.x; x++)
          for (int y = 0; y < MapSize.y; y++)
          {
              foreach (VoxelTile tilePrefab in TilePrefabs)
              {
                  _voxelGrid.Add(new VoxelPosition(new Vector2Int(x,y), tilePrefab));
              }
          }
          
          //распологаем первый тайл в центре
          Vector2Int mapCenter = new Vector2Int(MapSize.x / 2, MapSize.y / 2);
          _voxelGrid.Add(new VoxelPosition(mapCenter, TilePrefabs[Random.Range(0, TilePrefabs.Count+1)]));
          recalcPossibleTilesQueue.Clear();
          EnqueueNeighboursToRecalc(mapCenter);
          
         bool success = GeneratePossibleTiles();
            
         // if (success) break;
         
        
      }


      private bool GeneratePossibleTiles()
      {
          // Берем перый элемент из очереди 
          Vector2Int position = recalcPossibleTilesQueue.Dequeue();
          int maxInnerIterations = 100;
          int innerIterations = 0;
          
          //пока есть элементы в очереди пробуем подставить
          while (recalcPossibleTilesQueue.Count > 0 && innerIterations++ < maxInnerIterations)
          {
              //если граничный элемент, то пропускаем
              if (position.x == 0 || position.y == 0 ||
                  position.x == MapSize.x - 1 || position.y == MapSize.y - 1)
              {
                  continue;
              }
              //Берем список возможных элементов в данной ячейке
              
              List<VoxelPosition> possibleTilesHere = 
                      _voxelGrid.FindAll(tile => tile.Position == position); //possibleTiles[position.x, position.y];
              
              int countRemoved = possibleTilesHere.RemoveAll(t => !IsTilePossible(t.Tile, position));
              
             /* if (possibleTilesHere.Count == 0)
              {
                  // Зашли в тупик, в этих координатах невозможен ни один тайл. Попробуем ещё раз, разрешим все тайлы
                  // в этих и соседних координатах, и посмотрим устаканится ли всё
                  possibleTilesHere.AddRange(TilePrefabs);
                  possibleTiles[position.x + 1, position.y] = new List<VoxelTile>(TilePrefabs);
                  possibleTiles[position.x - 1, position.y] = new List<VoxelTile>(TilePrefabs);
                  possibleTiles[position.x, position.y + 1] = new List<VoxelTile>(TilePrefabs);
                  possibleTiles[position.x, position.y - 1] = new List<VoxelTile>(TilePrefabs);
                    
                  EnqueueNeighboursToRecalc(position);

                  //backtracks++;
              }*/
          }
          
          List<VoxelPosition> maxCountTile = _voxelGrid.FindAll(tile => tile.Position == Vector2Int.one);
              //possibleTiles[1, 1];
          Vector2Int maxCountTilePosition = Vector2Int.one;

          for (int x = 1; x < MapSize.x - 1; x++)
          for (int y = 1; y < MapSize.y - 1; y++)
          {
              var VoxelsInPositions = _voxelGrid.FindAll(tile => tile.Position == new Vector2Int(x, y));
              if (VoxelsInPositions.Count > maxCountTile.Count)
              {
                  maxCountTile = VoxelsInPositions;
                  maxCountTilePosition = new Vector2Int(x, y);
              }
          }

          if (maxCountTile.Count == 1)
          {
             // Debug.Log($"Generated for {iterations} iterations, with {backtracks} backtracks");
              return true;
          }

          VoxelPosition tileToCollapse = GetRandomTile(maxCountTile);
          var VoxelsToCollapce = _voxelGrid.Find(tile => tile.Position == new Vector2Int(maxCountTilePosition.x, maxCountTilePosition.y));
          VoxelsToCollapce = tileToCollapse;
          //possibleTiles[maxCountTilePosition.x, maxCountTilePosition.y] = new List<VoxelTile> {tileToCollapse};
          EnqueueNeighboursToRecalc(maxCountTilePosition);
     
        
      //Debug.Log($"Failed, run out of iterations with {backtracks} backtracks");

          return false;
      }
      
      private bool IsTilePossible(VoxelTile tile, Vector2Int position)
      {
          bool isAllRightImpossible = //possibleTiles[position.x - 1, position.y]
              _voxelGrid.FindAll(item=> item.Position == position + Vector2Int.left)
              .All(rightTile => !CanAppendTile(tile, rightTile.Tile, Direction.Right));
          if (isAllRightImpossible) return false;
        
          bool isAllLeftImpossible = //possibleTiles[position.x + 1, position.y]
              _voxelGrid.FindAll(item=> item.Position == position + Vector2Int.right)
              .All(leftTile => !CanAppendTile(tile, leftTile.Tile, Direction.Left));
          if (isAllLeftImpossible) return false;
        
          bool isAllForwardImpossible = //possibleTiles[position.x, position.y - 1]
              _voxelGrid.FindAll(item=> item.Position == position + Vector2Int.down)
              .All(fwdTile => !CanAppendTile(tile, fwdTile.Tile, Direction.Forward));
          if (isAllForwardImpossible) return false;
        
          bool isAllBackImpossible = //possibleTiles[position.x, position.y + 1]
              _voxelGrid.FindAll(item=> item.Position == position + Vector2Int.up)
              .All(backTile => !CanAppendTile(tile, backTile.Tile, Direction.Back));
          if (isAllBackImpossible) return false;

          return true;
      }
      
      private VoxelPosition GetRandomTile(List<VoxelPosition> availableTiles)
      {
          _chances.Clear();
          for (int i = 0; i < availableTiles.Count; i++)
          {
              _chances.Add(availableTiles[i].Tile.Weight);
          }

          float value = Random.Range(0, _chances.Sum());
          float sum = 0;

          for (int i = 0; i < _chances.Count; i++)
          {
              sum += _chances[i];
              if (value < sum)
              {
                  return availableTiles[i];
              }
          }

          return availableTiles[availableTiles.Count - 1];
      }
      
      private void EnqueueNeighboursToRecalc(Vector2Int position)
      {
          recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x + 1, position.y));
          recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x - 1, position.y));
          recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x, position.y + 1));
          recalcPossibleTilesQueue.Enqueue(new Vector2Int(position.x, position.y - 1));
      }
      
      private void PlaceAllTiles()
      {
          for (int x = 1; x < MapSize.x - 1; x++)
          for (int y = 1; y < MapSize.y - 1; y++)
          {
              PlaceTile(x, y);
          }
      }
      private void PlaceTile(int x, int y)
      {
          if (!_voxelGrid.Exists(item => item.Position == new Vector2Int(x,y))) return;

          VoxelTile selectedTile = _voxelGrid.Find(item =>item.Position == new Vector2Int(x,y)).Tile;
          Vector3 position = selectedTile.VoxelSize * selectedTile.TileSideVoxels * new Vector3(x, 0, y);
          Instantiate(selectedTile, position, selectedTile.transform.rotation);
      }
      private bool CanAppendTile(VoxelTile existingTile, VoxelTile tileToAppend, Direction direction)
      {
          if (existingTile == null) return true;

          if (direction == Direction.Right)
          {
              return Enumerable.SequenceEqual(existingTile.ColorsRight, tileToAppend.ColorsLeft);
          }
          else if (direction == Direction.Left)
          {
              return Enumerable.SequenceEqual(existingTile.ColorsLeft, tileToAppend.ColorsRight);
          }
          else if (direction == Direction.Forward)
          {
              return Enumerable.SequenceEqual(existingTile.ColorsForward, tileToAppend.ColorsBack);
          }
          else if (direction == Direction.Back)
          {
              return Enumerable.SequenceEqual(existingTile.ColorsBack, tileToAppend.ColorsForward);
          }
          else
          {
              throw new ArgumentException("Wrong direction value, should be Vector3.left/right/back/forward",
                  nameof(direction));
          }
      }
      
    }
}