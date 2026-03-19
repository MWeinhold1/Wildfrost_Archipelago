using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
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
                    //session.Items.ItemReceived += ItemReceived;
                    ServiceFactory.poolsManager.UpdatePools(GetAllReceivedItems());

                    WildfrostArchipelago.SwitchToSaveProfile(uriAndPort+"_"+slotName);

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

        public List<APItem> GetAllReceivedItems() {
            List<APItem> list = new List<APItem>();
            foreach (ItemInfo item in session.Items.AllItemsReceived.ToArray())
                list.Append(APItemConstants.GetItem(item.ItemId));
            return list;
        }

        public List<APLocation> GetAllRemainingLocations() { throw new NotImplementedException(); }

        public void EndSession() { throw new NotImplementedException(); }

        public bool CheckWinCon() { throw new NotImplementedException(); }

        public void ReceiveItemCallback() { throw new NotImplementedException(); }

        public void SendLocationsFound(int[] locationIDs) {
            foreach (int ID in locationIDs)
                session.Locations.GetLocationIdFromName("Wildfrost", APLocationConstants.LocationReferences[ID].localDescription);
        }

        public void SendDeath() { throw new NotImplementedException(); }
        /*private void ItemReceived(ReceivedItemsHelper.ItemReceivedHandler helper)
        {
            ServiceFactory.poolsManager.UpdatePools(GetAllReceivedItems());
        }*/

        public void InterceptChallenge(ChallengeData chal)
        {
            Logger.Log(LogType.Info, "CHALLENGE DATA " + chal.name + " HAS BEEN INTERCEPTED");
            SendLocationsFound(new int[]{ APLocationConstants.GetLocationIDFromName(chal.name)});
            (GameObject.FindObjectOfType(typeof(MonoBehaviour)) as MonoBehaviour).StartCoroutine(UndoChallenge(chal));
        }
        public System.Collections.IEnumerator UndoChallenge(ChallengeData chal)
        {
            yield return new WaitForEndOfFrame();
            Logger.Log(LogType.Info, "CHALLENGE DATA " + chal.name + " HAS BEEN WAITED FOR");
            List<string> list = SaveSystem.LoadProgressData<List<string>>("completedChallenges", null) ?? new List<string>();
            List<string> list2 = SaveSystem.LoadProgressData<List<string>>("townNew", null) ?? new List<string>();
            List<string> list3 = SaveSystem.LoadProgressData<List<string>>("unlocked", null) ?? new List<string>();
            foreach (string item in list3)
                Logger.Log(LogType.Info, item);
            list.Remove(chal.name);
            list2.Remove(chal.reward.name);
            list3.Remove(chal.reward.name);
            MetaprogressionSystem.Set(chal.name, false);
            SaveSystem.SaveProgressData<List<string>>("completedChallenges", list);
            SaveSystem.SaveProgressData<List<string>>("townNew", list2);
            SaveSystem.SaveProgressData<List<string>>("unlocked", list3);
            foreach (string item in list3)
                Logger.Log(LogType.Info, item);
            yield break;
        }
    }
}
