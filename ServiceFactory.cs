using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildfrost_Archipelago.Archipelago;
using Wildfrost_Archipelago.Interfaces;

namespace Wildfrost_Archipelago
{
    public static class ServiceFactory
    {
        private static bool init = false;
        private static ISessionManager sessionManager;
        private static AssetManager assetManager;

        public static void Init(bool debug = false)
        {
            if (init) return;

            assetManager = new AssetManager();
            if (debug)
                sessionManager = new MockSessionManager();
            else
                sessionManager = new APSessionManager();

            init = true;
        }

        public static ISessionManager GetSessionManager() => sessionManager;
        public static AssetManager GetAssetManager() => assetManager;
    }
}
