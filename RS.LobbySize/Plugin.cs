using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using Sons.Multiplayer;
using System.Threading.Tasks;

namespace RS.LobbySize
{
    [BepInPlugin("com.rabidsquirrel.sotf.lobbysize", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInProcess("SonsOfTheForestDS.exe")]
    public class DedicatedServerLobbySizePlugin : BasePlugin
    {
        internal ConfigEntry<int> MaximumPlayers { get; set; }
        internal ConfigEntry<int> ServerMaxPlayerSetting { get; set; }
        internal ConfigEntry<int> CheckInterval { get; set; }
        internal ConfigEntry<bool> ExtendedLogging { get; set; }
        internal static DedicatedServerLobbySizePlugin Instance { get; private set; }

        public async Task LoopHandler()
        {
            // This sucks, find a better way to do this
            while (GameServerManager.MaxPlayers != ServerMaxPlayerSetting.Value)
            {
                await Task.Delay(1);
            }

            // Log game server manager info
            Log.LogInfo("=============================================================================================");
            Log.LogInfo($"[{MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION}] [GameServerManager] IsDedicated:           {GameServerManager.IsDedicated}");
            Log.LogInfo($"[{MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION}] [GameServerManager] IsDedicated:           {GameServerManager.IsDedicated}");
            Log.LogInfo($"[{MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION}] [GameServerManager] IsDedicatedServer:     {GameServerManager.IsDedicatedServer}");
            Log.LogInfo($"[{MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION}] [GameServerManager] MaxPlayers:            {GameServerManager.MaxPlayers}");
            Log.LogInfo($"[{MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION}] [GameServerManager] IsPasswordProtected:   {GameServerManager.IsPasswordProtected}");
            Log.LogInfo($"[{MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION}] [GameServerManager] Password:              {GameServerManager.Password}");
            Log.LogInfo("=============================================================================================");

            if (GameServerManager.MaxPlayers != MaximumPlayers.Value)
            {
                Log.LogInfo($"[{MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION}] Correcting maximum player count from {GameServerManager.MaxPlayers} to {MaximumPlayers.Value}...");
                GameServerManager.MaxPlayers = MaximumPlayers.Value;
            }
        }

        public override void Load()
        {
            Instance = this;
            ServerMaxPlayerSetting = Config.Bind<int>(MyPluginInfo.PLUGIN_GUID, "ServerMaxPlayerSetting", 8, "This MUST correspond to your set player max in the default config (default is: 8)");
            MaximumPlayers = Config.Bind<int>(MyPluginInfo.PLUGIN_GUID, "MaximumPlayers", 16, "The currently set maximum number of players allowed (default is: 16)");
            CheckInterval = Config.Bind<int>(MyPluginInfo.PLUGIN_GUID, "CheckInterval", 30, "The interval in which to check the maximum number of players allowed (default is: 60 seconds)");
            ExtendedLogging = Config.Bind<bool>(MyPluginInfo.PLUGIN_GUID, "ExtendedLogging", false, "Whether or not extended logging is enabled (default is: false)");

            Task.Run(() => LoopHandler());

            Log.LogInfo($"Plugin [{MyPluginInfo.PLUGIN_GUID} {MyPluginInfo.PLUGIN_VERSION}] is loaded!");
        }
    }
}