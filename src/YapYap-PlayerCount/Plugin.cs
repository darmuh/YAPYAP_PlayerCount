using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Mirror;
using UnityEngine;
using YAPYAP;

namespace PlayerCount;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;
    internal static ConfigEntry<int> MaxPlayers { get; private set; } = null!;
    internal static ConfigEntry<QuotaStyle> QuotaModifier {  get; private set; } = null!;

    internal enum QuotaStyle
    {
        None = 0,
        OnlyScaleUp,
        AlwaysScale
    }

    private void Awake()
    {
        Log = Logger;

        Log.LogMessage($"Plugin {Name} is loaded!");
        MaxPlayers = Config.Bind("Settings", "Max Players", 10, new ConfigDescription("Set your desired maximum number of players here!", new AcceptableValueRange<int>(2, 20)));
        QuotaModifier = Config.Bind("Settings", "Quota Modifier", QuotaStyle.None, 
            """
            Determines if quota should be modified based on the number of players in the lobby
            None - No modifications will be applied.
            OnlyScaleUp - Modifications will only be applied when there is more than 6 players.
            AlwaysScale - Quota will always be scaled based on the number of players in the lobby, resulting in easier quotas when there is less than 6 players.
            """);
            
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        YapNetworkManager.OnClientConnectedOnServerAction += (conn) => PlayerCountChange(1);
        YapNetworkManager.OnClientDisconnectedFromServerAction += (conn) => PlayerCountChange(-1);
        Log.LogMessage($"Max players set to {MaxPlayers.Value}");
    }

    private static void PlayerCountChange(int modifier)
    {
        Log.LogDebug("PlayerCountChange");
        ModifyQuota(GameManager.Instance, modifier);
    }

    internal static void ModifyQuota(GameManager gameManager, int modifier = 0)
    {
        Log.LogDebug("ModifyQuota");

        if (gameManager == null)
            return;

        if (!gameManager.isServer)
        {
            Log.LogDebug("This is not the server");
            return;
        }

        int playerCount = Mathf.Clamp(gameManager.playersByActorId.Count + modifier, 1, MaxPlayers.Value);

        if (QuotaModifier.Value == QuotaStyle.None)
        {
            Log.LogDebug("QuotaModifier set to NONE, no modifications applied");
            return;
        }

        if (QuotaModifier.Value == QuotaStyle.OnlyScaleUp && playerCount < 6)
        {
            Log.LogDebug($"QuotaModifier set to OnlyScaleUp, no modifications applied due to player count {playerCount}");
            return;
        }

        float fairPercentage = (float)playerCount / 6f;
        Log.LogDebug($"fairPercentage: {fairPercentage}");
        int oldQuota = gameManager.GetQuotaForSession(gameManager.quotaSessionsCompleted);
        Log.LogDebug($"Old Quota: {oldQuota}");
        int newQuota = Mathf.RoundToInt((float)oldQuota * fairPercentage);
        Log.LogDebug($"New Quota: {newQuota}");
        gameManager.NetworkcurrentQuotaScoreGoal = newQuota;
        Log.LogMessage($"Quota modified from {oldQuota} to {newQuota}");
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

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.SvResetGameState))]
    public class QuotaHook
    {
        public static void Postfix(GameManager __instance)
        {
            Log.LogDebug("SvResetGameState");
            ModifyQuota(__instance);
        }
    }
}
