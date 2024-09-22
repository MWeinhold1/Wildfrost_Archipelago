using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wildfrost_Archipelago
{
    public class main : WildfrostMod
    {
        public main(string modDirectory) : base(modDirectory)
        {
        }

        public override string GUID => "mweinhold.wildfrost.archipelago";

        public override string[] Depends => new string[] { };

        public override string Title => "[WIP] Wildfrost Archipelago";

        public override string Description => "Adds Archipelago Randomizer support to Wildfrost";

        public static List<object> assets = new List<object>();

        private bool preLoaded = false;
        
        private void CreateModAssets()
        {
            assets.Add(new CardDataBuilder(this).CreateItem("archifact", "Archi-fact")
                .SetSprites("Archi-fact.png", "Archi-fact.png")
                .WithCardType("Item")
                .WithFlavour("Archipelago mod incoming!")
                .WithPlayType(Card.PlayType.None)
                );

            preLoaded = true;
        }
        public void UnloadFromClasses()
        {
            List<CardData> cards = AddressableLoader.GetGroup<CardData>("CardData");
            foreach (CardData card in cards)
            {
                cards.RemoveAllWhere((item) => item == null || item.ModAdded == this);
            }
        }

        protected override void Load()
        {
            if (!preLoaded) { CreateModAssets(); }
            base.Load();
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

        //protected override void Load()
        //{
        //    UnityEngine.Debug.Log("[Tutorial1] Loaded!");
        //    base.Load();
        //    Events.OnCardDataCreated += BigBooshu;
        //}

        //protected override void Unload()
        //{
        //    base.Unload();
        //    Events.OnCardDataCreated -= BigBooshu;
        //}

        //private void BigBooshu(CardData cardData) //cardData is the CardData that was created/duplicated
        //{
        //    UnityEngine.Debug.Log("[Tutorial1] New CardData Created: " + cardData.name);
        //    if (cardData.name == "Wolfie")     //Booshu's internal name is BerryPet 
        //    {
        //        cardData.hp = 99;                //Setting hp
        //        cardData.damage = 99;            //Setting damage
        //        UnityEngine.Debug.Log("[Tutorial1] Wolfie!");
        //    }
        //}
    }
}
