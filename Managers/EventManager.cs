using Archipelago.MultiClient.Net.Models;
using Deadpan.Enums.Engine.Components.Modding;
using FMODUnity;
using HarmonyLib;
using Rewired.ComponentControls;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine.TextCore;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.Utilities;
using UnityEngine.Localization.Tables;
using Wildfrost_Archipelago.Archipelago;
using Wildfrost_Archipelago.Constants;
using static ES3;
using static Mono.Security.X509.X520;

namespace Wildfrost_Archipelago.Managers
{
    public class EventManager
    {
        public void LoadEvents()
        {
            Logger.Log(LogType.Info, "Loading Events");
            Events.OnSceneLoaded += Events_OnSceneLoaded;
            Events.OnCampaignStart += Events_OnCampaignStart;
            Events.OnMapNodeSelect += Events_OnMapNodeSelect;
            Events.OnCardDataCreated += Events_OnCardDataCreated;
            Events.OnEntityEnterBackpack += Events_OnEntityEnterBackpack;
            Events.OnUpgradeGained += Events_OnUpgradeGained;
            Events.OnEventPopulated += Events_OnEventPopulated;
            Events.OnBattleWin += Events_OnBattleWin;
            Events.OnEntityKilled += Events_OnEntityKilled;
        }

        private void Events_OnSceneLoaded(UnityEngine.SceneManagement.Scene scene)
        {
            if (Town.instance != null)
            {
                Building[] array = Town.FindObjectsOfType<Building>(includeInactive: true);
                foreach (Building building in array)
                {
                    building.uncheckedUnlocks = null;
                }
            }
        }

        public void UnloadEvents()
        {
            Logger.Log(LogType.Info, "Unloading Events");
            Events.OnSceneLoaded -= Events_OnSceneLoaded;
            Events.OnCampaignStart -= Events_OnCampaignStart;
            Events.OnMapNodeSelect -= Events_OnMapNodeSelect;
            Events.OnCardDataCreated -= Events_OnCardDataCreated;
            Events.OnEntityEnterBackpack -= Events_OnEntityEnterBackpack;
            Events.OnUpgradeGained -= Events_OnUpgradeGained;
            Events.OnEventPopulated -= Events_OnEventPopulated;
            Events.OnBattleWin -= Events_OnBattleWin;
            Events.OnEntityKilled -= Events_OnEntityKilled;
           
            //if ((ServiceFactory.sessionManager as APSessionManager).SlotData != null && (ServiceFactory.sessionManager as APSessionManager).SlotData.Get<long>("fights_in_pool") != 0)
            //    Events.OnPreCampaignPopulate -= Events_OnPreCampaignPopulate;
        }

        public async void Events_OnEventPopulated(EventRoutine routine)
        {
            await Task.Delay(64);
            CreateCheckNames(routine);
        }
        private async void CreateCheckNames(EventRoutine routine)
        {
            if (!routine.node.data.ContainsKey("checks") || !routine.node.data.ContainsKey("entitiesToChange"))
                return;
            int[] checks = routine.node.data.Get<SaveCollection<int>>("checks").collection;
            if (checks.Length == 0)
                return;
            int i = 0;
            foreach (CardData card in routine.node.data.Get<List<CardData>>("entitiesToChange"))
            {
                while (checks[i] == -1 && i < checks.Length && i < 4)
                    i++;
                Card cardInstance = null;
                CardContainer[] containers = routine.GetComponentsInChildren<CardContainer>(true);
                foreach (CardContainer container in containers)
                {
                    if (container.Group.Any(entity => entity.data == card))
                    {
                        UpdateCheckInfo((container.Group.Where(entity => entity.data == card).ToList().First().display as Card), checks[i]);
                        await Task.Delay(16);
                        break;
                    }
                    else
                        Logger.Log(LogType.Warning, "No card found!");
                }
                i++;
            }
            routine.node.data["entitiesToChange"] = new List<CardData>();
        }
        public async void UpdateCheckInfo(Card cardInstance, int check)
        {
            CardData card = cardInstance.entity.data;
            card.flavour = check.ToString();
            Dictionary<long, ScoutedItemInfo> dict = (await ServiceFactory.sessionManager.GetLocationData(check));
            ScoutedItemInfo info = dict.First().Value;
            card.forceTitle = info.LocationDisplayName;
            Logger.Log(LogType.Info, info.LocationDisplayName);
            Logger.Log(LogType.Info, info.ItemDisplayName);
            StringTable collection = LocalizationHelper.GetCollection("Cards", new LocaleIdentifier(UnityEngine.SystemLanguage.English));
            collection.SetString("AP_desc_" + card.flavour.ToString() , "Send " + info.ItemDisplayName + " to " + info.Player);
            card.textKey = collection.GetString("AP_desc_" + card.flavour.ToString());
            cardInstance.Reset();
            cardInstance.entity.ClearStatuses();
            //await Task.Delay(1000);
            //cardInstance.StartCoroutine(cardInstance.UpdateData());
            cardInstance.SetName();
            cardInstance.SetDescription();
        }

