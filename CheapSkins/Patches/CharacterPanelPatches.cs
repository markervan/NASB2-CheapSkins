using CheapSkinss;
using HarmonyLib;
using Photon.Deterministic;
using Quantum;
using System;
using System.Collections.Generic;
using System.Text;
using static CheapSkinss.Plugin;



[HarmonyPatch]
public class CharacterPanelPatches
{

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterPanel), "UpdateData")]
    public static bool UpdateData(CharacterPanel __instance, CharacterUIData characterUIData, int skin)
    {
        if (!__instance.randomMode && __instance.currentCharacter == characterUIData.CharacterCodename && __instance.currentSkin == skin)
        {
            /*Plugin.Log.LogWarning("[ApplyMaterialOverride] Returning early because all conditions matched:");
            Plugin.Log.LogWarning($"   • randomMode = {__instance.randomMode} (expected false)");
            Plugin.Log.LogWarning($"   • currentCharacter = {__instance.currentCharacter}, CharacterCodename = {characterUIData.CharacterCodename}");
            Plugin.Log.LogWarning($"   • currentSkin = {__instance.currentSkin}, skin = {skin}");*/
            return false;
        }
        else
        {
            //Plugin.Log.LogWarning($"PROCEEDINGGGGGGGGGGGGGGGGGGGGGG");
        }

        CharacterUIData newCUID = characterUIData;

        List<int> customSkinlist = new List<int>();

        for (int i = 0; i < newCUID.Skins.Count; i++)
        {
            string debugName = newCUID.Skins[i].DebugName;

            if (debugName.Contains(":"))
            {
                // Get the part before the colon and parse it as an integer
                string numberPart = debugName.Split(':')[0];
                if (int.TryParse(numberPart, out int number))
                {
                    customSkinlist.Add(number);
                }
                else
                {
                    // If parsing fails, fall back to the index
                    customSkinlist.Add(i);
                }
            }
            else
            {
                // No colon found, just add the index
                customSkinlist.Add(i);
            }
        }


        if (__instance.currentCharacter == newCUID.CharacterCodename)
        {
            __instance.showIntroAnimation = false;
        }
        else
        {
            __instance.showIntroAnimation = true;
        }
        __instance.unlocked = true;
        if (!string.IsNullOrEmpty(characterUIData.DLCIdentifier))
        {
            __instance.unlocked = __instance.dlcManager.IsDLCUnlocked(characterUIData.DLCIdentifier);
        }
        string localizedString = characterUIData.CharacterName.GetLocalizedString();
        __instance.MainTitleText.text = localizedString;
        __instance.UnloadMesh(__instance.currentCharacter);
        __instance.currentCharacter = characterUIData.CharacterCodename;
        __instance.currentSkin = skin;
        __instance.ToggleRandom(false);
        __instance.ToggleAddPlayerMode(false);
        __instance.SetUI();
        __instance.currentSkins = customSkinlist;
        //Plugin.Log.LogWarning("about to call loadcharacter");
        if (!__instance.loadingMesh)
        {
            //Plugin.Log.LogWarning("calling loadcharacter");
            __instance.LoadCharacterGameObject(characterUIData.CharacterCodename, __instance.currentSkins[__instance.currentSkin]);
        }
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterSelect), "GridOverflow")]
    public static bool GridOverflow(CharacterSelect __instance, int playerIndex)
    {
        //Plugin.Log.LogWarning("MODDED GridOverflow");

        if (__instance.SquadStrikeSelection)
        {
            return false;
        }
        __instance.GridNavigator.DisableAllHighlights(false);
        int playerCurrentSelector = __instance.GetPlayerCurrentSelector(playerIndex);
        if (playerCurrentSelector >= __instance.Selectors.Count)
        {
            return false;
        }
        __instance.Selectors[playerCurrentSelector].OnCharacterSelector = false;
        __instance.inputManager.CleanInputDevice(__instance.Selectors[playerCurrentSelector].InputDeviceIndex);
        __instance.uiManager.PlaySound(__instance.NavigateBetweenSectionsSFX);
        __instance.RefreshUI();

        return false;

    }

}
