using Deadpan.Enums.Engine.Components.Modding;
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

        private HashSet<RewardPool> rewardPools;

        public CardDataBuilder GetItemBuilder()
        {
            Logger.Log(LogType.Info, "Getting item card builder");
            Random rng = new Random();
            return new CardDataBuilder(WildfrostArchipelago.modRef).CreateItem("archifact_item", "Archi-fact")
                .SetSprites("Archi-fact.png", "Archi-fact.png")
                .WithCardType("Item")
                .WithFlavour("Archipelago check")
                .WithPlayType(Card.PlayType.None)
                .WithValue(rng.Next(20, 70))
                .SubscribeToAfterAllBuildEvent((card) =>
                {
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
                .WithFlavour("Archipelago check")
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
            return new CardUpgradeDataBuilder(WildfrostArchipelago.modRef).CreateCharm("archifact_charm")
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

        public CardData CreateNewItemCard(APLocation location)
        {
            // Validate that this location can be converted to an item
            if (location.type != APLocationType.card)
            {
                // Not an item card, meaning this location isn't supposed to be for an item card
                string msg = "Attempted to make an item card asset for the location " + location.id.ToString() + ":" + location.localDescription;
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
                    string msg = "Unable to determine the item pool to place a card for " + location.id.ToString() + ":" + location.localDescription;
                    Logger.Log(LogType.Error, msg);
                    throw new ArgumentException(msg, "location.id");
            }

            var newCard = itemCard.Clone();
            newCard.name = newCard.name + location.id;
            

        }

        private string BuildFullDescription(APLocation location)
        {
            if (location.targetPlayerName.IsNullOrWhitespace())
                return "Unlock " + location.unlockedItem;
            else
                return "Send " + location.unlockedItem + " to " + location.targetPlayerName;
        }
    }
}