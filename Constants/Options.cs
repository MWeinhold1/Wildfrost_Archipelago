using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Constants
{
    public static class Options
    {
        public enum Goal
        {
            guardian = 0,
            heart = 1,
            completeTown = 2
        }

        [Flags]
        public enum TownSettings
        {
            vanilla = 0,
            shuffle_buildings = 1,
            shuffle_building_challenges = 2,
            buildings_in_parallel = 4,
            challenges_in_parallel = 8,
            tribes_shuffled = 16,
        }

        [Flags]
        public enum IdolDifficulty
        {
            all = 0,
            disable_sunbringer = 1,
            disable_undefeated = 2,
            disable_gnomebringer = 4
        }

        [Flags]
        public enum CardPoolSettings
        {
            vanilla = 0,
            shuffle_snoof = 1,
            shuffle_start_items = 2,
            shuffle_lumin_vase = 4,

        }
    }
}
