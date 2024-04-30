using Tactics.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tactics.Managment
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] float _endTurnTime;

        public List<Unit> allUnits;
        [SerializeField] List<Unit> _nextInTurn;
        [SerializeField] int _initiativeNeed;
        private Dictionary<Unit, int> _Initiatives;

        public Action<Unit> OnTurnStart;
        public Action<Unit> OnTurnStacked;
        public Action OnTurnEnd;

        private void StartTurn(Unit unit)
        {
            OnTurnStart?.Invoke(unit);
        }

        public void EndTurn()
        {
            OnTurnEnd?.Invoke();
            Invoke("PickNext", _endTurnTime);
        }

        private void PickNext()
        {
            if(_nextInTurn.Count >= 8)
            {
                Unit nextUnit = _nextInTurn[0];
                _nextInTurn.RemoveAt(0);
                StartTurn(nextUnit);
            }
            else
            {
                RunIniciative();
            }
        }

        public void RunIniciative()
        {
            while(_nextInTurn.Count < 8)
            {
                foreach (var unit in allUnits)
                {
                    int initiative = _Initiatives[unit];
                    initiative += unit.speed;

                    if (initiative >= _initiativeNeed)
                    {
                        _nextInTurn.Add(unit);
                        OnTurnStacked?.Invoke(unit);
                        initiative -= _initiativeNeed;
                    }

                    _Initiatives[unit] = initiative;
                }
            }

            PickNext();
        }

        public void OnUnitDeath(Unit deadUnit)
        {
            allUnits.RemoveAll(u => u == deadUnit);
            _nextInTurn.RemoveAll(u => u == deadUnit);
        }

        public void StartTurns(List<Unit> allUnits)
        {
            _Initiatives = new Dictionary<Unit, int>();
            _nextInTurn = new List<Unit>();
            this.allUnits = allUnits;

            foreach (Unit unit in this.allUnits)
            {
                unit.health.OnDeath += OnUnitDeath;
                _Initiatives[unit] = 0;
            }

            RunIniciative();
        }
    }
}