using UnityEngine;

namespace Tactics.Constants
{
    public static class Strings
    {
        //Team names
        private static string _team1Name = "Cyan";
        private static string _team2Name = "Magenta";


        public static string GetTeamName(int team)
        {
            if (team > 1) Debug.LogError("Time maior do que esperado");

            return team switch
            {
                0 => _team1Name,
                1 => _team2Name,
                _ => ""
            };
        }
    }
}