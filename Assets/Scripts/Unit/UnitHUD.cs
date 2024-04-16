using Brisanti.Tactics.Core;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Brisanti.Tactics.Units.HUD
{
    public class UnitHUD : MonoBehaviour
    {
        [SerializeField] TMP_Text _nameHolder;
        [SerializeField] TMP_Text _healthCounter;
        [SerializeField] Image _healthBar;
        [SerializeField] Unit unit;
        [SerializeField] Canvas _canvas;

        private float oldAmount = 1;
        private Coroutine _coroutine;



        // Start is called before the first frame update
        void Awake()
        {
            _canvas.worldCamera = Camera.main;
            _nameHolder.text = unit.name;

            unit.health.OnChangeHealth += UpdateHealth;
            unit.health.OnDeath += DisableHUD;
        }

        public void UpdateHealth(int current, int max)
        {
            _healthCounter.text = $"{current} / {max}";
            float newAmount = (float)current / max;

            if(_coroutine != null) StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(BarSmooth(oldAmount, newAmount));
            oldAmount = newAmount;
        }

        private IEnumerator BarSmooth(float oldValue, float newValue)
        {
            float time = 0;
            float fillAmount = 0;

            while (time < 1)
            {
                time += Time.fixedDeltaTime * 2;
                fillAmount = Mathf.Lerp(oldValue, newValue, time);
                _healthBar.fillAmount = fillAmount;

                yield return new WaitForFixedUpdate();
            }

        }


        public void DisableHUD(Unit unit)
        {
            gameObject.SetActive(false);
        }
    }
}