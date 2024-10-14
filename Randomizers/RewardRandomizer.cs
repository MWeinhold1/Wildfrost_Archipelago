using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Archipelago;

namespace Wildfrost_Archipelago.Randomizers
{
    public class RewardRandomizer
    {
        private Dictionary<string, (CardData card, RewardPool pool)> vanillaCardMap = new Dictionary<string, (CardData, RewardPool)>();

        public RewardRandomizer()
        {
            RegisterItemPools();
            RegisterEvents();
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
                    Logger.Log(LogType.Info, $"Registering {card.title} from {pool.name} pool");
                    vanillaCardMap.Add(card.name, (card, pool));
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
                    tuple.pool.list.Add(AssetManager.unitCard.Clone());
                }
                else if (tuple.pool.type == "Items")
                {
                    tuple.pool.list.Add(AssetManager.itemCard.Clone());
                }
                else
                {
                    Logger.Log(LogType.Warning, $"Tried to randomize {title}");
                }
            }
            Logger.Log(LogType.Info, "Finished item pool randomization");
        }

        public void RandomizeCharms()
        {
            Logger.Log(LogType.Info, "Starting charm randomization");
            var pools = Extensions.GetAllRewardPools().Where(p => p.type == "Charms");
            Dictionary<RewardPool, int> poolItemCount = new Dictionary<RewardPool, int>(pools.Count());
            foreach(var pool in pools)
            {
                Logger.Log(LogType.Info, $"{pool.name} has {pool.list.Count} charms");
                poolItemCount.Add(pool, pool.list.Count);
            }
            foreach(var kvp in poolItemCount)
            {
                kvp.Key.list.Clear();
                for(int i = 0; i < kvp.Value; i++)
                {
                    kvp.Key.list.Add(AssetManager.charm.Clone());
                }
            }
            Logger.Log(LogType.Info, "Finished charm randomization");
        }

        public void RegisterEvents()
        {
            Events.OnEntityEnterBackpack += Events_OnEntityEnterBackpack;
            Events.OnUpgradeGained += Events_OnUpgradeGained;
        }

        private void Events_OnUpgradeGained(CardUpgradeData charm)
        {
            // Works for charms
            Logger.Log(LogType.Info, $"Charm Found: {charm.title} : {charm.name}");
            if (charm.IsCharm())
            {
                References.PlayerData.inventory.upgrades.Remove(charm);
            }
        }

        private void Events_OnEntityEnterBackpack(Entity card)
        {
            // Works for cards, not charms
            Logger.Log(LogType.Info, $"Entity Entered Backpack: ({card.data?.title ?? "Not a card"} : {card.data?.name ?? "Not a card"})");
            References.PlayerData.inventory.deck.Remove(card.data);
            PromptSystem.Prompt.SetText("Programmatic Prompt");
            PromptSystem.Create(Prompt.Anchor.Mid, 0, 0, 3, Prompt.Emote.Type.Basic, Prompt.Emote.Position.Above);
        }
    }
}
