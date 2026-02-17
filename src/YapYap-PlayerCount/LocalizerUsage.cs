using System.Runtime.CompilerServices;
using UnityEngine;
using YapLocalizer;

namespace PlayerCount.Local;

internal class LocalizerUsage
{
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    internal static void Setup()
    {
        ModLocalizedText SettingText = new(Plugin.MaxPlayersKey, "Max Players");
        SettingText.SetLocalization(SystemLanguage.English, "Max Players");
        SettingText.SetLocalization(SystemLanguage.ChineseSimplified, "最大玩家数");
        SettingText.SetLocalization(SystemLanguage.ChineseTraditional, "最大玩家數");
        SettingText.SetLocalization(SystemLanguage.Korean, "최대 플레이어 수");
        SettingText.SetLocalization(SystemLanguage.Japanese, "最大プレイヤー数");
        SettingText.SetLocalization(SystemLanguage.Russian, "Максимальное количество игроков");
        SettingText.SetLocalization(SystemLanguage.Ukrainian, "Максимальна кількість гравців");
        SettingText.SetLocalization(SystemLanguage.French, "Nombre maximum de joueurs");
        SettingText.SetLocalization(SystemLanguage.German, "Maximale Spieleranzahl");
        SettingText.SetLocalization(SystemLanguage.Polish, "Maksymalna liczba graczy");
        SettingText.SetLocalization(SystemLanguage.Polish, "Maksymalna liczba graczy");
        SettingText.SetLocalization(SystemLanguage.Portuguese, "Número máximo de jogadores");
        SettingText.SetLocalization(SystemLanguage.Turkish, "Maksimum Oyuncu Sayısı");
        SettingText.SetLocalization(SystemLanguage.Italian, "Numero massimo di giocatori");
        SettingText.SetLocalization(SystemLanguage.Spanish, "Máximo de jugadores");
    }
}
