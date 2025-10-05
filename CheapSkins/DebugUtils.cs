using System;
using System.Diagnostics;
using BepInEx.Logging;
using CheapSkinss;

public static class DebugUtils
{
    // Pass your plugin logger to this once (or reference Plugin.Log directly if static)
    public static ManualLogSource Log = null;

    /// <summary>
    /// Logs the direct caller of the method where this is invoked.
    /// </summary>
    public static void LogDirectCaller()
    {
        try
        {
            var trace = new StackTrace();
            // Frame 0 = this method, 1 = caller of this method, 2 = the method that called YOUR patched method
            var frame = trace.GetFrame(2);
            var method = frame?.GetMethod();

            if (method != null)
            {
                Plugin.Log.LogWarning($"[DebugUtils] Direct caller: {method.DeclaringType}.{method.Name}");
            }
            else
            {
                Plugin.Log.LogWarning("[DebugUtils] Direct caller: <unknown>");
            }
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"[DebugUtils] Failed to get direct caller: {ex}");
        }
    }

    /// <summary>
    /// Logs the call stack up to a given depth (default = 5).
    /// </summary>
    public static void LogCallStack(int maxFrames = 5)
    {
        try
        {
            var trace = new StackTrace();
            Log?.LogWarning("[DebugUtils] Call stack:");

            for (int i = 2; i < Math.Min(trace.FrameCount, maxFrames + 2); i++) // skip DebugUtils + Harmony
            {
                var frame = trace.GetFrame(i);
                var method = frame?.GetMethod();
                Log?.LogWarning($"  #{i - 1}: {method?.DeclaringType}.{method?.Name}");
            }
        }
        catch (Exception ex)
        {
            Log?.LogError($"[DebugUtils] Failed to get stack trace: {ex}");
        }
    }
}