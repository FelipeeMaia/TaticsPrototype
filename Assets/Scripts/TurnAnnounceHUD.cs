using Brisanti.Tactics.Core;
using Brisanti.Tactics.Managment;
using System.Collections;
using TMPro;
using UnityEngine;

public class TurnAnnounceHUD : MonoBehaviour
{
    [SerializeField] TMP_Text _announcement;
    [SerializeField] CanvasGroup _canvas;
    [SerializeField] float _fadesTime;
    [SerializeField] float _waitTime;
    [SerializeField] Color[] _teamColors;
    [SerializeField] TurnManager _manager;

    public void AnnounceNewTurn(Unit unit)
    {
        _announcement.text = $"{unit.name}'s Turn";
        _announcement.color = _teamColors[unit.team];

        StartCoroutine(FadeAnnouncement());
    }

    private IEnumerator FadeAnnouncement()
    {
        _canvas.blocksRaycasts = true;
        float time = 0;

        while(time < 1)
        {
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime * _fadesTime;

            float fading = Mathf.SmoothStep(0, 1, time);
            _canvas.alpha = fading;
        }

        yield return new WaitForSeconds(_waitTime);

        while (time > 0)
        {
            yield return new WaitForFixedUpdate();
            time -= Time.fixedDeltaTime * _fadesTime;

            float fading = Mathf.SmoothStep(0, 1, time);
            _canvas.alpha = fading;
        }

        _canvas.blocksRaycasts = false;
    }


    void Start()
    {
        _manager.OnTurnStart += AnnounceNewTurn;
    }
}
