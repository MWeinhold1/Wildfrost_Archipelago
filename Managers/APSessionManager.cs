using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Diagnostics;
using Wildfrost_Archipelago.Constants;
using Wildfrost_Archipelago.Interfaces;

namespace Wildfrost_Archipelago.Archipelago
{
    public class APSessionManager : ISessionManager
    {
        private const string gameName = "Wildfrost";
        private ArchipelagoSession session;
        private List<string> RewardsToUndo = new List<string>(){};
        public Dictionary<string, object> SlotData;
        /* keys
            "goal"
            "excluded_idols"
            "bypass_town_order"
            "bypass_building_order"
            "bell_sanity"
            "fight_gating"
            "fight_rando"
            "wave_rando"
         */

        public bool StartSession(string uriAndPort, string slotName, string password)
        {
            Logger.Log(LogType.Info, $"Starting connection to {uriAndPort}");
            LoginResult result;
            try
            {
                session = ArchipelagoSessionFactory.CreateSession(uriAndPort);
                result = session.TryConnectAndLogin(gameName, slotName, ItemsHandlingFlags.AllItems, password: password, version: new Version(0, 6, 6));
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
                    InitializeSession(uriAndPort, slotName);
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

        public async void InitializeSession(string uriAndPort, string slotName)
        {
            session.Items.ItemReceived += ItemReceived;
            ServiceFactory.poolsManager.UpdatePools(GetAllReceivedItems());

            await Task.Delay(16);
            WildfrostArchipelago.SwitchToSaveProfile(uriAndPort + "_" + slotName);
            await Task.Delay(16);

            SaveSystem.SaveProgressData<int>("tutorialProgress", 2);
            SaveSystem.SaveProgressData<bool>("tutorialTownDone", true);
            Events.OnChallengeCompletedSaved += InterceptChallenge;

            slotdata = session.DataStorage.GetSlotData(session.Players.ActivePlayer.Slot);

            MetaprogressionSystem.Remove<string, string>("pets", "Wolfie", null);
            MetaprogressionSystem.Add<string, string>("pets", "Wolfie", "Pet 0");

            UnlockData snowdwellersUnlock = new UnlockData();
            snowdwellersUnlock.name = "Tribe 0";
            AddressableLoader.AddToGroup<UnlockData>("UnlockData", snowdwellersUnlock);

            ClassData snowdwellers = AddressableLoader.Get<ClassData>("ClassData", "Basic");
            snowdwellers.requiresUnlock = snowdwellersUnlock;

            UnlockData muncherUnlock = new UnlockData();
            muncherUnlock.name = "Event 4";
            AddressableLoader.AddToGroup<UnlockData>("UnlockData", muncherUnlock);

            UnlockData blingsnailUnlock = new UnlockData();
            blingsnailUnlock.name = "Event 5";
            AddressableLoader.AddToGroup<UnlockData>("UnlockData", blingsnailUnlock);

            foreach (ChallengeData chal in ChallengeSystem.GetAllChallenges())
            {
                chal.requires = new ChallengeData[] { };
                chal.hidden = false;
            }

            await Task.Delay(16);
            // AUTO-COMPLETING ALREADY COMPLETED LOCATIONS IN CASE OF GAME COMPLETIONS OR NEW CLIENT SAVES
            List<string> list = SaveSystem.LoadProgressData<List<string>>("completedChallenges", null) ?? new List<string>();
            await Task.Delay(16);
            foreach (long id in session.Locations.AllLocationsChecked)
            {
                if (id.ToString()[0] == '3' || id.ToString()[0] == '4' || id.ToString()[0] == '5' || id.ToString()[0] == '6' || id.ToString()[0] == '7')
                    //Don't do anything if its a kill location or a repeatable ID since it won't align with anything anyway
                    continue;
                APLocation loc = APLocationConstants.LocationReferences[(int)id];
                ChallengeData chal = AddressableLoader.Get<ChallengeData>("ChallengeData", loc.internalName);
                if (!list.Contains(chal.name))
                {
                    list.Add(chal.name);
                    SaveSystem.SaveProgressData<List<string>>("completedChallenges", list);
                }
            }
            Logger.Log(LogType.Info, $"Successful connection to {uriAndPort}");


        }

        public string GetLocationName(int APID) => session.Locations.GetLocationNameFromId(APID);

        public List<APItem> GetAllReceivedItems() {
            List<APItem> list = new List<APItem>();
            foreach (ItemInfo item in session.Items.AllItemsReceived.ToArray())
                list.Add(APItemConstants.GetItem(item.ItemId));
            return list;
        }

        public List<APLocation> GetAllRemainingLocations() { throw new NotImplementedException(); }

        public void EndSession() { throw new NotImplementedException(); }

        public bool CheckWinCon() { throw new NotImplementedException(); }

        public void ReceiveItemCallback() { throw new NotImplementedException(); }

        public void SendLocationsFound(int[] locationIDs)
        {
            foreach (int ID in locationIDs)
            {
                //session.Locations.GetLocationIdFromName("Wildfrost", APLocationConstants.LocationReferences[ID].localDescription);
                Logger.Log(LogType.Info, "Sending location " + ID.ToString());
                session.Locations.CompleteLocationChecksAsync((long)ID);
            }
        }

        public void SendDeath() { throw new NotImplementedException(); }
        private void ItemReceived(ReceivedItemsHelper helper)
        {
            ItemInfo item = helper.DequeueItem();
            Logger.Log(LogType.Info, "Receiving item of id " + item.ItemId.ToString());
            ServiceFactory.poolsManager.UpdatePools(new List<APItem>() { APItemConstants.GetItem(item.ItemId)});
        }
        
        public List<int> GetRepeatableLocations(char type, char tribe)
        {
            int[] list = { };
            foreach (long longID in session.Locations.AllMissingLocations)
            {
                int ID = (int)longID;
                if (ID.ToString()[0] == type && (ID.ToString()[1] == tribe || ID.ToString()[0] == '8'))
                {
                    list = list.AddItem(ID).ToArray();
                }
            }
            return list.ToList();
        }
        private bool awaitingUndo = false;
        public void InterceptChallenge(ChallengeData chal)
        {
            Logger.Log(LogType.Info, "CHALLENGE DATA " + chal.name + " HAS BEEN INTERCEPTED");
            SendLocationsFound(new int[]{ APLocationConstants.GetLocationIDFromName(chal.name)});
            RewardsToUndo.Add(chal.reward.name);
            if (!awaitingUndo)
                UndoReward();
        }
        public async void UndoReward()
        {
            awaitingUndo = true;
            await Task.Delay(16);
            List<string> list2 = SaveSystem.LoadProgressData<List<string>>("townNew", null) ?? new List<string>();
            List<string> list3 = SaveSystem.LoadProgressData<List<string>>("unlocked", null) ?? new List<string>();
            foreach (string name in RewardsToUndo) { 
                list2.Remove(name);
                list3.Remove(name);
            }
            RewardsToUndo.Clear();
            SaveSystem.SaveProgressData<List<string>>("townNew", list2);
            SaveSystem.SaveProgressData<List<string>>("unlocked", list3);
            awaitingUndo = false;
        }
        public async Task<Dictionary<long, ScoutedItemInfo>> GetLocationData(int ID)
        {
            return await session.Locations.ScoutLocationsAsync((long)ID);
        }

        public void SetGoalAchieved()
        {
            session.SetGoalAchieved();
        }
    }
}
