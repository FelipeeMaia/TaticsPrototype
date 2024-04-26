using System.Collections;
using Tactics.Constants;
using Tactics.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Tactics.HUD
{
    public class InitiativeSlider : MonoBehaviour
    {
        [SerializeField] float _lerpTime;
        [SerializeField] Slider _slider;
        [SerializeField] Image _icon;

        public void Bind(Unit unit)
        {
            //unit.initiative.OnInitiativeChange += UpdateSlider;
            unit.health.OnDeath += DestroySlider;
            _icon.sprite = unit.icon;
            _icon.color = Colors.GetTeamColor(unit.team);

            if(unit.team == 1)
            {
                Vector3 newScale = new Vector3(-1, 1, 1);
                _icon.transform.localScale = newScale;
            }
        }

        public void UpdateSlider(float newValue)
        {
            StartCoroutine(LerpSlide(_slider, newValue));
        }

        private IEnumerator LerpSlide(Slider slider, float newValue)
        {
            float startValue = slider.value;
            float time = 0;
            float value;

            while (time < 1)
            {
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime / _lerpTime;

                value = Mathf.Lerp(startValue, newValue, time);
                slider.value = value;
            }

        }

        public void DestroySlider(Unit unit)
        {
            //unit.initiative.OnInitiativeChange -= UpdateSlider;
            unit.health.OnDeath -= DestroySlider;
            Destroy(gameObject);
        }
    }
}