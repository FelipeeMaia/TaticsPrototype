using Tactics.Units;
using System.Collections.Generic;
using Tactics.Commands;
using System.Linq;
using UnityEngine;
using System;

namespace Tactics.Core
{
    public class Unit : MonoBehaviour
    {
        [Header("Unit Stats")]
        public string uName;
        public int team;
        public List<Command> Commands;

        [Header("References")]
        public UnitHealth health;
        public UnitAttack attack;
        public UnitInitiative initiative;
        [SerializeField] public UnitWalker walker;
        [SerializeField] UnitAnimations _animation;
        [HideInInspector] public Tile ocupedTile;

        public Action<Unit> OnActionEnd;

        public void Attack(Unit target, int damage)
        {
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

            walker.OnStopMoving += ActionEnd;
            walker.SetNewPath(path);
        }

        public void ActionEnd()
        {
            OnActionEnd?.Invoke(this);
        }

        public void SetupUnit(Tile startTile)
        {
            ocupedTile = startTile;
            ocupedTile.unitOnTile = this;
            transform.position = startTile.transform.position;
        }
    }
}