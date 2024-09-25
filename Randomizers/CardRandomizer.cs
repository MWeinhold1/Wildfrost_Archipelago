using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            RewardPool itemPool = parentMod.GetAsset<RewardPool>("GeneralItemPool");
            //RewardPool companionPool = parentMod.GetAsset<RewardPool>("GeneralUnitPool");
            RewardPool snowdwellItemPool = parentMod.GetAsset<RewardPool>("BasicItemPool");
            //RewardPool snowdwellCompanionPool = parentMod.GetAsset<RewardPool>("BasicUnitPool");
            ReplacePoolCardsWithArchifacts(itemPool);
            //ReplacePoolCardsWithArchifacts(companionPool);
            if (snowdwellItemPool == null)
            {
                Logger.Log(LogType.Info, "Null Pool!");
            }
            ReplacePoolCardsWithArchifacts(snowdwellItemPool);
            //ReplacePoolCardsWithArchifacts(snowdwellCompanionPool);

            Logger.Log(LogType.Info, "Completed item swapping");

            //try
            //{
            //    RewardPool pool = parentMod.GetAsset<RewardPool>("GeneralItemPool");
            //    Debug.Log($"Found the item pool with {pool.list.Count} items");
            //    foreach(int _ in Enumerable.Range(0, pool.list.Count))
            //    {
            //        DataFile card = pool.list.ElementAt(0);
            //        Debug.Log($"Removing {card.name} from {pool.name} pool");
            //        pool.list.RemoveAt(0);
            //    }

            //}
            //catch (Exception e) {
            //    Debug.LogWarning($"ERROR: {e.Message} | {e.StackTrace}");
            //}

            //AddDummyCards();
        }

        private void ReplacePoolCardsWithArchifacts(RewardPool pool)
        {
            try
            {
                int cardCount = 0;
                Logger.Log(LogType.Info, $"Removing {pool.list.Count} items from the {pool.name} pool");
                while (pool.list.Count > 0)
                {
                    cardCount++;
                    DataFile card = pool.list.ElementAt(0);
                    Logger.Log(LogType.Info, $"Removing {card.name} from {pool.name} pool");
                    pool.list.RemoveAt(0);
                }
                Logger.Log(LogType.Info, $"{pool.name} pool now has {pool.list.Count} items");
                foreach (var i in Enumerable.Range(0, cardCount))
                {
                    Logger.Log(LogType.Info, $"Loading Archifact #{i} into {pool.name}");
                    WildfrostArchipelago.assets.Add(new CardDataBuilder(parentMod).CreateItem($"{pool.name}_archifact_{i}", "Archi-fact")
                        .SetSprites("Archi-fact.png", "Archi-fact.png")
                        .WithCardType("Item")
                        .AddPool(pool.name)
                        .WithFlavour($"{pool.name} Archipelago check #{i}")
                        .WithPlayType(Card.PlayType.None)
                    );
                }
                Logger.Log(LogType.Info, $"{pool.name} pool now has {pool.list.Count} items");
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Warning, $"ERROR: {e}");
            }
        }
    }
}
