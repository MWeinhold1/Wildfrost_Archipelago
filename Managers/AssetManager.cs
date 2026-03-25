using Deadpan.Enums.Engine.Components.Modding;
using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Constants;

namespace Wildfrost_Archipelago.Managers
{
    public class AssetManager
    {
        public static string internalItemName = "archifact_item";
        public static string internalUnitName = "archifact_unit";
        public static string internalCharmName = "archifact_charm";
        //public static string internalStatusName = "archifact_status";
        public static string fullItemName = WildfrostArchipelago.modRef.GUID + "." + internalItemName;
        public static string fullUnitName = WildfrostArchipelago.modRef.GUID + "." + internalUnitName;
        public static string fullCharmName = WildfrostArchipelago.modRef.GUID + "." + internalCharmName;
        //public static string fullStatusName = WildfrostArchipelago.modRef.GUID + "." + internalStatusName;

        private Dictionary<string, List<(DataFile card, bool hasBeenFound)>> vanillaPoolReferences;

        private HashSet<RewardPool> rewardPools;

        public CardDataBuilder GetItemBuilder()
        {
            Logger.Log(LogType.Info, "Getting item card builder");
            Random rng = new Random();
            return new CardDataBuilder(WildfrostArchipelago.modRef).CreateItem(internalItemName, "Archi-fact")
                .SetSprites("ArchiSprite.png", "ArchiBG.png")
                .WithCardType("Item")
                .WithPlayType(Card.PlayType.None)
                .WithValue(rng.Next(20, 70))
                /*.SubscribeToAfterAllBuildEvent((card) =>
                {
                    card.attackEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack(internalStatusName, 1)
                    };
                })*/;
        }
        public CardDataBuilder GetUnitBuilder()
        {
            Logger.Log(LogType.Info, "Getting unit card builder");
            return new CardDataBuilder(WildfrostArchipelago.modRef).CreateUnit(internalUnitName, "Archi-fact")
                .SetSprites("ArchiSprite.png", "ArchiBG.png")
                .WithCardType("Friendly")
                .SetStats(null, null, 0)
                /*.SubscribeToAfterAllBuildEvent((card) =>
                {
                    card.attackEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack(internalStatusName, 1)
                    };
                })*/;
        }
        public CardUpgradeDataBuilder GetCharmBuilder()
        {
            Logger.Log(LogType.Info, "Getting charm builder");

            return new CardUpgradeDataBuilder(WildfrostArchipelago.modRef).Create(internalCharmName)
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("ArchiCharm.png")
                .WithTitle("Archipelago Item")
                .WithText("Archipelago Check");
        }

        /*public StatusEffectDataBuilder GetStatusEffectBuilder()
        {
            Logger.Log(LogType.Info, "Getting status effect builder");
            return new StatusEffectDataBuilder(WildfrostArchipelago.modRef).Create<StatusEffectSendAPCheck>(internalStatusName)
                .WithText("{0}")
                .WithTextInsert("ERROR: Unknown Item")
                .WithType("");
        }*/

        public class StatusEffectSendAPCheck : StatusEffectInstant
        {

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
    }
}