        private void Events_OnCardDataCreated(CardData card)
        {
            if (card.name == AssetManager.fullItemName ||
                card.name == AssetManager.fullUnitName)
            {
                //Logger.Log(LogType.Info, "Modifying an Archifact card");
                CampaignNode node = Campaign.FindCharacterNode(References.Player);
                if (node != null)
                {
                    List<CardData> list = new List<CardData>();
                    if (node.data.ContainsKey("entitiesToChange"))
                    {
                        list = node.data.Get<List<CardData>>("entitiesToChange");
                        list.Add(card);
                        node.data["entitiesToChange"] = list;
                    }
                    else
                    {
                        list.Add(card);
                        node.data.Add("entitiesToChange", list);
                    }
                }
            }
        }

        private void Events_OnMapNodeSelect(MapNode node)
        {
            if (node == null || node.campaignNode == null)
                return;
            switch (node.campaignNode.type)
            {
                case CampaignNodeTypeItem item:
                    ManageItemNode(node);
                    break;
                case CampaignNodeTypeShop shop:
                    ManageShopNode(node);
                    break;
                case CampaignNodeTypeCharm charm:
                    ManageCharmNode(node);
                    break;
                case CampaignNodeTypeCompanion companion:
                    ManageCompanionNode(node);
                    break;
                case CampaignNodeTypeCharmShop charmShop:
                    ManageCharmShopNode(node);
                    break;
                case CampaignNodeTypeCurseItems gnome:
                    ManageGnomeNode(node);
                    break;
                case CampaignNodeTypeBoss boss:
                    ManageBossNode(node);
                    break;
            }
        }

        private void Events_OnUpgradeGained(CardUpgradeData charm)
        {
            // Works for charms
            Logger.Log(LogType.Info, $"Charm Found: {charm.title} : {charm.name}");
            if (charm.name == AssetManager.fullCharmName)
            {
                List<int> possibleLocations = ServiceFactory.sessionManager.GetRepeatableLocations('7', ServiceFactory.poolsManager.curTribe);
                if (possibleLocations.Count == 0)
                    return;
                int[] locations = new int[] { possibleLocations.RandomItem() };
                ServiceFactory.sessionManager.SendLocationsFound(locations);
                References.PlayerData.inventory.upgrades.Remove(charm);
            }
        }

        private void Events_OnEntityEnterBackpack(Entity card)
        {
            // Works for cards, not charms
            Logger.Log(LogType.Info, $"Entity Entered Backpack: {card.data?.name ?? "Not a card"}");
            if (card.data.name == AssetManager.fullItemName ||
                card.data.name == AssetManager.fullUnitName)
            {
                if (card.data.flavour != "")
                {
                    int[] locations = new int[1];
                    locations[0] = Convert.ToInt32(card.data.flavour);
                    ServiceFactory.sessionManager.SendLocationsFound(locations);
                }
                References.PlayerData.inventory.deck.Remove(card.data);
            }
        }

