using Archipelago.MultiClient.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Interfaces
{
    interface ISessionManager
    {
        LoginResult StartSession(string uriAndPort, string slotName, string password);

        void EndSession();

        bool CheckWinCon();

        List<APItem> GetAllReceivedItems();

        void ReceiveItemCallback();

        void GetAllRemainingLocations();

        void SendLocationFound();

        void SendDeath();
    }
}
