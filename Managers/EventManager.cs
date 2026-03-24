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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine.Localization.SmartFormat.Utilities;
using static ES3;

namespace Wildfrost_Archipelago.Managers
{
    public class EventManager
    {
        public void LoadEvents()
        {
            Logger.Log(LogType.Info, "Loading Events");
            Events.OnCampaignStart += Events_OnCampaignStart;
            Events.OnMapNodeSelect += Events_OnMapNodeSelect;
            //Events.OnCardDataCreated += Events_OnCardDataCreated;
            Events.OnEntityEnterBackpack += Events_OnEntityEnterBackpack;
            Events.OnUpgradeGained += Events_OnUpgradeGained;
            //Events.OnEventPopulated += Events_OnEventPopulated;
        }

        public void UnloadEvents()
        {
            Logger.Log(LogType.Info, "Unloading Events");
            Events.OnCampaignStart -= Events_OnCampaignStart;
            Events.OnMapNodeSelect -= Events_OnMapNodeSelect;
            //Events.OnCardDataCreated -= Events_OnCardDataCreated;
            Events.OnEntityEnterBackpack -= Events_OnEntityEnterBackpack;
            Events.OnUpgradeGained -= Events_OnUpgradeGained;
            //Events.OnEventPopulated -= Events_OnEventPopulated;
        }

        public void Events_OnEventPopulated(EventRoutine routine)
        {
            int[] checks;
            string[] cards;
            int i = 0;
            Logger.Log(LogType.Info, routine.GetType().Name);
            switch (routine.GetType().Name)
            {
                case "ShopRoutine":
                    checks = routine.node.data.Get<SaveCollection<int>>("checks").collection;
                    ShopRoutine.Data data = routine.node.data.Get<ShopRoutine.Data>("shopData");
                    foreach (Entity entity in (routine as ShopRoutine).entities)
                    {
                        if (checks[i] != -1)
                            UpdateCheckInfo(entity, checks[i]);
                        i++;
                    }
                    break;
                case "EventRoutineCharmShop":
                    checks = routine.node.data.Get<SaveCollection<int>>("checks").collection;
                    EventRoutineCharmShop.Data data2 = routine.data.Get<EventRoutineCharmShop.Data>("data");
                    foreach (Entity entity in (routine as EventRoutineCharmShop).cardContainer.entities)
                    {
                        if (checks[i] != -1)
                            UpdateCheckInfo(entity, checks[i]);
                        i++;
                    }
                    break;
                case "EventRoutineCompanion":
                    checks = routine.node.data.Get<SaveCollection<int>>("checks").collection;
                    cards = routine.node.data.Get<SaveCollection<string>>("cards").collection;
                    foreach (Entity entity in (routine as EventRoutineCompanion).cardContainer.entities)
                    {
                        if (checks[i] != -1)
                            UpdateCheckInfo(entity, checks[i]);
                        i++;
                    }
                    break;
                case "ItemEventRoutine":
                    checks = routine.node.data.Get<SaveCollection<int>>("checks").collection;
                    cards = routine.node.data.Get<SaveCollection<string>>("cards").collection;
                    foreach (Entity entity in (routine as ItemEventRoutine).cardContainer.entities)
                    {
                        if (checks[i] != -1)
                            UpdateCheckInfo(entity, checks[i]);
                        i++;
                    }
                    break;
                case "EventRoutineGnomeShop":
                    checks = routine.node.data.Get<SaveCollection<int>>("checks").collection;
                    cards = routine.node.data.Get<SaveCollection<string>>("cards").collection;
                    foreach (Entity entity in (routine as EventRoutineGnomeShop).cardContainer.entities)
                    {
                        if (checks[i] != -1)
                            UpdateCheckInfo(entity, checks[i]);
                        i++;
                    }
                    break;
                default:
                    break;
            }
        }
        public async void UpdateCheckInfo(Entity entity, int check)
        {
            CardData card = entity.data;
            ScoutedItemInfo info = (await ServiceFactory.sessionManager.GetLocationData(check))[(long)check];
            card.forceTitle = info.LocationDisplayName;
            card.attackEffects.First().data = card.attackEffects.First().data.InstantiateKeepName();
            card.attackEffects.First().data.textInsert = "Send " + info.ItemDisplayName + " to " + info.Player;
        }

