using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    [SerializeField] GridBehaviour _grid;

    private Unit _selectedUnit;
    private HashSet<Tile> _tilesInUnitRange;
    private enum step { Selection, Move }
    [SerializeField] private step _currentStep = step.Selection;

    void Update()
    {
        GameObject clickedObject = null;

        if (Input.GetMouseButtonDown(0))
        {
            clickedObject = SelectObject();
        }

        if (clickedObject == null) return;

        if (_currentStep == step.Selection) SelectionStep(clickedObject);
        else if (_currentStep == step.Move) MoveStep(clickedObject);

    }

    private void SelectionStep(GameObject clickedObject)
    {
        var clickedUnit = clickedObject.GetComponent<Unit>();
        if (clickedUnit == null) return;

        _selectedUnit = clickedUnit;
        _tilesInUnitRange = _grid.HighlightRange(clickedUnit);

        _currentStep = step.Move;
    }

    private void MoveStep(GameObject clickedObject)
    {
        _currentStep = step.Selection;

        var clickedTile = clickedObject.GetComponent<Tile>();
        if (clickedTile == null) return;

        var path = _grid.GeneratePath(_selectedUnit.tile, clickedTile, _tilesInUnitRange);
        if (path == null) return;

        _selectedUnit.tile = clickedTile;
        _selectedUnit.MoveUnit(path);
    }


    private GameObject SelectObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            _grid.CleanTiles();
            return hit.transform.gameObject;
        }

        return null;
    }
}
