using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Mirror;
using YAPYAP;

namespace PlayerCount;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;
    internal static ConfigEntry<int> MaxPlayers = null!;

    private void Awake()
    {
        Log = Logger;

        Log.LogMessage($"Plugin {Name} is loaded!");
        MaxPlayers = Config.Bind("Settings", "Max Players", 10, new ConfigDescription("Set your desired maximum number of players here!", new AcceptableValueRange<int>(2, 20)));
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        Log.LogMessage($"Max players set to {MaxPlayers.Value}");
    }

    [HarmonyPatch(typeof(CoreFsmYap), nameof(CoreFsmYap.Setup))]
    public class FsmYapHook
    {
        public static void Postfix(CoreFsmYap __instance)
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
