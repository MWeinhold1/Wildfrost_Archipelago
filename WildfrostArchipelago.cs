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
            ServiceFactory.Init(debugMode);
            if (!preLoaded) { CreateModAssets(); }
            //Events.OnSceneLoaded += Events_OnSceneLoaded;
            Events.OnMapNodeSelect += Events_OnMapNodeSelect;
            Events.OnCardDataCreated += Events_OnCardDataCreated;
            base.Load();
        }
        int temp = 1;
        private void Events_OnCardDataCreated(CardData card)
        {
            if (card.name == "mweinhold.wildfrost.archipelago.archifact_item")
            {
                Logger.Log(LogType.Info, "Created an Archifact card");
                card.forceTitle = "Force Title " + temp.ToString();
                card.attackEffects.First().data.textInsert = "New <Text>";
                temp++;
            }
        }

        private void Events_OnMapNodeSelect(MapNode node)
        {
            Logger.Log(LogType.Info, node.name + ": " + node.campaignNode.name);

            var characters = node.campaignNode.characters.Select(i => i.ToString()).ToList();
            Logger.Log(LogType.Info, string.Join(", ", characters));

            var data = node.campaignNode.data.Select(kvp => kvp.Key + ":" + kvp.Value.ToString());
            Logger.Log(LogType.Info, string.Join(", ", data));

            if (node.campaignNode.type is CampaignNodeTypeItem)
            {
                string name = "mweinhold.wildfrost.archipelago.archifact_item";
                List<string> names = new List<string>
                {
                    name,
                    name,
                    name
                };
                var nameCollection = new SaveCollection<string>(names);
                node.campaignNode.data["cards"] = nameCollection;
                var temp = node.campaignNode.data.GetSaveCollection<string>("cards");
            }

            Logger.Log(LogType.Info, node.campaignNode.GetDesc());
        }

        private bool needsRandomizing = true;

        private void Events_OnSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            if (needsRandomizing && scene.name == "Town")
            {
                Logger.Log(LogType.Info, "Running town loaded scripts");
                (ServiceFactory.GetSessionManager() as MockSessionManager).DebugInitialRandomizeTest();
                needsRandomizing = false;
            }
        }

        protected override void Unload()
        {
            UnloadFromClasses();
            base.Unload();
        }
        #endregion

        public static List<object> assets = new List<object>();

        private bool preLoaded = false;

        private RewardRandomizer cardRando;

        private void CreateModAssets()
        {
            Logger.Log(LogType.Info, "Loading Mod Assets");

            var assetManager = ServiceFactory.GetAssetManager();
            assets.Add(assetManager.GetStatusEffectBuilder());
            //assets.Add(assetManager.GetCharmBuilder());
            //assets.Add(assetManager.GetUnitBuilder());
            assets.Add(assetManager.GetItemBuilder());

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
