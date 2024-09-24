using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wildfrost_Archipelago.Randomizers
{
    public class CardRandomizer : Randomizer
    {
        private WildfrostArchipelago parentMod;

        public CardRandomizer(WildfrostArchipelago mod)
        {
            parentMod = mod;
        }

        public override void Randomize()
        {
            try
            {
                RewardPool pool = parentMod.GetAsset<RewardPool>("GeneralItemPool");
                Debug.Log($"[{parentMod.Title}] Found the item pool with ${pool.list.Count} items");
                
            }
            catch (Exception e) {
                Debug.LogWarning($"[{parentMod.Title}] ERROR: {e.Message} | {e.StackTrace}");
            }

            AddDummyCards();
        }

        private void AddDummyCards()
        {
            foreach(int i in Enumerable.Range(0, 30))
            {
                Debug.LogWarning($"[{parentMod.Title}] Loading Archifact #{i}");
                WildfrostArchipelago.assets.Add(new CardDataBuilder(parentMod).CreateItem($"archifact{i}", "Archi-fact")
                    .SetSprites("Archi-fact.png", "Archi-fact.png")
                    .WithCardType("Item")
                    .AddPool("GeneralItemPool")
                    .WithFlavour($"Archipelago check #{i}")
                    .WithPlayType(Card.PlayType.None)
                );
            }
        }
    }
}