        private void Events_OnCampaignStart()
        {
            ServiceFactory.poolsManager.PopulatePools();
        }

        /*public void Events_OnPreCampaignPopulate()
        {
            List<BattleLockData> locks = new List<BattleLockData>();
            foreach (BattleData battle in AddressableLoader.GetGroup<BattleData>("BattleData"))
            {
                // Checking if semi-vanilla is enabled and cancelling if the fight is not a vanilla-locked fight
                if ((ServiceFactory.sessionManager as APSessionManager).SlotData.Get<long>("fights_in_pool") == 1)
                {
                    if (battle.name != "BabyBerries" && battle.name != "Bombers" && battle.name != "Shroomers" && battle.name != "Spikers" && battle.name != "Inkers" && battle.name != "Mimiks")
                        continue;
                }
                ChallengeData chal = new ChallengeData();
                chal.name = battle.name;

                BattleLockData lockData = new BattleLockData();
                lockData.battleName = battle.name;
                lockData.linkToChallenge = chal;
                locks.Add(lockData);
            }
            Campaign.Data.GameMode.populator.battleLockers = locks.ToArray();
        }*/

        #region Utility Functions
        private void ManageItemNode(MapNode node)
        {
            if (node.campaignNode.data.ContainsKey("AP_mod"))
                return;
            string name = AssetManager.fullItemName;
            Random rand = new Random();
            List<int> possibleChecks = ServiceFactory.sessionManager.GetRepeatableLocations('5', ServiceFactory.poolsManager.curTribe);
            int[] checksAdded = { -1, -1, -1 };
            SaveCollection<int> checksCollection = new SaveCollection<int>();
            SaveCollection<string> saveCollection = node.campaignNode.data.Get<SaveCollection<string>>("cards");
            foreach (int num3 in saveCollection.collection.GetIndices<string>().InRandomOrder<int>())
            {
                CardData item = ServiceFactory.poolsManager.PullItem();
                if ((rand.Next(0, 100) >= 50 || item == null) && possibleChecks.Count() > 0)
                {
                    saveCollection[num3] = name;
                    checksAdded[num3] = possibleChecks.TakeRandom();
                    ServiceFactory.poolsManager.ForceAdd(item, '5');
                }
                else if (item != null)
                    saveCollection[num3] = item.name;
                else
                    saveCollection[num3] = name;

                if (node.campaignNode.data.ContainsKey(string.Format("upgrades{0}", num3)))
                    node.campaignNode.data.Remove(string.Format("upgrades{0}", num3));
            }
            checksCollection.collection = checksAdded;
            node.campaignNode.data.Add("checks", checksCollection);
            node.campaignNode.data.Add("AP_mod", true);
        }

        private void ManageShopNode(MapNode node)
        {
            if (node.campaignNode.data.ContainsKey("AP_mod"))
                return;
            string itemName = AssetManager.fullItemName;
            string charmName = AssetManager.fullCharmName;
            Random rand = new Random();
            List<int> possibleCardChecks = ServiceFactory.sessionManager.GetRepeatableLocations('5', ServiceFactory.poolsManager.curTribe);
            List<int> possibleCharmChecks = ServiceFactory.sessionManager.GetRepeatableLocations('7', ServiceFactory.poolsManager.curTribe);

            int[] checksAdded = { -1, -1, -1, -1 };
            SaveCollection<int> checksCollection = new SaveCollection<int>();
            ShopRoutine.Data data = node.campaignNode.data.Get<ShopRoutine.Data>("shopData");

            foreach (ShopRoutine.Item card in data.items)
            {
                int i = data.items.IndexOf(card);
                CardData item = ServiceFactory.poolsManager.PullItem();
                if ((rand.Next(0, 100) >= 50 || item == null) && possibleCardChecks.Count() > 0)
                {
                    card.cardDataName = itemName;
                    checksAdded[0] = possibleCardChecks.TakeRandom();
                    ServiceFactory.poolsManager.ForceAdd(item, '5');
                }
                else if (item != null)
                    card.cardDataName = item.name;
                else
                    card.cardDataName = itemName;
            }
            Dictionary<int, string> charmsToChange = new Dictionary<int, string>(){ };
            foreach (string charmItem in data.charms)
            {
                int i = data.charms.IndexOf(charmItem);
                CardUpgradeData charm = ServiceFactory.poolsManager.PullCharm();
                if ((rand.Next(0, 100) >= 50 || charm == null) && possibleCharmChecks.Count() > 0)
                {
                    charmsToChange.Add(i, charmName);
                    ServiceFactory.poolsManager.ForceAdd(charm, '7');
                }
                else if (charm != null)
                    charmsToChange.Add(i, charm.name);
                else
                    charmsToChange.Add(i, charmName);
            }
            foreach (KeyValuePair<int, string> pair in charmsToChange)
                data.charms[pair.Key] = pair.Value;
            checksCollection.collection = checksAdded;
            node.campaignNode.data.Add("checks", checksCollection);
            node.campaignNode.data.Add("AP_mod", true);
        }

