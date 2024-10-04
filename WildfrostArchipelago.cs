using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wildfrost_Archipelago.Archipelago;
using Wildfrost_Archipelago.Randomizers;

namespace Wildfrost_Archipelago
{
    public class WildfrostArchipelago : WildfrostMod
    {
        public static WildfrostArchipelago modRef;
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
            if (!preLoaded) { CreateModAssets(); }
            Events.OnSceneLoaded += Events_OnSceneLoaded;
            base.Load();
        }

        private bool needsRandomizing = true;

        private void Events_OnSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            if (needsRandomizing && scene.name == "Town")
            {
                cardRando.RandomizeItemPools();
                cardRando.RandomizeCharms();
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

        private CardRandomizer cardRando;

        private void CreateModAssets()
        {
            Logger.Log(LogType.Info, "Loading Mod Assets");

            assets.Add(AssetManager.GetUnitBuilder());
            assets.Add(AssetManager.GetItemBuilder());
            assets.Add(AssetManager.GetCharmBuilder());
            cardRando = new CardRandomizer();

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
