using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("Dynamic Slots", "bmgjet", "1.0.0")]
    [Description("Auto Sizes Server Slots To Help JustWiped Fill On Wipe Stat")]
    class DynamicSlots : RustPlugin
    {
        #region Hooks
        private void OnPlayerConnected()
        {
            if (covalence.Server.MaxPlayers < config.maxSlots)
            {
                covalence.Server.MaxPlayers++;
            }
        }

        void OnPlayerDisconnected()
        {
            if (covalence.Server.MaxPlayers > config.startingSlots)
            {
                covalence.Server.MaxPlayers--;
            }
        }
        #endregion

        #region Configuration
        private static ConfigData config = new ConfigData();

        private class ConfigData
        {
            [JsonProperty(PropertyName = "Starting Slots")]
            public int startingSlots = 10;

            [JsonProperty(PropertyName = "Maximum Slots")]
            public int maxSlots = 300;

        }

        protected override void LoadConfig()
        {
            base.LoadConfig();

            try
            {
                config = Config.ReadObject<ConfigData>();
                if (config == null) LoadDefaultConfig();
            }
            catch
            {
                PrintError("Your config seems to be corrupted. Will load defaults.");
                LoadDefaultConfig();
                return;
            }
            SaveConfig();
        }

        protected override void LoadDefaultConfig()
        {
            config = new ConfigData();
        }

        protected override void SaveConfig() => Config.WriteObject(config);
        #endregion
    }
}
