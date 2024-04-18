using Brisanti.Tactics.Units;
using System.Collections.Generic;
using Brisanti.Tactics.Commands;
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
        public int speed;
        public int initiative;
        public string uName;
        public Command Commands;

        [Header("References")]
        public UnitHealth health;
        public UnitAttack attack;
        [SerializeField] public UnitWalker walker;
        [SerializeField] UnitAnimations _animation;
        [HideInInspector] public Tile ocupedTile;

        public Action<Unit> OnActionEnd;

        public void Attack(Unit target, int damage)
        {
            if (hasAttacked) return;
            hasAttacked = true;

            var targetPosition =  target.transform.position;
            walker.RotateTowards(targetPosition);

            attack.OnAttackHit += ActionEnd;
            attack.AimAt(target, damage);
        }

        public void MoveUnit(List<Tile> path)
        {
            Tile endOfPathTile = path.Last();

            ocupedTile.unitOnTile = null;
            ocupedTile = endOfPathTile;
            ocupedTile.unitOnTile = this;

            movementLeft -= endOfPathTile.distanceFromStart;
            walker.OnStopMoving += ActionEnd;
            walker.SetNewPath(path);
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

        public void SetupUnit(Tile startTile)
        {
            RestoreUnit();

            ocupedTile = startTile;
            ocupedTile.unitOnTile = this;
            transform.position = startTile.transform.position;
        }
    }
}