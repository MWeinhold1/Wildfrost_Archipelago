using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Archipelago
{
    public static class AssetManager
    {
        public static CardData itemCard { get; private set; }
        public static CardData unitCard { get; private set; }
        public static CardUpgradeData charm { get; private set; }

        public static CardDataBuilder GetItemBuilder()
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
        public static CardDataBuilder GetUnitBuilder()
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
        public static CardUpgradeDataBuilder GetCharmBuilder()
        {
            Logger.Log(LogType.Info, "Getting charm builder");
            Random rng = new Random();
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
    }
}
