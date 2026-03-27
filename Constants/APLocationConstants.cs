using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Constants
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
        public int id { get; private set; }
        public APLocationType type { get; private set; }
        public string localDescription { get; private set; }
        public string internalName { get; private set; }
        public bool multiple { get; private set; }
        public bool isLocal { get; private set; }
        public string unlockedItem { get; private set; }
        public string targetPlayerName { get; private set; }

        public APLocation(int id, int type, string internalName, string localDescription, bool multiple = false)
        {
            this.id = id;
            this.type = (APLocationType)type;
            this.internalName = internalName;
            this.localDescription = localDescription;
            this.multiple = multiple;
        }

        // Used for copying a location based on an extisting location reference
        public APLocation(int id, bool isLocal, string unlockName, string targetPlayerName)
        {
            APLocation locRef = APLocationConstants.LocationReferences[id];
            this.id = id;
            this.isLocal = isLocal;
            this.unlockedItem = unlockName;
            this.targetPlayerName = targetPlayerName;
            this.type = locRef.type;
            this.localDescription = locRef.localDescription;
            this.multiple = locRef.multiple;
        }
    };

    public static class APLocationConstants
    {
        public static int GetLocationIDFromName(string name)
        {
            return LocationReferences.Where(a => a.Value.internalName == name).First().Key;
        }
        public static Dictionary<int, APLocation> LocationReferences = new Dictionary<int, APLocation>
        {
            // Town Buildings
            //{10000, new APLocation(10000, 1, "", "Build Frostoscope")},
            {10000, new APLocation(10000, 1, "Challenge Hot Spring", "Build Hot Spring")},
            {10001, new APLocation(10001, 1, "Challenge Icebreakers", "Build Icebreaker Cabin")},
            {10002, new APLocation(10002, 1, "Challenge Inventors Hut", "Build Inventor Hut")},
            {10003, new APLocation(10003, 1, "Challenge Pet House", "Build Pet House")},
            //{10005, new APLocation(10005, 1, "", "Build Tribe Hall")},
            {10004, new APLocation(10004, 1, "Challenge Companion 1", "Hot Spring: Tiny Tyko")},
            {10005, new APLocation(10005, 1, "Challenge Companion 2", "Hot Spring: Bombom")},
            {10006, new APLocation(10006, 1, "Challenge Companion 3", "Hot Spring: Nova")},
            {10007, new APLocation(10007, 1, "Challenge Companion 4","Hot Spring: Lupa")},
            {10008, new APLocation(10008, 1, "Challenge Companion 5","Hot Spring: The Baker")},
            {10009, new APLocation(10009, 1, "Challenge Companion 6","Hot Spring: Toaster")},
            {10010, new APLocation(10010, 1, "Challenge Event 1", "Icebreaker: Shade Sculptor")},
            {10011, new APLocation(10011, 1, "Challenge Event 2", "Icebreaker: Charm Merchant")},
            {10012, new APLocation(10012, 1, "Challenge Event 3", "Icebreaker: Gnome Traveller")},
            {10013, new APLocation(10013, 1, "Challenge Item 1", "Inventor: Slapcrackers")},
            {10014, new APLocation(10014, 1, "Challenge Item 2","Inventor: Kobonker")},
            {10015, new APLocation(10015, 1, "Challenge Item 3","Inventor: Grabber")},
            {10016, new APLocation(10016, 1, "Challenge Item 4","Inventor: Scrap Pile")},
            {10017, new APLocation(10017, 1, "Challenge Item 5","Inventor: Mega Mimik")},
            {10018, new APLocation(10018, 1, "Challenge Item 6","Inventor: Krono")},
            {10019, new APLocation(10019, 1, "Challenge Pet 1", "Pet House: Booshu")},
            {10020, new APLocation(10020, 1, "Challenge Pet 2","Pet House: Loki")},
            {10021, new APLocation(10021, 1, "Challenge Pet 3","Pet House: Sneezle")},
            {10022, new APLocation(10022, 1, "Challenge Pet 4","Pet House: Spike")},
            {10023, new APLocation(10023, 1, "Challenge Pet 4a","Pet House: Binku")},
            {10024, new APLocation(10024, 1, "Challenge Pet 5","Pet House: Lil Gazi")},
            {10025, new APLocation(10025, 1, "Challenge Tribe 1", "Tribe Hall: Shademancers")},
            {10026, new APLocation(10026, 1, "Challenge Tribe 2","Tribe Hall: Clunkmasters")},
            // Idol Challenges
            {20000, new APLocation(20000, 2, "Challenge Charm 14", "Balloonist Idol")},
            {20001, new APLocation(20001, 2, "Challenge Charm 22", "Beastmaster Idol")},
            {20002, new APLocation(20002, 2, "Challenge Charm 24", "Berry Good Idol")},
            {20003, new APLocation(20003, 2, "Challenge Charm 21", "Best Friends Idol")},
            {20004, new APLocation(20004, 2, "Challenge Charm 3", "Big Hitter Idol")},
            {20005, new APLocation(20005, 2, "Challenge Charm 4", "Bigger Hitter Idol")},
            {20006, new APLocation(20006, 2, "Challenge Charm 19", "Challenge Charmless Idol")},
            {20007, new APLocation(20007, 2, "Challenge Charm 27", "Clunkmaster Idol")},
            {20008, new APLocation(20008, 2, "Challenge Charm 13", "Feed The Beast Idol")},
            {20009, new APLocation(20009, 2, "Challenge Charm 11", "Gnome Friend Idol")},
            {20010, new APLocation(20010, 2, "Challenge Charm 17", "Gnomebringer Idol")},
            {20011, new APLocation(20011, 2, "Challenge Charm 10", "High Roller Idol")},
            {20012, new APLocation(20012, 2, "Challenge Charm 18", "Hoarder Idol")},
            {20013, new APLocation(20013, 2, "Challenge Charm 9", "Icemaster Idol")},
            {20014, new APLocation(20014, 2, "Challenge Charm 1", "Lone Survivor Idol")},
            {20015, new APLocation(20015, 2, "Challenge Charm 12", "Long Live The King Idol")},
            {20016, new APLocation(20016, 2, "Challenge Charm 23", "Minimalist Idol")},
            {20017, new APLocation(20017, 2, "Challenge Charm 8", "One Punch Idol")},
            {20018, new APLocation(20018, 2, "Challenge Charm 20", "Rampage Idol")},
            {20019, new APLocation(20019, 2, "Challenge Charm 6", "Ritual Idol")},
            {20020, new APLocation(20020, 2, "Challenge Charm 26", "Shademancer Idol")},
            {20021, new APLocation(20021, 2, "Challenge Charm 2", "Snowball Fight Idol")},
            {20022, new APLocation(20022, 2, "Challenge Charm 24", "Snowdweller Idol")},
            {20023, new APLocation(20023, 2, "Challenge Charm 16", "Sunbringer Idol")},
            {20024, new APLocation(20024, 2, "Challenge Charm 5", "Tough Nut Idol")},
            {20025, new APLocation(20025, 2, "Challenge Charm 7", "Toxic Idol")},
            {20026, new APLocation(20026, 2, "Challenge Charm 15", "Undefeated Idol")},
            // Basic Enemy Kills
            {30000, new APLocation(30000, 3, "BabySnowbo", "Kill Baby Snowbo")},
            {30001, new APLocation(30001, 3, "Beeberry", "Kill Beeberry")},
            {30002, new APLocation(30002, 3, "BerryWitch", "Kill Berry Witch")},
            {30003, new APLocation(30003, 3, "Smakk", "Kill Bigfoot")},
            {30004, new APLocation(30004, 3, "Sheep", "Kill Blaze Beetles")},
            {30005, new APLocation(30005, 3, "BulbHead", "Kill Bulbhead")},
            {30006, new APLocation(30006, 3, "Burster", "Kill Burster")},
            {30007, new APLocation(30007, 3, "Chungoon", "Kill Chungoon")},
            {30008, new APLocation(30008, 3, "Conker", "Kill Conker")},
            {30009, new APLocation(30009, 3, "Smash", "Kill Dungrok")},
            {30010, new APLocation(30010, 3, "BerryMonster", "Kill Earth Berry")},
            {30011, new APLocation(30011, 3, "Frostinger", "Kill Frostinger")},
            {30012, new APLocation(30012, 3, "Gobbler", "Kill Gobbler")},
            {30013, new APLocation(30013, 3, "Gobling", "Kill Gobling")},
            {30014, new APLocation(30014, 3, "Smackgoon", "Kill Gogong")},
            {30015, new APLocation(30015, 3, "Gok", "Kill Gok")},
            {30016, new APLocation(30016, 3, "Grink", "Kill Grink")},
            {30017, new APLocation(30017, 3, "Sno", "Kill Grizzle")},
            {30018, new APLocation(30018, 3, "Grog", "Kill Grog")},
            {30019, new APLocation(30019, 3, "Noodle", "Kill Gromble")},
            {30020, new APLocation(30020, 3, "Grouchy", "Kill Grouchy")},
            {30021, new APLocation(30021, 3, "Chunky", "Kill Grumps")},
            {30022, new APLocation(30022, 3, "SBelly", "Kill Gunk Gobbler")},
            {30023, new APLocation(30023, 3, "SMime", "Kill Gunkback")},
            {30024, new APLocation(30024, 3, "Wildling", "Kill Hog")},
            {30025, new APLocation(30025, 3, "JabJoat", "Kill Jab Joat")},
            {30026, new APLocation(30026, 3, "Blockhead", "Kill Krab")},
            {30027, new APLocation(30027, 3, "Kraken", "Kill Kraken")},
            {30028, new APLocation(30028, 3, "Icemason", "Kill Krawler")},
            {30029, new APLocation(30029, 3, "Lump", "Kill Lump")},
            {30030, new APLocation(30030, 3, "Makoko", "Kill Makoko")},
            {30031, new APLocation(30031, 3, "Spyke", "Kill Marrow")},
            {30032, new APLocation(30032, 3, "Minimoko", "Kill Minimoko")},
            {30033, new APLocation(30033, 3, "Kalamari", "Kill Octako")},
            {30034, new APLocation(30034, 3, "OobaBear", "Kill Ooba Bear")},
            {30035, new APLocation(30035, 3, "Stinghorn", "Kill Paw Paw")},
            {30036, new APLocation(30036, 3, "Pecan", "Kill Pecan")},
            {30037, new APLocation(30037, 3, "Pengoon", "Kill Pengoon")},
            {30038, new APLocation(30038, 3, "PepperWitch", "Kill Pepper Witch")},
            {30039, new APLocation(30039, 3, "Berro", "Kill Plum")},
            {30040, new APLocation(30040, 3, "Popshroom", "Kill Popshroom")},
            {30041, new APLocation(30041, 3, "Sporkypine", "Kill Porkypine")},
            {30042, new APLocation(30042, 3, "Prickle", "Kill Prickle")},
            {30043, new APLocation(30043, 3, "Puffball", "Kill Puffball")},
            {30044, new APLocation(30044, 3, "Pygmy", "Kill Pygmy")},
            {30045, new APLocation(30045, 3, "Wally", "Kill Rockhog")},
            {30046, new APLocation(30046, 3, "ShellWitch", "Kill Shell Witch")},
            {30047, new APLocation(30047, 3, "ShroomGobbler", "Kill Shroom Gobbler")},
            {30048, new APLocation(30048, 3, "Shrootles", "Kill Shrootles")},
            {30049, new APLocation(30049, 3, "Confuddler", "Kill Smog")},
            {30050, new APLocation(30050, 3, "SnowGobbler", "Kill Snow Gobbler")},
            {30051, new APLocation(30051, 3, "Snowbirb", "Kill Snowbirb")},
            {30052, new APLocation(30052, 3, "Snowbo", "Kill Snowbo")},
            {30053, new APLocation(30053, 3, "Spuncher", "Kill Spuncher")},
            {30054, new APLocation(30054, 3, "Voido", "Kill Tentickle")},
            {30055, new APLocation(30055, 3, "Waddlegoons", "Kill Waddlegoons")},
            {30056, new APLocation(30056, 3, "Wrecker", "Kill Warthog")},
            {30057, new APLocation(30057, 3, "Snoolf", "Kill Wild Snoolf")},
            {30058, new APLocation(30058, 3, "Burner", "Kill Willow")},
            {30059, new APLocation(30059, 3, "SnormWorm", "Kill Winter Worm")},
            {30060, new APLocation(30060, 3, "WoollyDrek", "Kill Woolly Drek")},
            // Gnome
            {30061, new APLocation(30061, 3, "NakedGnome", "Kill Naked Gnome")},
            {30062, new APLocation(30062, 3, "", "Kill ArchipelaGnome")},
            // Clunkers
            {30063, new APLocation(30063, 3, "Vimik", "Kill Bombarder")},
            {30064, new APLocation(30064, 3, "IceForge", "Kill Ice Forge")},
            {30065, new APLocation(30065, 3, "MiniForge", "Kill Ice Lantern")},
            {30066, new APLocation(30066, 3, "Mega Mimik", "Kill Mega Mimik")},
            {30067, new APLocation(30067, 3, "Mimik", "Kill Mimik")},
            {30068, new APLocation(30068, 3, "InkBomb", "Kill Octobom")},
            {30069, new APLocation(30069, 3, "Plinker", "Kill Plinker")},
            {30070, new APLocation(30070, 3, "SpikeWall", "Kill Spike Wall")},
            // (Mini) Boss Kills
            {40000, new APLocation(40000, 4, "BigPeng", "Kill Big Peng")},
            {40001, new APLocation(40001, 4, "Muttonhead", "Kill Bigloo")}, //no, this is not a typo. Bigloo's internal name is another miniboss's real name
            {40002, new APLocation(40002, 4, "Bogberry", "Kill Bogberry")},
            {40003, new APLocation(40003, 4, "Bolgo", "Kill Bolgo")},
            {40004, new APLocation(40004, 4, "Bumbo", "Kill Bumbo")},
            {40005, new APLocation(40005, 4, "MonkeyKing", "Kill King Moko")},
            {40006, new APLocation(40006, 4, "Blot", "Kill Lumako")},
            {40007, new APLocation(40007, 4, "Toothless", "Kill Maw Jaw")},
            {40008, new APLocation(40008, 4, "GukaGuka", "Kill Muttonhead")},
            {40009, new APLocation(40009, 4, "Bomber", "Kill Nimbus")},
            {40010, new APLocation(40010, 4, "Numskull", "Kill Numskull")},
            {40011, new APLocation(40011, 4, "Turnip", "Kill Queen Globerry")},
            {40012, new APLocation(40012, 4, "CrazyEyes", "Kill Razor")},
            {40013, new APLocation(40013, 4, "Frosty", "Kill The Ringer")},
            {40014, new APLocation(40014, 4, "SnowKnight", "Kill The Snow Knight")},
            {40015, new APLocation(40015, 4, "VeiledLady", "Kill Veiled Lady")},
            {40016, new APLocation(40016, 4, "Smosh", "Kill Weevil")},
            // Boss Kills
            {40017, new APLocation(40017, 4, "Split Boss", "Kill Bamboozle")}, //the spaces are there because these locations are named after the battle names - the check is awarded upon winning the boss's respective battle
            {40018, new APLocation(40018, 4, "Frenzy Boss", "Kill Infernoko")},
            {40019, new APLocation(40019, 4, "Clunker Boss", "Kill Krunker")},
            {40020, new APLocation(40020, 4, "Toadstool Boss", "Kill Truffle")},
            {40021, new APLocation(40021, 4, "Final Boss", "Kill Frost Guardian")},
            {40022, new APLocation(40022, 4, "Final Final Boss", "Kill Frost Bomber")},
            {40023, new APLocation(40023, 4, "Final Final Boss", "Kill Frost Crusher")},
            {40024, new APLocation(40024, 4, "Final Final Boss", "Kill Frost Jailer")},
            {40025, new APLocation(40025, 4, "Final Final Boss", "Kill Frost Junker")},
            {40026, new APLocation(40026, 4, "Final Final Boss", "Kill Frost Muncher")},
            {40027, new APLocation(40027, 4, "Final Final Boss", "Kill Frost Lancer")},
            // Multi-location item pools
            {50000, new APLocation(50000, 5, "", "Snowdweller Card", true)},
            {51000, new APLocation(51000, 5, "", "Shademancer Card", true)},
            {52000, new APLocation(52000, 5, "", "Clunkmaster Card", true)},
            {60000, new APLocation(60000, 6, "","Snowdweller Companion", true)},
            {61000, new APLocation(61000, 6, "","Shademancer Companion", true)},
            {62000, new APLocation(62000, 6, "","Clunkmaster Companion", true)},
            {70000, new APLocation(70000, 7, "","Snowdweller Charm", true)},
            {71000, new APLocation(71000, 7, "","Shademancer Charm", true)},
            {72000, new APLocation(72000, 7, "","Clunkmaster Charm", true)},
            {80000, new APLocation(80000, 8, "","Boss Reward Bell", true)}
        };
    }
}
