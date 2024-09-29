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

        //private void RandomizeVersionTwo()
        //{
        //    var companions = Extensions.GetCategoryCardData("Friendly");
        //    foreach (var companion in companions)
        //    {
        //        Extensions.
        //    }
        //}

        public override void Randomize()
        {
            var pools = Extensions.GetAllRewardPools();
            Logger.Log(LogType.Info, "Starting item swapping");
            foreach (var pool in pools)
            {
                if (pool.type == "Items")
                {
                    Logger.Log(LogType.Info, $"Updating the {pool.name} pool");
                    ReplacePoolCardsWithArchifacts(pool);
                }
                else
                {
                    Logger.Log(LogType.Info, $"Skipping the {pool.name} pool, type is {pool.type}");
                }
            }
            Logger.Log(LogType.Info, "Completed item swapping");
        }

        private void ReplacePoolCardsWithArchifacts(RewardPool pool)
        {
            try
            {
                Logger.Log(LogType.Info, $"Removing {pool.list.Count} items from the {pool.name} pool");
                int cardCount = pool.list.Count;
                pool.list.Clear();
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
