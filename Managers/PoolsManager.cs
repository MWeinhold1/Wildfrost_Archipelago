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
        List<APItem> ProcessedItems = new List<APItem>(); // Name can be misleading - this is for all items
        List<APItem> AllItems = new List<APItem>(); // Name can be misleading - this is for units, items, charms. Not buildings and bells and stuff
        List<CardData> ItemPool = new List<CardData>();
        List<CardData> UnitPool = new List<CardData>();
        List<CardUpgradeData> CharmPool = new List<CardUpgradeData>();
        List<APItem> ItemsToAddOnLoad = new List<APItem>();
        List<string> UnlocksToSave = new List<string>();
        public char curTribe { 
            get {
                switch (References.PlayerData.classData.name)
                {
                    case "Basic":
                        return '1';
                    case "Magic":
                        return '2';
                    case "Clunk":
                        return '3';
                    default:
                        return '0';
                }
            }
        }
        public void LoadSave()
        {
            List<int> list = SaveSystem.LoadProgressData<List<int>>("ProcessedItems") ?? new List<int> { };
            foreach (int id in list)
            {
                ProcessedItems.Add(APItemConstants.GetItem(id));
            }
            List<int> list2 = SaveSystem.LoadProgressData<List<int>>("AllItems") ?? new List<int> { };
            foreach (int id in list2)
            {
                AllItems.Add(APItemConstants.GetItem(id));
            }
            if (Campaign.instance != null)
            {
                Logger.Log(LogType.Info, "trying to load pools");
                ItemPool = SaveSystem.LoadCampaignData<List<CardData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "ItemPool") ?? new List<CardData>();
                UnitPool = SaveSystem.LoadCampaignData<List<CardData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "UnitPool") ?? new List<CardData>();
                CharmPool = SaveSystem.LoadCampaignData<List<CardUpgradeData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "CharmPool") ?? new List<CardUpgradeData>();
                foreach (APItem item in ItemsToAddOnLoad)
                    AddToTribe(item);
            }
            UpdatePools(ServiceFactory.sessionManager.GetAllReceivedItems());
        }
        public void UpdatePools(List<APItem> list)
        {
            foreach (APItem item in list)
            {
                if (!ProcessedItems.Contains(item))
                {
                    UnlockData unlock;
                    switch (item.type)
                    {
                        case APItemType.building:
                            unlock = AddressableLoader.Get<UnlockData>("UnlockData", item.internalName);
                            UnlocksToSave.Add(unlock.name);
                            break;
                        case APItemType.tribe:
                            unlock = AddressableLoader.Get<UnlockData>("UnlockData", item.internalName);
                            UnlocksToSave.Add(unlock.name);
                            break;
                        case APItemType.pet:
                            unlock = AddressableLoader.Get<UnlockData>("UnlockData", item.internalName);
                            UnlocksToSave.Add(unlock.name);
                            break;
                        case APItemType.bell:
                            break;
                        case APItemType.trap_boon:
                            break;
                        default:
                            AllItems.Add(item);
                            if (Campaign.instance != null)
                                AddToTribe(item);
                            else
                                ItemsToAddOnLoad.Add(item);
                            break;
                    }
                    ProcessedItems.Add(item);
                }
            }
            if (UnlocksToSave.Count > 0)
                SaveUnlock();
            List<int> list2 = new List<int>() { };
            foreach (APItem item in ProcessedItems) {
                list2.Add(item.APID);
            }
            SaveSystem.SaveProgressData<List<int>>("ProcessedItems", list2);

            List<int> list3 = new List<int>() { };
            foreach (APItem item in AllItems)
            {
                list3.Add(item.APID);
            }
            SaveSystem.SaveProgressData<List<int>>("AllItems", list3);
        }
        public void AddToTribe(APItem item)
        {
            char tribeID = item.APID.ToString()[1];
            if (References.PlayerData == null)
                return;
            if (tribeID == curTribe || tribeID == '0')
                AddToPool(item);
        }
        public void AddToPool(APItem item)
        {
            switch (item.APID.ToString()[0])
            {
                case '5':
                    ItemPool.Add(AddressableLoader.Get<CardData>(typeof(CardData).Name, item.internalName));
                    break;
                case '6':
                    UnitPool.Add(AddressableLoader.Get<CardData>(typeof(CardData).Name, item.internalName));
                    break;
                case '7':
                    CharmPool.Add(AddressableLoader.Get<CardUpgradeData>(typeof(CardUpgradeData).Name, item.internalName));
                    break;
            }
        }
        public void PopulatePools()
        {
            foreach (APItem item in AllItems)
                AddToTribe(item);
        }
        public void PopulatePool(char type)
        {
            foreach (APItem item in AllItems)
                if (item.APID.ToString()[0] == type)
                    AddToTribe(item);
        }
        public CardData PullItem()
        {
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
        public async void SavePools()
        {
            if (Campaign.instance != null)
            {
                await Task.Delay(16);
                Logger.Log(LogType.Info, "SAVING POOLS");
                SaveSystem.SaveCampaignData<List<CardData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "ItemPool", ItemPool);
                SaveSystem.SaveCampaignData<List<CardData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "UnitPool", UnitPool);
                SaveSystem.SaveCampaignData<List<CardUpgradeData>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "CharmPool", CharmPool);
            }
        }
        public async void SaveUnlock()
        {
            await Task.Delay(16);
            List<string> list2 = SaveSystem.LoadProgressData<List<string>>("townNew", null) ?? new List<string>();
            List<string> list3 = SaveSystem.LoadProgressData<List<string>>("unlocked", null) ?? new List<string>();
            UnlocksToSave = UnlocksToSave.Where(name => !list3.Contains(name)).ToList();
            list2.AddRange(UnlocksToSave);
            list3.AddRange(UnlocksToSave);
            UnlocksToSave.Clear();
            SaveSystem.SaveProgressData<List<string>>("townNew", list2);
            SaveSystem.SaveProgressData<List<string>>("unlocked", list3);
        }
    }
}
