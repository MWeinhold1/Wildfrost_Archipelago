using Archipelago.MultiClient.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Archipelago.Constants;

namespace Wildfrost_Archipelago.Interfaces
{
    interface ISessionManager
    {
        bool StartSession(string uriAndPort, string slotName, string password);

        void EndSession();

        bool CheckWinCon();

        List<APItem> GetAllReceivedItems();

        void ReceiveItemCallback();

        List<APLocation> GetAllRemainingLocations();

        void SendLocationsFound(int[] locationIDs);

        void SendDeath();
    }
}
