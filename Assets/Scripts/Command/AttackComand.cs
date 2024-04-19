using Tactics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Commands
{
    [CreateAssetMenu(fileName = "newAttack", menuName = "Commands/attackComand", order = 1)]
    public class AttackComand : Command
    {
        public int damage;
        public int area;

        protected Unit _targetUnit;

        public override void Visualize(Unit unit, Tile expectedPosition, GridBehaviour grid)
        {
            _avoidUnits = false;
            base.Visualize(unit, expectedPosition, grid);
        }

        public override bool Prepare(Tile selectedTile, out Tile expectedPosition)
        {
            expectedPosition = _myUnit.ocupedTile;

            if (!CheckForEnemy(selectedTile) ||
                !base.Prepare(selectedTile, out expectedPosition)) return false;

            _targetUnit = selectedTile.unitOnTile;
            selectedTile.highlight.Attack();
            return true;
        }

        public override void Unprepare(out Tile initialTile)
        {
            initialTile = _initialTile;
            _targetUnit.ocupedTile.highlight.Clean();

            // ToDo:
            //  - Undo highlight.
            //  - Turn ghost toward original position.
        }
        public override void Execute()
        {
            _targetUnit.ocupedTile.highlight.Clean();
            _myUnit.Attack(_targetUnit, damage);
        }

        protected override void VisualizeHighlight(Tile tile)
        {
            tile.highlight.Range(_tilesInRange, tile.neighbours);

            if(CheckForEnemy(tile))
            {
                tile.highlight.Attack();
            }
        }

        private bool CheckForEnemy(Tile tile)
        {
            bool tileHasEnemy = false;

            Unit unitFound = tile.unitOnTile;
            if (unitFound)
            {
                tileHasEnemy = _myUnit.team != unitFound.team;
            }

            return tileHasEnemy;
        }
    }
}