using System;
using Tactics.Commands;
using UnityEngine;
using UnityEngine.UI;

namespace Tactics.SkillBar
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField] Button _button;
        [SerializeField] Image _image;

        public Action<Command> OnButtonClick;
        private Command _myCommand;

        public void SetupButton(Command newCommand)
        {
            _myCommand = newCommand;
            _button.interactable = true;
            _image.sprite = newCommand.icon;

            gameObject.SetActive(true);
        }

        public void EnableDisable(bool status)
        {
            _button.interactable = status;
        }

        public void SendCommand()
        {
            OnButtonClick?.Invoke(_myCommand);
        }

        private void Start()
        {
            _button.onClick.AddListener(SendCommand);
        }
    }
}