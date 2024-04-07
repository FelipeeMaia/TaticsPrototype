using Brisanti.Tactics.Core;
using System;
using UnityEngine;

namespace Brisanti.Tactics.Units
{
    public class UnitAttack : MonoBehaviour
    {
        public int range;
        public int damage;

        private Unit target;

        public Action OnAttack;
        public Action OnAttackHit;

        //Targets an unit, and start attacking
        public void AimAt(Unit newTarget)
        {
            target = newTarget;
            OnAttack?.Invoke();
        }

        //When the attack lands, deals damage an ends action
        public void Hit()
        {
            target.health.Damage(damage);
            OnAttackHit?.Invoke();
        }
    }
}