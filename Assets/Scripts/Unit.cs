using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int movement;
    public Tile tile;
    public GridBehaviour grid;

    private void OnMouseDown()
    {
        SelectUnit();
    }

    public void SelectUnit()
    {
        grid.HighlightRange(this);
    }

    public void MoveUnit(List<Tile> newPath)
    {

    }
}
