using Tactics.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Tiles
{
    public class TileHighlight : MonoBehaviour
    {
        [SerializeField] GameObject _movementHighlight;
        [SerializeField] GameObject _attackHighlight;
        [SerializeField] GameObject[] _rangeHighlights;


        public void Movement()
        {
            _movementHighlight.SetActive(true);
        }

        public void Attack()
        {
            _movementHighlight.SetActive(false);
            _attackHighlight.SetActive(true);
        }

        public void Range(HashSet<Tile> _tilesInRange, List<Tile> neighbours)
        {
            for (int i = 0; i < 4; i++)
            {
                bool isLimitOfRange;

                isLimitOfRange = (neighbours[i] != null &&
                    _tilesInRange.Contains(neighbours[i]));

                _rangeHighlights[i].SetActive(!isLimitOfRange);
            }
        }

        public void Clean()
        {
            _movementHighlight.SetActive(false);
            _attackHighlight.SetActive(false);
            foreach (GameObject side in _rangeHighlights)
            {
                side.SetActive(false);
            }
        }
    }
}