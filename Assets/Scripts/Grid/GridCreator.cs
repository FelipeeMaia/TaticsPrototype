using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [Header("Grid Parameters")]
    [SerializeField] GridBehaviour _gridBehaviour;
    [SerializeField] GameObject _tilePrefab;
    [SerializeField] int width, height;
    [SerializeField] float _spacing;
    private Tile[,] _Grid;

    //Creation of the Grid
    public Tile[,] CreateGrid()
    {
         Tile[,] newGrid = new Tile[width, height];

        newGrid = new Tile[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var go = Instantiate(_tilePrefab);
                go.transform.SetParent(transform);

                var tile = go.GetComponent<Tile>();
                newGrid[i, j] = tile;
                tile.SetTile(i, j, _spacing);
                _gridBehaviour.OnCleanTiles += tile.CleanUp;
            }
        }

        _Grid = newGrid;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                newGrid[i, j].neighbours = FindNeighbours(newGrid[i, j]);
            }
        }

        return newGrid;
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

    //Try to get a tile from a coordinate
    private Tile TryGetTile(int x, int y)
    {
        if (x >= width || y >= height
            || x < 0 || y < 0) return null;

        //Debug.Log($"{x}-{y}");
        return _Grid[x, y];
    }
}

