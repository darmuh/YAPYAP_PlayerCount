using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using QuotaQueen.QuotaStrategies;

namespace PlayerCount.Quota;

public static class QuotaQueenUsage
{

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static void AddQuotaStrategies()
    {
#pragma warning disable IDE0090 // Use 'new(...)' suppressed to fix soft compatibility
        Func<GameSnapshot, int> scaleUp = new Func<GameSnapshot, int>(ScaleQuotaUp);
        Func<GameSnapshot, int> scaleAlways = new Func<GameSnapshot, int>(ScaleQuota);
#pragma warning restore IDE0090 // Use 'new(...)'
        QuotaStrategyManager.RegisterStrategy("darmuh.PlayerCount", "ScaleUp", scaleUp);
        QuotaStrategyManager.RegisterStrategy("darmuh.PlayerCount", "ScaleAlways", scaleAlways);
    }

    [MethodImpl(MethodImplOptions.NoOptimization)]
    private static int ScaleQuotaUp(GameSnapshot snapshot)
    {
        if (snapshot.PlayersInSession < 6)
        {
            Plugin.Log.LogDebug($"QuotaModifier set to OnlyScaleUp, no modifications applied due to player count {snapshot.PlayersInSession}");
            return snapshot.CurrentQuotaGoal;
        }

        return ScaleQuota(snapshot);
    }

    [MethodImpl(MethodImplOptions.NoOptimization)]
    private static int ScaleQuota(GameSnapshot snapshot)
    {
        float fairPercentage = snapshot.PlayersInSession / 6f;
        Plugin.Log.LogDebug($"fairPercentage: {fairPercentage}");
        int oldQuota = snapshot.CurrentQuotaGoal;
        Plugin.Log.LogDebug($"Old Quota: {oldQuota}");
        int newQuota = Mathf.RoundToInt(oldQuota * fairPercentage);
        Plugin.Log.LogDebug($"New Quota: {newQuota}");
        Plugin.Log.LogMessage($"Quota modified from {oldQuota} to {newQuota}");
        return newQuota;
    }
    
}
