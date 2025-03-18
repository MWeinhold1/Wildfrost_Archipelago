using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using Wildfrost_Archipelago.Archipelago;
using Wildfrost_Archipelago.Randomizers;

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
        protected override void Load()
        {
            Logger.Log(LogType.Info, "Loading Wildfrost Archipelago Mod");
            ServiceFactory.Init(debugMode);
            if (!preLoaded) { CreateModAssets(); }
            ServiceFactory.eventManager.LoadEvents();
            base.Load();
            Logger.Log(LogType.Info, "Finished Loading Wildfrost Archipelago Mod");
        }

        protected override void Unload()
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
