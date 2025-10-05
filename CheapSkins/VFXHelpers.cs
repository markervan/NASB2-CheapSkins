using CheapSkinss;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class VFXHelpers
{
    public static void SetColor(string propName, Color value, string selectedMaterial, Dictionary<string, List<Material>> materialGroups)
    {
        var materials = FindMatchingMaterials(selectedMaterial, materialGroups);
        if (materials == null)
        {
            Plugin.Log.LogWarning($"[VFXHelpers] No matching material found for identifier '{selectedMaterial}'");
            return;
        }

        foreach (var mat in materials)
        {
            if (mat == null) continue;
            mat.SetColor(propName, value);
        }
    }

    public static void SetFloat(string propName, float value, string selectedMaterial, Dictionary<string, List<Material>> materialGroups)
    {
        var materials = FindMatchingMaterials(selectedMaterial, materialGroups);
        if (materials == null)
        {
            Plugin.Log.LogWarning($"[VFXHelpers] No matching material found for identifier '{selectedMaterial}'");
            return;
        }

        foreach (var mat in materials)
        {
            if (mat == null) continue;
            mat.SetFloat(propName, value);
        }
    }

    public static void SetVector(string propName, Vector4 value, string selectedMaterial, Dictionary<string, List<Material>> materialGroups)
    {
        var materials = FindMatchingMaterials(selectedMaterial, materialGroups);
        if (materials == null)
        {
            Plugin.Log.LogWarning($"[VFXHelpers] No matching material found for identifier '{selectedMaterial}'");
            return;
        }

        foreach (var mat in materials)
        {
            if (mat == null) continue;
            mat.SetVector(propName, value);
        }
    }

    public static void SetTexture(string propName, Texture tex, string selectedMaterial, Dictionary<string, List<Material>> materialGroups)
    {
        var materials = FindMatchingMaterials(selectedMaterial, materialGroups);
        if (materials == null)
        {
            Plugin.Log.LogWarning($"[VFXHelpers] No matching material found for identifier '{selectedMaterial}'");
            return;
        }

        foreach (var mat in materials)
        {
            if (mat == null) continue;
            mat.SetTexture(propName, tex);
        }
    }

    /// <summary>
    /// Finds all materials that match the given identifier,
    /// handling Unity's "(Instance)" suffix or partial matches.
    /// </summary>
    private static List<Material> FindMatchingMaterials(string selectedMaterial, Dictionary<string, List<Material>> materialGroups)
    {
        // ✅ Exact match first
        if (materialGroups.TryGetValue(selectedMaterial, out var exactList))
            return exactList;

        // ✅ Fallback: try ignoring "(Instance)" and do partial match
        string normalized = selectedMaterial.Trim();
        if (normalized.EndsWith(" (Instance)"))
            normalized = normalized.Replace(" (Instance)", "");

        // Check if any material name starts with or contains the same base name
        var match = materialGroups
            .FirstOrDefault(kv =>
                kv.Key.Equals(normalized) ||
                kv.Key.StartsWith(normalized) ||
                kv.Key.Contains(normalized));

        if (!string.IsNullOrEmpty(match.Key))
        {
            Plugin.Log.LogWarning($"[VFXHelpers] Matched '{selectedMaterial}' -> '{match.Key}'");
            return match.Value;
        }

        // ✅ No match found
        Plugin.Log.LogWarning($"[VFXHelpers] Could not find any material matching '{selectedMaterial}' (available: {string.Join(", ", materialGroups.Keys)})");
        return null;
    }
}