        private void ManageCompanionNode(MapNode node)
        {
            if (node.campaignNode.data.ContainsKey("AP_mod"))
                return;
            string name = AssetManager.fullUnitName;
            Random rand = new Random();
            List<int> possibleChecks = ServiceFactory.sessionManager.GetRepeatableLocations('6', ServiceFactory.poolsManager.curTribe);
            int[] checksAdded = { -1, -1, -1 };
            SaveCollection<int> checksCollection = new SaveCollection<int>();
            SaveCollection<string> saveCollection = node.campaignNode.data.Get<SaveCollection<string>>("cards");
            foreach (int num3 in saveCollection.collection.GetIndices<string>().InRandomOrder<int>())
            {
                CardData unit = ServiceFactory.poolsManager.PullUnit();
                if ((rand.Next(0, 100) >= 50 || unit == null) && possibleChecks.Count() > 0)
                {
                    saveCollection[num3] = name;
                    checksAdded[num3] = possibleChecks.TakeRandom();
                    ServiceFactory.poolsManager.ForceAdd(unit, '6');
                }
                else if (unit != null)
                    saveCollection[num3] = unit.name;
                else
                    saveCollection[num3] = name;

                if (node.campaignNode.data.ContainsKey(string.Format("upgrades{0}", num3)))
                    node.campaignNode.data.Remove(string.Format("upgrades{0}", num3));
            }
            checksCollection.collection = checksAdded;
            node.campaignNode.data.Add("checks", checksCollection);
            node.campaignNode.data.Add("AP_mod", true);
        }

        private void ManageCharmNode(MapNode node)
        {
            if (node.campaignNode.data.ContainsKey("AP_mod"))
                return;
            List<int> possibleChecks = ServiceFactory.sessionManager.GetRepeatableLocations('7', ServiceFactory.poolsManager.curTribe);
            string name = AssetManager.fullCharmName;
            Random rand = new Random();
            CardUpgradeData charm = ServiceFactory.poolsManager.PullCharm();
            if ((rand.Next(0, 100) >= 50 || charm == null) && possibleChecks.Count() > 0)
            {
                node.campaignNode.data["charm"] = name;
                ServiceFactory.poolsManager.ForceAdd(charm, '7');
            }
            else if (charm != null)
                node.campaignNode.data["charm"] = charm.name;
            else
                node.campaignNode.data["charm"] = name;
            node.campaignNode.data.Add("AP_mod", true);
        }

