using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Stats")]
    public int team;
    [SerializeField] int movement;
    public int movementLeft;
    private int currentHealth;
    [SerializeField] int health;
    public int attackRange;
    [SerializeField] int attackDamage;
    public bool hasAttacked = false;

    [Header("References")]
    [SerializeField] UnitAnimations _animation;
    [SerializeField] Transform _graffics;
    [HideInInspector] public Tile ocupedTile;

    public Action<Unit> OnActionEnd;

    public void Attack(Unit target)
    {
        hasAttacked = true;

        StartCoroutine(RotateTowards(target.transform.position));
        _animation.Attack();

        target.Damage(attackDamage);
        OnActionEnd?.Invoke(this);
    }

    public bool Damage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{currentHealth}/{health}");

        if (currentHealth <= 0)
        {
            Death();
            return true;
        }
        else
        {
            _animation.Hit();
            return false;
        }
    }

    private void Death()
    {
        _animation.Die();
    }

    //Recieve a new path
    public void MoveUnit(List<Tile> path)
    {
        Tile endOfPathTile = path.Last(); 

        ocupedTile.unitOnTile = null;
        ocupedTile = endOfPathTile;
        ocupedTile.unitOnTile = this;

        movementLeft -= endOfPathTile.distanceFromStart;
        StartCoroutine(MoveThroughPath(path));
    }

    //Move unit through each tile of the path
    private IEnumerator MoveThroughPath(List<Tile> path)
    {
        _animation.Run();

        while(path.Count > 1)
        {
            Vector3 startPos = path[0].transform.position;
            Vector3 endPos = path[1].transform.position;
            float lerpTime = 0;

            StartCoroutine(RotateTowards(endPos));

            while (lerpTime < 1)
            {
                lerpTime += 0.05f;
                transform.position = Vector3.Lerp(startPos, endPos, lerpTime);
                yield return new WaitForFixedUpdate();
            }

            path.RemoveAt(0);
        }

        ocupedTile = path[0];
        transform.position = ocupedTile.transform.position;
        OnActionEnd?.Invoke(this);
        _animation.Idle();
    }

    //Rotate unit towards direction it's moving to
    private IEnumerator RotateTowards(Vector3 target)
    {
        Vector3 dist = target - transform.position;
        Quaternion startRotation = _graffics.rotation;
        Quaternion desiredRotation = Quaternion.LookRotation(dist);

        if (startRotation == desiredRotation) yield break;

        float lerpTime = 0;

        while (lerpTime < 1)
        {
            lerpTime += 0.1f;
            _graffics.rotation = Quaternion.Lerp(startRotation, desiredRotation, lerpTime);
            yield return new WaitForFixedUpdate();
        }
    }

    public void RestoreMovement()
    {
        movementLeft = movement;
        hasAttacked = false;
    }

    public void Start()
    {
        ocupedTile = GameObject.Find("Tile: 0-0").GetComponent<Tile>();
        currentHealth = health;
        RestoreMovement();
    }
}
