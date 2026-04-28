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
        List<GameModifierData> BellPool = new List<GameModifierData>();
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
            /*List<int> list = SaveSystem.LoadProgressData<List<int>>("ProcessedItems") ?? new List<int> { };
            foreach (int id in list)
            {
                if (!ProcessedItems.Contains(APItemConstants.GetItem(id)) || id.ToString()[0] == '9')
                    ProcessedItems.Add(APItemConstants.GetItem(id));
            }
            List<int> list2 = SaveSystem.LoadProgressData<List<int>>("AllItems") ?? new List<int> { };
            foreach (int id in list2)
            {
                if (!AllItems.Contains(APItemConstants.GetItem(id)))
                    AllItems.Add(APItemConstants.GetItem(id));
            }*/
            if (Campaign.instance != null)
            {
                Logger.Log(LogType.Info, "trying to load pools");
                List<int> ItemIDs = SaveSystem.LoadCampaignData<List<int>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "ItemPool") ?? new List<int>();
                foreach (int id in ItemIDs)
                    AddToPool(AllItems.Single(item => item.APID == id));
                List<int> UnitIDs = SaveSystem.LoadCampaignData<List<int>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "UnitPool") ?? new List<int>();
                foreach (int id in UnitIDs)
                    AddToPool(AllItems.Single(item => item.APID == id));
                List<int> CharmIDs = SaveSystem.LoadCampaignData<List<int>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "CharmPool") ?? new List<int>();
                foreach (int id in CharmIDs)
                    AddToPool(AllItems.Single(item => item.APID == id));
                List<int> BellIDs = SaveSystem.LoadCampaignData<List<int>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "BellPool") ?? new List<int>();
                foreach (int id in BellIDs)
                    AddToPool(ProcessedItems.Single(item => item.APID == id));
                foreach (APItem item in ItemsToAddOnLoad)
                    AddToTribe(item);
            }
        }
        public void UpdatePools(List<APItem> list)
        {
            foreach (APItem item in list)
            {
                Logger.Log(LogType.Info,"Processing item of ID " + item.APID);
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
                        if (Campaign.instance != null)
                            AddToPool(item);
                        break;
                    case APItemType.trap_boon:
                        break;
                    case APItemType.progressive:
                        break;
                    default:
                        if (AllItems.Contains(item))
                            break;
                        AllItems.Add(item);
                        if (Campaign.instance != null)
                            AddToTribe(item);
                        else
                            ItemsToAddOnLoad.Add(item);
                        break;
                }
                ProcessedItems.Add(item);
                //condition no longer needed because processed items isn't saved, and is instead always taking from "all processed items" on load and from "item received" mid-game.
                //if (!ProcessedItems.Contains(item) || item.type == APItemType.trap_boon || item.displayName.StartsWith("Progressive"))
                //{
                //}
            }
            if (UnlocksToSave.Count > 0)
                SaveUnlock();
            /*List<int> list2 = new List<int>() { };
            foreach (APItem item in ProcessedItems) {
                list2.Add(item.APID);
            }
            SaveSystem.SaveProgressData<List<int>>("ProcessedItems", list2);

            List<int> list3 = new List<int>() { };
            foreach (APItem item in AllItems)
            {
                list3.Add(item.APID);
            }
            SaveSystem.SaveProgressData<List<int>>("AllItems", list3);*/
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
                    ItemPool.Add(AddressableLoader.Get<CardData>("CardData", item.internalName));
                    break;
                case '6':
                    UnitPool.Add(AddressableLoader.Get<CardData>("CardData", item.internalName));
                    break;
                case '7':
                    CharmPool.Add(AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", item.internalName));
                    break;
                case '8':
                    BellPool.Add(AddressableLoader.Get<GameModifierData>("GameModifierData", item.internalName));
                    break;
            }
        }
        public void PopulatePools()
        {
            foreach (APItem item in ProcessedItems)
            {
                if (AllItems.Contains(item))
                    AddToTribe(item);
                if (item.type == APItemType.bell)
                    AddToPool(item);
            }
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
        public GameModifierData PullBell()
        {
            if (BellPool.Count > 0)
                return BellPool.TakeRandom();
            else
                return null; //Bells don't repopulate
        }
        public void ForceAdd(DataFile data, char type)
        {
            if (data == null)
                return;
            switch (type)
            {
                case '5':
                    ItemPool.Add(data as CardData);
                    break;
                case '6':
                    UnitPool.Add(data as CardData);
                    break;
                case '7':
                    CharmPool.Add(data as CardUpgradeData);
                    break;
            }
        }
        public async void SavePools()
        {
            if (Campaign.instance != null)
            {
                await Task.Delay(16);
                Logger.Log(LogType.Info, "SAVING POOLS");
                List<int> ItemIDs = new List<int>();
                foreach (CardData card in ItemPool)
                    ItemIDs.Add(AllItems.Single(item => item.internalName == card.name).APID);
                List<int> UnitIDs = new List<int>();
                foreach (CardData card in UnitPool)
                    UnitIDs.Add(AllItems.Single(item => item.internalName == card.name).APID);
                List<int> CharmIDs = new List<int>();
                foreach (CardUpgradeData charm in CharmPool)
                    CharmIDs.Add(AllItems.Single(item => item.internalName == charm.name).APID);
                List<int> BellIDs = new List<int>();
                foreach (GameModifierData bell in BellPool)
                    BellIDs.Add(ProcessedItems.Single(item => item.internalName == bell.name).APID);
                SaveSystem.SaveCampaignData<List<int>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "ItemPool", ItemIDs);
                SaveSystem.SaveCampaignData<List<int>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "UnitPool", UnitIDs);
                SaveSystem.SaveCampaignData<List<int>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "CharmPool", CharmIDs);
                SaveSystem.SaveCampaignData<List<int>>(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), "BellPool", BellIDs);
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