        private void ManageGnomeNode(MapNode node)
        {
            if (node.campaignNode.data.ContainsKey("AP_mod"))
                return;
            string name = AssetManager.fullItemName;
            Random rand = new Random();
            List<int> possibleChecks = ServiceFactory.sessionManager.GetRepeatableLocations('5', ServiceFactory.poolsManager.curTribe);
            int[] checksAdded = { -1, -1, -1 };
            SaveCollection<int> checksCollection = new SaveCollection<int>();
            SaveCollection<string> saveCollection = node.campaignNode.data.Get<SaveCollection<string>>("cards");
            foreach (int num3 in saveCollection.collection.GetIndices<string>().InRandomOrder<int>())
            {
                CardData item = ServiceFactory.poolsManager.PullItem();
                if ((rand.Next(0, 100) >= 50 || item == null) && possibleChecks.Count() > 0)
                {
                    saveCollection[num3] = name;
                    checksAdded[num3] = possibleChecks.TakeRandom();
                    ServiceFactory.poolsManager.ForceAdd(item, '5');
                }
                else if (item != null)
                    saveCollection[num3] = item.name;
                else
                    saveCollection[num3] = name;

                if (node.campaignNode.data.ContainsKey(string.Format("upgrades{0}", num3)))
                    node.campaignNode.data.Remove(string.Format("upgrades{0}", num3));
            }
            checksCollection.collection = checksAdded;
            node.campaignNode.data.Add("checks", checksCollection);
            node.campaignNode.data.Add("AP_mod", true);
        }

        private void ManageCharmShopNode(MapNode node)
        {
            if (node.campaignNode.data.ContainsKey("AP_mod"))
                return;
            string itemName = AssetManager.fullItemName;
            string charmName = AssetManager.fullCharmName;
            Random rand = new Random();
            List<int> possibleCardChecks = ServiceFactory.sessionManager.GetRepeatableLocations('5', ServiceFactory.poolsManager.curTribe);
            List<int> possibleCharmChecks = ServiceFactory.sessionManager.GetRepeatableLocations('7', ServiceFactory.poolsManager.curTribe);

            int[] checksAdded = { -1 };
            SaveCollection<int> checksCollection = new SaveCollection<int>();
            EventRoutineCharmShop.Data data = node.campaignNode.data.Get<EventRoutineCharmShop.Data>("data");

            foreach (EventRoutineCharmShop.UpgradedCard card in data.cards)
            {
                CardData item = ServiceFactory.poolsManager.PullItem();
                if ((rand.Next(0, 100) >= 50 || item == null) && possibleCardChecks.Count() > 0)
                {
                    card.cardDataName = itemName;
                    card.upgradeNames = new string[0];
                    checksAdded[0] = possibleCardChecks.TakeRandom();
                    ServiceFactory.poolsManager.ForceAdd(item, '5');
                }
                else if (item != null)
                {
                    card.cardDataName = item.name;
                    card.upgradeNames = new string[0];
                }
                else
                {
                    card.cardDataName = itemName;
                    card.upgradeNames = new string[0];
                }

                if (node.campaignNode.data.ContainsKey(string.Format("upgrades{0}", 0)))
                    node.campaignNode.data.Remove(string.Format("upgrades{0}", 0));
            }

            foreach (EventRoutineCharmShop.CharmShopItemData item in data.items)
            {
                int i = node.campaignNode.data.Get<EventRoutineCharmShop.Data>("data").items.IndexOf(item);
                CardUpgradeData charm = ServiceFactory.poolsManager.PullCharm();
                if ((rand.Next(0, 100) >= 50 || charm == null) && possibleCharmChecks.Count() > 0)
                {
                    item.upgradeDataName = charmName;
                    ServiceFactory.poolsManager.ForceAdd(charm, '7');
                }
                else if (charm != null)
                    item.upgradeDataName = charm.name;
                else
                    item.upgradeDataName = charmName;
            }

            checksCollection.collection = checksAdded;
            node.campaignNode.data.Add("checks", checksCollection);
            node.campaignNode.data.Add("AP_mod", true);
        }

