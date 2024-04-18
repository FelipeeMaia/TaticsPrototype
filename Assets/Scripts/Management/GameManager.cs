using Tactics.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Managment
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] TurnManager _turnManager;
        [SerializeField] GridBehaviour _grid;

        [SerializeField] Unit[] _unitsToSpawn;
        [SerializeField] Vector2[] _whereToSpawn;


        [SerializeField] List<Unit> _team1Units;
        [SerializeField] List<Unit> _team2Units;


        // Start is called before the first frame update
        void Start()
        {
            _team1Units = new List<Unit>();
            _team2Units = new List<Unit>();
            var allUnits = SpawnUnits();
            _turnManager.StartCombat(allUnits);
        }

        private List<Unit> SpawnUnits()
        {
            var unitsSpawn = new List<Unit>();

            for (int i = 0; i < _unitsToSpawn.Length; i++)
            {
                var newUnit = Instantiate(_unitsToSpawn[i]);

                int x = (int)_whereToSpawn[i].x;
                int y = (int)_whereToSpawn[i].y;

                Tile spawnTile = _grid.GetTile(x, y);
                newUnit.SetupUnit(spawnTile);

                newUnit.health.OnDeath += OnUnitDeath;

                unitsSpawn.Add(newUnit);

                if (newUnit.team == 0)
                    _team1Units.Add(newUnit);

                else if (newUnit.team == 1)
                    _team2Units.Add(newUnit);
            }

            return unitsSpawn;
        }

        public void OnUnitDeath(Unit unit)
        {
            var teamList = unit.team == 0 ? _team1Units : _team2Units;

            teamList.Remove(unit);

            if(teamList.Count == 0)
            {
                Debug.Log($"Team {unit.team + 1} have lost all units.");
            }
        }
    }
}