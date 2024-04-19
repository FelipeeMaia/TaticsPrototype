using System.Collections;
using System.Collections.Generic;
using Tactics.Commands;
using Tactics.Core;
using Tactics.Managment;
using UnityEngine;
using UnityEngine.UI;

namespace Tactics.SkillBar
{
    public class SkillBar : MonoBehaviour
    {
        [SerializeField] SkillButton[] _buttons;
        [SerializeField] List<Command> _skills;
        [SerializeField] GameObject _skillBar;
        [SerializeField] GameObject _cancelBar;
        [SerializeField] Button  _unprepareButton;

        private const int buttonCount = 3;

        [SerializeField] TurnManager _turnManager;
        [SerializeField] CommandManager _commandManager;

        public void SetupBar(Unit unit)
        {
            _skills = unit.Commands;

            for(int i = 0; i < buttonCount; i++)
            {
                if(i < _skills.Count)
                {
                    _buttons[i].SetupButton(_skills[i]);
                }
                else
                {
                    _buttons[i].gameObject.SetActive(false);
                }
            }

            _unprepareButton.interactable = false;
            _skillBar.SetActive(true);
        }

        public void SendCommand(Command command)
        {
            _commandManager.VisualizeCommand(command);

            _skillBar.SetActive(false);
            _cancelBar.SetActive(true);
        }

        public void UnprepareCommand()
        {
            var command = _commandManager.UnprepareCommand();

            int i = _skills.IndexOf(command);
            _buttons[i].EnableDisable(true);

            CheckUnprepare();
        }

        public void ExecuteCommands()
        {
            _skillBar.SetActive(false);
            _commandManager.ExecuteCommands();
        }

        public void CancelCommand()
        {
            _commandManager.CancelCommand();

            _cancelBar.SetActive(false);
            _skillBar.SetActive(true);
        }

        public void OnCommandPrepared(Command command)
        {
            int i = _skills.IndexOf(command);
            _buttons[i].EnableDisable(false);
            CheckUnprepare();

            _cancelBar.SetActive(false);
            _skillBar.SetActive(true);
        }

        private void CheckUnprepare()
        {
            int count = _commandManager.commandCount;
            bool canUnprepare = count > 0;
            _unprepareButton.interactable = canUnprepare;
        }

        private void Awake()
        {
            _turnManager.OnTurnStart += SetupBar;
            _commandManager.CommandPrepared += OnCommandPrepared;

            foreach(var button in _buttons)
            {
                button.OnButtonClick += SendCommand;
            }
        }
    }
}