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

        protected Unit target;

        public override bool Prepare(Unit unit, Tile targetTile)
        {
            myUnit = unit;
            target = targetTile.unitOnTile;

            if (target == null || unit.team == target.team) return false;

            // ToDo:
            //  - Highlight prepared action.
            //  - Turn ghost toward target.

            return true;
        }

        public override void Unprepare()
        {
            // ToDo:
            //  - Undo highlight.
            //  - Turn ghost toward original position.
        }
        public override void Execute()
        {
            myUnit.Attack(target);
        }
    }
}