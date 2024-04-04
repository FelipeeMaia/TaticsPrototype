using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    [Header("Testing")]
    //[SerializeField] int movement;
    [SerializeField] int sourceX;
    [SerializeField] int sourceY;
    [SerializeField] int targetX;
    [SerializeField] int targetY;

    private Tile[,] _Tiles;
    public Action OnCleanTiles;

    //Get Tiles in range form an initial tile
    public HashSet<Tile> HighlightRange(Unit unit)
    {
        //Set groups
        HashSet<Tile> tilesToSearch = new HashSet<Tile>();
        HashSet<Tile> newTilesToSearch = new HashSet<Tile>();
        HashSet<Tile> tilesFound = new HashSet<Tile>();

        //Set initial tile
        //Tile initialTile = _Tiles[sourceX, sourceY];
        Tile initialTile = unit.tile;
        initialTile.movementNeed = 0;
        tilesToSearch.Add(initialTile);
        tilesFound.Add(initialTile);
        int movement = unit.movement;

        while(tilesToSearch.Count != 0)
        {
            foreach(Tile tile in tilesToSearch)
            {
                //List<Tile> neighbours = FindNeighbours(tile);

                foreach(Tile neighbour in tile.neighbours)
                {
                    if (tilesFound.Contains(neighbour)) continue;

                    int cost = tile.movementNeed + neighbour.movementCost;
                    if (movement >= cost)
                    {
                        neighbour.movementNeed = cost;
                        newTilesToSearch.Add(neighbour);
                        neighbour.Highlight();
                    }
                }
            }

            tilesToSearch = newTilesToSearch;
            tilesFound.UnionWith(tilesToSearch);
            newTilesToSearch = new HashSet<Tile>();
        }

        return tilesFound;
    }

    public List<Tile> GeneratePath(Tile startTile, Tile endTile, HashSet<Tile> tilesInRange)
    {
        if (endTile == null || startTile == null || 
            !tilesInRange.Contains(endTile)) return null;

        startTile.distanceFromStart = 0;

        List<Tile> unvisitedTiles = new List<Tile>();
        unvisitedTiles = tilesInRange.ToList();

        while(unvisitedTiles.Count != 0)
        {
            Tile nextTile = null;

            foreach(Tile tile in unvisitedTiles)
            {
                if(nextTile == null || tile.distanceFromStart < nextTile.distanceFromStart)
                {
                    nextTile = tile;
                }
            }

            if (nextTile == endTile) break;

            unvisitedTiles.Remove(nextTile);

            foreach(Tile neighbour in nextTile.neighbours)
            {
                float newDistance = nextTile.distanceFromStart + neighbour.movementCost;

                if(newDistance < neighbour.distanceFromStart)
                {
                    neighbour.distanceFromStart = newDistance;
                    neighbour.previusTile = nextTile;
                }
            }
        }

        List<Tile> newPath = new List<Tile>();
        Tile currentTile = endTile;
        while(currentTile != null)
        {
            currentTile.Highlight();
            newPath.Add(currentTile);
            currentTile = currentTile.previusTile;
        }

        newPath.Reverse();
        return newPath;
    }

    private void Awake()
    {
        var creator = FindObjectOfType<GridCreator>();
        _Tiles = creator.CreateGrid();
    }

    public void CleanTiles()
    {
        OnCleanTiles?.Invoke();
    }
}
