using Tactics.Core;
using System;
using UnityEngine;

namespace Tactics.Units
{
    public class UnitHealth : MonoBehaviour
    {
        [SerializeField] int _currentHealth;
        [SerializeField] int _maxHealth;
        [SerializeField] Unit _parent;

        public Action<int, int> OnChangeHealth;
        public Action<Unit> OnDeath;
        public Action OnHit;

        private bool _isDead;

        public void Damage(int amount)
        {
            if (_isDead) return;

            _currentHealth -= amount;
            OnChangeHealth?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke(_parent);
                _isDead = true;
            }
            else
            {
                OnHit?.Invoke();
            }
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
            OnChangeHealth?.Invoke(_currentHealth, _maxHealth);
        }
    }
}