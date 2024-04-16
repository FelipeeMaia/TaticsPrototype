using Brisanti.Tactics.Core;
using UnityEngine;

namespace Brisanti.Tactics.Commands
{
    public abstract class Command : ScriptableObject
    {
        [Header("UI")]
        public Sprite icon;
        public int highlightID;

        [Header("Stats")]
        public int range;



        protected Unit myUnit;

        public abstract bool Prepare(Unit unit, Tile target);
        public abstract void Unprepare();
        public abstract void Execute();
    }
}