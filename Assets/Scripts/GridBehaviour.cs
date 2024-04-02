using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    [Header("Range Test")]
    [SerializeField] int movement;
    [SerializeField] int tileX, tileY;

    [Header("Grid Creation")]
    [SerializeField] Vector2 _gridDimension;
    [SerializeField] GameObject _tilePrefab;
    [SerializeField] float _spacing;

    Tile[,] _Tiles;

    private Action OnResetTiles;

    //Creation of the Grid
    public void SetupGrid()
    {
        _Tiles = new Tile[(int)_gridDimension.x, (int)_gridDimension.y];

        for (int i = 0; i < _gridDimension.x; i++)
        {
            for (int j = 0; j < _gridDimension.y; j++)
            {
                var go = Instantiate(_tilePrefab);
                go.transform.SetParent(transform);
                
                var tile = go.GetComponent<Tile>();
                _Tiles[i, j] = tile;
                tile.SetPosition(i, j, _spacing);
                OnResetTiles += tile.CleanUp;
            }
        }
    }

    //Try to get a tile from a coordinate
    private Tile TryGetTile(int x, int y)
    {
        if (x >= _gridDimension.x || y >= _gridDimension.y
            || x < 0 || y < 0) return null;

        return _Tiles[x, y];
    }

    //Get a Tile's neighbours
    private List<Tile> FindNeighbours(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();

        neighbours.Add(TryGetTile(tile.x + 1, tile.y));
        neighbours.Add(TryGetTile(tile.x - 1, tile.y));
        neighbours.Add(TryGetTile(tile.x, tile.y + 1));
        neighbours.Add(TryGetTile(tile.x, tile.y - 1));
        neighbours.RemoveAll(neighbor => neighbor == null);

        return neighbours;
    }

    //Get Tiles in range form an initial tile
    private void HighlightRange()
    {
        //Set groups
        HashSet<Tile> tilesToSearch = new HashSet<Tile>();
        HashSet<Tile> newTilesToSearch = new HashSet<Tile>();
        HashSet<Tile> tilesFound = new HashSet<Tile>();

        //Set initial tile
        Tile initialTile = _Tiles[tileX, tileY];
        initialTile.movementNeed = 0;
        tilesToSearch.Add(initialTile);
        tilesFound.Add(initialTile);

        while(tilesToSearch.Count != 0)
        {
            foreach(Tile tile in tilesToSearch)
            {
                List<Tile> neighbours = FindNeighbours(tile);

                foreach(Tile neighbour in neighbours)
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
    }

    private void ResetTiles()
    {
        OnResetTiles?.Invoke();
    }

    private void Start()
    {
        SetupGrid();
        FindNeighbours(_Tiles[2, 2]);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ResetTiles();
            HighlightRange();
        }
    }
}
