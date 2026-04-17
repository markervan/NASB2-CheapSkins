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
            //Plugin.Log.LogWarning($"[VFXHelpers] No matching material found for identifier '{selectedMaterial}'");
            return;
        }

        string texName = tex != null ? tex.name : "null";

        //Plugin.Log.LogWarning($"[VFXHelpers] Applying texture '{texName}' to property '{propName}' for '{selectedMaterial}' ({materials.Count} materials)");

        int appliedCount = 0;

        foreach (var mat in materials)
        {
            if (mat == null)
            {
                //Plugin.Log.LogWarning($"[VFXHelpers] Skipped null material in '{selectedMaterial}'");
                continue;
            }

            if (!mat.HasProperty(propName))
            {
                //Plugin.Log.LogWarning($"[VFXHelpers] Material '{mat.name}' does not have property '{propName}' — skipped.");
                continue;
            }

            mat.SetTexture(propName, tex);
            appliedCount++;

            //Plugin.Log.LogWarning($"[VFXHelpers] → SetTexture on '{mat.name}' (shader: {mat.shader?.name ?? "null"})");
        }

        //Plugin.Log.LogWarning($"[VFXHelpers] Done applying texture '{texName}' to '{selectedMaterial}' — affected {appliedCount}/{materials.Count} materials");
    }

    /// <summary>
    /// Finds all materials that match the given identifier,
    /// handling Unity's "(Instance)" suffix or partial matches.
    /// </summary>
    private static List<Material> FindMatchingMaterials(string selectedMaterial, Dictionary<string, List<Material>> materialGroups)
    {
        // Try exact match first
        if (materialGroups.TryGetValue(selectedMaterial, out var exactList))
        {
            //Plugin.Log.LogWarning($"[VFXHelpers] Matched (exact) '{selectedMaterial}'");
            return exactList;
        }

        // Try fallback without "(Instance)"
        string normalized = selectedMaterial.Replace(" (Instance)", "").Trim();
        if (materialGroups.TryGetValue(normalized, out var normalizedList))
        {
            //Plugin.Log.LogWarning($"[VFXHelpers] Matched (normalized) '{selectedMaterial}' -> '{normalized}'");
            return normalizedList;
        }

        // No match found
        //Plugin.Log.LogWarning($"[VFXHelpers] ❌ No material found for '{selectedMaterial}' (Available: {string.Join(", ", materialGroups.Keys)})");
        return null;
    }
}