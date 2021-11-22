using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Proxet.Tournament
{
    public class TeamGenerator
    {
        public (string[] team1, string[] team2) GenerateTeams(string filePath)
        {
            string[] team1 = new string[9];
            string[] team2 = new string[9];

            string[] eligiblePlayers = new string[18];
            int[] countersForEachVehicleType = new int[3];

            List<Player> players = new();

            using (StreamReader reader = File.OpenText(filePath))
            {
                reader.ReadLine();  // skip the first line
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] playerInfo = line.Split("\t");
                    players.Add(
                        new Player() { 
                            Name = playerInfo[0], 
                            WaitingTime = Convert.ToInt32(playerInfo[1]), 
                            VehicleType = Convert.ToInt32(playerInfo[2]) 
                        });
                }
            }
            Player[] sortedPlayers = players.OrderByDescending(p => p.WaitingTime).ToArray();
            foreach (Player player in sortedPlayers)
            {
                int vehicleType = player.VehicleType;
                int count = countersForEachVehicleType[vehicleType - 1]++;
                if (count < 6)
                {
                    // players with vehicle type n will get slots {0(n-1), 1(n-1), ... , 5(n-1)} in eligiblePlayers array
                    eligiblePlayers[(vehicleType - 1) * 6 + count] = player.Name;
                }
            }

            for (int i = 0; i < eligiblePlayers.Length; i++)
            {
                string playerName = eligiblePlayers[i];
                // for team1 pick 3 first players for each vehicle type i.e. 0, 1, 2, 6, 7, 8, 12, 13, 14
                // for team2 pick the rest of the players
                if ((i / 3) % 2 == 0)
                {
                    // convert indexes of picked players and add them to the team
                    team1[i - 3 * (i / 6)] = playerName;
                }
                else
                {
                    team2[i - 3 * (i / 6) - 3] = playerName;
                }
            }

            return (team1, team2);

            
        }

        private class Player
        {
            public string Name { get; set; }
            public int WaitingTime { get; set; }
            public int VehicleType { get; set; }
        }

    }
}