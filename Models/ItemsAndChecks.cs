using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Models
{
    public static class ItemsAndChecks
    {
        
    }

    public class ArchipelagoCheck
    {

    }

    public class ArchipelagoItem
    {
        // Item JSON definitions:
        // 
        // id: The local ID of the item. May vary from the archipelago id
        // displayName: the human readable name
        // itemType: the kind of item it unlocks
        //  - 0: Building
        //  - 1: Charm
        //  - 2: Companion
        //  - 3: Item, non clunker
        //  - 4: Item, clunker
        //  - 5: Sun Bell
        //  - 6: Storm or Challenge Bell
        //  - 7: Map Event
        //  - 8: Tribe
        // progressionTier: how important it is to unlock this item
        //  - 0: useless/trap
        //  - 1: generic
        //  - 2: useful, no extra checks
        //  - 3: useful, unlocks more checks
        //  - 4: storm bell, 1 power
        //  - 5: storm bell, 2 power
        //  - 6: storm bell, 3 power
        //  - 7: lumin vase
    }
}
