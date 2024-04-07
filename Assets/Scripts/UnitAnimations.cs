using UnityEngine;

public class UnitAnimations : MonoBehaviour
{
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

    public void Hit()
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

}
