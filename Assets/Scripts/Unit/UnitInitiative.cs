using System;
using UnityEngine;

namespace Tactics.Units
{
    public class UnitInitiative : MonoBehaviour
    {
        public float ammount;
        [SerializeField] int _speed;

        public Action<float> OnInitiativeChange;

        public void Advance()
        {
            ammount += _speed;
            OnInitiativeChange?.Invoke(ammount);
        }

        public void RollBack(int ammountUsed)
        {
            ammount -= ammountUsed;
            OnInitiativeChange?.Invoke(ammount);
        }
    }
}