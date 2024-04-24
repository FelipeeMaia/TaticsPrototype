using UnityEngine;

namespace Tactics.Constants
{
    public static class Colors
    {
        private static Color colorTeam1 = Color.cyan;
        private static Color colorTeam2 = Color.magenta;

        public static Color GetTeamColor(int team)
        {
            if (team > 1) Debug.LogError("Time maior do que esperado");
            return team switch
            {
                0 => colorTeam1,
                1 => colorTeam2,
                _ => Color.black
            };
        }
    }
}