using Brisanti.Tactics.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Brisanti.Tactics.Commands
{
    [CreateAssetMenu(fileName = "newMovement", menuName = "Commands/moveComand", order = 2)]
    public class MoveCommand : Command
    {
        GridBehaviour _grid;
        List<Tile> _movementPath;

        public override void Visualize(Unit unit, GridBehaviour grid)
        {
            _grid = grid;
            range = unit.movementLeft;

            base.Visualize(unit, grid);
        }

        protected override void VisualizeHighlight(Tile tile)
        {
            if (tile == myUnit.ocupedTile) return;
            tile.highlight.Movement();
        }

        public override bool Prepare(Tile selectedTile)
        {
            if (!base.Prepare(selectedTile)) return false;

            Tile startTile = myUnit.ocupedTile;
            _movementPath = _grid.GeneratePath(startTile, selectedTile, _tilesInRange);

            foreach(Tile tile in _movementPath)
            {
                tile.highlight.Movement();
                //ToDo: highlight arrowshaped
            }
            
            return true;
        }

        public override void Unprepare()
        {
            foreach (Tile tile in _movementPath)
            {
                tile.highlight.Clean();
            }

            _movementPath = null;
        }

        public override void Execute()
        {
            myUnit.MoveUnit(_movementPath);
        }
    
    }
}