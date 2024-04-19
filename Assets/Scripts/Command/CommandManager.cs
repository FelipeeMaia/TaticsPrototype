using System.Collections.Generic;
using Tactics.Managment;
using Tactics.Core;
using System.Linq;
using UnityEngine;
using System;

namespace Tactics.Commands
{
    public class CommandManager : MonoBehaviour
    {
        [HideInInspector] public int commandCount;
        public Action<Command> CommandPrepared;

        [SerializeField] TurnManager _turnManager;
        [SerializeField] GridBehaviour _grid;

        List<Command> _commandList;

        Command _selectedCommand;
        Unit _selectedUnit;

        public void SelectUnit(Unit newUnit)
        {
            _selectedUnit = newUnit;
            _commandList = new List<Command>();
            commandCount = _commandList.Count;
        }

        public void VisualizeCommand(Command newCommand)
        {
            _selectedCommand = newCommand;
            newCommand.Visualize(_selectedUnit, _grid);
        }

        public void CancelCommand()
        {
            _selectedCommand.Cancel();
            _selectedCommand = null;
        }

        public void PrepareCommand(Tile selectedTile)
        {
            bool success = _selectedCommand.Prepare(selectedTile);
            if (success)
            {
                _commandList.Add(_selectedCommand);
            }

            commandCount = _commandList.Count;
            CommandPrepared?.Invoke(_selectedCommand);
        }

        public Command UnprepareCommand()
        {
            Command lastCommand = _commandList.Last();
            lastCommand.Unprepare();
            _commandList.Remove(lastCommand);

            commandCount = _commandList.Count;
            return lastCommand;
        }

        public void ExecuteCommands(Unit unit = null)
        {
            if (_commandList.Count > 0)
            {
                _selectedUnit.OnActionEnd += ExecuteCommands;

                var nextCommand = _commandList[0];
                nextCommand.Execute();

                _commandList.RemoveAt(0);
            }
            else
            {
                _turnManager.EndTurn();
            }
        }

        void Awake()
        {
            _turnManager.OnTurnStart += SelectUnit;
        }
    }
}