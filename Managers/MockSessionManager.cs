using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Constants;
using Wildfrost_Archipelago.Interfaces;

namespace Wildfrost_Archipelago.Archipelago
{
    class MockSessionManager : ISessionManager
    {
        public bool StartSession(string uriAndPort, string slotName, string password)
        {
            return true;
        }

        public List<APItem> GetAllReceivedItems() {
            List<APItem> list = new List<APItem>();
            foreach (long item in ReceivedItemIDs)
                list = list.Append(APItemConstants.GetItem(item)).ToList();
            return list;
        }

        public List<APLocation> GetAllRemainingLocations() { throw new NotImplementedException(); }

        public void EndSession() { throw new NotImplementedException(); }

        public bool CheckWinCon() { throw new NotImplementedException(); }

        public void ReceiveItemCallback() { throw new NotImplementedException(); }

        public void SendLocationsFound(int[] locationIDs) { foreach (int ID in locationIDs) Logger.Log(LogType.Info, "Got location " + APLocationConstants.LocationReferences[ID].localDescription); }

        public void SendDeath() { throw new NotImplementedException(); }

        private static readonly Dictionary<string, object> MockOptions = new Dictionary<string, object>
        {
            {"goal", 0},
            {"deathlink", false},
        };
        private static readonly long[] ReceivedItemIDs = {
            50000,
            52001,
            62006,
            70027
        };
    }
}