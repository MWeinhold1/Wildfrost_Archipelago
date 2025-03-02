using Deadpan.Enums.Engine.Components.Modding;
using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Archipelago.Constants;

namespace Wildfrost_Archipelago.Archipelago
{
    public class AssetManager
    {
        public CardData itemCard { get; private set; }
        public CardData unitCard { get; private set; }
        public CardUpgradeData charm { get; private set; }
        public StatusEffectData effect { get; private set; }

        private Dictionary<string, List<(DataFile card, bool hasBeenFound)>> vanillaPoolReferences;

        private HashSet<RewardPool> rewardPools;

        public CardDataBuilder GetItemBuilder()
        {
            Logger.Log(LogType.Info, "Getting item card builder");
            Random rng = new Random();
            return new CardDataBuilder(WildfrostArchipelago.modRef).CreateItem("archifact_item", "Archi-fact")
                .SetSprites("Archi-fact.png", "Archi-fact.png")
                .WithCardType("Item")
                .WithPlayType(Card.PlayType.None)
                .WithValue(rng.Next(20, 70))
                .SubscribeToAfterAllBuildEvent((card) =>
                {
                    card.attackEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("New Status Effect", 1)
                    };
                    itemCard = card;
                });
        }
        public CardDataBuilder GetUnitBuilder()
        {
            Logger.Log(LogType.Info, "Getting item card builder");
            return new CardDataBuilder(WildfrostArchipelago.modRef).CreateUnit("archifact_unit", "Archi-fact")
                .SetSprites("Archi-fact.png", "Archi-fact.png")
                .WithCardType("Friendly")
                .SetStats(null, null, 0)
                .SubscribeToAfterAllBuildEvent((card) =>
                {
                    unitCard = card;
                });
        }
        public CardUpgradeDataBuilder GetCharmBuilder()
        {
            new StatusEffectDataBuilder(WildfrostArchipelago.modRef).Create("archipelago_effect")
                .WithText("{0}");

            Logger.Log(LogType.Info, "Getting charm builder");
            return new CardUpgradeDataBuilder(WildfrostArchipelago.modRef).CreateCharm("archifact_charm", "pool")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Archi-fact.png")
                .WithTitle("Archipelago Charm")
                .WithText("Archipelago Check")
                .WithTier(2)
                .SubscribeToAfterAllBuildEvent((c) =>
                {
                    charm = c;
                });
        }

        public StatusEffectDataBuilder GetStatusEffectBuilder()
        {
            Logger.Log(LogType.Info, "Getting status effect builder");
            return new StatusEffectDataBuilder(WildfrostArchipelago.modRef).Create<StatusEffectSendAPCheck>("New Status Effect")
                .WithText("Test: {0}")
                .WithTextInsert("Default Insert")
                .WithType("");
        }

        public class StatusEffectSendAPCheck : StatusEffectInstant
        {

        }

        public void AddLocationToItemRewardPool(APLocation location)
        {
            // Validate that this location can be converted to an item
            if (location.type != APLocationType.card)
            {
                // Not an item card, meaning this location isn't supposed to be for an item card
                string msg = "Attempted to make an item card asset for the location " + location.id.ToString() + ":" + location.unlockedItem;
                Logger.Log(LogType.Error, msg);
                throw new ArgumentException(msg, "location.type");
            }

            if (rewardPools == null || rewardPools.Count == 0)
            {
                rewardPools = Extensions.GetAllRewardPools();
            }

            // Determine which item pool this needs to go into
            RewardPool pool;
            int itemPoolNum = (location.id / 1000) % 10;
            switch (itemPoolNum)
            {
                case 0:
                    pool = rewardPools.Where(p => p.name == "BasicItemPool").Single();
                    break;
                case 1:
                    pool = rewardPools.Where(p => p.name == "MagicItemPool").Single();
                    break;
                case 2:
                    pool = rewardPools.Where(p => p.name == "ClunkItemPool").Single();
                    break;
                case 3:
                    pool = rewardPools.Where(p => p.name == "GeneralItemPool").Single();
                    break;
                default:
                    // Not in one of the 4 item pools (snow pool items are considered generic items)
                    string msg = "Unable to determine the item pool to place a card for " + location.id.ToString() + ":" + location.unlockedItem;
                    Logger.Log(LogType.Error, msg);
                    throw new ArgumentException(msg, "location.id");
            }

            var newCard = itemCard.Clone();
            newCard.titleFallback = newCard.name + ":" + location.id;
            newCard.flavour = BuildFlavorText(location);
            newCard.desc = location.id.ToString();
            //pool.list.Add(newCard);

            Logger.Log(LogType.Info, "Added card for " + location.id.ToString() + ":" + location.unlockedItem + " to the reward pool " + pool.name);
        }

