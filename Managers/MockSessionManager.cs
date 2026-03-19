using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Wildfrost_Archipelago.Constants;
using Wildfrost_Archipelago.Interfaces;
using Deadpan.Enums.Engine.Components.Modding;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using HarmonyLib;
using UnityEngine.TextCore.Text;

namespace Wildfrost_Archipelago.Archipelago
{
    class MockSessionManager : ISessionManager
    {
        public bool StartSession(string uriAndPort, string slotName, string password)
        {
            Logger.Log(LogType.Info, "STARTING A MOCK SESSION WITH PARAMETERS: " + uriAndPort + " ; " + slotName + " ; " + password);
            WildfrostArchipelago.SwitchToSaveProfile(uriAndPort + "_" + slotName);
            (GameObject.FindObjectOfType(typeof(MainMenu)) as MainMenu).skipTutorial = true;
            SaveSystem.SaveProgressData<bool>("tutorialTownDone", true);
            Events.OnChallengeCompletedSaved += InterceptChallenge;

            MetaprogressionSystem.Remove<string, string>("pets", "Wolfie", null);
            MetaprogressionSystem.Add<string, string>("pets", "Wolfie", "Pet 0");

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

        public void InterceptChallenge(ChallengeData chal)
        {
            Logger.Log(LogType.Info, "CHALLENGE DATA " + chal.name + " HAS BEEN INTERCEPTED");
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