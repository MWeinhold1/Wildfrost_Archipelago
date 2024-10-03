using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Archipelago;
using Wildfrost_Archipelago.Models;

namespace Wildfrost_Archipelago.Randomizers
{
    public class CardRandomizer : Randomizer
    {
        private Dictionary<string, (CardData card, RewardPool pool)> vanillaCardMap = new Dictionary<string, (CardData, RewardPool)>();

        public CardRandomizer()
        {
            RegisterItemPools();
        }

        private void RegisterItemPools()
        {
            Logger.Log(LogType.Info, "Starting item pool registration");
            var pools = Extensions.GetAllRewardPools();
            foreach (var pool in pools)
            {
                if(pool.type != "Items" && pool.type != "Units")
                {
                    Logger.Log(LogType.Info, $"Skipping {pool.name} registration");
                    continue;
                }

                var cards = pool.list.Select(data => (CardData)data);
                foreach (var card in cards)
                {
                    if (card.name.StartsWith("archifact"))
                    {
                        Logger.Log(LogType.Info, $"Skipping {card.name} registration");
                    }
                    else
                    {
                        Logger.Log(LogType.Info, $"Registering {card.title} from {pool.name} pool");
                        vanillaCardMap.Add(card.name, (card, pool));
                    }
                }
            }
            Logger.Log(LogType.Info, "Finished item pool registration");
        }

        public void RandomizeItemPools()
        {
            Logger.Log(LogType.Info, "Starting item pool randomization");
            foreach(var tuple in vanillaCardMap.Values)
            {
                var title = tuple.card.title;
                Logger.Log(LogType.Info, $"Replacing {title} in {tuple.pool.name} pool");
                tuple.pool.list.Remove(tuple.card);
                if (tuple.pool.type == "Units")
                {
                    tuple.pool.list.Add(AssetBuilder.unitCard.Clone());
                }
                else if (tuple.pool.type == "Items")
                {
                    tuple.pool.list.Add(AssetBuilder.itemCard.Clone());
                }
                else
                {
                    Logger.Log(LogType.Warning, $"Tried to randomize {title}");
                }
            }
            Logger.Log(LogType.Info, "Finished item pool randomization");
        }

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
            Random rng = new Random();
            try
            {
                Logger.Log(LogType.Info, $"Removing {pool.list.Count} items from the {pool.name} pool");
                int cardCount = pool.list.Count;
                pool.list.Clear();
                Logger.Log(LogType.Info, $"{pool.name} pool now has {pool.list.Count} items");
                foreach (var i in Enumerable.Range(0, cardCount))
                {
                    Logger.Log(LogType.Info, $"Loading Archifact #{i} into {pool.name}");
                    WildfrostArchipelago.assets.Add(new CardDataBuilder(WildfrostArchipelago.modRef).CreateItem($"{pool.name}_archifact_{i}", "Archi-fact")
                        .SetSprites("Archi-fact.png", "Archi-fact.png")
                        .WithCardType("Item")
                        .AddPool(pool.name)
                        .WithFlavour($"{pool.name} Archipelago check #{i}")
                        .WithPlayType(Card.PlayType.None)
                        .WithValue(rng.Next(20, 70))
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
