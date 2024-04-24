using System.Collections;
using System.Collections.Generic;
using Tactics.Core;
using Tactics.Managment;
using UnityEngine;
using UnityEngine.UI;

namespace Tactics.HUD
{
    public class InitiativeTrackingHUD : MonoBehaviour
    {
        [SerializeField] float _lerpTime;
        [SerializeField] Transform _sliderContainer;

        [SerializeField] GameManager _gameManager;
        [SerializeField] Slider _sliderPrefab;

        Dictionary<Unit, Slider> _SliderDictionary;

        // Start is called before the first frame update
        void Awake()
        {
            _SliderDictionary = new Dictionary<Unit, Slider>();
            _gameManager.OnUnitSpawn += UnitBind;
        }

        public void UnitBind(Unit newUnit)
        {
            Slider newSlider = Instantiate(_sliderPrefab, _sliderContainer);
            _SliderDictionary.Add(newUnit, newSlider);
            newUnit.initiative.OnInitiativeChange += UpdateSlider;
            newUnit.health.OnDeath += DestroySlider;
        }

        public void UpdateSlider(Unit unit, float newValue)
        {
            Slider slider;

            if (_SliderDictionary.TryGetValue(unit, out slider))
            {
                StartCoroutine(LerpSlide(slider, newValue));
            }
        }

        private IEnumerator LerpSlide(Slider slider, float newValue)
        {
            float startValue = slider.value;
            float time = 0;
            float value;

            while(time < 1)
            {
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime / _lerpTime;

                value = Mathf.Lerp(startValue, newValue, time);
                slider.value = value;
            }

        }

        public void DestroySlider(Unit deadUnit)
        {
            Slider deadSlider;

            if (_SliderDictionary.TryGetValue(deadUnit, out deadSlider))
            {
                _SliderDictionary.Remove(deadUnit);
                Destroy(deadSlider.gameObject);
            }
        }
    }
}