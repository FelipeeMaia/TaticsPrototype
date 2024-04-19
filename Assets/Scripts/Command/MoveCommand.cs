using Tactics.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Commands
{
    [CreateAssetMenu(fileName = "newMovement", menuName = "Commands/moveComand", order = 2)]
    public class MoveCommand : Command
    {
        GridBehaviour _grid;
        List<Tile> _movementPath;

        public override void Visualize(Unit unit, Tile expectedPosition, GridBehaviour grid)
        {
            _grid = grid;
            range = unit.movementLeft;
            _highlightID = 0;

            base.Visualize(unit, expectedPosition, grid);
        }

        protected override void VisualizeHighlight(Tile tile)
        {
            if (tile == _myUnit.ocupedTile) return;
            tile.highlight.Movement();
        }

        public override bool Prepare(Tile selectedTile, out Tile expectedPosition)
        {
            expectedPosition = null;
            if (!base.Prepare(selectedTile, out expectedPosition)) return false;

            Tile startTile = _myUnit.ocupedTile;
            _movementPath = _grid.GeneratePath(startTile, selectedTile, _tilesInRange);

            foreach (Tile tile in _movementPath)
            {
                bool last = tile == selectedTile;
                tile.highlight.Path(_movementPath, tile.neighbours, last);
                //ToDo: highlight arrowshaped
            }

            expectedPosition = selectedTile;
            return true;
        }

        public override void Unprepare(out Tile initialTile)
        {
            foreach (Tile tile in _movementPath)
            {
                tile.highlight.CleanMovement();
            }

            initialTile = _initialTile;
            _movementPath = null;
        }

        public override void Execute()
        {
            _myUnit.MoveUnit(_movementPath);
        }
    
    }
}