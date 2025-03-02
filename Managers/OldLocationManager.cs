using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Archipelago
{
    public class OldLocationManager
    {
        //private Dictionary<LocationType, List<DataFile>> locationMap = new Dictionary<LocationType, List<DataFile>>();

        //public List<int> GetTestVanillaLocations()
        //{
        //    List<int> retVal = new List<int>();

        //    retVal.AddRange(Enumerable.Range(0, 24).Select(i => ((int)LocationType.common_item * 1000) + i));
        //    retVal.AddRange(Enumerable.Range(0, 22).Select(i => ((int)LocationType.snow_item * 1000) + i));
        //    retVal.AddRange(Enumerable.Range(0, 22).Select(i => ((int)LocationType.shade_item * 1000) + i));
        //    retVal.AddRange(Enumerable.Range(0, 30).Select(i => ((int)LocationType.clunk_item * 1000) + i));
        //    retVal.AddRange(Enumerable.Range(0, 24).Select(i => ((int)LocationType.common_item * 1000) + i));
        //    retVal.AddRange(Enumerable.Range(0, 13).Select(i => ((int)LocationType.snow_item * 1000) + i));
        //    retVal.AddRange(Enumerable.Range(0, 14).Select(i => ((int)LocationType.shade_item * 1000) + i));
        //    retVal.AddRange(Enumerable.Range(0, 13).Select(i => ((int)LocationType.clunk_item * 1000) + i));

        //    return retVal;
        //    //throw new NotImplementedException();
        //}

        //public void InitializeLocations(List<int> APIDs)
        //{
        //    // Location APIDs are 5 digit IDs. First 2 digits denote LocationType, last 3 are unique IDs
        //    foreach (int id in APIDs)
        //    {
        //        string displayName = id.ToString(); /*SessionManager.GetLocationName(id);*/
        //        var type = (LocationType)(id / 1000);
        //        switch(type)
        //        {
        //            case LocationType.common_item:
        //            case LocationType.snow_item:
        //            case LocationType.shade_item:
        //            case LocationType.clunk_item:
        //                AddCardToMap(type, true, id, displayName);
        //                break;
        //            case LocationType.common_unit:
        //            case LocationType.snow_unit:
        //            case LocationType.shade_unit:
        //            case LocationType.clunk_unit:
        //                AddCardToMap(type, false, id, displayName);
        //                break;
        //            case LocationType.common_charm:
        //            case LocationType.snow_charm:
        //            case LocationType.shade_charm:
        //            case LocationType.clunk_charm:
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    foreach (var kvp in locationMap)
        //    {
        //        var pool = LocationTypeToRewardPool(kvp.Key);
        //        if (pool != null)
        //        {
        //            pool.list.AddRange(kvp.Value);
        //        }
        //    }
        //}

        //private void AddCardToMap(LocationType type, bool isItem, int APID, string displayName)
        //{
        //    CardData cardData;
        //    if (isItem)
        //        cardData = AssetManager.itemCard.Clone();
        //    else
        //        cardData = AssetManager.unitCard.Clone();

        //    cardData.flavour = displayName;
        //    cardData.name = $"{cardData.name}:{APID}";
        //    if (!locationMap.ContainsKey(type))
        //    {
        //        locationMap[type] = new List<DataFile>();
        //    }
        //    locationMap[type].Add(cardData);
        //}

        //private void AddCharmToMap(LocationType type, int APID, string displayName)
        //{
        //    CardUpgradeData charmData = AssetManager.charm.Clone();
        //    StatusEffectData effectData = WildfrostArchipelago.modRef.Get<StatusEffectData>("").
        //    CardData.StatusEffectStacks effect = new CardData.StatusEffectStacks()
        //    charmData.effects.Append()
        //}

    //    private RewardPool LocationTypeToRewardPool(LocationType type)
    //    {
    //        var rewardPools = Extensions.GetAllRewardPools();
    //        switch (type)
    //        {
    //            case LocationType.common_item:
    //                return rewardPools.Where(p => p.name == "GeneralItemPool").Single();
    //            case LocationType.snow_item:
    //                return rewardPools.Where(p => p.name == "BasicItemPool").Single();
    //            case LocationType.shade_item:
    //                return rewardPools.Where(p => p.name == "MagicItemPool").Single();
    //            case LocationType.clunk_item:
    //                return rewardPools.Where(p => p.name == "ClunkItemPool").Single();
    //            case LocationType.common_unit:
    //                return rewardPools.Where(p => p.name == "GeneralItemPool").Single();
    //            case LocationType.snow_unit:
    //                return rewardPools.Where(p => p.name == "BasicItemPool").Single();
    //            case LocationType.shade_unit:
    //                return rewardPools.Where(p => p.name == "MagicItemPool").Single();
    //            case LocationType.clunk_unit:
    //                return rewardPools.Where(p => p.name == "ClunkItemPool").Single();
    //            case LocationType.common_charm:
    //                return rewardPools.Where(p => p.name == "GeneralCharmPool").Single();
    //            case LocationType.snow_charm:
    //                return rewardPools.Where(p => p.name == "BasicCharmPool").Single();
    //            case LocationType.shade_charm:
    //                return rewardPools.Where(p => p.name == "MagicCharmPool").Single();
    //            case LocationType.clunk_charm:
    //                return rewardPools.Where(p => p.name == "ClunkCharmPool").Single();

    //            default:
    //                return null;
    //        }
    //    }
    }

    //public enum LocationType
    //{
    //    unknown = 0,
    //    building = 10,
    //    building_challenge = 20,
    //    idol_challenge = 30,
    //    enemy_kill = 40,
    //    miniboss_kill = 41,
    //    boss_kill = 42,
    //    common_item = 50,
    //    snow_item = 51,
    //    shade_item = 52,
    //    clunk_item = 53,
    //    common_unit = 60,
    //    snow_unit = 61,
    //    shade_unit = 62,
    //    clunk_unit = 63,
    //    common_charm = 70,
    //    snow_charm = 71,
    //    shade_charm = 72,
    //    clunk_charm = 73,
    //    boss_reward = 80
    //}
}
