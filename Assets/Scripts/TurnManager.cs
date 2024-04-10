using Brisanti.Tactics.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brisanti.Tactics.Managment
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] ActionManager _actionManager;
        [SerializeField] List<Unit> _allUnits;
        [SerializeField] List<Unit> _nextInTurn;
        [SerializeField] int _initiativeNeed;

        private Unit _unitsTurn;
        public Action<Unit> OnTurnStart;

        private void TurnStart(Unit unit)
        {
            OnTurnStart?.Invoke(unit);
            _unitsTurn = unit;

            unit.RestoreUnit();
            _actionManager.SelectUnit(unit);
        }

        private void TurnEnd()
        {
            _unitsTurn.initiative -= _initiativeNeed;
            RollQueue();
        }

        private void RollQueue()
        {
            if(_nextInTurn.Count > 0)
            {
                Unit nextUnit = _nextInTurn[0];
                _nextInTurn.RemoveAt(0);
                TurnStart(nextUnit);
            }
            else
            {
                RunIniciative();
            }
        }

        public void RunIniciative()
        {
            //ToDo:
            //use inumerator to put a wait between cicles to animate UI

            while (_nextInTurn.Count == 0)
            {
                foreach (var unit in _allUnits)
                {
                    unit.initiative += unit.speed;
                    if (unit.initiative >= _initiativeNeed)
                    {
                        _nextInTurn.Add(unit);
                    }
                }
            }

            _nextInTurn.OrderBy(unit => unit.initiative);
            RollQueue();
        }

        public void OnUnitDeath(Unit unit)
        {
            _allUnits.Remove(unit);
            _nextInTurn.Remove(unit);
        }

        private void Start()
        {
            _nextInTurn = new List<Unit>();
            _allUnits = FindObjectsOfType<Unit>().ToList();
            RunIniciative();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                TurnEnd();
            }
        }

    }
}