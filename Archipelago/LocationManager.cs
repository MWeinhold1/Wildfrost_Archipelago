using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Archipelago
{
    public class LocationManager
    {

    }

    public class APLocation
    {
        public int APID { get; }
        public int localID { get; }
        public string displayName { get; }
        public LocationType type { get; }

        public APLocation(int id)
        {
            // Location APIDs are 5 digit IDs. First 2 digits denote LocationType, the rest are unique IDs that may encode more information
            try
            {
                APID = id;
                localID = id % 1000;
                type = (LocationType)(id / 1000);
                displayName = SessionManager.GetLocationName(id);
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Error, $"Unable to build APLocation with ID {id}: {e.ToString()}");
            }
        }
    }

    public enum LocationType
    {
        unknown = 0,
        building = 10,
        building_challenge = 20,
        idol_challenge = 30,
        enemy_kill = 40,
        miniboss_kill = 41,
        boss_kill = 42,
        common_item = 50,
        snow_item = 51,
        shade_item = 52,
        clunk_item = 53,
        common_unit = 60,
        snow_unit = 61,
        shade_unit = 62,
        clunk_unit = 63,
        common_charm = 70,
        snow_charm = 71,
        shade_charm = 72,
        clunk_charm = 73,
        boss_reward = 80
    }
}
