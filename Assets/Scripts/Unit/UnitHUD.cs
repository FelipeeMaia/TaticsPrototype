using Brisanti.Tactics.Core;
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
            _healthBar.fillAmount = (float)current / max;
        }

        public void DisableHUD()
        {
            gameObject.SetActive(false);
        }
    }
}