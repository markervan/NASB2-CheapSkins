using Quantum;
using System;
using System.Collections.Generic;
using System.Text;
using static CharacterUIData;
using System.Text.RegularExpressions;
using UnityEngine;

public class Utils
{
    /*void TryMatchRenderer(string sourceType, string objectName, Renderer renderer, )
    {
        if (renderer == null) return;
        if (characterCodenames != null &&
        characterCodenames.TryGetValue(skinData.characterCodename.ToString(), out var altParts) &&
            altParts?.TryGetValue(skinData.skinIndex, out var parts) == true &&
            parts?.TryGetValue(normalizedIdentifier, out var partList) == true)
        {
            if (partList?.Any(part => objectName == part) == true)
            {
                Debug.Log($"[MaterialOverride] ✅ Match found for Identifier '{normalizedIdentifier}' on {sourceType} object '{objectName}'.");

                if (renderer.materials != null)
                {
                    for (int matIndex = 0; matIndex < renderer.materials.Length; matIndex++)
                    {
                        var mat = renderer.materials[matIndex];
                        if (mat == null) continue;

                        if (mat.name.Contains(normalizedIdentifier))
                        {
                            Debug.Log($"[MaterialOverride]   ↳ Matched Material: '{mat.name}' (Renderer: {renderer.name})");

                            // If identifier starts with a digit, use it as material index
                            if (!string.IsNullOrEmpty(MOGroup.Identifier) && char.IsDigit(MOGroup.Identifier[0]))
                            {
                                int firstDigit = int.Parse(MOGroup.Identifier[0].ToString());
                                Debug.Log($"[MaterialOverride]   ↳ Using Identifier digit '{firstDigit}' as MaterialIndex.");

                                MOGroup.Targets.Add(new CharacterMaterialOverridesHandler.TextureOverrideTarget
                                {
                                    Target = renderer,
                                    MaterialIndex = firstDigit
                                });
                            }
                            else
                            {
                                Debug.LogWarning($"[MaterialOverride]   ↳ Identifier '{MOGroup.Identifier}' does not start with a digit, cannot resolve MaterialIndex.");
                            }
                        }
                    }
                }
            }
        }
    }*/

    public static void AddLabel(NavigationButtons navButton, string text, UIKey key)
    {
        navButton.ButtonLabels[navButton.enabledLabels].Set(text, key);
        if (navButton.ButtonLabels[navButton.enabledLabels].gameObject != null)
        {
            navButton.ButtonLabels[navButton.enabledLabels].gameObject.SetActive(true);
        }
        navButton.enabledLabels++;
        navButton.Refresh();
    }

    public static void AddLabel(NavigationButtons navButton, Sprite icon, UIKey buttonType)
    {
        navButton.ButtonLabels[navButton.enabledLabels].Set(icon, buttonType);
        if (navButton.ButtonLabels[navButton.enabledLabels].gameObject != null)
        {
            navButton.ButtonLabels[navButton.enabledLabels].gameObject.SetActive(true);
        }
        navButton.enabledLabels++;
        navButton.Refresh();
    }
}
