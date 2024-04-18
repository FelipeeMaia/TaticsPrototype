using Brisanti.Tactics.Commands;
using Brisanti.Tactics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brisanti.Tactics.Commands
{
    [CreateAssetMenu(fileName = "newAttack", menuName = "Commands/attackComand", order = 1)]
    public class AttackComand : Command
    {
        public int damage;
        public int area;

        protected Unit targetUnit;


        public override bool Prepare(Tile selectedTile)
        {
            if (!base.Prepare(selectedTile) ||
                !CheckForEnemy(selectedTile)) return false;

            selectedTile.highlight.Attack();
            return true;
        }

        public override void Unprepare()
        {
            targetUnit.ocupedTile.highlight.Clean();

            // ToDo:
            //  - Undo highlight.
            //  - Turn ghost toward original position.
        }
        public override void Execute()
        {
            targetUnit.ocupedTile.highlight.Clean();
            myUnit.Attack(targetUnit, damage);
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
                tileHasEnemy = myUnit.team != unitFound.team;
            }

            return tileHasEnemy;
        }
    }
}