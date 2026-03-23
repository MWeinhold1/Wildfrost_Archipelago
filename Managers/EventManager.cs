using Deadpan.Enums.Engine.Components.Modding;
using FMODUnity;
using HarmonyLib;
using Rewired.ComponentControls;
using System;
using System.Collections.Generic;
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
            Events.OnCardDataCreated += Events_OnCardDataCreated;
            Events.OnEntityEnterBackpack += Events_OnEntityEnterBackpack;
            //Events.OnUpgradeGained += Events_OnUpgradeGained;
        }

        public void UnloadEvents()
        {
            Logger.Log(LogType.Info, "Unloading Events");
            Events.OnCampaignStart -= Events_OnCampaignStart;
            Events.OnMapNodeSelect -= Events_OnMapNodeSelect;
            Events.OnCardDataCreated -= Events_OnCardDataCreated;
            Events.OnEntityEnterBackpack -= Events_OnEntityEnterBackpack;
            //Events.OnUpgradeGained -= Events_OnUpgradeGained;
        }

        private void Events_OnCardDataCreated(CardData card)
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
                        case CampaignNodeTypeItem item:
                            checks = node.data.Get<SaveCollection<int>>("checks").collection;
                            cards = node.data.Get<SaveCollection<string>>("cards").collection;
                            foreach (int check in checks)
                            {
                                if (check != -1 && cards.Contains(card.name))
                                {
                                    card.flavour = checks[cards.ToList().IndexOf(card.name)].ToString();
                                }
                            }
                            break;
                        case CampaignNodeTypeShop shop:
                            break;
                        case CampaignNodeTypeCompanion companion:
                            checks = node.data.Get<SaveCollection<int>>("checks").collection;
                            cards = node.data.Get<SaveCollection<string>>("cards").collection;
                            foreach (int check in checks)
                            {
                                if (check != -1 && cards.Contains(card.name))
                                {
                                    card.flavour = checks[cards.ToList().IndexOf(card.name)].ToString();
                                }
                            }
                            break;
                        case CampaignNodeTypeCharmShop charmShop:
                            break;
                        case CampaignNodeTypeCurseItems gnome:
                            break;
                        default:
                            break;
                    }
                }
                card.forceTitle = "AP Item Name";
                card.attackEffects.First().data.textInsert = "Send <Sample Text> to <Other Player>";
            }
        }

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
                    //ManageCharmShopNode(node);
                    break;
                case CampaignNodeTypeCurseItems gnome:
                    //ManageGnomeNode(node);
                    break;
            }
        }

        private void Events_OnUpgradeGained(CardUpgradeData charm)
        {
            // Works for charms
            Logger.Log(LogType.Info, $"Charm Found: {charm.title} : {charm.name}");
            //if (charm.IsCharm())
            //{
            //    References.PlayerData.inventory.upgrades.Remove(charm);
            //}
        }

        private void Events_OnEntityEnterBackpack(Entity card)
        {
            // Works for cards, not charms
            Logger.Log(LogType.Info, $"Entity Entered Backpack: {card.data?.name ?? "Not a card"}");
            ClassData tribe = References.PlayerData.classData;
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
            var shopData = node.campaignNode.data["shopData"] as ShopRoutine.Data;
            shopData.charms = new List<string> { AssetManager.fullCharmName, AssetManager.fullCharmName, AssetManager.fullCharmName };
            var newShopItems = new List<ShopRoutine.Item>();
            var rand = new Random();
            for(int i = 0; i < 4; i++)
            {
                var item = new ShopRoutine.Item();
                if (newShopItems.Count == 0)
                    item.category = "Consumables";
                else
                    item.category = "Items";
                item.cardDataName = AssetManager.fullItemName;
                item.price = rand.Next(20, 70);
                item.priceFactor = 1;
                item.purchased = false;
                newShopItems.Add(item);
            }
            shopData.items = newShopItems;
            node.campaignNode.data["shopData"] = shopData;
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
            throw new NotImplementedException();



            var dataNames = node.campaignNode.data.Keys.ToList();
            tempPrompt("Gnome Data: " + string.Join(", ", dataNames));
            throw new NotImplementedException();
        }

        private void ManageCharmShopNode(MapNode node)
        {
            throw new NotImplementedException();



            var dataNames = node.campaignNode.data.Keys.ToList();
            tempPrompt("Charm Shop Data: " + string.Join(", ", dataNames));
            throw new NotImplementedException();
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
