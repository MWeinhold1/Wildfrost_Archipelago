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
        public static ISessionManager sessionManager { get; private set; }
        public static AssetManager assetManager { get; private set; }
        public static Managers.EventManager eventManager { get; private set; }

        public static void Init(bool debug = false)
        {
            if (init) return;

            assetManager = new AssetManager();
            eventManager = new Managers.EventManager();
            if (debug)
                sessionManager = new MockSessionManager();
            else
                sessionManager = new APSessionManager();

            init = true;
        }
    }
}
