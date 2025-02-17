using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Archipelago.Constants;
using Wildfrost_Archipelago.Interfaces;

namespace Wildfrost_Archipelago.Archipelago
{
    class MockSessionManager : ISessionManager
    {
        public bool StartSession(string uriAndPort, string slotName, string password)
        {
            return true;
        }

        public List<APItem> GetAllReceivedItems() { throw new NotImplementedException(); }

        public List<APLocation> GetAllRemainingLocations() { throw new NotImplementedException(); }

        public void EndSession() { throw new NotImplementedException(); }

        public bool CheckWinCon() { throw new NotImplementedException(); }

        public void ReceiveItemCallback() { throw new NotImplementedException(); }

        public void SendLocationsFound(int[] locationIDs) { throw new NotImplementedException(); }

        public void SendDeath() { throw new NotImplementedException(); }

        private APLocation DebugConvertAPItemToAPLocation(APItem item, int locationId)
        {
            var locRef = APLocationConstants.LocationReferences[locationId];
            return new APLocation(locationId, true, item.displayName, null);
        }

        public void DebugInitialRandomizeTest()
        {
            ServiceFactory.GetAssetManager().EmptyCardRewardPools();

            List<APLocation> itemLocations = new List<APLocation>();
            itemLocations.AddRange(APItemConstants.CommonItems.Values.Select(item => DebugConvertAPItemToAPLocation(item, 53000)));
            itemLocations.AddRange(APItemConstants.SnowItems.Values.Select(item => DebugConvertAPItemToAPLocation(item, 50000)));

            List<APLocation> unitLocations = new List<APLocation>();
            unitLocations.AddRange(APItemConstants.CommonUnits.Values.Select(item => DebugConvertAPItemToAPLocation(item, 63000)));
            unitLocations.AddRange(APItemConstants.SnowUnits.Values.Select(item => DebugConvertAPItemToAPLocation(item, 60000)));

            itemLocations.ForEach(ServiceFactory.GetAssetManager().AddLocationToItemRewardPool);
            unitLocations.ForEach(ServiceFactory.GetAssetManager().AddLocationToItemRewardPool);
        }

        private static readonly Dictionary<string, object> MockOptions = new Dictionary<string, object>
        {
            {"goal", 0},
            {"deathlink", false},
        };
    }
}