using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int movement;
    public Tile tile;
    public GridBehaviour grid;

    public void SelectUnit()
    {
        grid.HighlightRange(this);
    }

    public void MoveUnit(List<Tile> newPath)
    {
        transform.position = tile.transform.position;
    }

    public void Start()
    {
        tile = GameObject.Find("Tile: 0-0").GetComponent<Tile>();
    }
}
