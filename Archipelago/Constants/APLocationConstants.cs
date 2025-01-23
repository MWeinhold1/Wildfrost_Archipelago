using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Archipelago.Constants
{
    public enum APLocationType
    {
        unknown = 0,
        buildingChallenge = 1,
        idolChallenge = 2,
        enemyKill = 3,
        bossKill = 4,
        card = 5,
        companion = 6,
        charm = 7,
        bell = 8,
    }

    public class APLocation
    {
        int id;
        APLocationType type;
        string description;
        bool multiple;

        public APLocation(int id, int type, string description, bool multiple = false)
        {
            this.id = id;
            this.type = (APLocationType)type;
            this.description = description;
            this.multiple = multiple;
        }
    };

    public static class APLocationConstants
    {
        public static Dictionary<int, APLocation> Locations = new Dictionary<int, APLocation>
        {
            // Town Buildings
            {1000, new APLocation(1000, 1, "Build Frostoscope")},
            {1001, new APLocation(1001, 1, "Build Hot Spring")},
            {1002, new APLocation(1002, 1, "Build Icebreaker Cabin")},
            {1003, new APLocation(1003, 1, "Build Inventor Hut")},
            {1004, new APLocation(1004, 1, "Build Pet House")},
            {1005, new APLocation(1005, 1, "Build Tribe Hall")},
            {1010, new APLocation(1010, 1, "Hot Spring: Tiny Tyko")},
            {1011, new APLocation(1011, 1, "Hot Spring: Bombom")},
            {1012, new APLocation(1012, 1, "Hot Spring: Nova")},
            {1013, new APLocation(1013, 1, "Hot Spring: Lupa")},
            {1014, new APLocation(1014, 1, "Hot Spring: The Baker")},
            {1015, new APLocation(1015, 1, "Hot Spring: Toaster")},
            {1020, new APLocation(1020, 1, "Icebreaker: Shade Sculptor")},
            {1021, new APLocation(1021, 1, "Icebreaker: Charm Merchant")},
            {1022, new APLocation(1022, 1, "Icebreaker: Gnome Traveller")},
            {1030, new APLocation(1030, 1, "Inventor: Slapcrackers")},
            {1031, new APLocation(1031, 1, "Inventor: Kobonker")},
            {1032, new APLocation(1032, 1, "Inventor: Grabber")},
            {1033, new APLocation(1033, 1, "Inventor: Scrap Pile")},
            {1034, new APLocation(1034, 1, "Inventor: Mega Mimik")},
            {1035, new APLocation(1035, 1, "Inventor: Krono")},
            {1040, new APLocation(1040, 1, "Pet House: Booshu")},
            {1041, new APLocation(1041, 1, "Pet House: Loki")},
            {1042, new APLocation(1042, 1, "Pet House: Sneezle")},
            {1043, new APLocation(1043, 1, "Pet House: Spike")},
            {1044, new APLocation(1044, 1, "Pet House: Binku")},
            {1045, new APLocation(1045, 1, "Pet House: Lil Gazi")},
            {1050, new APLocation(1050, 1, "Tribe Hall: Shademancers")},
            {1051, new APLocation(1051, 1, "Tribe Hall: Clunkmasters")},
            // Idol Challenges
            {2000, new APLocation(2000, 2, "Balloonist Idol")},
            {2001, new APLocation(2001, 2, "Beastmaster Idol")},
            {2002, new APLocation(2002, 2, "Berry Good Idol")},
            {2003, new APLocation(2003, 2, "Best Friends Idol")},
            {2004, new APLocation(2004, 2, "Big Hitter Idol")},
            {2005, new APLocation(2005, 2, "Bigger Hitter Idol")},
            {2006, new APLocation(2006, 2, "Charmless Idol")},
            {2007, new APLocation(2007, 2, "Clunkmaster Idol")},
            {2008, new APLocation(2008, 2, "Feed The Beast Idol")},
            {2009, new APLocation(2009, 2, "Gnome Friend Idol")},
            {2010, new APLocation(2010, 2, "Gnomebringer Idol")},
            {2011, new APLocation(2011, 2, "High Roller Idol")},
            {2012, new APLocation(2012, 2, "Hoarder Idol")},
            {2013, new APLocation(2013, 2, "Icemaster Idol")},
            {2014, new APLocation(2014, 2, "Lone Survivor Idol")},
            {2015, new APLocation(2015, 2, "Long Live The King Idol")},
            {2016, new APLocation(2016, 2, "Minimalist Idol")},
            {2017, new APLocation(2017, 2, "One Punch Idol")},
            {2018, new APLocation(2018, 2, "Rampage Idol")},
            {2019, new APLocation(2019, 2, "Ritual Idol")},
            {2020, new APLocation(2020, 2, "Shademancer Idol")},
            {2021, new APLocation(2021, 2, "Snowball Fight Idol")},
            {2022, new APLocation(2022, 2, "Snowdweller Idol")},
            {2023, new APLocation(2023, 2, "Sunbringer Idol")},
            {2024, new APLocation(2024, 2, "Tough Nut Idol")},
            {2025, new APLocation(2025, 2, "Toxic Idol")},
            {2026, new APLocation(2026, 2, "Undefeated Idol")},
            // Basic Enemy Kills
            {3000, new APLocation(3000, 3, "Kill Baby Snowbo")},
            {3001, new APLocation(3001, 3, "Kill Beeberry")},
            {3002, new APLocation(3002, 3, "Kill Berry Witch")},
            {3003, new APLocation(3003, 3, "Kill Bigfoot")},
            {3004, new APLocation(3004, 3, "Kill Blaze Beetles")},
            {3005, new APLocation(3005, 3, "Kill Bulbhead")},
            {3006, new APLocation(3006, 3, "Kill Burster")},
            {3007, new APLocation(3007, 3, "Kill Chungoon")},
            {3008, new APLocation(3008, 3, "Kill Conker")},
            {3009, new APLocation(3009, 3, "Kill Dungrok")},
            {3010, new APLocation(3010, 3, "Kill Earth Berry")},
            {3011, new APLocation(3011, 3, "Kill Frostinger")},
            {3012, new APLocation(3012, 3, "Kill Gobbler")},
            {3013, new APLocation(3013, 3, "Kill Gobling")},
            {3014, new APLocation(3014, 3, "Kill Gogong")},
            {3015, new APLocation(3015, 3, "Kill Gok")},
            {3016, new APLocation(3016, 3, "Kill Grink")},
            {3017, new APLocation(3017, 3, "Kill Grizzle")},
            {3018, new APLocation(3018, 3, "Kill Grog")},
            {3019, new APLocation(3019, 3, "Kill Gromble")},
            {3020, new APLocation(3020, 3, "Kill Grouchy")},
            {3021, new APLocation(3021, 3, "Kill Grumps")},
            {3022, new APLocation(3022, 3, "Kill Gunk Gobbler")},
            {3023, new APLocation(3023, 3, "Kill Gunkback")},
            {3024, new APLocation(3024, 3, "Kill Hog")},
            {3025, new APLocation(3025, 3, "Kill Jab Joat")},
            {3026, new APLocation(3026, 3, "Kill Krab")},
            {3027, new APLocation(3027, 3, "Kill Kraken")},
            {3028, new APLocation(3028, 3, "Kill Krawler")},
            {3029, new APLocation(3029, 3, "Kill Lump")},
            {3030, new APLocation(3030, 3, "Kill Makoko")},
            {3031, new APLocation(3031, 3, "Kill Marrow")},
            {3032, new APLocation(3032, 3, "Kill Minimoko")},
            {3033, new APLocation(3033, 3, "Kill Octako")},
            {3034, new APLocation(3034, 3, "Kill Ooba Bear")},
            {3035, new APLocation(3035, 3, "Kill Paw Paw")},
            {3036, new APLocation(3036, 3, "Kill Pecan")},
            {3037, new APLocation(3037, 3, "Kill Pengoon")},
            {3038, new APLocation(3038, 3, "Kill Pepper Witch")},
            {3039, new APLocation(3039, 3, "Kill Plum")},
            {3040, new APLocation(3040, 3, "Kill Popshroom")},
            {3041, new APLocation(3041, 3, "Kill Porkypine")},
            {3042, new APLocation(3042, 3, "Kill Prickle")},
            {3043, new APLocation(3043, 3, "Kill Puffball")},
            {3044, new APLocation(3044, 3, "Kill Pygmy")},
            {3045, new APLocation(3045, 3, "Kill Rockhog")},
            {3046, new APLocation(3046, 3, "Kill Shell Witch")},
            {3047, new APLocation(3047, 3, "Kill Shroom Gobbler")},
            {3048, new APLocation(3048, 3, "Kill Shrootles")},
            {3049, new APLocation(3049, 3, "Kill Smog")},
            {3050, new APLocation(3050, 3, "Kill Snow Gobbler")},
            {3051, new APLocation(3051, 3, "Kill Snowbirb")},
            {3052, new APLocation(3052, 3, "Kill Snowbo")},
            {3053, new APLocation(3053, 3, "Kill Spuncher")},
            {3054, new APLocation(3054, 3, "Kill Tentickle")},
            {3055, new APLocation(3055, 3, "Kill Waddlegoons")},
            {3056, new APLocation(3056, 3, "Kill Warthog")},
            {3057, new APLocation(3057, 3, "Kill Wild Snoolf")},
            {3058, new APLocation(3058, 3, "Kill Willow")},
            {3059, new APLocation(3059, 3, "Kill Winter Worm")},
            {3060, new APLocation(3060, 3, "Kill Woolly Drek")},
            {3098, new APLocation(3098, 3, "Kill Naked Gnome")},
            {3099, new APLocation(3099, 3, "Kill ArchipelaGnome")},
            // (Mini) Boss Kills
            {4000, new APLocation(4000, 4, "Kill Big Peng")},
            {4001, new APLocation(4001, 4, "Kill Bigloo")},
            {4002, new APLocation(4002, 4, "Kill Bogberry")},
            {4003, new APLocation(4003, 4, "Kill Bolgo")},
            {4004, new APLocation(4004, 4, "Kill Bumbo")},
            {4005, new APLocation(4005, 4, "Kill King Moko")},
            {4006, new APLocation(4006, 4, "Kill Lumako")},
            {4007, new APLocation(4007, 4, "Kill Maw Jaw")},
            {4008, new APLocation(4008, 4, "Kill Muttonhead")},
            {4009, new APLocation(4009, 4, "Kill Nimbus")},
            {4010, new APLocation(4010, 4, "Kill Numskull")},
            {4011, new APLocation(4011, 4, "Kill Queen Globerry")},
            {4012, new APLocation(4012, 4, "Kill Razor")},
            {4013, new APLocation(4013, 4, "Kill The Ringer")},
            {4014, new APLocation(4014, 4, "Kill The Snow Knight")},
            {4015, new APLocation(4015, 4, "Kill Veiled Lady")},
            {4016, new APLocation(4016, 4, "Kill Weevil")},
            {4017, new APLocation(4017, 4, "Kill Bamboozle")},
            {4018, new APLocation(4018, 4, "Kill Infernoko")},
            {4019, new APLocation(4019, 4, "Kill Krunker")},
            {4020, new APLocation(4020, 4, "Kill Truffle")},
            {4021, new APLocation(4021, 4, "Kill Frost Guardian")},
            {4022, new APLocation(4022, 4, "Kill Frost Bomber")},
            {4023, new APLocation(4023, 4, "Kill Frost Crusher")},
            {4024, new APLocation(4024, 4, "Kill Frost Jailer")},
            {4025, new APLocation(4025, 4, "Kill Frost Junker")},
            {4026, new APLocation(4026, 4, "Kill Frost Muncher")},
            {4027, new APLocation(4027, 4, "Kill Frost Lancer")},
            // Multi-location item pools
            {5000, new APLocation(5000, 5, "Snowdweller Card", true)},
            {5100, new APLocation(5100, 5, "Shademancer Card", true)},
            {5200, new APLocation(5200, 5, "Clunkmaster Card", true)},
            {5300, new APLocation(5300, 5, "Common Card", true)},
            {6000, new APLocation(6000, 6, "Snowdweller Companion", true)},
            {6100, new APLocation(6100, 6, "Shademancer Companion", true)},
            {6200, new APLocation(6200, 6, "Clunkmaster Companion", true)},
            {6300, new APLocation(6300, 6, "Common Companion", true)},
            {7000, new APLocation(7000, 7, "Snowdweller Charm", true)},
            {7100, new APLocation(7100, 7, "Shademancer Charm", true)},
            {7200, new APLocation(7200, 7, "Clunkmaster Charm", true)},
            {7300, new APLocation(7300, 7, "Common Charm", true)},
            {8000, new APLocation(8000, 8, "Boss Reward Bell", true)}
        };
    }
}
