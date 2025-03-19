using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Constants;
using Wildfrost_Archipelago.Interfaces;

namespace Wildfrost_Archipelago.Archipelago
{
    public class APSessionManager : ISessionManager
    {
        private const string gameName = "Wildfrost";
        private ArchipelagoSession session;

        public bool StartSession(string uriAndPort, string slotName, string password)
        {
            Logger.Log(LogType.Info, $"Starting connection to {uriAndPort}");
            LoginResult result;

            try
            {
                session = ArchipelagoSessionFactory.CreateSession(uriAndPort);
                result = session.TryConnectAndLogin(gameName, slotName, ItemsHandlingFlags.AllItems, password: password);
                if (!result.Successful)
                {
                    session = null;
                    LoginFailure failure = (LoginFailure)result;
                    string errorCodeString = string.Join(", ", failure.ErrorCodes.Select(ec => ec.ToString()));
                    Logger.Log(LogType.Warning, $"Failed to connect to archipelago: ${errorCodeString}");
                    return false;
                }
                else
                {
                    LoginSuccessful success = (LoginSuccessful)result;
                    Logger.Log(LogType.Info, $"Successful connection to {uriAndPort}: Connected to slot ${success.Slot}");
                    return true;
                }
            }
            catch (Exception e)
            {
                session = null;
                Logger.Log(LogType.Error, e.ToString());
                return false;
            }
        }

        public string GetLocationName(int APID) => session.Locations.GetLocationNameFromId(APID);

        public List<APItem> GetAllReceivedItems() { throw new NotImplementedException(); }

        public List<APLocation> GetAllRemainingLocations() { throw new NotImplementedException(); }

        public void EndSession() { throw new NotImplementedException(); }

        public bool CheckWinCon() { throw new NotImplementedException(); }

        public void ReceiveItemCallback() { throw new NotImplementedException(); }

        public void SendLocationsFound(int[] locationIDs) { throw new NotImplementedException(); }

        public void SendDeath() { throw new NotImplementedException(); }
    }
}
