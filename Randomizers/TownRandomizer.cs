using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wildfrost_Archipelago.Randomizers
{
    public class TownRandomizer : Randomizer
    {
        public TownRandomizer() { }

        public override void Randomize()
        {
            StartAllBuildings();
        }

        private void StartAllBuildings()
        {
            Debug.LogWarning($"[TownRandomizer] Attempting to start all buildings");
            List<Building> buildings = UnityEngine.Object.FindObjectsOfType<Building>(true).ToList();
            if (buildings.Count == 0)
            {
                Debug.LogWarning($"[TownRandomizer] No buildings found");
            }
            foreach(var building in buildings)
            {
                //TODO: Fix
                var unlocks = String.Join(", ", building.unlocks);
                Debug.LogWarning($"[TownRandomizer] found a building with the following unlocks left: {unlocks}");
                building.buildStarted = true;
                building.Bloop();
            }
        }
    }
}