        private void ManageBossNode(MapNode node)
        {
            if (node.campaignNode.data.ContainsKey("AP_mod"))
                return;
            string name = AssetManager.fullCharmName;
            Random rand = new Random();
            List<int> possibleChecks = ServiceFactory.sessionManager.GetRepeatableLocations('8', ServiceFactory.poolsManager.curTribe);
            foreach (BossRewardData.Data reward in node.campaignNode.data.Get<CampaignNodeTypeBoss.RewardData>("rewards").rewards)
            {
                int check = -1;
                switch (reward.type)
                {
                    case BossRewardData.Type.Charm:
                        CardUpgradeData charm = ServiceFactory.poolsManager.PullCharm();
                        Logger.Log(LogType.Warning, ((reward as BossRewardDataRandomCharm.Data) != null).ToString());
                        if ((rand.Next(0, 100) >= 50 || charm == null) && possibleChecks.Count() > 0)
                        {
                            ((BossRewardDataRandomCharm.Data)reward).upgradeName = name;
                        }
                        else if (charm != null)
                            ((BossRewardDataRandomCharm.Data)reward).upgradeName = charm.name;
                        else
                            ((BossRewardDataRandomCharm.Data)reward).upgradeName = name;
                        break;
                    case BossRewardData.Type.Modifier:
                        GameModifierData bell = ServiceFactory.poolsManager.PullBell();
                        Logger.Log(LogType.Warning, ((reward as BossRewardDataModifier.Data) != null).ToString());
                        if (bell != null)
                            ((BossRewardDataModifier.Data)reward).modifierName = bell.name;
                        break;
                    default:
                        break;
                }
            }
            node.campaignNode.data.Add("AP_mod", true);

        }
        private async void Events_OnBattleWin()
        {
            if (!Campaign.FindCharacterNode(References.Player).type.isBattle)
                return;
            CampaignNode battleNode = Campaign.FindCharacterNode(References.Player);
            string battleName = battleNode.data.Get<string>("battle");
            int[] locations = { };
            foreach (APLocation loc in  APLocationConstants.LocationReferences.Values.Where(location => location.internalName == battleName))
            {
                locations = locations.Append(loc.id).ToArray();
            }
            if (locations.Length > 0)
                ServiceFactory.sessionManager.SendLocationsFound(locations);
            if (battleName == "Final Battle" && (ServiceFactory.sessionManager as APSessionManager).SlotData.Get<long>("goal") == 0)
            {
                await Task.Delay(16);
                ServiceFactory.sessionManager.SetGoalAchieved();
            }
            else if (battleName == "Final Final Battle" && (ServiceFactory.sessionManager as APSessionManager).SlotData.Get<long>("goal") == 1)
            {
                await Task.Delay(16);
                ServiceFactory.sessionManager.SetGoalAchieved();
            }
            //battleTier is between 0 and 8
            //areaIndex is between 0 and 2

            //is 1 or 3 (Fights. Fights and Acts)
            APSessionManager manager = (ServiceFactory.sessionManager as APSessionManager);
            if (manager.SlotData.Get<long>("fight_gating") % 2 != 0)
            {
                int maxTier = manager.GetAllReceivedItems().Where(item => item.displayName == "Progressive Fight").Count();
                if (battleNode.tier >= maxTier)
                    References.Campaign.End(Campaign.Result.Restart);
            }
            //is 2 or 3 (Acts. Fights and Acts)
            if (manager.SlotData.Get<long>("fight_gating") >= 2)
            {
                int maxArea = manager.GetAllReceivedItems().Where(item => item.displayName == "Progressive Act").Count();
                if (battleNode.areaIndex >= maxArea)
                    References.Campaign.End(Campaign.Result.Restart);
            }
        }
        private void Events_OnEntityKilled(Entity entity, DeathType type)
        {
            if (entity.owner == Battle.GetOpponent(References.Player))
            {
                if (APLocationConstants.LocationReferences.Values.Where(location => location.internalName == entity.data.name).Count() > 0)
                    ServiceFactory.sessionManager.SendLocationsFound(new int[]{APLocationConstants.LocationReferences.Values.Single(location => location.internalName == entity.data.name).id});
            }
        }

        private void tempPrompt(string text)
        {
            Logger.Log(LogType.Info, text);
            PromptSystem.Prompt.SetText(text);
            PromptSystem.Create(Prompt.Anchor.Mid, 0, 0, 3, Prompt.Emote.Type.Basic, Prompt.Emote.Position.Above);
        }
        #endregion
    }
}
