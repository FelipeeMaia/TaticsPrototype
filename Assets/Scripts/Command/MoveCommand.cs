using Brisanti.Tactics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brisanti.Tactics.Commands
{
    [CreateAssetMenu(fileName = "newMovement", menuName = "Commands/moveComand", order = 2)]
    public class MoveCommand : Command
    {
        public int distance;


        public override bool Prepare(Unit unit, Tile target)
        {
            return true;
        }

        public override void Unprepare()
        {
            throw new System.NotImplementedException();
        }

        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}