        /*private async void Events_OnCardDataCreated(CardData card)
        {
            if (card.name == AssetManager.fullItemName ||
                card.name == AssetManager.fullUnitName)
            {
                Logger.Log(LogType.Info, "Modifying an Archifact card");
                CampaignNode node = Campaign.FindCharacterNode(References.Player);
                if (node != null)
                {
                    int[] checks;
                    string[] cards;
                    switch (node.type)
                    {
                        case CampaignNodeTypeShop shop:
                            checks = node.data.Get<SaveCollection<int>>("checks").collection;
                            ShopRoutine.Data data = node.data.Get<ShopRoutine.Data>("shopData");
                            foreach (int check in checks)
                            {
                                int i = checks.ToList().IndexOf(check);
                                if (check != -1 && data.items[i].cardDataName == card.name)
                                {
                                    card.flavour = check.ToString();
                                    data.items[i] = null;
                                    break;
                                }
                            }
                            break;
                        case CampaignNodeTypeCharmShop charmShop:
                            checks = node.data.Get<SaveCollection<int>>("checks").collection;
                            cards = new string[]{node.data.Get<EventRoutineCharmShop.Data>("data").cards[0].cardDataName};
                            if (checks[0] != -1)
                            {
                                card.flavour = checks[0].ToString();
                            }
                            break;
                        default:
                            checks = node.data.Get<SaveCollection<int>>("checks").collection;
                            cards = node.data.Get<SaveCollection<string>>("cards").collection;
                            foreach (int check in checks)
                            {
                                int i = checks.ToList().IndexOf(check);
                                if (check != -1 && cards[i] == card.name)
                                {
                                    card.flavour = check.ToString();
                                    cards[i] = null;
                                    break;
                                }
                            }
                            break;
                    }
                    if (card.flavour == null || (card.flavour[0] != '6' && card.flavour[0] != '5'))
                        return;
                    ScoutedItemInfo info = (await ServiceFactory.sessionManager.GetLocationData(Convert.ToInt32(card.flavour)))[Convert.ToInt64(card.flavour)];
                    card.forceTitle = info.LocationDisplayName;
                    card.attackEffects.First().data = card.attackEffects.First().data.InstantiateKeepName(); 
                    card.attackEffects.First().data.textInsert = "Send " + info.ItemDisplayName + " to " + info.Player;
                }
            }
        } */

        private void Events_OnMapNodeSelect(MapNode node)
        {
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
            }
        }

