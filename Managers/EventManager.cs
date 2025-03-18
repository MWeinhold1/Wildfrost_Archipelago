using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Managers
{
    public class EventManager
    {
        public void LoadEvents()
        {
            Logger.Log(LogType.Info, "Loading Events");
            Events.OnMapNodeSelect += Events_OnMapNodeSelect;
            Events.OnCardDataCreated += Events_OnCardDataCreated;
            //Events.OnEntityEnterBackpack += Events_OnEntityEnterBackpack;
            //Events.OnUpgradeGained += Events_OnUpgradeGained;
        }

        public void UnloadEvents()
        {
            Logger.Log(LogType.Info, "Unloading Events");
            Events.OnMapNodeSelect -= Events_OnMapNodeSelect;
            Events.OnCardDataCreated -= Events_OnCardDataCreated;
            //Events.OnEntityEnterBackpack -= Events_OnEntityEnterBackpack;
            //Events.OnUpgradeGained -= Events_OnUpgradeGained;
        }

        int temp = 1;
        private void Events_OnCardDataCreated(CardData card)
        {
            if (card.name == "mweinhold.wildfrost.archipelago.archifact_item")
            {
                Logger.Log(LogType.Info, "Modifying an Archifact card");
                card.forceTitle = "Force Title " + temp.ToString();
                card.attackEffects.First().data.textInsert = "New <Text>";
                temp++;
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
            if (charm.IsCharm())
            {
                References.PlayerData.inventory.upgrades.Remove(charm);
            }
        }

        private void Events_OnEntityEnterBackpack(Entity card)
        {
            // Works for cards, not charms
            Logger.Log(LogType.Info, $"Entity Entered Backpack: {card.data?.name ?? "Not a card"}");
            References.PlayerData.inventory.deck.Remove(card.data);
        }

        #region Utility Functions
        private void ManageItemNode(MapNode node)
        {
            string name = "mweinhold.wildfrost.archipelago.archifact_item";
            List<string> names = new List<string>
                {
                    name,
                    name,
                    name
                };
            var nameCollection = new SaveCollection<string>(names);
            node.campaignNode.data["cards"] = nameCollection;
        }

        private void ManageShopNode(MapNode node)
        {
            var shopData = node.campaignNode.data["shopData"] as ShopRoutine.Data;
            //shopData.charms = new List<string> { "mweinhold.wildfrost.archipelago.archifact_charm", "mweinhold.wildfrost.archipelago.archifact_charm", "mweinhold.wildfrost.archipelago.archifact_charm" };
            var newShopItems = new List<ShopRoutine.Item>();
            var rand = new Random();
            for(int i = 0; i < 5; i++)
            {
                var item = new ShopRoutine.Item();
                if (newShopItems.Count == 0)
                    item.category = "Consumables";
                else
                    item.category = "Items";
                item.cardDataName = "mweinhold.wildfrost.archipelago.archifact_item";
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
            string name = "mweinhold.wildfrost.archipelago.archifact_unit";
            List<string> names = new List<string>
                {
                    name,
                    name,
                    name
                };
            var nameCollection = new SaveCollection<string>(names);
            node.campaignNode.data["cards"] = nameCollection;
        }

        private void ManageCharmNode(MapNode node)
        {
            string name = "mweinhold.wildfrost.archipelago.archifact_charm";
            node.campaignNode.data["charm"] = name;
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
