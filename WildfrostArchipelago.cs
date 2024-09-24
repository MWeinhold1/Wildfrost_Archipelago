using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wildfrost_Archipelago.Randomizers;

namespace Wildfrost_Archipelago
{
    public class WildfrostArchipelago : WildfrostMod
    {
        public WildfrostArchipelago(string modDirectory) : base(modDirectory) {}

        public override string GUID => "mweinhold.wildfrost.archipelago";

        public override string[] Depends => new string[] { };

        public override string Title => "[WIP] Wildfrost Archipelago";

        public override string Description => "Adds Archipelago Randomizer support to Wildfrost";

        public static List<object> assets = new List<object>();

        public static List<Randomizer> randomizers = new List<Randomizer>();

        private bool preLoaded = false;
        
        private void CreateModAssets()
        {
            Debug.LogWarning($"[{Title}] Loading Mod Assets");

            CardRandomizer rando = new CardRandomizer(this);
            rando.Randomize();

            AddAssets<CardDataBuilder, CardData>();
            preLoaded = true;
        }

        public void UnloadFromClasses()
        {
            Debug.LogWarning($"[{Title}] Unloading Mod Assets");
            List<CardData> cards = AddressableLoader.GetGroup<CardData>("CardData");
            cards.RemoveAllWhere((item) => item == null || item.ModAdded == this);
        }

        protected override void Load()
        {
            if (!preLoaded) { CreateModAssets(); }
            Events.OnSceneLoaded += Events_OnSceneLoaded;
            base.Load();
        }

        private void Events_OnSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            if (scene.name == "Town")
            {
                Debug.LogWarning($"[{Title}] Detected Town load");
                //TODO
            }
        }

        protected override void Unload()
        {
            UnloadFromClasses();
            base.Unload();
        }

        //Credits to Hopeful for this AddAssets code.
        public override List<T> AddAssets<T, Y>()   //AddAssets is called somewhere inside base.Load(). It is called multiple times, and each time T and Y are different DataFile and DataFileBuilders
        {
            if (assets.OfType<T>().Any())           //Checks if assets has any builders of the corresponding type. 
                Debug.LogWarning($"[{Title}] adding {typeof(Y).Name}s: {assets.OfType<T>().Count()}"); //Debug statement
            return assets.OfType<T>().ToList();     //Return the correct builders.
        }

        public T TryGet<T>(string name) where T : DataFile
        {
            T data;
            if (typeof(StatusEffectData).IsAssignableFrom(typeof(T)))
                data = Get<StatusEffectData>(name) as T;
            else
                data = Get<T>(name);

            if (data == null)
                throw new Exception($"TryGet Error: Could not find a [{typeof(T).Name}] with the name [{name}] or [{Extensions.PrefixGUID(name, this)}]");

            return data;
        }
    }
}
