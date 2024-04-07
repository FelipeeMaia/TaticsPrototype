using System;
using UnityEngine;

namespace Brisanti.Tactics.Units
{
    public class UnitHealth : MonoBehaviour
    {
        [SerializeField] int _currentHealth;
        [SerializeField] int _maxHealth;

        public Action<int, int> OnLoseHealth;
        public Action OnDeath;
        public Action OnHit;

        private bool _isDead;

        public void Damage(int amount)
        {
            if (_isDead) return;

            _currentHealth -= amount;
            OnLoseHealth?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke();
                _isDead = true;
            }
            else
            {
                OnHit?.Invoke();
            }

            Debug.Log($"{_currentHealth}/{_maxHealth}");
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
        }
    }
}