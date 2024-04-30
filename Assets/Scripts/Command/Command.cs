using Tactics.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Commands
{
    public abstract class Command : ScriptableObject
    {
        [Header("UI")]
        public Sprite icon;

        [Header("Stats")]
        public int range;

        protected HashSet<Tile> _tilesInRange;
        protected Unit _myUnit;
        protected Tile _initialTile;
        protected int _highlightID;
        protected bool _avoidUnits = true;

        public virtual void Visualize(Unit unit, Tile initialTile, GridBehaviour grid)
        {
            _myUnit = unit;
            _initialTile = initialTile;

            _tilesInRange = grid.GetTilesInRange(initialTile, range, _avoidUnits);
            foreach (Tile tile in _tilesInRange)
            {
                VisualizeHighlight(tile);
            }
        }

        protected abstract void VisualizeHighlight(Tile tile);

        public void Cancel()
        {
            foreach (Tile tile in _tilesInRange)
            {
                tile.highlight.Clean(_highlightID);
            }
        }

        public virtual bool Prepare(Tile selectedTile, out Tile expectedPosition)
        {
            bool isTargetInRange = _tilesInRange.Contains(selectedTile);

            if(isTargetInRange)
            {
                Vector3 targetPosition = selectedTile.transform.position;
                _myUnit.walker.RotateTowards(targetPosition);

                foreach (Tile tile in _tilesInRange)
                {
                    tile.highlight.Clean(_highlightID);
                }
            }

            expectedPosition = _myUnit.ocupedTile;
            return isTargetInRange;
        }

        public abstract void Unprepare(out Tile initialTile);

        public abstract void Execute();
    }
}