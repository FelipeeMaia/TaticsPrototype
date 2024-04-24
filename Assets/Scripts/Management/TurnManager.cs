using Tactics.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tactics.Managment
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] float _initiativeTime;
        [SerializeField] float _endTurnTime;

        [SerializeField] List<Unit> _allUnits;
        [SerializeField] List<Unit> _nextInTurn;
        [SerializeField] int _initiativeNeed;

        private Unit _selectedUnit;
        public Action<Unit> OnTurnStart;

        private void StartTurn(Unit unit)
        {
            OnTurnStart?.Invoke(unit);
            _selectedUnit = unit;
        }

        public void EndTurn()
        {
            _selectedUnit.initiative.RollBack(_initiativeNeed);
            Invoke("RollQueue", _endTurnTime);
        }

        private void RollQueue()
        {
            if(_nextInTurn.Count > 0)
            {
                Unit nextUnit = _nextInTurn[0];
                _nextInTurn.RemoveAt(0);
                StartTurn(nextUnit);
            }
            else
            {
                Invoke("RunIniciative", _initiativeTime);
            }
        }

        public void RunIniciative()
        {
            foreach (var unit in _allUnits)
            {
                unit.initiative.Advance();
                if (unit.initiative.ammount >= _initiativeNeed)
                {
                    _nextInTurn.Add(unit);
                }
            }

            if (_nextInTurn.Count != 0)
            {
                _nextInTurn.OrderBy(unit => unit.initiative);
                RollQueue();
            }
            else
            {
                Invoke("RunIniciative", _initiativeTime);
            }
        }

        public void OnUnitDeath(Unit unit)
        {
            _allUnits.Remove(unit);
            _nextInTurn.Remove(unit);
        }

        public void StartTurns(List<Unit> allUnits)
        {
            _nextInTurn = new List<Unit>();
            _allUnits = allUnits;

            foreach (Unit unit in _allUnits)
            {
                unit.health.OnDeath += OnUnitDeath;
            }

            RunIniciative();
        }
    }
}