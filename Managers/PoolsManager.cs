using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Archipelago;
using Wildfrost_Archipelago.Constants;
using Wildfrost_Archipelago.Interfaces;

namespace Wildfrost_Archipelago.Managers
{
    public class PoolsManager
    {
        List<APItem> AllItems = new List<APItem>();
        List<CardData> ItemPool = new List<CardData>();
        List<CardData> UnitPool = new List<CardData>();
        List<CardUpgradeData> CharmPool = new List<CardUpgradeData>();
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
        }
        public void AddToTribe(APItem item)
        {
            if (References.PlayerData == null)
                return;
            switch (item.APID.ToString()[1])
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
                if (AllItems.Where(item => item.APID.ToString()[0] == '5').Count() <= 0)
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
                if (AllItems.Where(item => item.APID.ToString()[0] == '6').Count() <= 0)
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
                if (AllItems.Where(item => item.APID.ToString()[0] == '7').Count() <= 0)
                    return null;
                PopulatePool('7');
                return CharmPool.TakeRandom();
            }
        }
    }
}
