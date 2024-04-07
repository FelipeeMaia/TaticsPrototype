using Brisanti.Tactics.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Brisanti.Tactics.Managment
{
    public class ActionManager : MonoBehaviour
    {
        [SerializeField] GridBehaviour _grid;

        private Unit _selectedUnit;
        private HashSet<Tile> _tilesInMovementRange;
        private HashSet<Tile> _tilesInAttackRange;

        [SerializeField] bool _isWaiting = false;

        void Update()
        {
            if (_isWaiting) return;

            if (Input.GetMouseButtonDown(0))
            {
                SelectObject();
            }

            if (Input.GetKeyDown(KeyCode.P) && _selectedUnit)
            {
                _selectedUnit.RestoreUnit();
                SelectUnit(_selectedUnit);
            }
        }

        //Selects an unit and show it's action possibilities
        private void SelectUnit(Unit clickedUnit)
        {
            if (clickedUnit.team != 0) return;
            _selectedUnit = clickedUnit;

            _tilesInMovementRange = _grid.GetTilesInRange(clickedUnit.ocupedTile, clickedUnit.movementLeft);
            foreach (Tile tile in _tilesInMovementRange)
            {
                if (tile == _selectedUnit.ocupedTile) continue;
                tile.highlight.Movement();
            }

            if (_selectedUnit.hasAttacked) return;

            _tilesInAttackRange = _grid.GetTilesInRange(clickedUnit.ocupedTile, clickedUnit.attack.range, false);
            foreach (Tile tile in _tilesInAttackRange)
            {
                tile.highlight.Range(_tilesInAttackRange, tile.neighbours);
            }
        }

        //Order _selectedUnit to move along a path
        private void MoveUnitTo(Tile clickedTile)
        {
            var path = _grid.GeneratePath(_selectedUnit.ocupedTile, clickedTile, _tilesInMovementRange);
            if (path == null) return;

            _selectedUnit.MoveUnit(path);
            StartWaiting(_selectedUnit);
        }

        //Order _selectedUnit to attack an target
        private void AttackUnit(Unit target)
        {
            StartWaiting(_selectedUnit);
            _selectedUnit.Attack(target);
            //waits for action end
        }

        //Uses raycast to get info of clicked object
        private void SelectObject()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject objectFound = hit.transform.gameObject;
                if (CheckForUnit(objectFound) ||
                    CheckForTile(objectFound)) return;
            }

            _selectedUnit = null;
            _grid.CleanTiles();
        }

        //Checks if objectFound is an Unit and what to do with it 
        private bool CheckForUnit(GameObject objectFound)
        {
            var unitFound = objectFound.GetComponent<Unit>();

            if (unitFound == _selectedUnit ||
                unitFound == null) return false;

            if (_selectedUnit == null)
            {
                SelectUnit(unitFound);
            }
            else if (_selectedUnit.team != unitFound.team)
            {
                return CheckForCombat(unitFound);
            }

            return true;
        }

        //Checks if objectFound is a Tile and what to do with it 
        private bool CheckForTile(GameObject objectFound)
        {
            var tileFound = objectFound.GetComponent<Tile>();
            if (!tileFound || !_selectedUnit) return false;

            if (_tilesInMovementRange.Contains(tileFound))
            {
                MoveUnitTo(tileFound);
            }
            else if (tileFound.unitOnTile != null)
            {
                return CheckForCombat(tileFound.unitOnTile);
            }
            else
            {
                return false;
            }

            return true;
        }

        //Checks if found target is an enemy and is in range
        private bool CheckForCombat(Unit target)
        {
            if (!_tilesInAttackRange.Contains(target.ocupedTile) ||
                target.team == _selectedUnit.team ||
                _selectedUnit.hasAttacked) return false;

            AttackUnit(target);
            return true;
        }

        private void StartWaiting(Unit unit)
        {
            unit.OnActionEnd += EndWaitign;
            _grid.CleanTiles();
            _isWaiting = true;
        }

        public void EndWaitign(Unit unit)
        {
            unit.OnActionEnd -= EndWaitign;
            _isWaiting = false;
            SelectUnit(unit);
        }
    }
}