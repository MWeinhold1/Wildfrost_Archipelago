using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Constants
{
    public class APItem
    {
        public int APID { get; }
        public string internalName { get; }
        public string displayName { get; }
        public APItemType type { get; }

        public APItem(int APID, int type, string internalName, string displayName)
        {
            this.APID = APID;
            this.type = (APItemType)type;
            this.internalName = internalName;
            this.displayName = displayName;
        }
    }

    public enum APItemType
    {
        filler = 0,
        building = 10,
        tribe = 20,
        pet = 30,
        common_item = 50,
        snow_item = 51,
        shade_item = 52,
        clunk_item = 53,
        common_unit = 60,
        snow_unit = 61,
        shade_unit = 62,
        clunk_unit = 63,
        common_charm = 70,
        snow_charm = 71,
        shade_charm = 72,
        clunk_charm = 73,
        bell = 80,
        trap_boon = 90
    }

    public static class APItemConstants
    {
        public static APItem GetItem(long id)
        {
            switch (id.ToString().Remove(2, 3))
            {
                case "00":
                    return APItemConstants.Filler[id];
                case "10":
                    return APItemConstants.Buildings[id];
                case "20":
                    return APItemConstants.Tribes[id];
                case "30":
                    return APItemConstants.Pets[id];
                case "50":
                    return APItemConstants.CommonItems[id];
                case "51":
                    return APItemConstants.SnowItems[id];
                case "52":
                    return APItemConstants.ShadeItems[id];
                case "53":
                    return APItemConstants.ClunkItems[id];
                case "60":
                    return APItemConstants.CommonUnits[id];
                case "61":
                    return APItemConstants.SnowUnits[id];
                case "62":
                    return APItemConstants.ShadeUnits[id];
                case "63":
                    return APItemConstants.ClunkItems[id];
                case "70":
                    return APItemConstants.CommonCharm[id];
                case "71":
                    return APItemConstants.SnowCharm[id];
                case "72":
                    return APItemConstants.ShadeCharm[id];
                case "73":
                    return APItemConstants.ClunkCharm[id];
                case "80":
                    return APItemConstants.Bell[id];
                case "90":
                    return APItemConstants.TrapBoon[id];
                default:
                    return null;
            }
        }
        public static Dictionary<long, APItem> Filler = new Dictionary<long, APItem> { };

        public static Dictionary<long, APItem> Buildings = new Dictionary<long, APItem>
        {
            {10000, new APItem(10000, 10, "frostoscope", "Frostoscope")},
            {10001, new APItem(10001, 10, "hot_spring", "Hot Spring")},
            {10002, new APItem(10002, 10, "icebreaker_cabin", "Icebreaker Cabin")},
            {10003, new APItem(10003, 10, "inventor_hut", "Inventor Hut")},
            {10004, new APItem(10004, 10, "pet_house", "Pet House")},
            {10005, new APItem(10005, 10, "tribe_hall", "Tribe Hall")},
        };

        public static Dictionary<long, APItem> Tribes = new Dictionary<long, APItem>
        {
            {20000, new APItem(20000, 20, "Basic", "Snowdwellers")},
            {20001, new APItem(20001, 20, "Magic", "Shademancers")},
            {20002, new APItem(20002, 20, "Clunk", "Clunkmasters")},
        };

        public static Dictionary<long, APItem> Pets = new Dictionary<long, APItem>
        {
            {30000, new APItem(30000, 30, "Wolfie", "Snoof")},
            {30001, new APItem(30001, 30, "BerryPet", "Booshu")},
            {30002, new APItem(30002, 30, "DemonPet", "Loki")},
            {30003, new APItem(30003, 30, "DrawPet", "Sneezle")},
            {30004, new APItem(30004, 30, "Jagzag", "Spike")},
            {30005, new APItem(30005, 30, "InkPet", "Binku")},
            {30006, new APItem(30006, 30, "BoostPet", "Lil' Gazi")},
        };

        public static Dictionary<long, APItem> CommonItems = new Dictionary<long, APItem>
        {
            {50000, new APItem(50000, 50, "BerryBasket", "Berry Basket")},
            {50001, new APItem(50001, 50, "BerryBlade", "Berry Blade")},
            {50002, new APItem(50002, 50, "BlazeTea", "Blaze Tea")},
            {50004, new APItem(50003, 50, "Demonheart", "Demonheart")},
            {50005, new APItem(50004, 50, "FrostBell", "Frost Bell")},
            {50006, new APItem(50005, 50, "FrostBloom", "Frostbloom")},
            {50007, new APItem(50006, 50, "IceDice", "Ice Dice")},
            {50008, new APItem(50007, 50, "MoltenDip", "Molten Dip")},
            {50009, new APItem(50008, 50, "NoomlinBiscuit", "Noomlin Biscuit")},
            {50010, new APItem(50009, 50, "PinkberryJuice", "Pinkberry Juice")},
            {50011, new APItem(50010, 50, "PomegranateBomb", "Pombomb")},
            {50012, new APItem(50011, 50, "Snowcake", "Snowcake")},
            {50013, new APItem(50012, 50, "SnowGlobe", "Storm Globe")},
            {50014, new APItem(50013, 50, "SunlightDrum", "Sunlight Drum")},
            {50015, new APItem(50014, 50, "ZoomlinWafers", "Zoomlin Wafers")},
            {50016, new APItem(50015, 50, "Bitebox", "Bitebox")},
            {50017, new APItem(50016, 50, "Blingo", "Bling Bank")},
            {50018, new APItem(50017, 50, "HeartmistStation", "Heartmist Station")},
            {50019, new APItem(50018, 50, "Mimik", "Mimik")},
            {50020, new APItem(50019, 50, "TotemOfTheGoat", "Totem of the Goat")},
            {50021, new APItem(50020, 50, "ZoomlinNest", "Zoomlin Nest")},
            {50022, new APItem(50021, 50, "BrokenVase", "Broken Vase")},
            {50023, new APItem(50022, 50, "LuminSealant", "Lumin Goop")},
            {50024, new APItem(50023, 50, "LuminVase", "The Lumin Vase")},
        };

        public static Dictionary<long, APItem> SnowItems = new Dictionary<long, APItem>
        {
            {51000, new APItem(51000, 51, "DragonflamePepper", "Dragon Pepper")},
            {51001, new APItem(51001, 51, "FlameWater", "Flamewater")},
            {51002, new APItem(51002, 51, "HongosHammer", "Hongos Hammer")},
            {51003, new APItem(51003, 51, "NutshellCake", "Nutshell Cake")},
            {51004, new APItem(51004, 51, "Peppereaper", "Peppereaper")},
            {51005, new APItem(51005, 51, "Peppering", "Peppering")},
            {51006, new APItem(51006, 51, "ShellShield", "Shell Shield")},
            {51007, new APItem(51007, 51, "Shellbo", "Shelbo")},
            {51008, new APItem(51008, 51, "SnowStick", "Snow Stick")},
            {51009, new APItem(51009, 51, "SpiceStones", "Spice Stones")},
            {51010, new APItem(51010, 51, "SporePack", "Spore Pack")},
            {51011, new APItem(51011, 51, "StormbearSpirit", "Stormbear Spirit")},
            {51012, new APItem(51012, 51, "SunRod", "Sun Rod")},
            {51013, new APItem(51013, 51, "ShroomLauncher", "Fungo Blaster")},
            {51014, new APItem(51014, 51, "Heartforge", "Heartforge")},
            {51015, new APItem(51015, 51, "MobileCampfire", "Mobile Campfire")},
            {51016, new APItem(51016, 51, "Peppermaton", "Moko Totem")},
            {51017, new APItem(51017, 51, "PepperFlag", "Pepper Flag")},
            {51018, new APItem(51018, 51, "Shroominator", "Shroominator")},
            {51019, new APItem(51019, 51, "Shroomine", "Shroomine")},
            {51020, new APItem(51020, 51, "SpiceSparklers", "Spice Sparklers")},
            {51021, new APItem(51021, 51, "Woodhead", "Woodhead")},
        };

        public static Dictionary<long, APItem> ShadeItems = new Dictionary<long, APItem>
        {
            {52000, new APItem(52000, 52, "BoltHarpoon", "Azul Battle Axe")},
            {52001, new APItem(52001, 52, "FlashWhip", "Azul Candle")},
            {52002, new APItem(52002, 52, "ZapOrb", "Azul Skull")},
            {52003, new APItem(52003, 52, "BeepopMask", "Beepop Mask")},
            {52004, new APItem(52004, 52, "Plum", "Berry Bell")},
            {52005, new APItem(52005, 52, "Dittostone", "Blank Mask")},
            {52005, new APItem(52006, 52, "Shwooper", "Blizzard Bottle")},
            {52006, new APItem(52007, 52, "Bonescraper", "Bonescrapper")},
            {52007, new APItem(52008, 52, "FallowMask", "Fallow Mask")},
            {52008, new APItem(52009, 52, "JunjunMask", "Junjun Mask")},
            {52009, new APItem(52010, 52, "Leecher", "Leech Mask")},
            {52010, new APItem(52011, 52, "PigeonCage", "Pom Mask")},
            {52011, new APItem(52012, 52, "Putty", "Shade Clay")},
            {52012, new APItem(52013, 52, "EnemyCloner", "Shade Wisp")},
            {52013, new APItem(52014, 52, "PopPopper", "Sheepopper Mask")},
            {52014, new APItem(52015, 52, "SkullMuffin", "Skull Muffin")},
            {52015, new APItem(52016, 52, "Scythe", "Skullmist Tea")},
            {52016, new APItem(52017, 52, "SnufferMask", "Snuffer Mask")},
            {52017, new APItem(52018, 52, "VoidStaff", "Soulbound Skulls")},
            {52018, new APItem(52019, 52, "SunburstDart", "Sunburst Tootoo")},
            {52019, new APItem(52020, 52, "SharkTooth", "Tiger Skull")},
            {52020, new APItem(52021, 52, "TigrisMask", "Tigris Mask")},
            {52021, new APItem(52022, 52, "SnowMaul", "Yeti Skull")},
        };

        public static Dictionary<long, APItem> ClunkItems = new Dictionary<long, APItem>
        {
            {53000, new APItem(53000, 53, "Nullifier", "B.I.N.K")},
            {53001, new APItem(53001, 53, "Bumblebee", "Blaze Bom")},
            {53002, new APItem(53002, 53, "Badoo", "Bom Barrel")},
            {53003, new APItem(53003, 53, "EnergyDart", "Clockwork Bom")},
            {53004, new APItem(53004, 53, "Voidstone", "Flask of Ink")},
            {53005, new APItem(53005, 53, "FoggyBrew", "Foggy Brew")},
            {53006, new APItem(53006, 53, "Recycler", "Forging Stove")},
            {53007, new APItem(53007, 53, "FrenzyShard", "Frenzy Wrench")},
            {53008, new APItem(53008, 53, "IceShard", "Frostbite Shard")},
            {53009, new APItem(53009, 53, "Junberry", "Gigi's Cookie Box")},
            {53010, new APItem(53010, 53, "Juicepot", "Gigi's Gizmo")},
            {53011, new APItem(53011, 53, "HazeBlaze", "Haze Keg")},
            {53012, new APItem(53012, 53, "LuminShard", "Lumin Lantern")},
            {53013, new APItem(53013, 53, "EyeDrops", "Magma Booster")},
            {53014, new APItem(53014, 53, "Wrenchy", "Mini Muncher")},
            {53015, new APItem(53015, 53, "Crowbar", "Proto-Stomper")},
            {53016, new APItem(53016, 53, "SnowCannon", "Snowzooka")},
            {53017, new APItem(53017, 53, "SunberryJuice", "Suncream")},
            {53018, new APItem(53018, 53, "SunShard", "Sunsong Box")},
            {53019, new APItem(53019, 53, "BlizzPop", "Supersnower")},
            {53020, new APItem(53020, 53, "Junkmuncher", "Blundertank")},
            {53021, new APItem(53021, 53, "Vox", "Bombarder")},
            {53022, new APItem(53022, 53, "PomDispenser", "Gachapomper")},
            {53023, new APItem(53023, 53, "Vimifier", "Haze Balloon")},
            {53024, new APItem(53024, 53, "OhNo", "I.C.G.M")},
            {53025, new APItem(53025, 53, "Junkhead", "Junkhead")},
            {53026, new APItem(53026, 53, "Plinker", "Plinker")},
            {53027, new APItem(53027, 53, "Bonfire", "Portable Workbench")},
            {53028, new APItem(53028, 53, "Madness", "Sunglass Chime")},
            {53029, new APItem(53029, 53, "Joob", "Tootordion")},
        };

        public static Dictionary<long, APItem> CommonUnits = new Dictionary<long, APItem>
        {
            {60000, new APItem(60000, 60, "BigBerry", "Big Berry")},
            {60001, new APItem(60001, 60, "Blunky", "Blunky")},
            {60002, new APItem(60002, 60, "Bonnie", "Bonnie")},
            {60003, new APItem(60003, 60, "Dimona", "Dimona")},
            {60004, new APItem(60004, 60, "Foxee", "Foxee")},
            {60005, new APItem(60005, 60, "Noggin", "Gojiber")},
            {60006, new APItem(60006, 60, "Klutz", "Jumbo")},
            {60007, new APItem(60007, 60, "NakedGnome", "Naked Gnome")},
            {60007, new APItem(60008, 60, "Blue", "Nova")},
            {60008, new APItem(60009, 60, "MagmaBlacksmith", "Roibos")},
            {60009, new APItem(60010, 60, "Snobble", "Snobble")},
            {60010, new APItem(60011, 60, "Snoffel", "Snoffel")},
        };

        public static Dictionary<long, APItem> SnowUnits = new Dictionary<long, APItem>
        {
            {61000, new APItem(61000, 61, "Chompom", "Chompom")},
            {61001, new APItem(61001, 61, "Firefist", "Firefist")},
            {61002, new APItem(61002, 61, "Fulbert", "Fulbert")},
            {61003, new APItem(61003, 61, "Fungoose", "Fungun")},
            {61004, new APItem(61004, 61, "Kernel", "Kernel")},
            {61005, new APItem(61005, 61, "LilBerry", "Lil Berry")},
            {61006, new APItem(61006, 61, "Pimento", "Pimento")},
            {61007, new APItem(61007, 61, "Pootie", "Pootie")},
            {61008, new APItem(61008, 61, "Pyra", "Pyra")},
            {61009, new APItem(61009, 61, "Shelly", "Shelly")},
            {61010, new APItem(61010, 61, "Wallop", "Wallop")},
            {61011, new APItem(61011, 61, "Wort", "Wort")},
            {61012, new APItem(61012, 61, "Yuki", "Yuki")},
        };

        public static Dictionary<long, APItem> ShadeUnits = new Dictionary<long, APItem>
        {
            {62000, new APItem(62000, 62, "BloodBoy", "Berry Sis")},
            {62001, new APItem(62001, 62, "TailsFive", "Chikichi")},
            {62002, new APItem(62002, 62, "Reaper", "Devicro")},
            {62003, new APItem(62003, 62, "Egg", "Egg")},
            {62004, new APItem(62004, 62, "Boggler", "Groff")},
            {62005, new APItem(62005, 62, "Monch", "Monch")},
            {62006, new APItem(62006, 62, "Zoog", "Shen")},
            {62007, new APItem(62007, 62, "Ditto", "Splinter")},
            {62008, new APItem(62008, 62, "Spoof", "Spoof")},
            {62009, new APItem(62009, 62, "Kokonut", "Taiga")},
            {62010, new APItem(62010, 62, "Tusk", "Tusk")},
            {62011, new APItem(62011, 62, "BoBo", "Van Jun")},
            {62012, new APItem(62012, 62, "Flash", "Vesta")},
            {62013, new APItem(62013, 62, "Zula", "Zula")},
        };

        public static Dictionary<long, APItem> ClunkUnits = new Dictionary<long, APItem>
        {
            {63000, new APItem(63000, 63, "Turmeep", "Alloy")},
            {63001, new APItem(63001, 63, "Witch", "Biji")},
            {63002, new APItem(63002, 63, "Gnomlings", "Fizzle")},
            {63003, new APItem(63003, 63, "Gearhead", "Folby")},
            {63004, new APItem(63004, 63, "Voodoo", "Hazeblazer")},
            {63005, new APItem(63005, 63, "Knuckles", "Knuckles")},
            {63006, new APItem(63006, 63, "Bunnight", "Kreggo")},
            {63007, new APItem(63007, 63, "MamaTinkerson", "Mama Tinkerson")},
            {63008, new APItem(63008, 63, "Timmy", "Mini Mika")},
            {63009, new APItem(63009, 63, "Ruckus", "Needle")},
            {63010, new APItem(63010, 63, "GuardianGnome", "Nom & Stompy")},
            {63011, new APItem(63011, 63, "Bear", "Scaven")},
            {63012, new APItem(63012, 63, "Jummo", "Tinkerson Jr")},
        };

        public static Dictionary<long, APItem> CommonCharm = new Dictionary<long, APItem>
        {
            {70000, new APItem(70000, 70, "CardUpgradeBalanced", "Balance Charm")},
            {70001, new APItem(70001, 70, "CardUpgradeBattle", "Battle Charm")},
            {70002, new APItem(70002, 70, "CardUpgradePlink", "Beetle Charm")},
            {70003, new APItem(70003, 70, "CardUpgradeBling", "Bling Charm")},
            {70004, new APItem(70004, 70, "CardUpgradeBlock", "Block Charm")},
            {70005, new APItem(70005, 70, "CardUpgradeBombskull", "Bombskull Charm")},
            {70006, new APItem(70006, 70, "CardUpgradeCake", "Cake Charm")},
            {70007, new APItem(70007, 70, "CardUpgradeRemoveCharmLimit", "Chuckle Charm")},
            {70008, new APItem(70008, 70, "CardUpgradeCloudberry", "Cloudberry Charm")},
            {70009, new APItem(70009, 70, "CardUpgradeCritical", "Critical Charm")},
            {70010, new APItem(70010, 70, "CardUpgradeAttackRemoveEffects", "Durian Charm")},
            {70011, new APItem(70011, 70, "CardUpgradeFrenzyConsume", "Frenzy Charm")},
            {70012, new APItem(70012, 70, "CardUpgradeFury", "Frog Charm")},
            {70013, new APItem(70013, 70, "CardUpgradeFrosthand", "Frosthand Charm")},
            {70014, new APItem(70014, 70, "CardUpgradeBlue", "Frozen Heart Charm")},
            {70015, new APItem(70015, 70, "CardUpgradeWildcard", "Gnome Charm")},
            {70016, new APItem(70016, 70, "CardUpgradeDemonize", "Goat Charm")},
            {70017, new APItem(70017, 70, "CardUpgradeGreed", "Greed Charm")},
            {70018, new APItem(70018, 70, "CardUpgradeHeart", "Heart Charm")},
            {70019, new APItem(70019, 70, "CardUpgradePig", "Hog Charm")},
            {70020, new APItem(70020, 70, "CardUpgradeHook", "Hook Charm")},
            {70021, new APItem(70021, 70, "CardUpgradeBootleg", "Jimbo Charm")},
            {70022, new APItem(70022, 70, "CardUpgradeBoost", "Lumin Ring")},
            {70023, new APItem(70023, 70, "CardUpgradeFrenzyReduceAttack", "Moko Charm")},
            {70024, new APItem(70024, 70, "CardUpgradeAttackConsume", "Molten Egg Charm")},
            {70025, new APItem(70025, 70, "CardUpgradeAttackIncreaseCounter", "Moose Charm")},
            {70026, new APItem(70026, 70, "CardUpgradeMuncher", "Muncher Charm")},
            {70027, new APItem(70027, 70, "CardUpgradeNoomlin", "Noomlin Charm")},
            {70028, new APItem(70028, 70, "CardUpgradeHeartmist", "Nourish Charm")},
            {70029, new APItem(70029, 70, "CardUpgradeSnowImmune", "Pengu Charm")},
            {70030, new APItem(70030, 70, "CardUpgradeDraw", "Pinch Charm")},
            {70031, new APItem(70031, 70, "CardUpgradeBarrage", "Pomegranate Charm")},
            {70032, new APItem(70032, 70, "CardUpgradePunchfist", "Punchfist Charm")},
            {70033, new APItem(70033, 70, "CardUpgradeAttackAndHealth", "Raspberry Charm")},
            {70034, new APItem(70034, 70, "CardUpgradeFlameberry", "Scorchberry Charm")},
            {70035, new APItem(70035, 70, "CardUpgradeShadeClay", "Shade Slug")},
            {70036, new APItem(70036, 70, "CardUpgradeSnowball", "Snowball Charm")},
            {70037, new APItem(70037, 70, "CardUpgradeSpark", "Spark Charm")},
            {70038, new APItem(70038, 70, "CardUpgradeConsumeAddHealth", "Strawberry Charm")},
            {70039, new APItem(70039, 70, "CardUpgradeSun", "Sun Charm")},
            {70040, new APItem(70040, 70, "CardUpgradeGlass", "Sunglass Charm")},
            {70041, new APItem(70041, 70, "CardUpgradeHunger", "Zoomlin Charm")},
            {70042, new APItem(70042, 70, "CardUpgradeScrap", "Scrap Charm")},
        };
        public static Dictionary<long, APItem> SnowCharm = new Dictionary<long, APItem>
        {
            {71000, new APItem(71000, 71, "CardUpgradeAcorn", "Acorn Charm")},
            {71001, new APItem(71001, 71, "CardUpgradeHeartburn", "Jewelberry Charm")},
            {71002, new APItem(71002, 71, "CardUpgradeShellBecomesSpice", "Peppernut Charm")},
            {71003, new APItem(71003, 71, "CardUpgradeShellOnKill", "Shield Charm")},
            {71004, new APItem(71004, 71, "CardUpgradeShroom", "Shroom Charm")},
            {71005, new APItem(71005, 71, "CardUpgradeSpice", "Spice Charm")},
            {71006, new APItem(71006, 71, "CardUpgradeShroomReduceHealth", "Truffle Charm")},
        };
        public static Dictionary<long, APItem> ShadeCharm = new Dictionary<long, APItem>
        {
            {72000, new APItem(72000, 72, "CardUpgradeSpiky", "Bite Charm")},
            {72001, new APItem(72001, 72, "CardUpgradeConsumeOverload", "Boonfire Charm")},
            {72002, new APItem(72002, 72, "CardUpgradeOverload", "Flameblade Charm")},
            {72003, new APItem(72003, 72, "CardUpgradeEffigy", "Lamb Charm")},
            {72004, new APItem(72004, 72, "CardUpgradeMime", "Mime Charm")},
            {72005, new APItem(72005, 72, "CardUpgradeTeethWhenHit", "Tiger Charm")},
        };
        public static Dictionary<long, APItem> ClunkCharm = new Dictionary<long, APItem>
        {
            {73000, new APItem(73000, 73, "CardUpgradeBom", "Bom Charm")},
            {73001, new APItem(73001, 73, "CardUpgradeShredder", "Fidget Charm")},
            {73002, new APItem(73002, 73, "CardUpgradeTrash", "Gear Charm")},
            {73003, new APItem(73003, 73, "CardUpgradeCrush", "Recycle Charm")},
            {73004, new APItem(73004, 73, "CardUpgradeInk", "Squid Charm")},
        };
        public static Dictionary<long, APItem> Bell = new Dictionary<long, APItem>
        {
            // Sun Bells
            {80000, new APItem(80000, 80, "BlessingHand", "Sun Bell of Hands")},
            {80001, new APItem(80001, 80, "BlessingCompanions", "Sun Bell of Fellowship")},
            {80002, new APItem(80002, 80, "BlessingRedrawBell", "Sun Bell of the Bell")},
            {80003, new APItem(80003, 80, "BlessingHealth", "Sun Bell of Health")},
            {80004, new APItem(80004, 80, "BlessingTime", "Sun Bell of Time")},
            {80005, new APItem(80005, 80, "BlessingRecall", "Sun Bell of Recall")},
            {80006, new APItem(80006, 80, "BlessingCharge", "Sun Bell of Charge")},
            {80007, new APItem(80007, 80, "BlessingNoomlin", "Noomlin Sun Bell")},
            {80008, new APItem(80008, 80, "BlessingStrength", "Sun Bell of Strength")},
            {80009, new APItem(80009, 80, "BlessingConsume", "Breakfast Sun Bell")},
            {80010, new APItem(80010, 80, "BlessingInfinity", "Infinity Sun Bell")},
            // Storm Bells
            {80100, new APItem(80100, 80, "ExpensiveShops", "Blingsnail Bell")},
            {80101, new APItem(80101, 80, "CompanionInjuries", "Bell of Death")},
            {80102, new APItem(80102, 80, "BossesHaveCharms", "Titan Bell")},
            {80103, new APItem(80103, 80, "ReduceBossWaveCounter", "Frosthand Bell")},
            {80104, new APItem(80104, 80, "DeadweightAfterBosses", "Gunk Bell")},
            {80105, new APItem(80105, 80, "IceCavesEnemiesIncreaseAttack", "Icebourne Bell")},
            {80106, new APItem(80106, 80, "EarlyMinibosses", "Horde Bell")},
            {80107, new APItem(80107, 80, "CardsStartWithNegativeCharms", "Gloom Bell")},
            {80108, new APItem(80108, 80, "FrostlandEnemiesIncreaseAttack", "Frostbourne Bell")},
            {80109, new APItem(80109, 80, "Sunbringer", "Gobbler Bell")},
            {80110, new APItem(80110, 80, "CursedCrowns", "Tyrant Bell")},
            {80111, new APItem(80111, 80, "EnemyCharms", "Dread Bell")},
            {80112, new APItem(80112, 80, "DrainLeader", "Blood Bell")},
            // Daily Voyage Bells
            {80200, new APItem(80200, 80, "BoostEnemyDamage", "Battle Bell")},
            {80201, new APItem(80201, 80, "DoubleBlingsFromGoblings", "Blingsack Bell")},
            {80202, new APItem(80202, 80, "BombskullClunkers", "Bombskull Bell")},
            {80203, new APItem(80203, 80, "DrawMoreCardsWhenRedrawNotCharged", "Broken Bell")},
            {80204, new APItem(80204, 80, "AimlessStartingItems", "Fog Bell")},
            {80205, new APItem(80205, 80, "FrenzyBosses", "Frenzy Bell")},
            {80206, new APItem(80206, 80, "BlockForMinibosses", "Frozen Heart Bell")},
            {80207, new APItem(80207, 80, "DemonizeEnemies", "Goat Bell")},
            {80208, new APItem(80208, 80, "DoubleBlingsFromCombos", "Gold Blade Bell")},
            {80209, new APItem(80209, 80, "BoostEveryonesHealth", "Heart Bell")},
            {80210, new APItem(80210, 80, "BoostAllEffects", "Lumin Bell")},
            {80211, new APItem(80211, 80, "NoCompanionLimit", "Party Bell")},
        };
        public static Dictionary<long, APItem> TrapBoon = new Dictionary<long, APItem>
        {
        
        };
    }
}
