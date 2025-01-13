using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Archipelago.Constants
{
    public class APItem
    {
        public int APID { get; }
        public string internalName { get; }
        public string displayName { get; }
        public APItemType type { get; }

        public APItem(int APID, string internalName, string displayName, APItemType type)
        {
            this.APID = APID;
            this.internalName = internalName;
            this.displayName = displayName;
            this.type = type;
        }
    }

    public enum APItemType
    {
        filler = 0,
        building = 10,
        tribe = 20,
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
        /// <summary>
        /// Searches the set of possible items that can be sent by the server, and returns a copy value
        /// </summary>
        /// <param name="APID"></param>
        /// <returns>A copy of the item that matches the passed in ID</returns>
        public static APItem GetItem(long APID)
        {
            APItemType type = (APItemType)(APID / 1000);
            switch (type)
            {
                case APItemType.filler:
                    return Filler[APID];
                case APItemType.building:
                    return Buildings[APID];
                default:
                    break;
            }
        }

        public static Dictionary<long, APItem> Filler = new Dictionary<long, APItem> { };

        public static Dictionary<long, APItem> Buildings = new Dictionary<long, APItem>
        {
            {10000, new APItem(10000, "frostoscope", "Frostoscope", APItemType.building)},
            {10001, new APItem(10001, "hot_spring", "Hot Spring", APItemType.building)},
            {10002, new APItem(10002, "icebreaker_cabin", "Icebreaker Cabin", APItemType.building)},
            {10003, new APItem(10003, "inventor_hut", "Inventor Hut", APItemType.building)},
            {10004, new APItem(10004, "pet_house", "Pet House", APItemType.building)},
            {10005, new APItem(10005, "tribe_hall", "Tribe Hall", APItemType.building)},
        };

        public static Dictionary<long, APItem> Tribes = new Dictionary<long, APItem>
        {
            {20000, new APItem(20000, "Magic", "Shademancers", APItemType.tribe)},
            {20001, new APItem(20001, "Clunk", "Clunkmasters", APItemType.tribe)},
        };

        public static Dictionary<long, APItem> CommonItems = new Dictionary<long, APItem>
        {
            {50000, new APItem(50000, "BerryBasket", "Berry Basket", APItemType.common_item)},
            {50001, new APItem(50001, "BerryBlade", "Berry Blade", APItemType.common_item)},
            {50002, new APItem(50002, "BlazeTea", "Blaze Tea", APItemType.common_item)},
            {50003, new APItem(50003, "Shwooper", "Blizzard Bottle", APItemType.common_item)},
            {50004, new APItem(50004, "Demonheart", "Demonheart", APItemType.common_item)},
            {50005, new APItem(50005, "FrostBell", "Frost Bell", APItemType.common_item)},
            {50006, new APItem(50006, "FrostBloom", "Frostbloom", APItemType.common_item)},
            {50007, new APItem(50007, "IceDice", "Ice Dice", APItemType.common_item)},
            {50008, new APItem(50008, "MoltenDip", "Molten Dip", APItemType.common_item)},
            {50009, new APItem(50009, "NoomlinBiscuit", "Noomlin Biscuit", APItemType.common_item)},
            {50010, new APItem(50010, "PinkberryJuice", "Pinkberry Juice", APItemType.common_item)},
            {50011, new APItem(50011, "PomegranateBomb", "Pombomb", APItemType.common_item)},
            {50012, new APItem(50012, "Snowcake", "Snowcake", APItemType.common_item)},
            {50013, new APItem(50013, "SnowGlobe", "Storm Globe", APItemType.common_item)},
            {50014, new APItem(50014, "SunlightDrum", "Sunlight Drum", APItemType.common_item)},
            {50015, new APItem(50015, "ZoomlinWafers", "Zoomlin Wafers", APItemType.common_item)},
            {50016, new APItem(50016, "Bitebox", "Bitebox", APItemType.common_item)},
            {50017, new APItem(50017, "Blingo", "Bling Bank", APItemType.common_item)},
            {50018, new APItem(50018, "HeartmistStation", "Heartmist Station", APItemType.common_item)},
            {50019, new APItem(50019, "Mimik", "Mimik", APItemType.common_item)},
            {50020, new APItem(50020, "TotemOfTheGoat", "Totem of the Goat", APItemType.common_item)},
            {50021, new APItem(50021, "ZoomlinNest", "Zoomlin Nest", APItemType.common_item)},
            {50022, new APItem(50022, "BrokenVase", "Broken Vase", APItemType.common_item)},
            {50023, new APItem(50023, "LuminSealant", "Lumin Goop", APItemType.common_item)},
            {50024, new APItem(50024, "LuminVase", "The Lumin Vase", APItemType.common_item)},
        };

        public static Dictionary<long, APItem> SnowItems = new Dictionary<long, APItem>
        {
            {51000, new APItem(51000, "DragonflamePepper", "Dragon Pepper", APItemType.snow_item)},
            {51001, new APItem(51001, "FlameWater", "Flamewater", APItemType.snow_item)},
            {51002, new APItem(51002, "HongosHammer", "Hongos Hammer", APItemType.snow_item)},
            {51003, new APItem(51003, "NutshellCake", "Nutshell Cake", APItemType.snow_item)},
            {51004, new APItem(51004, "Peppereaper", "Peppereaper", APItemType.snow_item)},
            {51005, new APItem(51005, "Peppering", "Peppering", APItemType.snow_item)},
            {51006, new APItem(51006, "ShellShield", "Shell Shield", APItemType.snow_item)},
            {51007, new APItem(51007, "Shellbo", "Shelbo", APItemType.snow_item)},
            {51008, new APItem(51008, "SnowStick", "Snow Stick", APItemType.snow_item)},
            {51009, new APItem(51009, "SpiceStones", "Spice Stones", APItemType.snow_item)},
            {51010, new APItem(51010, "SporePack", "Spore Pack", APItemType.snow_item)},
            {51011, new APItem(51011, "StormbearSpirit", "Stormbear Spirit", APItemType.snow_item)},
            {51012, new APItem(51012, "SunRod", "Sun Rod", APItemType.snow_item)},
            {51013, new APItem(51013, "ShroomLauncher", "Fungo Blaster", APItemType.snow_item)},
            {51014, new APItem(51014, "Heartforge", "Heartforge", APItemType.snow_item)},
            {51015, new APItem(51015, "MobileCampfire", "Mobile Campfire", APItemType.snow_item)},
            {51016, new APItem(51016, "Peppermaton", "Moko Totem", APItemType.snow_item)},
            {51017, new APItem(51017, "PepperFlag", "Pepper Flag", APItemType.snow_item)},
            {51018, new APItem(51018, "Shroominator", "Shroominator", APItemType.snow_item)},
            {51019, new APItem(51019, "Shroomine", "Shroomine", APItemType.snow_item)},
            {51020, new APItem(51020, "SpiceSparklers", "Spice Sparklers", APItemType.snow_item)},
            {51021, new APItem(51021, "Woodhead", "Woodhead", APItemType.snow_item)},
        };

        public static Dictionary<long, APItem> ShadeItems = new Dictionary<long, APItem>
        {
            {52000, new APItem(52000, "BoltHarpoon", "Azul Battle Axe", APItemType.shade_item)},
            {52001, new APItem(52001, "FlashWhip", "Azul Candle", APItemType.shade_item)},
            {52002, new APItem(52002, "ZapOrb", "Azul Skull", APItemType.shade_item)},
            {52003, new APItem(52003, "BeepopMask", "Beepop Mask", APItemType.shade_item)},
            {52004, new APItem(52004, "Plum", "Berry Bell", APItemType.shade_item)},
            {52005, new APItem(52005, "Dittostone", "Blank Mask", APItemType.shade_item)},
            {52006, new APItem(52006, "Bonescraper", "Bonescrapper", APItemType.shade_item)},
            {52007, new APItem(52007, "FallowMask", "Fallow Mask", APItemType.shade_item)},
            {52008, new APItem(52008, "JunjunMask", "Junjun Mask", APItemType.shade_item)},
            {52009, new APItem(52009, "Leecher", "Leech Mask", APItemType.shade_item)},
            {52010, new APItem(52010, "PigeonCage", "Pom Mask", APItemType.shade_item)},
            {52011, new APItem(52011, "Putty", "Shade Clay", APItemType.shade_item)},
            {52012, new APItem(52012, "EnemyCloner", "Shade Wisp", APItemType.shade_item)},
            {52013, new APItem(52013, "PopPopper", "Sheepopper Mask", APItemType.shade_item)},
            {52014, new APItem(52014, "SkullMuffin", "Skull Muffin", APItemType.shade_item)},
            {52015, new APItem(52015, "Scythe", "Skullmist Tea", APItemType.shade_item)},
            {52016, new APItem(52016, "SnufferMask", "Snuffer Mask", APItemType.shade_item)},
            {52017, new APItem(52017, "VoidStaff", "Soulbound Skulls", APItemType.shade_item)},
            {52018, new APItem(52018, "SunburstDart", "Sunburst Tootoo", APItemType.shade_item)},
            {52019, new APItem(52019, "SharkTooth", "Tiger Skull", APItemType.shade_item)},
            {52020, new APItem(52020, "TigrisMask", "Tigris Mask", APItemType.shade_item)},
            {52021, new APItem(52021, "SnowMaul", "Yeti Skull", APItemType.shade_item)},
        };

        public static Dictionary<long, APItem> ClunkItems = new Dictionary<long, APItem>
        {
            {53000, new APItem(53000, "Nullifier", "B.I.N.K", APItemType.clunk_item)},
            {53001, new APItem(53001, "Bumblebee", "Blaze Bom", APItemType.clunk_item)},
            {53002, new APItem(53002, "Badoo", "Bom Barrel", APItemType.clunk_item)},
            {53003, new APItem(53003, "EnergyDart", "Clockwork Bom", APItemType.clunk_item)},
            {53004, new APItem(53004, "Voidstone", "Flask of Ink", APItemType.clunk_item)},
            {53005, new APItem(53005, "FoggyBrew", "Foggy Brew", APItemType.clunk_item)},
            {53006, new APItem(53006, "Recycler", "Forging Stove", APItemType.clunk_item)},
            {53007, new APItem(53007, "FrenzyShard", "Frenzy Wrench", APItemType.clunk_item)},
            {53008, new APItem(53008, "IceShard", "Frostbite Shard", APItemType.clunk_item)},
            {53009, new APItem(53009, "Junberry", "Gigi's Cookie Box", APItemType.clunk_item)},
            {53010, new APItem(53010, "Juicepot", "Gigi's Gizmo", APItemType.clunk_item)},
            {53011, new APItem(53011, "HazeBlaze", "Haze Keg", APItemType.clunk_item)},
            {53012, new APItem(53012, "LuminShard", "Lumin Lantern", APItemType.clunk_item)},
            {53013, new APItem(53013, "EyeDrops", "Magma Booster", APItemType.clunk_item)},
            {53014, new APItem(53014, "Wrenchy", "Mini Muncher", APItemType.clunk_item)},
            {53015, new APItem(53015, "Crowbar", "Proto-Stomper", APItemType.clunk_item)},
            {53016, new APItem(53016, "SnowCannon", "Snowzooka", APItemType.clunk_item)},
            {53017, new APItem(53017, "SunberryJuice", "Suncream", APItemType.clunk_item)},
            {53018, new APItem(53018, "SunShard", "Sunsong Box", APItemType.clunk_item)},
            {53019, new APItem(53019, "BlizzPop", "Supersnower", APItemType.clunk_item)},
            {53020, new APItem(53020, "Junkmuncher", "Blundertank", APItemType.clunk_item)},
            {53021, new APItem(53021, "Vox", "Bombarder", APItemType.clunk_item)},
            {53022, new APItem(53022, "PomDispenser", "Gachapomper", APItemType.clunk_item)},
            {53023, new APItem(53023, "Vimifier", "Haze Balloon", APItemType.clunk_item)},
            {53024, new APItem(53024, "OhNo", "I.C.G.M", APItemType.clunk_item)},
            {53025, new APItem(53025, "Junkhead", "Junkhead", APItemType.clunk_item)},
            {53026, new APItem(53026, "Plinker", "Plinker", APItemType.clunk_item)},
            {53027, new APItem(53027, "Bonfire", "Portable Workbench", APItemType.clunk_item)},
            {53028, new APItem(53028, "Madness", "Sunglass Chime", APItemType.clunk_item)},
            {53029, new APItem(53029, "Joob", "Tootordion", APItemType.clunk_item)},
        };

        public static Dictionary<long, APItem> CommonUnits = new Dictionary<long, APItem>
        {
            {60000, new APItem(60000, "BigBerry", "Big Berry", APItemType.common_unit)},
            {60001, new APItem(60001, "Blunky", "Blunky", APItemType.common_unit)},
            {60002, new APItem(60002, "Bonnie", "Bonnie", APItemType.common_unit)},
            {60003, new APItem(60003, "Dimona", "Dimona", APItemType.common_unit)},
            {60004, new APItem(60004, "Foxee", "Foxee", APItemType.common_unit)},
            {60005, new APItem(60005, "Noggin", "Gojiber", APItemType.common_unit)},
            {60006, new APItem(60006, "Klutz", "Jumbo", APItemType.common_unit)},
            {60007, new APItem(60007, "NakedGnome", "Naked Gnome", APItemType.common_unit)},
            {60008, new APItem(60008, "MagmaBlacksmith", "Roibos", APItemType.common_unit)},
            {60009, new APItem(60009, "Snobble", "Snobble", APItemType.common_unit)},
            {60010, new APItem(60010, "Snoffel", "Snoffel", APItemType.common_unit)},
        };

        public static Dictionary<long, APItem> SnowUnits = new Dictionary<long, APItem>
        {
            {61000, new APItem(61000, "Chompom", "Chompom", APItemType.snow_unit)},
            {61001, new APItem(61001, "Firefist", "Firefist", APItemType.snow_unit)},
            {61002, new APItem(61002, "Fulbert", "Fulbert", APItemType.snow_unit)},
            {61003, new APItem(61003, "Fungoose", "Fungun", APItemType.snow_unit)},
            {61004, new APItem(61004, "Kernel", "Kernel", APItemType.snow_unit)},
            {61005, new APItem(61005, "LilBerry", "Lil Berry", APItemType.snow_unit)},
            {61006, new APItem(61006, "Pimento", "Pimento", APItemType.snow_unit)},
            {61007, new APItem(61007, "Pootie", "Pootie", APItemType.snow_unit)},
            {61008, new APItem(61008, "Pyra", "Pyra", APItemType.snow_unit)},
            {61009, new APItem(61009, "Shelly", "Shelly", APItemType.snow_unit)},
            {61010, new APItem(61010, "Wallop", "Wallop", APItemType.snow_unit)},
            {61011, new APItem(61011, "Wort", "Wort", APItemType.snow_unit)},
            {61012, new APItem(61012, "Yuki", "Yuki", APItemType.snow_unit)},
        };

        public static Dictionary<long, APItem> ShadeUnits = new Dictionary<long, APItem>
        {
            {62000, new APItem(62000, "BloodBoy", "Berry Sis", APItemType.shade_unit)},
            {62001, new APItem(62001, "TailsFive", "Chikichi", APItemType.shade_unit)},
            {62002, new APItem(62002, "Reaper", "Devicro", APItemType.shade_unit)},
            {62003, new APItem(62003, "Egg", "Egg", APItemType.shade_unit)},
            {62004, new APItem(62004, "Boggler", "Groff", APItemType.shade_unit)},
            {62005, new APItem(62005, "Monch", "Monch", APItemType.shade_unit)},
            {62006, new APItem(62006, "Zoog", "Shen", APItemType.shade_unit)},
            {62007, new APItem(62007, "Ditto", "Splinter", APItemType.shade_unit)},
            {62008, new APItem(62008, "Spoof", "Spoof", APItemType.shade_unit)},
            {62009, new APItem(62009, "Kokonut", "Taiga", APItemType.shade_unit)},
            {62010, new APItem(62010, "Tusk", "Tusk", APItemType.shade_unit)},
            {62011, new APItem(62011, "BoBo", "Van Jun", APItemType.shade_unit)},
            {62012, new APItem(62012, "Flash", "Vesta", APItemType.shade_unit)},
            {62013, new APItem(62013, "Zula", "Zula", APItemType.shade_unit)},
        };

        public static Dictionary<long, APItem> ClunkUnits = new Dictionary<long, APItem>
        {
            {63000, new APItem(63000, "Turmeep", "Alloy", APItemType.clunk_unit)},
            {63001, new APItem(63001, "Witch", "Biji", APItemType.clunk_unit)},
            {63002, new APItem(63002, "Gnomlings", "Fizzle", APItemType.clunk_unit)},
            {63003, new APItem(63003, "Gearhead", "Folby", APItemType.clunk_unit)},
            {63004, new APItem(63004, "Voodoo", "Hazeblazer", APItemType.clunk_unit)},
            {63005, new APItem(63005, "Knuckles", "Knuckles", APItemType.clunk_unit)},
            {63006, new APItem(63006, "Bunnight", "Kreggo", APItemType.clunk_unit)},
            {63007, new APItem(63007, "MamaTinkerson", "Mama Tinkerson", APItemType.clunk_unit)},
            {63008, new APItem(63008, "Timmy", "Mini Mika", APItemType.clunk_unit)},
            {63009, new APItem(63009, "Ruckus", "Needle", APItemType.clunk_unit)},
            {63010, new APItem(63010, "GuardianGnome", "Nom & Stompy", APItemType.clunk_unit)},
            {63011, new APItem(63011, "Bear", "Scaven", APItemType.clunk_unit)},
            {63012, new APItem(63012, "Jummo", "Tinkerson Jr", APItemType.clunk_unit)},
        };

        public static Dictionary<long, APItem> CommonCharm = new Dictionary<long, APItem> { };
        public static Dictionary<long, APItem> SnowCharm = new Dictionary<long, APItem> { };
        public static Dictionary<long, APItem> ShadeCharm = new Dictionary<long, APItem> { };
        public static Dictionary<long, APItem> ClunkCharm = new Dictionary<long, APItem> { };
        public static Dictionary<long, APItem> Bell = new Dictionary<long, APItem> { };
        public static Dictionary<long, APItem> TrapBoon = new Dictionary<long, APItem> { };
    }
}
