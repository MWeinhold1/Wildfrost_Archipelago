using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Archipelago
{
    public static class SessionManager
    {
        private const string gameName = "Wildfrost";
        private static ArchipelagoSession session;
        private static WildfrostArchipelago parentMod;

        public static void StartSession(WildfrostArchipelago mod, string uriAndPort, string slotName, string password)
        {
            parentMod = mod;
            LoginResult result;

            try
            {
                session = ArchipelagoSessionFactory.CreateSession(uriAndPort);
                result = session.TryConnectAndLogin(gameName, slotName, ItemsHandlingFlags.AllItems, password: password);
                if (!result.Successful)
                {
                    session = null;
                    LoginFailure failure = (LoginFailure)result;
                    //TODO - Show error message
                    return;
                }
            }
            catch (Exception e)
            {
                session = null;
                Logger.Log(LogType.Info, e.ToString());
                //TODO - Show error message
                return;
            }

            LoginSuccessful success = (LoginSuccessful)result;
        }
    }
}
