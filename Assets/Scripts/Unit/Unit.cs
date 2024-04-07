using Brisanti.Tactics.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Brisanti.Tactics.Core
{
    public class Unit : MonoBehaviour
    {
        [Header("Unit Stats")]
        public int team;
        [SerializeField] int movement;
        [HideInInspector] public int movementLeft;
        public bool hasAttacked = false;

        [Header("References")]
        public UnitHealth health;
        public UnitAttack attack;
        [SerializeField] UnitWalker _walker;
        [SerializeField] UnitAnimations _animation;
        [HideInInspector] public Tile ocupedTile;

        public Action<Unit> OnActionEnd;

        public void Attack(Unit target)
        {
            if (hasAttacked) return;
            hasAttacked = true;

            var targetPosition =  target.transform.position;
            _walker.RotateTowards(targetPosition);

            attack.OnAttackHit += ActionEnd;
            attack.AimAt(target);
        }

        public void MoveUnit(List<Tile> path)
        {
            Tile endOfPathTile = path.Last();

            ocupedTile.unitOnTile = null;
            ocupedTile = endOfPathTile;
            ocupedTile.unitOnTile = this;

            movementLeft -= endOfPathTile.distanceFromStart;
            _walker.OnStopMoving += ActionEnd;
            _walker.SetNewPath(path);
        }

        public void ActionEnd()
        {
            OnActionEnd?.Invoke(this);
        }

        public void RestoreUnit()
        {
            movementLeft = movement;
            hasAttacked = false;
        }

        public void Start()
        {
            ocupedTile = GameObject.Find("Tile: 0-0").GetComponent<Tile>();
            RestoreUnit();
        }
    }
}