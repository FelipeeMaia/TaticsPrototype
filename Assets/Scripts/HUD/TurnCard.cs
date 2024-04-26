using System.Collections;
using Tactics.Constants;
using Tactics.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tactics.HUD
{
    public class TurnCard : MonoBehaviour
    {
        public Unit unit;

        [SerializeField] Image _card;
        [SerializeField] Image _icon;
        [SerializeField] TMP_Text _name;
        [SerializeField] RectTransform _transform;
        public float lerpDuration;


        public void Link(Unit unit, int index)
        {
            var teamColor = Colors.GetTeamColor(unit.team);

            _card.color = teamColor;
            _icon.sprite = unit.icon;
            _name.text = unit.uName;
            this.unit = unit;

            SetEndPosition();
            gameObject.SetActive(true);
            ChangePosition(index);
        }

        public void Unlink()
        {
            float newX = _transform.rect.width * 3;
            Vector3 newPosition = new Vector3
                (newX, transform.localPosition.y);

            StartCoroutine(_ChangePosition(transform.localPosition, newPosition));
            Invoke("Deactivate", lerpDuration);
        }

        public void ChangePosition(int to)
        {
            float newY = (_transform.rect.height + 5) * (to - 4);

            Vector3 fromPos = transform.localPosition;
            Vector3 toPos = new Vector3(0, -newY);

            StartCoroutine(_ChangePosition(fromPos, toPos));
        }

        private IEnumerator _ChangePosition(Vector3 from, Vector3 to)
        {
            float lerpTime = 0;

            while (lerpTime < 1)
            {
                lerpTime += Time.fixedDeltaTime / lerpDuration;
                transform.localPosition = Vector3.Lerp(from, to, lerpTime);
                yield return new WaitForFixedUpdate();
            }
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);

            SetEndPosition();
        }

        private void SetEndPosition()
        {
            float newY = (_transform.rect.height + 5) * 9;
            Vector3 newPos = new Vector3(0, -newY);

            transform.localPosition = newPos;
        }
    }
}
