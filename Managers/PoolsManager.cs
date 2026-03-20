using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using Wildfrost_Archipelago.Archipelago;
using Wildfrost_Archipelago.Constants;
using Wildfrost_Archipelago.Interfaces;
using static CameraAnimationSystem;

namespace Wildfrost_Archipelago.Managers
{
    public class PoolsManager
    {
        List<APItem> AllItems = new List<APItem>();
        List<CardData> ItemPool = new List<CardData>();
        List<CardData> UnitPool = new List<CardData>();
        List<CardUpgradeData> CharmPool = new List<CardUpgradeData>();
        char curTribe { 
            get {
                switch (References.PlayerData.classData.name)
                {
                    case "Snow":
                        return '1';
                    case "Magic":
                        return '2';
                    case "Clunk":
                        return '3';
                    default:
                        return '4';
                }
            }
        }
        public void LoadSave()
        {
            List<int> list = SaveSystem.LoadProgressData<List<int>>("APItems") ?? new List<int> { };
            if (list.Count > 0)
            {
                foreach (int id in list)
                {
                    APItem item = APItemConstants.GetItem(id);
                    if (!AllItems.Contains(item))
                        AllItems = AllItems.Append(item).ToList();
                }
            }    
            if (Campaign.instance != null)
            {
                ItemPool = SaveSystem.LoadCampaignData<List<CardData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "ItemPool");
                UnitPool = SaveSystem.LoadCampaignData<List<CardData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "UnitPool");
                CharmPool = SaveSystem.LoadCampaignData<List<CardUpgradeData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "CharmPool");
            }
        }
        public void UpdatePools(List<APItem> list)
        {
            foreach (APItem item in list)
            {
                if (!AllItems.Contains(item))
                {
                    AllItems = AllItems.Append(item).ToList();
                    AddToTribe(item);
                }
            }
            List<int> list2 = new List<int> { };
            foreach (APItem item in AllItems)
                list2 = list2.Append(item.APID).ToList();
            SaveSystem.SaveProgressData<List<int>>("APItems", list2);
            SavePools();
        }
        public void AddToTribe(APItem item)
        {
            char tribeID = item.APID.ToString()[1];
            if (References.PlayerData == null || tribeID != curTribe)
                return;
            switch (tribeID)
            {
                case '0':
                    AddToPool(item);
                    break;
                case '1':
                    if (References.PlayerData.classData.name == "Snow")
                        AddToPool(item);
                    break;
                case '2':
                    if (References.PlayerData.classData.name == "Magic")
                        AddToPool(item);
                    break;
                case '3':
                    if (References.PlayerData.classData.name == "Clunk")
                        AddToPool(item);
                    break;
            }
        }
        public void AddToPool(APItem item)
        {
            switch (item.APID.ToString()[0])
            {
                case '5':
                    ItemPool = ItemPool.Append(AddressableLoader.Get<CardData>(typeof(CardData).Name, item.internalName)).ToList();
                    break;
                case '6':
                    UnitPool = UnitPool.Append(AddressableLoader.Get<CardData>(typeof(CardData).Name, item.internalName)).ToList();
                    break;
                case '7':
                    CharmPool = CharmPool.Append(AddressableLoader.Get<CardUpgradeData>(typeof(CardUpgradeData).Name, item.internalName)).ToList();
                    break;
            }
        }
        public void PopulatePools()
        {
            SavePools();
            foreach (APItem item in AllItems)
                AddToTribe(item);
        }
        public void PopulatePool(char type)
        {
            SavePools();
            foreach (APItem item in AllItems)
                if (item.APID.ToString()[0] == type)
                    AddToTribe(item);
        }
        public CardData PullItem()
        {
            SavePools();
            if (ItemPool.Count > 0)
                return ItemPool.TakeRandom();
            else
            {
                if (AllItems.Where(item => item.APID.ToString()[0] == '5' && (item.APID.ToString()[1] == curTribe || item.APID.ToString()[1] == '0')).Count() <= 0)
                    return null;
                PopulatePool('5');
                return ItemPool.TakeRandom();
            }
        }
        public CardData PullUnit()
        {
            SavePools();
            if (UnitPool.Count > 0)
                return UnitPool.TakeRandom();
            else
            {
                if (AllItems.Where(item => item.APID.ToString()[0] == '6' && (item.APID.ToString()[1] == curTribe || item.APID.ToString()[1] == '0')).Count() <= 0)
                    return null;
                PopulatePool('6');
                return UnitPool.TakeRandom();
            }
        }
        public CardUpgradeData PullCharm()
        {
            SavePools();
            if (CharmPool.Count > 0)
                return CharmPool.TakeRandom();
            else
            {
                if (AllItems.Where(item => item.APID.ToString()[0] == '7' && (item.APID.ToString()[1] == curTribe || item.APID.ToString()[1] == '0')).Count() <= 0)
                    return null;
                PopulatePool('7');
                return CharmPool.TakeRandom();
            }
        }
        public System.Collections.IEnumerable SavePools()
        {
            yield return new WaitForEndOfFrame();
            if (Campaign.instance != null)
            {
                SaveSystem.SaveCampaignData<List<CardData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "ItemPool", ItemPool);
                SaveSystem.SaveCampaignData<List<CardData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "UnitPool", UnitPool);
                SaveSystem.SaveCampaignData<List<CardUpgradeData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "CharmPool", CharmPool);
            }
            yield break;
        }
    }
}
