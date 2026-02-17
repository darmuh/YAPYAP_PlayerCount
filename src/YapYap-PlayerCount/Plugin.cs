using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Mirror;
using TMPro;
using UnityEngine;
using YAPYAP;

namespace PlayerCount;

[BepInAutoPlugin]
[BepInDependency(QuotaQueenGUID, BepInDependency.DependencyFlags.SoftDependency)]
public partial class Plugin : BaseUnityPlugin
{
    internal const string MaxPlayersKey = "MOD-PLAYERCOUNT-MAXPLAYERS";
    private const string QuotaQueenGUID = "dev.mamallama.quotaqueen";
    private const string YapalizerGUID = "com.github.darmuh.yaplocalizer";
    internal static ManualLogSource Log { get; private set; } = null!;
    internal static ConfigEntry<int> MaxPlayers { get; private set; } = null!;
    internal static ConfigEntry<KeyCode> UIHintToggle { get; private set; } = null!;
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

    internal static bool YapalizerExists
    {
        get
        {
            return Chainloader.PluginInfos.ContainsKey(YapalizerGUID);
        }
    }

    private static GameObject? UIPlayerCount;

    private void Awake()
    {
        Log = Logger;

        Log.LogMessage($"Plugin {Name} is loaded!");
        MaxPlayers = Config.Bind("Settings", "Max Players", 10, new ConfigDescription("Set your desired maximum number of players here!", new AcceptableValueRange<int>(2, 20)));
        UIHintToggle = Config.Bind("Settings", "Toggle UI Hint", KeyCode.Tab, "Set key used to toggle UI hint displaying the lobby's maximum players");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        Log.LogMessage($"Max players set to {MaxPlayers.Value}");
    }

    private void Start()
    {
        QuotaQueenStuff();
        Yapalizer();
    }

    private void QuotaQueenStuff()
    {
        if (!QuotaQueenExists)
            return;

        Log.LogMessage("Adding QuotaQueen Strategies!");
        Quota.QuotaQueenUsage.AddQuotaStrategies();
    }

    private void Yapalizer()
    {
        if (!YapalizerExists)
            return;

        Log.LogMessage("Yapalizing menu text!");
        Local.LocalizerUsage.Setup();
    }

    private void Update()
    {
        if (UIPlayerCount == null)
            return;

        if (!NetworkServer.active && NetworkClient.active)
        {
            if (UIPlayerCount.activeSelf)
                UIPlayerCount.SetActive(false);

            return;
        }
            
        //toggle UI hint
        if (UnityInput.Current.GetKeyDown(UIHintToggle.Value))
            UIPlayerCount.SetActive(!UIPlayerCount.activeSelf);

        if(!UIPlayerCount.activeSelf)
            return;

        //check/update text value if object is visible
        if(!UIPlayerCount.GetComponent<TMP_Text>().text.Contains($"{NetworkServer.maxConnections}"))
            UIPlayerCount.GetComponent<TMP_Text>().text = $"Lobby Max Players: {NetworkServer.maxConnections}";
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

    [HarmonyPatch(typeof(UIGame), nameof(UIGame.Awake))]
    public class UIGameHook
    {
        public static void Prefix(UIGame __instance)
        {
            UIPlayerCount = GameObject.Instantiate(__instance.gameInfoText.gameObject, __instance.gameInfoText.gameObject.transform.parent);
            UIPlayerCount.transform.localPosition = new(-1025f, 35f, 0f);
            UIPlayerCount.GetComponent<TMP_Text>().text = $"Lobby Max Players: {NetworkServer.maxConnections}";
        }
    }

    [HarmonyPatch(typeof(UISettings), nameof(UISettings.Awake))]
    public class InjectSettingHook
    {
        public static void Prefix(UISettings __instance)
        {
            var fovSetting = __instance.fovSetting.gameObject;
            var parent = fovSetting.transform.parent;

            GameObject PlayerCountSetting = GameObject.Instantiate(fovSetting, parent);
            PlayerCountSetting.name = "PlayerCount MaxPlayers";
            var title = PlayerCountSetting.transform.Find("Title");
            if(YapalizerExists)
                title.GetComponent<LocalisedTMP>()._key = MaxPlayersKey;
            else
                title.GetComponent<LocalisedTMP>()._key = "Max Players";
            var uiSettings = PlayerCountSetting.GetComponent<UICustomSlider>();
            uiSettings.OnSettingChanged.RemoveAllListeners();
            uiSettings.settingKey = MaxPlayersKey;
            uiSettings.currentValue = MaxPlayers.Value;
            uiSettings.defaultValue = (int)MaxPlayers.DefaultValue;
            uiSettings.minValue = 2;
            uiSettings.maxValue = 20;
            uiSettings.OnSettingChanged.AddListener((value) => MaxPlayers.Value = value);
            uiSettings.ApplyValue(MaxPlayers.Value);
        }
    }
}