        private void Events_OnUpgradeGained(CardUpgradeData charm)
        {
            // Works for charms
            Logger.Log(LogType.Info, $"Charm Found: {charm.title} : {charm.name}");
            int[] locations = new int[1];
            if (charm.name == AssetManager.fullCharmName)
            {
                CampaignNode node = Campaign.FindCharacterNode(References.Player);
                if (node != null)
                {
                    int[] checks;
                    List<string> charms = new List<string>();
                    switch (node.type)
                    {
                        case CampaignNodeTypeCharm charmNode:
                            checks = node.data.Get<SaveCollection<int>>("checks").collection;
                            foreach (int check in checks)
                            {
                                if (check != -1)
                                {
                                    locations[0] = check;
                                    break;
                                }
                            }
                            break;
                        case CampaignNodeTypeShop shop:
                            checks = node.data.Get<SaveCollection<int>>("checks").collection;
                            ShopRoutine.Data data = node.data.Get<ShopRoutine.Data>("data");
                            foreach (string charmName in data.charms)
                            {
                                int i = data.charms.IndexOf(charmName);
                                if (checks[4 + i] != -1 && charm.name == charmName)
                                {
                                    locations[0] = checks[4+i];
                                    break;
                                }
                            }
                            break;
                        case CampaignNodeTypeCharmShop charmShop:
                            checks = node.data.Get<SaveCollection<int>>("checks").collection;
                            foreach (EventRoutineCharmShop.CharmShopItemData item in node.data.Get<EventRoutineCharmShop.Data>("data").items)
                            {
                                int i = node.data.Get<EventRoutineCharmShop.Data>("data").items.IndexOf(item);
                                if (checks[1 + i] != -1 && charm.name == item.upgradeDataName)
                                {
                                    locations[0] = checks[1+i];
                                    break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                ServiceFactory.sessionManager.SendLocationsFound(locations);
                References.PlayerData.inventory.upgrades.Remove(charm);
            }
        }

        private void Events_OnEntityEnterBackpack(Entity card)
        {
            // Works for cards, not charms
            Logger.Log(LogType.Info, $"Entity Entered Backpack: {card.data?.name ?? "Not a card"}");
            int[] locations = new int[1];
            locations[0] = Convert.ToInt32(card.data.flavour);
            ServiceFactory.sessionManager.SendLocationsFound(locations);
            References.PlayerData.inventory.deck.Remove(card.data);
        }

        private void Events_OnCampaignStart()
        {
            ServiceFactory.poolsManager.PopulatePools();
        }

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

            int[] checksAdded = { -1, -1, -1, -1, -1, -1, -1 };
            SaveCollection<int> checksCollection = new SaveCollection<int>();
            ShopRoutine.Data data = node.campaignNode.data.Get<ShopRoutine.Data>("data");

            foreach (ShopRoutine.Item card in data.items)
            {
                int i = data.items.IndexOf(card);
                CardData item = ServiceFactory.poolsManager.PullItem();
                if ((rand.Next(0, 100) >= 50 || item == null) && possibleCardChecks.Count() > 0)
                {
                    card.cardDataName = itemName;
                    checksAdded[0] = possibleCardChecks.TakeRandom();
                }
                else if (item != null)
                    card.cardDataName = item.name;
                else
                    card.cardDataName = itemName;
            }

            foreach (string charmItem in data.charms)
            {
                int i = data.charms.IndexOf(charmItem);
                CardUpgradeData charm = ServiceFactory.poolsManager.PullCharm();
                if ((rand.Next(0, 100) >= 50 || charm == null) && possibleCharmChecks.Count() > 0)
                {
                    data.charms[i] = charmName;
                    checksAdded[4 + i] = possibleCardChecks.TakeRandom();
                }
                else if (charm != null)
                    data.charms[i] = charm.name;
                else
                    data.charms[i] = charmName;
            }
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
                node.campaignNode.data["check"] = possibleChecks.TakeRandom();
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

            int[] checksAdded = { -1, -1, -1, -1 };
            SaveCollection<int> checksCollection = new SaveCollection<int>();
            EventRoutineCharmShop.Data data = node.campaignNode.data.Get<EventRoutineCharmShop.Data>("data");

            foreach (EventRoutineCharmShop.UpgradedCard card in data.cards)
            {
                CardData item = ServiceFactory.poolsManager.PullItem();
                if ((rand.Next(0, 100) >= 50 || item == null) && possibleCardChecks.Count() > 0)
                {
                    card.cardDataName = itemName;
                    checksAdded[0] = possibleCardChecks.TakeRandom();
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
                    checksAdded[1 + i] = possibleCardChecks.TakeRandom();
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

        private void tempPrompt(string text)
        {
            Logger.Log(LogType.Info, text);
            PromptSystem.Prompt.SetText(text);
            PromptSystem.Create(Prompt.Anchor.Mid, 0, 0, 3, Prompt.Emote.Type.Basic, Prompt.Emote.Position.Above);
        }
        #endregion
    }
}