        public void AddLocationToUnitRewardPool(APLocation location)
        {
            // Validate that this location can be converted to an item
            if (location.type != APLocationType.companion)
            {
                // Not an unit card, meaning this location isn't supposed to be for an unit card
                string msg = "Attempted to make an companion card asset for the location " + location.id.ToString() + ":" + location.unlockedItem;
                Logger.Log(LogType.Error, msg);
                throw new ArgumentException(msg, "location.type");
            }

            if (rewardPools == null || rewardPools.Count == 0)
            {
                rewardPools = Extensions.GetAllRewardPools();
            }

            // Determine which item pool this needs to go into
            RewardPool pool;
            int itemPoolNum = (location.id / 1000) % 10;
            switch (itemPoolNum)
            {
                case 0:
                    pool = rewardPools.Where(p => p.name == "BasicUnitPool").Single();
                    break;
                case 1:
                    pool = rewardPools.Where(p => p.name == "MagicUnitPool").Single();
                    break;
                case 2:
                    pool = rewardPools.Where(p => p.name == "ClunkUnitPool").Single();
                    break;
                case 3:
                    pool = rewardPools.Where(p => p.name == "GeneralUnitPool").Single();
                    break;
                default:
                    // Not in one of the 4 item pools (snow pool items are considered generic items)
                    string msg = "Unable to determine the unit pool to place a card for " + location.id.ToString() + ":" + location.unlockedItem;
                    Logger.Log(LogType.Error, msg);
                    throw new ArgumentException(msg, "location.id");
            }

            var newCard = unitCard.Clone();
            newCard.name = newCard.name + ":" + location.id;
            newCard.flavour = BuildFlavorText(location);
            newCard.desc = location.id.ToString();
            pool.list.Add(newCard);
            Logger.Log(LogType.Info, "Added card for " + location.id.ToString() + ":" + location.unlockedItem + " to the reward pool " + pool.name);
        }

        public void EmptyCardRewardPools()
        {
            if (vanillaPoolReferences != null && vanillaPoolReferences.Count > 0)
            {
                Logger.Log(LogType.Error, "Attempted to empty the item reward pools multiple times");
                return;
            }

            if (rewardPools == null || rewardPools.Count == 0)
            {
                rewardPools = Extensions.GetAllRewardPools();
            }

            vanillaPoolReferences = new Dictionary<string, List<(DataFile, bool)>>();
            foreach (var pool in rewardPools)
            {
                if (pool.type == "Modifiers" || pool.type == "Charms") continue;

                vanillaPoolReferences[pool.name] = new List<(DataFile, bool)>();
                foreach (var card in pool.list)
                {
                    var input = (card as CardData, false);
                    vanillaPoolReferences[pool.name].Add(input);
                }
            }
        }

        private string BuildFlavorText(APLocation location)
        {
            if (location.targetPlayerName.IsNullOrWhitespace())
                return "Unlock " + location.unlockedItem;
            else
                return "Send " + location.unlockedItem + " to " + location.targetPlayerName;
        }

        public T TryGet<T>(string name) where T : DataFile
        {
            var mod = WildfrostArchipelago.modRef;

            T data;
            if (typeof(StatusEffectData).IsAssignableFrom(typeof(T)))
                data = mod.Get<StatusEffectData>(name) as T;
            else if (typeof(KeywordData).IsAssignableFrom(typeof(T)))
                data = mod.Get<KeywordData>(name.ToLower()) as T;
            else
                data = mod.Get<T>(name);

            if (data == null)
                throw new Exception($"TryGet Error: Could not find a [{typeof(T).Name}] with the name [{name}] or [{Extensions.PrefixGUID(name, mod)}]");

            return data;
        }

        public CardData.StatusEffectStacks SStack(string name, int amount) => new CardData.StatusEffectStacks(TryGet<StatusEffectData>(name), amount);
        //See above

        //Note: you need to add the reference DeadExtensions.dll in order to use InstantiateKeepName(). 
        public StatusEffectDataBuilder StatusCopy(string oldName, string newName)
        {
            var mod = WildfrostArchipelago.modRef;

            StatusEffectData data = TryGet<StatusEffectData>(oldName).InstantiateKeepName();
            data.name = mod.GUID + "." + newName;
            data.targetConstraints = new TargetConstraint[0];
            StatusEffectDataBuilder builder = data.Edit<StatusEffectData, StatusEffectDataBuilder>();
            builder.Mod = mod;
            return builder;
        }
    }
}