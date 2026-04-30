using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.MessageLog.Parts;
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
        private List<string> RewardsToUndo = new List<string>() { };
        public Dictionary<string, object> SlotData;
        /* keys
            "goal"
            "bypass_town_order"
            "bypass_building_order"
            "bell_sanity"
            "fight_gating"
            "fight_rando"
            "fights_in_pool"
            "wave_rando"
         */

        //TODOS: Account for no battles being in a given battle pool
        //add fight_rando and wave_rando stuff
        //add bell_sanity stuff
        //test everything
        //add archipelagnome stuff

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

            session.MessageLog.OnMessageReceived += OnMessageReceived;
            Application.wantsToQuit += EndSession;

            await Task.Delay(16);
            WildfrostArchipelago.SwitchToSaveProfile(uriAndPort + "_" + slotName);
            await Task.Delay(16);

            session.Items.ItemReceived += ItemReceived;
            ServiceFactory.poolsManager.UpdatePools(GetAllReceivedItems());

            await Task.Delay(16);

            SaveSystem.SaveProgressData<int>("tutorialProgress", 2);
            SaveSystem.SaveProgressData<bool>("tutorialTownDone", true);
            Events.OnChallengeCompletedSaved += InterceptChallenge;

            SlotData = session.DataStorage.GetSlotData(session.Players.ActivePlayer.Slot);

            //if ((ServiceFactory.sessionManager as APSessionManager).SlotData.Get<long>("fights_in_pool") != 0)
            //    Events.OnPreCampaignPopulate += ServiceFactory.eventManager.Events_OnPreCampaignPopulate;
            SelectStartingPet

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
                if (chal.name.Contains("Hot Spring") || chal.name.Contains("Icebreakers") || chal.name.Contains("Inventors Hut") || chal.name.Contains("Pet House"))
                {
                    if (SlotData.Get<long>("bypass_town_order") != 1)
                        continue;
                }
                else if (!chal.name.StartsWith("Challenge Charm")) //Challenge Companion X, Challenge Item X, Challenge Event X, Challenge Pet X
                {
                    if (SlotData.Get<long>("bypass_building_order") != 1)
                        continue;
                }
                chal.requires = new ChallengeData[] { };
                chal.hidden = false;
            }
            //Finds challenges that aren't locations and marks them as complete
            List<ChallengeData> challengeLocationsToIgnore = ChallengeSystem.GetAllChallenges().ToList().Where(a => !session.Locations.AllLocations.Contains(APLocationConstants.GetLocationIDFromName(a.name))).ToList();

            await Task.Delay(16);
            List<string> list = SaveSystem.LoadProgressData<List<string>>("completedChallenges", null) ?? new List<string>();
            await Task.Delay(16);

            foreach (ChallengeData chal in challengeLocationsToIgnore)
            {
                if (!list.Contains(chal.name))
                    list.Add(chal.name);
            }

            // AUTO-COMPLETING ALREADY COMPLETED LOCATIONS IN CASE OF GAME COMPLETIONS OR NEW CLIENT SAVES
            foreach (long id in session.Locations.AllLocationsChecked)
            {
                if (id.ToString()[0] == '3' || id.ToString()[0] == '4' || id.ToString()[0] == '5' || id.ToString()[0] == '6' || id.ToString()[0] == '7')
                    //Don't do anything if its a kill location or a repeatable ID since it won't align with anything anyway
                    continue;
                APLocation loc = APLocationConstants.LocationReferences[(int)id];
                ChallengeData chal;
                if (!AddressableLoader.TryGet<ChallengeData>("ChallengeData", loc.internalName, out chal))
                    //failsafe
                    continue;
                if (!list.Contains(loc.internalName))
                    list.Add(loc.internalName);
            }
            SaveSystem.SaveProgressData<List<string>>("completedChallenges", list);
            Logger.Log(LogType.Info, $"Successful connection to {uriAndPort}");
        }

        private void OnMessageReceived(LogMessage message)
        {
            foreach (MessagePart part in message.Parts)
            {
                Logger.Log(LogType.Info, "MESSAGE PART RECEIVED: " + part.Text);
            }
        }

        public string GetLocationName(int APID) => session.Locations.GetLocationNameFromId(APID);

        public List<APItem> GetAllReceivedItems() {
            List<APItem> list = new List<APItem>();
            foreach (ItemInfo item in session.Items.AllItemsReceived.ToArray())
                list.Add(APItemConstants.GetItem(item.ItemId));
            return list;
        }

        public List<APLocation> GetAllRemainingLocations() {
            List<APLocation> list = new List<APLocation>();
            foreach (long id in session.Locations.AllMissingLocations)
                list.Add(APLocationConstants.LocationReferences[(int)id]);
            return list;
        }

        public bool EndSession() {
            session.Socket.DisconnectAsync();
            Application.wantsToQuit -= EndSession;
            return true;
        }

        public bool CheckWinCon() { throw new NotImplementedException(); }

        public void ReceiveItemCallback() { throw new NotImplementedException(); }

        public async void SendLocationsFound(int[] locationIDs)
        {
            foreach (int ID in locationIDs)
            {
                //session.Locations.GetLocationIdFromName("Wildfrost", APLocationConstants.LocationReferences[ID].localDescription);
                Logger.Log(LogType.Info, "Sending location " + ID.ToString());
                await session.Locations.CompleteLocationChecksAsync((long)ID);
                if (SlotData.Get <long>("goal") == 2)
                {
                    List<long> list = new List<long>();
                    //list of all snowdwell challenges
                    foreach (int id in APLocationConstants.LocationReferences.Keys.Where(a => a < 30000 && session.Locations.AllLocations.Contains(Convert.ToInt64(a))))
                        list.Add(Convert.ToInt64(id));
                    if (session.Locations.AllLocationsChecked.ContainsAll(list))
                    {
                        await Task.Delay(16);
                        SetGoalAchieved();
                    }
                }
                ScoutedItemInfo item = session.Locations.ScoutLocationsAsync((long)ID).Result.Values.First();
                if (PromptSystem.Prompt.active)
                    await waitUntilPromptHidden(item);
                PromptSystem.SetTextAction(() => "Sent " + item.ItemDisplayName + " to " + item.Player);// Player.Alias;
                await Task.Delay(2000);
                PromptSystem.Hide();
            }
        }

        public void SendDeath() { throw new NotImplementedException(); }

        private async void ItemReceived(ReceivedItemsHelper helper)
        {
            ItemInfo item = helper.DequeueItem();
            Logger.Log(LogType.Info, "Receiving item of id " + item.ItemId.ToString());
            ServiceFactory.poolsManager.UpdatePools(new List<APItem>() { APItemConstants.GetItem(item.ItemId) });

            if (PromptSystem.Prompt.active)
                await waitUntilPromptHidden(item);
            PromptSystem.SetTextAction(() => "Received " + item.ItemDisplayName + " from " + item.Player);// Player.Alias;
            await Task.Delay(2000);
            PromptSystem.Hide();
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


        List<object> promptQueue;
        async Task waitUntilPromptHidden(object item)
        {
            if (promptQueue.Contains(item))
                return;
            promptQueue.Add(item);
            while (promptQueue.Count > 1 && promptQueue[0] != item)
                await Task.Delay(16);
            await Task.Delay(16);
            while (PromptSystem.Prompt.active)
                await Task.Delay(16);
            promptQueue.Remove(item);
            return;
        }
    }
}
