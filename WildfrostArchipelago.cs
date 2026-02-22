using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Wildfrost_Archipelago
{
    public class WildfrostArchipelago : WildfrostMod
    {
        public static WildfrostArchipelago modRef;
        public static bool debugMode = true;
        public WildfrostArchipelago(string modDirectory) : base(modDirectory)
        {
            modRef = this;
        }

        #region Overrides

        public override string GUID => "mweinhold.wildfrost.archipelago";

        public override string[] Depends => new string[] { };

        public override string Title => "[WIP] Wildfrost Archipelago";

        public override string Description => "Adds Archipelago Randomizer support to Wildfrost";
        public override void Load()
        {
            Logger.Log(LogType.Info, "Loading Wildfrost Archipelago Mod");
            ServiceFactory.Init(debugMode);
            if (!preLoaded) { CreateModAssets(); }
            ServiceFactory.eventManager.LoadEvents();
            base.Load();
            Logger.Log(LogType.Info, "Finished Loading Wildfrost Archipelago Mod");
        }

        public override void Unload()
        {
            UnloadFromClasses();
            base.Unload();
        }
        #endregion

        public static List<object> assets = new List<object>();

        private bool preLoaded = false;

        private void CreateModAssets()
        {
            Logger.Log(LogType.Info, "Loading Mod Assets");

            var assetManager = ServiceFactory.assetManager;
            assets.Add(assetManager.GetStatusEffectBuilder());
            assets.Add(assetManager.GetUnitBuilder());
            assets.Add(assetManager.GetItemBuilder());
            assets.Add(assetManager.GetCharmBuilder());

            Logger.Log(LogType.Info, "Finished Loading Mod Assets");

            preLoaded = true;
        }

        public void UnloadFromClasses()
        {
            Logger.Log(LogType.Info, "Unloading Mod Assets");
            List<CardData> cards = AddressableLoader.GetGroup<CardData>("CardData");
            cards.RemoveAllWhere((item) => item == null || item.ModAdded == this);
        }

        //Credits to Hopeful for this AddAssets code.
        public override List<T> AddAssets<T, Y>()   //AddAssets is called somewhere inside base.Load(). It is called multiple times, and each time T and Y are different DataFile and DataFileBuilders
        {
            Logger.Log(LogType.Info, $"Checking for {typeof(Y).Name}s");
            if (assets.OfType<T>().Any())           //Checks if assets has any builders of the corresponding type. 
                Logger.Log(LogType.Info, $"Adding {typeof(Y).Name}s: {assets.OfType<T>().Count()}"); //Debug statement
            return assets.OfType<T>().ToList();     //Return the correct builders.
        }

        // Code from Snowfall by Jacorb
        public static void SwitchToSaveProfile(string switchTo, bool copyFiles = false)
        {
            SaveSystem.Profile = switchTo;
            SaveSystem.folderName = SaveSystem.profileFolder + "/" + switchTo;
            if (SaveSystem.Enabled)
            {
                Events.InvokeSaveSystemProfileChanged();
            }

            var loadFrom = SaveSystem.profileFolder + "/Default";
            var dir = Directory.GetParent(SaveSystem.settings.FullPath);
            /*if (!Directory.Exists(dir.FullName + "/" + SaveSystem.folderName) && Directory.Exists(dir.FullName + "/" + loadFrom) && copyFiles)
            {
                var innerDir = Directory.CreateDirectory(dir.FullName + "/" + SaveSystem.folderName);
                var saveFilesToCopy = new string[] { "Campaign", "Battle", "History", "Save", "Stats" };

                foreach (var sf in saveFilesToCopy)
                {
                    if (File.Exists(dir + "/" + loadFrom + "/" + sf + ".sav"))
                    {
                        File.Copy(dir + "/" + loadFrom + "/" + sf + ".sav", innerDir + "/" + sf + ".sav");
                    }
                    if (File.Exists(dir + "/" + loadFrom + "/" + sf + ".sav.bac"))
                    {
                        File.Copy(dir + "/" + loadFrom + "/" + sf + ".sav.bac", innerDir + "/" + sf + ".sav.bac");
                    }
                }
            }*/
        }

        // Code stolen from hopeful_phan's profile manager mod. 
        /*public void InitUI()
        {
            bool flag2 = !ProfileManagerModBehaviour.buttonGroup;
            if (flag2)
            {
                ProfileManagerMod.behaviour = new GameObject("Profile Manager");
                UnityEngine.Object.DontDestroyOnLoad(ProfileManagerMod.behaviour);
                ProfileManagerMod.behaviour.hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontUnloadUnusedAsset);
                ProfileManagerMod.uiItems = new GameObject("UI Items").transform;
                ProfileManagerMod.uiItems.SetParent(ProfileManagerMod.behaviour.transform);
                ProfileManagerMod.uiItems.gameObject.SetActive(false);
                ProfileManagerModBehaviour e = ProfileManagerMod.behaviour.AddComponent<ProfileManagerModBehaviour>();
            }
            ProfileManagerMod.behaviour.SetActive(true);
            GameObject gameObject = GameObject.Find("Canvas/Safe Area/TopButtons");
            if (gameObject != null)
            {
                gameObject.SetActive(true);
            }
            Events.OnSaveSystemProfileChanged += OverallStatsSystem.instance.GameStart;
            UnityAction unityAction;
            if ((unityAction = ProfileManagerMod.<> O.< 0 > __OnProfileChanged) == null)
            {
                unityAction = (ProfileManagerMod.<> O.< 0 > __OnProfileChanged = new UnityAction(ProfileManagerModBehaviour.OnProfileChanged));
            }
            Events.OnSaveSystemProfileChanged += unityAction;
            UnityAction<Scene> unityAction2;
            if ((unityAction2 = ProfileManagerMod.<> O.< 1 > __OnSceneChanged) == null)
            {
                unityAction2 = (ProfileManagerMod.<> O.< 1 > __OnSceneChanged = new UnityAction<Scene>(ProfileManagerModBehaviour.OnSceneChanged));
            }
            Events.OnSceneChanged += unityAction2;
        }*/



        //private void DataFileReferences()
        //{
        //    // Known useful
        //    CardData d;
        //    CardUpgradeData f;
        //    GameModifierData l;

        //    // Other unsure
        //    BattleData a;
        //    BossRewardData b;
        //    CampaignNodeType c;
        //    CardType e;
        //    ChallengeListener g;
        //    ChallengeData h;
        //    ClassData i;
        //    EyeData j;
        //    GameMode k;
        //    KeywordData m;
        //    StatusEffectData n;
        //    BuildingPlotType o;
        //    BuildingType p;
        //    TraitData q;
        //    UnlockData r;
        //}
    }

    public static class Logger
    {
        public static void Log(LogType type, object message)
        {
            string titledMessage = $"[{WildfrostArchipelago.modRef.Title}] {message}";
            switch (type)
            {
                case LogType.Info:
                    Debug.Log(titledMessage);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(titledMessage);
                    break;
                case LogType.Error:
                    Debug.LogError(titledMessage);
                    break;
            }
        }
    }

    public enum LogType
    {
        Info,
        Warning,
        Error
    }
}
