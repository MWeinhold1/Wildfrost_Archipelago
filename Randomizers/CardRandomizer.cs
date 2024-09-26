using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Models;

namespace Wildfrost_Archipelago.Randomizers
{
    public class CardRandomizer : Randomizer
    {
        private WildfrostArchipelago parentMod;

        public CardRandomizer(WildfrostArchipelago mod)
        {
            parentMod = mod;
        }

        public void InitializeCardAssets()
        {
            int index = 0;
            foreach(var cards in archifactPools)
            {
                Logger.Log(LogType.Info, $"Beginning Archifact creation for the {cards.name} group");
                foreach (int i in Enumerable.Range(0, cards.count))
                {
                    Logger.Log(LogType.Info, $"Loading Archifact #{index} to the {cards.name} group");
                    WildfrostArchipelago.assets.Add(new CardDataBuilder(parentMod).CreateItem($"{cards.name}_archifact_{i}", "Archi-fact")
                        .SetSprites("Archi-fact.png", "Archi-fact.png")
                        .WithCardType("Item")
                        .WithFlavour($"{cards.name} #{i}; Archipelago check #{index}")
                        .WithPlayType(Card.PlayType.None)
                    );
                    index++;
                }
            }
        }

        public void CheckRewardPools()
        {
            Logger.Log(LogType.Info, "Checking for reward pools");
            var pools = Extensions.GetAllRewardPools();
            foreach(var pool in pools)
            {
                Logger.Log(LogType.Info, $"Found the {pool.name} pool, of type {pool.type}");
            }
        }

        public override void Randomize()
        {
            Logger.Log(LogType.Info, "Starting item swapping");
            foreach (var poolName in itemPools)
            {
                var pool = parentMod.GetAsset<RewardPool>(poolName);
                if (pool == null)
                {
                    Logger.Log(LogType.Info, $"Couldn't find the {poolName} pool");
                    continue;
                }
                else if (pool.type == "Items")
                {
                    Logger.Log(LogType.Info, $"Updating the {poolName} pool");
                    ReplacePoolCardsWithArchifacts(pool);
                }
                else
                {
                    Logger.Log(LogType.Info, $"Skipping the {poolName} pool, type is {pool.type}");
                }
            }
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

        private List<ArchifactCardPool> archifactPools = new List<ArchifactCardPool>
        {
            new ArchifactCardPool("Clunk_Clunker", 10, RewardPool.Type.Items),
            new ArchifactCardPool("Clunk_Item", 20, RewardPool.Type.Items),
            new ArchifactCardPool("Generic_Clunker", 6, RewardPool.Type.Items),
            new ArchifactCardPool("Generic_Item", 16, RewardPool.Type.Items),
            new ArchifactCardPool("Inventor_Hut", 6, RewardPool.Type.Items),
            new ArchifactCardPool("Lumin_Vase", 3, RewardPool.Type.Items),
            new ArchifactCardPool("Shade_Item", 22, RewardPool.Type.Items),
            new ArchifactCardPool("Basic_Clunker", 9, RewardPool.Type.Items),
            new ArchifactCardPool("Basic_Item", 13, RewardPool.Type.Items)
        };

        private List<string> itemPools = new List<string> {
            "BasicCharmPool",
            "BasicItemPool",
            "BasicUnitPool",
            "ClunkCharmPool",
            "ClunkItemPool",
            "ClunkUnitPool",
            "GeneralCharmPool",
            "GeneralItemPool",
            "GeneralModifierPool",
            "GeneralUnitPool",
            "MagicCharmPool",
            "MagicItemPool",
            "MagicUnitPool",
            "SnowCharmPool",
            "SnowItemPool",
            "SnowUnitPool"
        };
    }
}
