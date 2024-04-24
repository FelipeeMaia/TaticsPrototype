using UnityEngine;

namespace Tactics.Constants
{
    public static class Colors
    {
        //Team colors
        private static Color _team1 = Color.cyan;
        private static Color _team2 = Color.magenta;

        //Lifebar colors
        private static Color[] _lifeColors = new Color[3] 
            { Color.red, Color.yellow, Color.green };


        public static Color GetTeamColor(int team)
        {
            if (team > 1) Debug.LogError("Time maior do que esperado");
            return team switch
            {
                0 => _team1,
                1 => _team2,
                _ => Color.black
            };
        }

        public static Color GetLifeColor(float lifePercent)
        {
            if (lifePercent == 1) return _lifeColors[2];

            float scaledPercent = lifePercent * (_lifeColors.Length - 1);
            Color firstColor = _lifeColors[(int)scaledPercent];
            Color lastColor = _lifeColors[(int)scaledPercent + 1];

            float percent = scaledPercent - Mathf.Floor(scaledPercent);
            Color lifeColor = Color.Lerp(firstColor, lastColor, percent);

            return lifeColor;
        }
    }
}