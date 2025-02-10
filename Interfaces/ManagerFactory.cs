using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Archipelago;

namespace Wildfrost_Archipelago.Interfaces
{
    static class ManagerFactory
    {
        private static ISessionManager sessionManager;
        private static AssetManager assetManager;
        private static ItemManager itemManager;

        public static ISessionManager GetSessionManager()
        {
            if (sessionManager == null && WildfrostArchipelago.debugMode) sessionManager = new MockSessionManager();
            else if (sessionManager == null) sessionManager = new APSessionManager();  
            return sessionManager;
        }

        public static AssetManager GetAssetManager()
        {
            if (assetManager == null) assetManager = new AssetManager();
            return assetManager;
        }

        public static ItemManager GetItemManager()
        {
            if (itemManager == null) itemManager = new ItemManager();
            return itemManager;
        }
    }
}
