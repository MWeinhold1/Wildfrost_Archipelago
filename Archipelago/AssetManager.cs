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

        public static Dictionary<int, VanillaDataReference> vanillaData;

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
    }

    public class VanillaDataReference
    {
        public int APID { get; }
        public string internalName;
        public int rewardPool;
        public DataFile data;
        public bool isInPool;

        public VanillaDataReference(DataFile dataFile, RewardPool pool)
        {
            internalName = data.name;
            rewardPool = poolToInt(pool);
            data = dataFile;
        }

        private static RewardPool[] pools = null;

        public static RewardPool intToPool(int id)
        {
            if (pools == null)
                pools = Extensions.GetAllRewardPools().ToArray();

            try
            {
                return pools[id];
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Error, $"Error in VanillaDataReference.intToPool: {e}");
                throw e;
            }
        }

        public static int poolToInt(RewardPool pool)
        {
            if (pools == null)
                pools = Extensions.GetAllRewardPools().ToArray();

            try
            {
                return Array.IndexOf(pools, pool);
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Error, $"Error in VanillaDataReference.poolToInt: {e}");
                throw e;
            }
        }
    }
}
