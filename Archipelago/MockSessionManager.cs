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




        private static readonly Dictionary<string, object> MockOptions = new Dictionary<string, object>
        {
            {"goal", 0},
            {"deathlink", false},
        };
    }
}