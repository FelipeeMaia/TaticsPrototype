using Tactics.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Commands
{
    public abstract class Command : ScriptableObject
    {
        [Header("UI")]
        public Sprite icon;
        public int highlightID;

        [Header("Stats")]
        public int range;


        protected HashSet<Tile> _tilesInRange;
        protected Unit myUnit;


        public virtual void Visualize(Unit unit, GridBehaviour grid)
        {
            myUnit = unit;

            _tilesInRange = grid.GetTilesInRange(unit.ocupedTile, range);
            foreach (Tile tile in _tilesInRange)
            {
                VisualizeHighlight(tile);
            }
        }

        protected abstract void VisualizeHighlight(Tile tile);

        public virtual bool Prepare(Tile selectedTile)
        {
            bool isTargetInRange = _tilesInRange.Contains(selectedTile);

            if(isTargetInRange)
            {
                Vector3 targetPosition = selectedTile.transform.position;
                myUnit.walker.RotateTowards(targetPosition);

                foreach (Tile tile in _tilesInRange)
                {
                    tile.highlight.Clean();
                }
            }

            return isTargetInRange;
        }

        public abstract void Unprepare();
        public abstract void Execute();
    }
}