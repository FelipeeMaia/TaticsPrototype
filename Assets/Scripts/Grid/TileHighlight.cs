using Tactics.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Tiles
{
    public class TileHighlight : MonoBehaviour
    {
        [SerializeField] GameObject _movementHighlight;
        [SerializeField] GameObject _attackHighlight;
        [SerializeField] GameObject _mouseHighlight;

        //0 - Right, 1 - Left, 2 - Up, 3 - Down
        [SerializeField] GameObject[] _rangeHighlights;
        [SerializeField] GameObject[] _pathHighlights;

        private void OnMouseEnter()
        {
            _mouseHighlight.SetActive(true);
        }

        private void OnMouseExit()
        {
            _mouseHighlight.SetActive(false);
        }

        public void Movement()
        {
            _movementHighlight.SetActive(true);
        }

        public void Attack()
        {
            _movementHighlight.SetActive(false);
            _attackHighlight.SetActive(true);
        }

        public void Range(HashSet<Tile> tilesInRange, List<Tile> neighbours)
        {
            for (int i = 0; i < 4; i++)
            {
                bool activeBorder;

                activeBorder = (neighbours[i] != null &&
                    tilesInRange.Contains(neighbours[i]));

                _rangeHighlights[i].SetActive(!activeBorder);
            }
        }

        public void Path(List<Tile> path, List<Tile> neighbours, bool arrowHead)
        {
            for (int i = 0; i < 4; i++)
            {
                bool pathOn;

                pathOn = (neighbours[i] != null &&
                    path.Contains(neighbours[i]));

                int index = arrowHead ? i + 4 : i;
                _pathHighlights[index].SetActive(pathOn);
            }
        }

        public void CleanAttack()
        {
            _attackHighlight.SetActive(false);

            foreach (GameObject side in _rangeHighlights)
            {
                side.SetActive(false);
            }
        }

        public void CleanMovement()
        {
            _movementHighlight.SetActive(false);

            foreach (GameObject side in _pathHighlights)
            {
                side.SetActive(false);
            }
        }

        public void Clean(int id = 3)
        {
            if(id != 0)
                CleanAttack();

            if(id != 1)
            CleanMovement();
        }
    }
}