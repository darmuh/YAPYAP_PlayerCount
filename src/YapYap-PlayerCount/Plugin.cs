using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Mirror;
using YAPYAP;

namespace PlayerCount;

[BepInAutoPlugin]
[BepInDependency(QuotaQueenGUID, BepInDependency.DependencyFlags.SoftDependency)]
public partial class Plugin : BaseUnityPlugin
{
    internal const string QuotaQueenGUID = "dev.mamallama.quotaqueen";
    internal static ManualLogSource Log { get; private set; } = null!;
    internal static ConfigEntry<int> MaxPlayers { get; private set; } = null!;
    private static System.Version MinimumQueenVer = new("0.3.0");
    internal static bool QuotaQueenExists
    {
        get
        {
            if(Chainloader.PluginInfos.TryGetValue(QuotaQueenGUID, out var info))
            {
                return info.Metadata.Version >= MinimumQueenVer;
            }
            return false;
        }
    }

    private void Awake()
    {
        Log = Logger;

        Log.LogMessage($"Plugin {Name} is loaded!");
        MaxPlayers = Config.Bind("Settings", "Max Players", 10, new ConfigDescription("Set your desired maximum number of players here!", new AcceptableValueRange<int>(2, 20)));
            
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        Log.LogMessage($"Max players set to {MaxPlayers.Value}");
    }

    private void Start()
    {
        if (!QuotaQueenExists)
            return;

        Log.LogMessage("Adding QuotaQueen Strategies!");
        Quota.QuotaQueenUsage.AddQuotaStrategies();
    }

    [HarmonyPatch(typeof(YapFsm), nameof(YapFsm.Initialise))]
    public class FsmYapHook
    {
        public static void Prefix(YapFsm __instance)
        {
            __instance._maxPlayers = MaxPlayers.Value;
        }
    }

    [HarmonyPatch(typeof(NetworkManager), nameof(NetworkManager.StartHost))]
    public class MirrorNetManHook
    {
        public static void Prefix(NetworkManager __instance)
        {
            __instance.maxConnections = MaxPlayers.Value;
        }
    }
}
