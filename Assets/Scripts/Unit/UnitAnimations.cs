using UnityEngine;

namespace Brisanti.Tactics.Units
{
    public class UnitAnimations : MonoBehaviour
    {
        [Header("Unit References")]
        [SerializeField] UnitHealth _health;
        [SerializeField] UnitAttack _attack;
        [SerializeField] UnitWalker _walker;

        [Header("Animation Things")]
        [SerializeField] int numberOfAttacks;
        [SerializeField] Animator _anim;

        public void Idle()
        {
            _anim.SetTrigger("Idle");
        }

        public void Run()
        {
            _anim.SetTrigger("Run");
        }

        public void GetHit()
        {
            _anim.SetTrigger("Hit");
        }

        public void Die()
        {
            _anim.SetTrigger("Die");
        }

        public void Attack()
        {
            int randomAttack = Random.Range(0, numberOfAttacks);
            Debug.Log(randomAttack);
            _anim.SetInteger("Randomizer", randomAttack);
            _anim.SetTrigger("Attack");
        }

        private void Start()
        {
            _health.OnHit += GetHit;
            _health.OnDeath += Die;
            _attack.OnAttack += Attack;
            _walker.OnStartMoving += Run;
            _walker.OnStopMoving += Idle;
        }

    }
}