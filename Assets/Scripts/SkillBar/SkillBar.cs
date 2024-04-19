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
        [SerializeField] CommandManager _commander;

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
            _commander.VisualizeCommand(command);

            _skillBar.SetActive(false);
            _cancelBar.SetActive(true);
        }

        public void UnprepareCommand()
        {
            var command = _commander.UnprepareCommand();

            int i = _skills.IndexOf(command);
            _buttons[i].EnableDisable(true);

            CheckUnprepare();
        }

        public void ExecuteCommands()
        {
            _skillBar.SetActive(false);
            _commander.ExecuteCommands();
        }

        public void CancelCommand()
        {
            _commander.CancelCommand();

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
            int count = _commander.commandCount;
            bool canUnprepare = count > 0;
            _unprepareButton.interactable = canUnprepare;
        }

        private void Awake()
        {
            _turnManager.OnTurnStart += SetupBar;
            _commander.CommandPrepared += OnCommandPrepared;

            foreach(var button in _buttons)
            {
                button.OnButtonClick += SendCommand;
            }
        }
    }
}