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

    private Tile[,] _Grid;
    private HashSet<Tile> _tilesInUnitRange;

    private Action OnResetTiles;

    //Get Tiles in range form an initial tile
    public void HighlightRange(Unit unit)
    {
        OnResetTiles?.Invoke();

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

        _tilesInUnitRange = tilesFound;
    }

    private void GeneratePath(int targetX, int targetY)
    {
        Tile startTile = _Grid[sourceX, sourceY];
        Tile endTile = _Grid[targetX, targetY];

        if (endTile == null || startTile == null || 
            !_tilesInUnitRange.Contains(endTile)) return;

        OnResetTiles?.Invoke();
        startTile.distanceFromStart = 0;

        List<Tile> unvisitedTiles = new List<Tile>();
        unvisitedTiles = _tilesInUnitRange.ToList();

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
    }

    private void Awake()
    {
        var creator = FindObjectOfType<GridCreator>();
        _Grid = creator.CreateGrid();
    }

    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            //HighlightRange();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GeneratePath(targetX, targetY);
        }

    }*/
}
