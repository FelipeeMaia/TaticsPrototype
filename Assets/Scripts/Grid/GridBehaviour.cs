using Tactics.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tactics.Core
{
    public class GridBehaviour : MonoBehaviour
    {
        private Tile[,] _Tiles;
        public Action OnCleanTiles;

        public Tile GetTile(int x, int y)
        {
            return (_Tiles[x, y]);
        }

        //Get Tiles in range form an initial tile
        public HashSet<Tile> GetTilesInRange(Tile initialTile, int range, bool checkUnits = true)
        {
            //Set groups
            HashSet<Tile> tilesToSearch = new HashSet<Tile>();
            HashSet<Tile> newTilesToSearch = new HashSet<Tile>();
            HashSet<Tile> tilesFound = new HashSet<Tile>();

            initialTile.movementNeed = 0;
            tilesToSearch.Add(initialTile);
            tilesFound.Add(initialTile);

            while (tilesToSearch.Count != 0)
            {
                foreach (Tile tile in tilesToSearch)
                {
                    foreach (Tile neighbour in tile.neighbours)
                    {
                        if (neighbour == null) continue;

                        if (tilesFound.Contains(neighbour) ||
                            (neighbour.unitOnTile && checkUnits)) continue;

                        int cost = tile.movementNeed + neighbour.movementCost;
                        if (range >= cost)
                        {
                            neighbour.movementNeed = cost;
                            newTilesToSearch.Add(neighbour);
                        }
                    }
                }

                tilesToSearch = newTilesToSearch;
                tilesFound.UnionWith(tilesToSearch);
                newTilesToSearch = new HashSet<Tile>();
            }

            return tilesFound;
        }

        //Generate a path from start to end tiles in the grid
        public List<Tile> GeneratePath(Tile startTile, Tile endTile, HashSet<Tile> tilesInRange)
        {
            if (endTile == null || startTile == null ||
                !tilesInRange.Contains(endTile)) return null;

            startTile.distanceFromStart = 0;

            List<Tile> unvisitedTiles = new List<Tile>();
            unvisitedTiles = tilesInRange.ToList();

            while (unvisitedTiles.Count != 0)
            {
                Tile nextTile = null;

                foreach (Tile tile in unvisitedTiles)
                {
                    if (nextTile == null || tile.distanceFromStart < nextTile.distanceFromStart)
                    {
                        nextTile = tile;
                    }
                }

                if (nextTile == endTile) break;

                unvisitedTiles.Remove(nextTile);

                foreach (Tile neighbour in nextTile.neighbours)
                {
                    if (neighbour == null) continue;

                    int newDistance = nextTile.distanceFromStart + neighbour.movementCost;

                    if (newDistance < neighbour.distanceFromStart)
                    {
                        neighbour.distanceFromStart = newDistance;
                        neighbour.previusTile = nextTile;
                    }
                }
            }

            List<Tile> newPath = new List<Tile>();
            Tile currentTile = endTile;
            while (currentTile != null)
            {
                newPath.Add(currentTile);
                currentTile = currentTile.previusTile;
            }

            newPath.Reverse();
            return newPath;
        }

        //Reset all tiles in the grid
        public void CleanTiles()
        {
            OnCleanTiles?.Invoke();
        }

        private void Awake()
        {
            var creator = FindObjectOfType<GridCreator>();
            _Tiles = creator.CreateGrid();
        }
    }
}