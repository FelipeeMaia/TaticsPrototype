using Tactics.Core;
using Tactics.Managment;
using System.Collections;
using TMPro;
using UnityEngine;
using Tactics.Constants;

namespace Tactics.HUD
{
    public class TurnAnnounceHUD : MonoBehaviour
    {
        [SerializeField] TMP_Text _announcement;
        [SerializeField] CanvasGroup _canvas;
        [SerializeField] float _fadeTime;
        [SerializeField] float _waitTime;
        [SerializeField] TurnManager _manager;

        public void AnnounceNewTurn(Unit unit)
        {
            Color teamColor = Colors.GetTeamColor(unit.team);
            string teamName = Strings.GetTeamName(unit.team);
            string announcement = $"{teamName} {unit.uName}'s Turn";

            _announcement.text = announcement;
            _announcement.color = teamColor;

            StartCoroutine(FadeAnnouncement());
        }

        private IEnumerator FadeAnnouncement()
        {
            _canvas.blocksRaycasts = true;
            float time = 0;

            while (time < 1)
            {
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime * _fadeTime;

                float fading = Mathf.SmoothStep(0, 1, time);
                _canvas.alpha = fading;
            }

            yield return new WaitForSeconds(_waitTime);

            while (time > 0)
            {
                yield return new WaitForFixedUpdate();
                time -= Time.fixedDeltaTime * _fadeTime;

                float fading = Mathf.SmoothStep(0, 1, time);
                _canvas.alpha = fading;
            }

            _canvas.blocksRaycasts = false;
        }


        void Awake()
        {
            if (_manager == null)
                _manager = FindObjectOfType<TurnManager>();

            _manager.OnTurnStart += AnnounceNewTurn;
        }
    }
}