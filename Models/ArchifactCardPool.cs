using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Models
{
    public class ArchifactCardPool
    {
        /// <summary>
        /// Name of the item pool these cards are replacing
        /// </summary>
        public string name;

        /// <summary>
        /// Number of replacement cards to make
        /// </summary>
        public int count;

        /// <summary>
        /// Record of what the original cards were
        /// </summary>
        public List<DataFile> originalCards = new List<DataFile>();

        /// <summary>
        /// Replacement card builders
        /// </summary>
        public List<CardDataBuilder> builders = new List<CardDataBuilder>();

        /// <summary>
        /// Was the original reward pool found
        /// </summary>
        public bool found;

        /// <summary>
        /// The expected card type
        /// </summary>
        public RewardPool.Type type;

        public ArchifactCardPool(string name, int count, RewardPool.Type type)
        {
            this.name = name;
            this.count = count;
            this.type = type;
        }
    }
}
