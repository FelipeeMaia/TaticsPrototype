using System;
using Tactics.Core;
using UnityEngine;

namespace Tactics.Units
{
    public class UnitInitiative : MonoBehaviour
    {
        public float ammount;
        [SerializeField] int _speed;
        [SerializeField] Unit _parent;

        public Action<Unit, float> OnInitiativeChange;

        public void Advance()
        {
            ammount += _speed;
            OnInitiativeChange?.Invoke(_parent, ammount);
        }

        public void RollBack(int initiativeNedded)
        {
            ammount -= initiativeNedded;
            OnInitiativeChange?.Invoke(_parent, ammount);
        }
    }
}