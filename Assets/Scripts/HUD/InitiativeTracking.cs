using Tactics.Managment;
using Tactics.Core;
using UnityEngine;

namespace Tactics.HUD
{
    public class InitiativeTracking : MonoBehaviour
    {
        [SerializeField] Transform _sliderContainer;
        [SerializeField] InitiativeSlider _sliderPrefab;

        [SerializeField] GameManager _gameManager;

        void Awake()
        {
            _gameManager.OnUnitSpawn += SlideSpawn;
        }

        public void SlideSpawn(Unit newUnit)
        {
            var newSlider = Instantiate(_sliderPrefab, _sliderContainer);
            newSlider.Bind(newUnit);
        }
    }
}