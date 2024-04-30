using System.Collections;
using Tactics.Constants;
using Tactics.Managment;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] TMP_Text _winnerText;
    [SerializeField] float _fadeTime;
    [SerializeField] float _waitTime;
    [SerializeField] CanvasGroup _canvas;
    [SerializeField] GameManager _manager;

    public void OnGameEnded(int winnerTeam)
    {
        string teamName = Strings.GetTeamName(winnerTeam);
        _winnerText.text = $"{teamName}'s Victory";

        Color teamColor = Colors.GetTeamColor(winnerTeam);
        _winnerText.color = teamColor;

        _canvas.blocksRaycasts = true;
        _canvas.interactable = true;

        StartCoroutine(FadeIn());
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(_waitTime);

        float time = 0;

        while (time < 1)
        {
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime / _fadeTime;

            float fading = Mathf.SmoothStep(0, 1, time);
            _canvas.alpha = fading;
        }       
    }

    private void Awake()
    {
        _manager.OnGameEnd += OnGameEnded;
    }
}
