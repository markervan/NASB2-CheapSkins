using CheapSkinss;
using HarmonyLib;
using Photon.Realtime;
using Quantum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static CharacterUIData;
using static CheapSkinss.Plugin;

[HarmonyPatch]
public class MatchLoadingCharacter_Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MatchLoadingCharacter), "Init")]
    public static bool Init(MatchLoadingCharacter __instance, int versusScreenIndex, UIManager.PlayerColor playerColor, CharacterCodename character, int characterSkin, Player player, string playerName = "")
    {
        __instance.VersusScreenIndex = versusScreenIndex;


        if (__instance.dataManager.Online)
        {
            CustomSetCharacterOnline(__instance, playerColor, character, characterSkin, player, playerName);
        }
        else
        {
            //offline_logic
            CustomSetCharacter(__instance, playerColor, character, characterSkin, playerName);
        }

        if (!__instance.dataManager.Online || player == null)
        {
            __instance.PlayerOnlineBadge.gameObject.SetActive(false);
            return false;
        }
        __instance.PlayerOnlineBadge.gameObject.SetActive(true);
        __instance.CharacterName.transform.parent.gameObject.SetActive(false);
        __instance.PlayerName.transform.parent.gameObject.SetActive(false);
        string playerBadgeData = __instance.onlineManager.Properties.GetPlayerBadgeData(player);
        OnlineBadge onlineBadge = __instance.badgeData.StringToOnlineBadge(playerBadgeData);
        Sprite playerPlatformIcon = __instance.onlineManager.GetPlayerPlatformIcon(player);
        __instance.PlayerOnlineBadge.SetData(playerName, onlineBadge, playerPlatformIcon);
        if (__instance.onlineManager.Properties.GetLobbyType() == OnlineLobbyType.Ranked)
        {
            __instance.PlayerOnlineBadge.InitForRankedLoading(player);
        }

        return false;
    }

    public static void CustomSetCharacter(MatchLoadingCharacter __instance, UIManager.PlayerColor playerColor, CharacterCodename character, int characterSkin, string playerName = "")
    {
        __instance.characterCodename = character;
        __instance.skin = characterSkin;
        CharacterUIData characterUIData = GameManager.Instance.GameResourcesManager.GetCharacterUIData(character);
        if (characterUIData == null)
        {
            return;
        }

        CustomLoadSlotImage(__instance, false);

        __instance.CharacterBackground.color = playerColor.MainColor;
        if (__instance.BackgroundDetail1 != null)
        {
            __instance.BackgroundDetail1.startColor = playerColor.ExtraColor1;
        }
        if (__instance.BackgroundDetail2 != null)
        {
            __instance.BackgroundDetail2.startColor = playerColor.ExtraColor2;
        }



        if (characterUIData.CharacterName != null && !characterUIData.CharacterName.IsEmpty)
        {
            __instance.CharacterName.transform.parent.gameObject.SetActive(true);
            __instance.CharacterName.SetText(characterUIData.CharacterName.GetLocalizedString(), true);
        }
        else
        {
            __instance.CharacterName.SetText(characterUIData.CharacterCodename.ToString(), true);
            __instance.CharacterName.transform.parent.gameObject.SetActive(true);
        }
        if (!playerName.Equals(""))
        {
            __instance.PlayerName.transform.parent.gameObject.SetActive(true);
            __instance.PlayerName.SetText(playerName, true);
        }
        else
        {
            __instance.PlayerName.transform.parent.gameObject.SetActive(false);
        }
        __instance.CharacterBackground.SetAllDirty();
    }

    public static void CustomLoadSlotImage(MatchLoadingCharacter __instance, bool forceDefault = false)
    {
        int characterSkin = __instance.skin;
        if (forceDefault)
        {
            characterSkin = 0;
        }
        __instance.CharacterImage.gameObject.SetActive(false);
        __instance.uiManager.LoadResourcesVersusSprite(__instance.characterCodename, characterSkin, delegate (Sprite sprite)
        {
            if (sprite == null)
            {
                if (characterSkin != 0)
                {
                    __instance.LoadSlotImage(true);
                }
                return;
            }

            Texture2D customSprite = null;

            if (Plugin.metaDataDict.TryGetValue(__instance.CharacterIndex, out var metaData))
            {
                if (Plugin.dictCustomSkinDatas.TryGetValue(metaData.customSkinName, out var customSkinData))
                {
                    CustomSkinData skinData = customSkinData;

                    customSprite = skinData.VSRender;
                }
            }

            if (customSprite == null)
            {
                __instance.CharacterImage.sprite = sprite;
            }
            else
            {
                __instance.CharacterImage.sprite = Utility.ConvertTextureToSprite(customSprite);

            }

            CharacterUIData characterUIData = __instance.gameResourcesManager.GetCharacterUIData(__instance.characterCodename);
            RenderOffsetData renderOffsetData = default(RenderOffsetData);
            bool flag = true;
            switch (__instance.VersusScreenIndex)
            {
                case 0:
                    flag = false;
                    __instance.CharacterImage.SetNativeSize();
                    break;
                case 1:
                    if (__instance.CharacterIndex == 0)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus2Player1;
                    }
                    if (__instance.CharacterIndex == 1)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus2Player2;
                    }
                    break;
                case 2:
                    if (__instance.CharacterIndex == 0)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus3Player1;
                    }
                    if (__instance.CharacterIndex == 1)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus3Player2;
                    }
                    if (__instance.CharacterIndex == 2)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus3Player3;
                    }
                    break;
                case 3:
                    if (__instance.CharacterIndex == 0)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus4Player1;
                    }
                    if (__instance.CharacterIndex == 1)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus4Player2;
                    }
                    if (__instance.CharacterIndex == 2)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus4Player3;
                    }
                    if (__instance.CharacterIndex == 3)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus4Player4;
                    }
                    break;
            }
            if (flag)
            {
                __instance.CharacterImage.transform.localPosition = renderOffsetData.Position;
                __instance.CharacterImage.GetComponent<RectTransform>().sizeDelta = renderOffsetData.Size;
            }
            __instance.CharacterImage.gameObject.SetActive(true);
        });
    }

    public static void CustomSetCharacterOnline(MatchLoadingCharacter __instance, UIManager.PlayerColor playerColor, CharacterCodename character, int characterSkin, Player onlinePlayer, string playerName = "")
    {
        __instance.characterCodename = character;
        __instance.skin = characterSkin;
        CharacterUIData characterUIData = GameManager.Instance.GameResourcesManager.GetCharacterUIData(character);
        if (characterUIData == null)
        {
            return;
        }

        //originalMethod
        //__instance.LoadSlotImage(false);

        CustomLoadSlotImageOnline(__instance, onlinePlayer, false);

        __instance.CharacterBackground.color = playerColor.MainColor;
        if (__instance.BackgroundDetail1 != null)
        {
            __instance.BackgroundDetail1.startColor = playerColor.ExtraColor1;
        }
        if (__instance.BackgroundDetail2 != null)
        {
            __instance.BackgroundDetail2.startColor = playerColor.ExtraColor2;
        }
        if (characterUIData.CharacterName != null && !characterUIData.CharacterName.IsEmpty)
        {
            __instance.CharacterName.transform.parent.gameObject.SetActive(true);
            __instance.CharacterName.SetText(characterUIData.CharacterName.GetLocalizedString(), true);
        }
        else
        {
            __instance.CharacterName.SetText(characterUIData.CharacterCodename.ToString(), true);
            __instance.CharacterName.transform.parent.gameObject.SetActive(true);
        }
        if (!playerName.Equals(""))
        {
            __instance.PlayerName.transform.parent.gameObject.SetActive(true);
            __instance.PlayerName.SetText(playerName, true);
        }
        else
        {
            __instance.PlayerName.transform.parent.gameObject.SetActive(false);
        }
        __instance.CharacterBackground.SetAllDirty();
    }

    public static void CustomLoadSlotImageOnline(MatchLoadingCharacter __instance, Player onlinePlayer, bool forceDefault = false)
    {
        int characterSkin = __instance.skin;
        if (forceDefault)
        {
            characterSkin = 0;
        }
        __instance.CharacterImage.gameObject.SetActive(false);
        __instance.uiManager.LoadResourcesVersusSprite(__instance.characterCodename, characterSkin, delegate (Sprite sprite)
        {
            if (sprite == null)
            {
                if (characterSkin != 0)
                {
                    __instance.LoadSlotImage(true);
                }
                return;
            }

            Texture2D customSprite = null;

            foreach (var player in onlineManager.GetPlayersList())
            {
                Plugin.Log.LogWarning(player.NickName);

                int baseSkin = Plugin.onlineManager.Properties.GetPlayerCharacterSkin(player);
                string customSkin = Plugin.Patches.GetPlayerCharacterCustomSkin(player);

                if (player.NickName == __instance.PlayerName.text)
                {
                    if (Plugin.dictCustomSkinDatas.TryGetValue(customSkin, out var customSkinData))
                    {
                        CustomSkinData skinData = customSkinData;

                        customSprite = skinData.VSRender;

                    }
                }
            }

            if(customSprite == null)
            {
                __instance.CharacterImage.sprite = sprite;
            }
            else
            {
                __instance.CharacterImage.sprite = Utility.ConvertTextureToSprite(customSprite);

            }
            



            CharacterUIData characterUIData = __instance.gameResourcesManager.GetCharacterUIData(__instance.characterCodename);
            RenderOffsetData renderOffsetData = default(RenderOffsetData);
            bool flag = true;
            switch (__instance.VersusScreenIndex)
            {
                case 0:
                    flag = false;
                    __instance.CharacterImage.SetNativeSize();
                    break;
                case 1:
                    if (__instance.CharacterIndex == 0)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus2Player1;
                    }
                    if (__instance.CharacterIndex == 1)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus2Player2;
                    }
                    break;
                case 2:
                    if (__instance.CharacterIndex == 0)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus3Player1;
                    }
                    if (__instance.CharacterIndex == 1)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus3Player2;
                    }
                    if (__instance.CharacterIndex == 2)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus3Player3;
                    }
                    break;
                case 3:
                    if (__instance.CharacterIndex == 0)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus4Player1;
                    }
                    if (__instance.CharacterIndex == 1)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus4Player2;
                    }
                    if (__instance.CharacterIndex == 2)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus4Player3;
                    }
                    if (__instance.CharacterIndex == 3)
                    {
                        renderOffsetData = characterUIData.RenderOffsetData.Versus4Player4;
                    }
                    break;
            }
            if (flag)
            {
                __instance.CharacterImage.transform.localPosition = renderOffsetData.Position;
                __instance.CharacterImage.GetComponent<RectTransform>().sizeDelta = renderOffsetData.Size;
            }
            __instance.CharacterImage.gameObject.SetActive(true);
        });
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CharacterManager), "OnInstantiated")]
    public static void OnInstantiated(CharacterManager __instance, QuantumGame game)
    {
        //Debug.Log("INSTANTIATING CHARACTER...");

        if (!dataManager.Online)
        {
            if (Plugin.metaDataDict.TryGetValue(__instance.Data.Character.index, out var metaData))
            {
                if (Plugin.dictCustomSkinDatas.TryGetValue(metaData.customSkinName, out var customSkinData))
                {
                    CustomSkinData skinData = customSkinData;
                    CharacterMaterialOverridesHandler characterMaterialOverridesHandler = Plugin.Patches.GetCharacterMaterialOverridesHandler(__instance);
                    if (characterMaterialOverridesHandler == null)
                    {
                        Debug.LogError("CharacterMaterialOverridesHandler is null!");
                        return;
                    }

                    //__instance.CustomQuantumAnimator.DataAsset = null;
                    /*CustomAnimatorGraphAsset customAnimatorGraphAsset = __instance.CustomQuantumAnimator.DataAsset;
                    Debug.Log($"Starting animator behavior adjustments for {skinData.customAnimatorBehaviours.Count} entries.");

                    foreach (var entry in skinData.customAnimatorBehaviours)
                    {
                        Debug.Log($"Processing behavior entry - Key: {entry.Key}, Value: {entry.Value?.name ?? "NULL"}");

                        bool foundState = false;

                        foreach (var layer in customAnimatorGraphAsset.Settings.layers)
                        {
                            Debug.Log($"Checking layer: {layer.name} (States: {layer.states?.Length ?? 0})");

                            foreach (var state in layer.states)
                            {
                                if (state.name.Equals(entry.Key, StringComparison.OrdinalIgnoreCase))
                                {
                                    Debug.Log($"Found matching state: {state.name} (Target: {entry.Key})");

                                    CharacterAnimatorStateAsset stateAsset = UnityDB.FindAsset<CharacterAnimatorStateAsset>(state.StateAsset.Id.Value);
                                    if (stateAsset != null)
                                    {
                                        Debug.Log($"Loaded CharacterAnimatorStateAsset: {stateAsset.name} (GUID: {state.StateAsset.Id.Value})");

                                        if (entry.Value != null && entry.Value.Settings?.BakedData.BakedVisibilityData != null)
                                        {
                                            stateAsset.Settings.BakedData.BakedVisibilityData = entry.Value.Settings.BakedData.BakedVisibilityData;
                                            Debug.Log($"Updated BakedVisibilityData for state: {state.name}");
                                            foundState = true;
                                        }
                                        else
                                        {
                                            Debug.LogWarning($"Skipping null/invalid entry.Value data for state: {state.name}");
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError($"Failed to find CharacterAnimatorStateAsset with GUID: {state.StateAsset.Id.Value}");
                                    }
                                }
                            }
                        }

                        if (!foundState)
                        {
                            Debug.LogWarning($"No matching state found for behavior key: {entry.Key}");
                        }
                    }

                    Debug.Log("Finished processing all animator behavior adjustments.");
                    */


                    if (skinData.CustomMOGList == null) return;

                    foreach (var originalMOGroup in skinData.CustomMOGList)
                    {
                        if (originalMOGroup == null)
                        {
                            //Debug.LogWarning("[MaterialOverride] Skipping null MOGroup in skinData.CustomMOGList.");
                            continue;
                        }

                        var MOGroup = Plugin.Patches.CloneMOGroup(originalMOGroup);
                        if (MOGroup == null)
                        {
                            //Debug.LogWarning($"[MaterialOverride] Failed to clone MOGroup '{originalMOGroup.Identifier}'.");
                            continue;
                        }

                        string normalizedIdentifier = MOGroup.Identifier;
                        if (string.IsNullOrEmpty(normalizedIdentifier))
                        {
                            //Debug.LogWarning("[MaterialOverride] MOGroup has empty identifier, skipping.");
                            continue;
                        }

                        // Strip prefix before colon (e.g. "Part:Head" -> "Head")
                        int colonIndex = normalizedIdentifier.IndexOf(':');
                        if (colonIndex != -1)
                        {
                            normalizedIdentifier = normalizedIdentifier.Substring(colonIndex + 1);
                        }

                        //Debug.Log($"[MaterialOverride] Processing MOGroup Identifier: {MOGroup.Identifier} (normalized: {normalizedIdentifier})");

                        // 🔹 Local helper to handle matching for any renderer
                        void TryMatchRenderer(string sourceType, string objectName, Renderer renderer)
                        {
                            if (renderer == null) return;

                            if (characterCodenames != null &&
                                characterCodenames.TryGetValue(skinData.characterCodename.ToString(), out var altParts) &&
                                altParts?.TryGetValue(skinData.skinIndex, out var parts) == true &&
                                parts?.TryGetValue(normalizedIdentifier, out var partList) == true)
                            {
                                if (partList?.Any(part => objectName == part) == true)
                                {
                                    //Debug.Log($"[MaterialOverride] ✅ Match found for Identifier '{normalizedIdentifier}' on {sourceType} object '{objectName}'.");

                                    if (renderer.materials != null)
                                    {
                                        for (int matIndex = 0; matIndex < renderer.materials.Length; matIndex++)
                                        {
                                            var mat = renderer.materials[matIndex];
                                            if (mat == null) continue;

                                            if (mat.name.Contains(normalizedIdentifier))
                                            {
                                                //Debug.Log($"[MaterialOverride]   ↳ Matched Material: '{mat.name}' (Renderer: {renderer.name})");

                                                // If identifier starts with a digit, use it as material index
                                                if (!string.IsNullOrEmpty(MOGroup.Identifier) && char.IsDigit(MOGroup.Identifier[0]))
                                                {
                                                    int firstDigit = int.Parse(MOGroup.Identifier[0].ToString());
                                                    //Debug.Log($"[MaterialOverride]   ↳ Using Identifier digit '{firstDigit}' as MaterialIndex.");

                                                    MOGroup.Targets.Add(new CharacterMaterialOverridesHandler.TextureOverrideTarget
                                                    {
                                                        Target = renderer,
                                                        MaterialIndex = firstDigit
                                                    });
                                                }
                                                else
                                                {
                                                    //Debug.LogWarning($"[MaterialOverride]   ↳ Identifier '{MOGroup.Identifier}' does not start with a digit, cannot resolve MaterialIndex.");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // 🔹 Process CharacterRenderer.Objects
                        if (__instance.CharacterRenderer?.Objects != null)
                        {
                            foreach (var materialObject in __instance.CharacterRenderer.Objects)
                            {
                                TryMatchRenderer("CharacterRenderer", materialObject.ObjectRenderer?.name, materialObject.ObjectRenderer);
                            }
                        }

                        // 🔹 Process MultipleConstraintObjectsList
                        if (__instance.CharacterObjects?.MultipleConstraintObjectsList != null)
                        {
                            foreach (var materialObject in __instance.CharacterObjects.MultipleConstraintObjectsList)
                            {
                                if (materialObject.VisibleObjects == null || materialObject.VisibleObjects.Count == 0 || materialObject.VisibleObjects[0] == null)
                                    continue;

                                var meshRenderer = materialObject.VisibleObjects[0].GetComponent<MeshRenderer>();
                                TryMatchRenderer("ConstraintObject", materialObject.VisibleObjects[0].name, meshRenderer);
                            }
                        }

                        // 🔹 Process ModelProps
                        if (__instance.CharacterObjects?.ModelProps != null)
                        {
                            foreach (var materialObject in __instance.CharacterObjects.ModelProps)
                            {
                                var meshRenderer = materialObject.ObjectRef?.GetComponent<MeshRenderer>();
                                TryMatchRenderer("ModelProp", materialObject.ObjectRef?.name, meshRenderer);
                            }
                        }

                        // 🔹 Finally register and apply
                        characterMaterialOverridesHandler.TextureOverrides.Add(MOGroup);
                        characterMaterialOverridesHandler.ApplyMaterialOverride(MOGroup.Identifier);

                        //Debug.Log($"[MaterialOverride] Finished applying MOGroup '{MOGroup.Identifier}' (Targets: {MOGroup.Targets.Count}).");
                    }

                    if (skinData.customSFXData != null)
                    {
                        try
                        {
                            SFXSpawner[] componentsInChildren = __instance.gameObject.GetComponentsInChildren<SFXSpawner>(true);
                            if (componentsInChildren != null)
                            {
                                foreach (SFXSpawner sfxspawner in componentsInChildren)
                                {
                                    if (sfxspawner?.SFXAudioClipData == null) continue;

                                    //add the custom SFXData from the Custom Skin Data
                                    sfxspawner.SFXAudioClipData.Add(skinData.customSFXData);

                                    //Initializes the SFXData
                                    if (sfxspawner.SFXAudioClipData.Count > 2)
                                    {
                                        sfxspawner.SFXAudioClipData[2]?.Initialize();

                                        //sfxspawner.SFXAudioClipData[2].containersReference
                                        if (sfxspawner.SFXAudioClipData[2]?.Containers != null)
                                        {
                                            foreach (var entry in sfxspawner.SFXAudioClipData[2].Containers)
                                            {
                                                entry?.UpdateClipsList();
                                            }
                                        }

                                        if (sfxspawner.SFXAudioClipData.Count > 1)
                                        {
                                            sfxspawner.SFXAudioClipData.Remove(sfxspawner.SFXAudioClipData[1]);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //Plugin.Log.LogError($"Error processing SFXSpawners for character {__instance.Codename}: {ex}");
                        }
                    }


                    __instance.StartCoroutine(ApplyCustomVFXData(__instance, skinData));

                    if (characterMaterialOverridesHandler.TextureOverrides != null && skinData.materialBanksForMeshes != null)
                    {
                        foreach (var textureOverride in characterMaterialOverridesHandler.TextureOverrides)
                        {
                            if (textureOverride?.Targets == null) continue;

                            string groupIdentifier = textureOverride.Identifier;
                            if (string.IsNullOrEmpty(groupIdentifier))
                            {
                                //Debug.LogWarning("[MeshReplacement] Skipping TextureOverride with empty Identifier.");
                                continue;
                            }

                            // Normalize identifier (remove prefix before colon)
                            string normalizedIdentifier = groupIdentifier;
                            int colonIndex = normalizedIdentifier.IndexOf(':');
                            if (colonIndex != -1)
                            {
                                normalizedIdentifier = normalizedIdentifier.Substring(colonIndex + 1);
                            }

                            //Debug.Log($"[MeshReplacement] Processing TextureOverride Group: '{groupIdentifier}' (normalized: '{normalizedIdentifier}')");

                            // Try to get dictionary for this identifier
                            if (!skinData.materialBanksForMeshes.TryGetValue(normalizedIdentifier, out var currentMeshDictionary) ||
                                currentMeshDictionary == null)
                            {
                                //Debug.LogWarning($"[MeshReplacement] ❌ No material bank found for Identifier '{normalizedIdentifier}'. Skipping group.");
                                continue;
                            }

                            //int successCount = 0;
                            //int failCount = 0;

                            foreach (var targetOverride in textureOverride.Targets)
                            {
                                if (targetOverride.Target == null)
                                {
                                    //Debug.LogWarning($"[MeshReplacement] ❌ Null target in group '{normalizedIdentifier}'.");
                                    //failCount++;
                                    continue;
                                }

                                if (targetOverride.MaterialIndex != 0)
                                {
                                    //Debug.Log($"[MeshReplacement] Skipping target '{targetOverride.Target.name}' in group '{normalizedIdentifier}' (MaterialIndex {targetOverride.MaterialIndex} != 0).");
                                    continue;
                                }

                                string targetName = targetOverride.Target.name;

                                // Handle SkinnedMeshRenderer
                                if (targetOverride.Target is SkinnedMeshRenderer skinnedMeshRenderer)
                                {
                                    if (currentMeshDictionary.TryGetValue(skinnedMeshRenderer.name, out Mesh replacementMesh) && replacementMesh != null)
                                    {
                                        skinnedMeshRenderer.sharedMesh = replacementMesh;
                                        //Debug.Log($"[MeshReplacement] ✅ Replaced SkinnedMeshRenderer '{skinnedMeshRenderer.name}' with mesh '{replacementMesh.name}' (Group: {normalizedIdentifier}).");
                                        //successCount++;
                                    }
                                }
                                // Handle MeshRenderer
                                else if (targetOverride.Target is MeshRenderer meshRenderer)
                                {
                                    if (currentMeshDictionary.TryGetValue(meshRenderer.name, out Mesh replacementMesh) && replacementMesh != null)
                                    {
                                        MeshFilter meshFilter = meshRenderer.gameObject.GetComponent<MeshFilter>();

                                        if (meshFilter != null)
                                        {
                                            meshFilter.sharedMesh = replacementMesh;
                                            //Debug.Log($"[MeshReplacement] ✅ Replaced MeshRenderer '{meshRenderer.name}' with mesh '{replacementMesh.name}' (Group: {normalizedIdentifier}).");
                                            //successCount++;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    return;
                }
            }
        }

        foreach (var player in onlineManager.GetPlayersList())
        {
            Plugin.Log.LogWarning(player.NickName);

            int baseSkin = Plugin.onlineManager.Properties.GetPlayerCharacterSkin(player);
            string customSkin = Plugin.Patches.GetPlayerCharacterCustomSkin(player);


            //Plugin.Log.LogWarning("Character Skin: " + baseSkin + " - Custom Skin Name: " + customSkin);


            if (player.NickName == __instance.Data.Character.nickName)
            {
                if (Plugin.dictCustomSkinDatas.TryGetValue(customSkin, out var customSkinData))
                {
                    //Plugin.Log.LogWarning("Skin: " + customSkin + " found for Player: " + player.NickName);

                    CustomSkinData skinData = customSkinData;
                    CharacterMaterialOverridesHandler characterMaterialOverridesHandler = Plugin.Patches.GetCharacterMaterialOverridesHandler(__instance);
                    if (characterMaterialOverridesHandler == null)
                    {
                        //Debug.LogError("CharacterMaterialOverridesHandler is null!");
                        return;
                    }

                    if (skinData.CustomMOGList == null) return;

                    foreach (CharacterMaterialOverridesHandler.MaterialOverrideGroup originalMOGroup in skinData.CustomMOGList)
                    {
                        if (originalMOGroup == null) continue;

                        var MOGroup = Plugin.Patches.CloneMOGroup(originalMOGroup);
                        if (MOGroup == null) continue;

                        if (__instance.CharacterRenderer?.Objects != null)
                        {
                            foreach (var materialObject in __instance.CharacterRenderer.Objects)
                            {
                                if (materialObject.ObjectRenderer == null)
                                {
                                    //Debug.LogWarning("MaterialObject's ObjectRenderer is null!");
                                    continue;
                                }

                                string originalIdentifier = MOGroup.Identifier;
                                if (string.IsNullOrEmpty(originalIdentifier)) continue;

                                // Check if the identifier contains a colon and remove the prefix if it's there
                                int colonIndex = originalIdentifier.IndexOf(':');
                                if (colonIndex != -1)
                                {
                                    // Remove everything before and including the colon
                                    originalIdentifier = originalIdentifier.Substring(colonIndex + 1);
                                }

                                if (characterCodenames != null &&
                                    characterCodenames.TryGetValue(skinData.characterCodename.ToString(), out var altParts) &&
                                    altParts?.TryGetValue(skinData.skinIndex, out var parts) == true &&
                                    parts?.TryGetValue(originalIdentifier.ToString(), out var partList) == true)
                                {
                                    if (partList?.Any(part => materialObject.ObjectRenderer.name == part) == true)
                                    {
                                        if (materialObject.ObjectRenderer.materials != null)
                                        {
                                            foreach (var obj in materialObject.ObjectRenderer.materials)
                                            {
                                                if (obj?.name?.Contains(originalIdentifier) == true)
                                                {
                                                    if (!string.IsNullOrEmpty(MOGroup.Identifier) && char.IsDigit(MOGroup.Identifier[0]))
                                                    {
                                                        char firstChar = MOGroup.Identifier[0];
                                                        int firstDigit = int.Parse(firstChar.ToString());

                                                        MOGroup.Targets.Add(new CharacterMaterialOverridesHandler.TextureOverrideTarget
                                                        {
                                                            Target = materialObject.ObjectRenderer,
                                                            MaterialIndex = firstDigit
                                                        });
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (__instance.CharacterObjects?.MultipleConstraintObjectsList != null)
                        {
                            foreach (var materialObject in __instance.CharacterObjects.MultipleConstraintObjectsList)
                            {
                                if (materialObject.VisibleObjects == null || materialObject.VisibleObjects.Count == 0 || materialObject.VisibleObjects[0] == null)
                                {
                                    //Debug.LogWarning("MaterialObject's ObjectRenderer is null!");
                                    return;
                                }

                                string originalIdentifier = MOGroup.Identifier;
                                if (string.IsNullOrEmpty(originalIdentifier)) continue;

                                // Check if the identifier contains a colon and remove the prefix if it's there
                                int colonIndex = originalIdentifier.IndexOf(':');
                                if (colonIndex != -1)
                                {
                                    // Remove everything before and including the colon
                                    originalIdentifier = originalIdentifier.Substring(colonIndex + 1);
                                }

                                if (characterCodenames != null &&
                                    characterCodenames.TryGetValue(skinData.characterCodename.ToString(), out var altParts) &&
                                    altParts?.TryGetValue(skinData.skinIndex, out var parts) == true &&
                                    parts?.TryGetValue(originalIdentifier.ToString(), out var partList) == true)
                                {
                                    if (partList?.Any(part => materialObject.VisibleObjects[0].name == part) == true)
                                    {
                                        var meshRenderer = materialObject.VisibleObjects[0].GetComponent<MeshRenderer>();
                                        if (meshRenderer?.materials != null)
                                        {
                                            foreach (var obj in meshRenderer.materials)
                                            {
                                                if (obj?.name?.Contains(originalIdentifier) == true)
                                                {
                                                    if (!string.IsNullOrEmpty(MOGroup.Identifier) && char.IsDigit(MOGroup.Identifier[0]))
                                                    {
                                                        char firstChar = MOGroup.Identifier[0];
                                                        int firstDigit = int.Parse(firstChar.ToString());

                                                        MOGroup.Targets.Add(new CharacterMaterialOverridesHandler.TextureOverrideTarget
                                                        {
                                                            Target = meshRenderer,
                                                            MaterialIndex = firstDigit
                                                        });
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (__instance.CharacterObjects.ModelProps != null)
                        {
                            foreach (var materialObject in __instance.CharacterObjects.ModelProps)
                            {


                                string originalIdentifier = MOGroup.Identifier;
                                if (string.IsNullOrEmpty(originalIdentifier)) continue;

                                // Check if the identifier contains a colon and remove the prefix if it's there
                                int colonIndex = originalIdentifier.IndexOf(':');
                                if (colonIndex != -1)
                                {
                                    // Remove everything before and including the colon
                                    originalIdentifier = originalIdentifier.Substring(colonIndex + 1);
                                }

                                if (characterCodenames != null &&
                                    characterCodenames.TryGetValue(skinData.characterCodename.ToString(), out var altParts) &&
                                    altParts?.TryGetValue(skinData.skinIndex, out var parts) == true &&
                                    parts?.TryGetValue(originalIdentifier.ToString(), out var partList) == true)
                                {
                                    if (partList?.Any(part => materialObject.ObjectRef.name == part) == true)
                                    {
                                        var meshRenderer = materialObject.ObjectRef.GetComponent<MeshRenderer>();
                                        if (meshRenderer?.materials != null)
                                        {
                                            foreach (var obj in meshRenderer.materials)
                                            {
                                                if (obj?.name?.Contains(originalIdentifier) == true)
                                                {
                                                    if (!string.IsNullOrEmpty(MOGroup.Identifier) && char.IsDigit(MOGroup.Identifier[0]))
                                                    {
                                                        char firstChar = MOGroup.Identifier[0];
                                                        int firstDigit = int.Parse(firstChar.ToString());

                                                        MOGroup.Targets.Add(new CharacterMaterialOverridesHandler.TextureOverrideTarget
                                                        {
                                                            Target = meshRenderer,
                                                            MaterialIndex = firstDigit
                                                        });
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        characterMaterialOverridesHandler.TextureOverrides.Add(MOGroup);
                        characterMaterialOverridesHandler.ApplyMaterialOverride(MOGroup.Identifier);
                    }

                    if (skinData.customSFXData != null)
                    {
                        try
                        {
                            SFXSpawner[] componentsInChildren = __instance.gameObject.GetComponentsInChildren<SFXSpawner>(true);
                            if (componentsInChildren != null)
                            {
                                foreach (SFXSpawner sfxspawner in componentsInChildren)
                                {
                                    if (sfxspawner?.SFXAudioClipData == null) continue;

                                    //add the custom SFXData from the Custom Skin Data
                                    sfxspawner.SFXAudioClipData.Add(skinData.customSFXData);

                                    //Initializes the SFXData
                                    if (sfxspawner.SFXAudioClipData.Count > 2)
                                    {
                                        sfxspawner.SFXAudioClipData[2]?.Initialize();

                                        //sfxspawner.SFXAudioClipData[2].containersReference
                                        if (sfxspawner.SFXAudioClipData[2]?.Containers != null)
                                        {
                                            foreach (var entry in sfxspawner.SFXAudioClipData[2].Containers)
                                            {
                                                entry?.UpdateClipsList();
                                            }
                                        }

                                        if (sfxspawner.SFXAudioClipData.Count > 1)
                                        {
                                            sfxspawner.SFXAudioClipData.Remove(sfxspawner.SFXAudioClipData[1]);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Plugin.Log.LogError($"Error processing SFXSpawners for character {__instance.Codename}: {ex}");
                        }



                    }
                    __instance.StartCoroutine(ApplyCustomVFXData(__instance, skinData));

                    //Debug.Log("Processing characterMaterialOverridesHandler.");
                    if (characterMaterialOverridesHandler.TextureOverrides != null && skinData.materialBanksForMeshes != null)
                    {
                        foreach (var textureOverride in characterMaterialOverridesHandler.TextureOverrides)
                        {
                            if (textureOverride?.Targets == null) continue;

                            foreach (var targetOverride in textureOverride.Targets)
                            {
                                if (targetOverride.Target == null || targetOverride.MaterialIndex != 0)
                                {
                                    continue;
                                }

                                string originalIdentifier = textureOverride.Identifier;
                                if (string.IsNullOrEmpty(originalIdentifier)) continue;

                                // Check if the identifier contains a colon and remove the prefix if it's there
                                int colonIndex = originalIdentifier.IndexOf(':');
                                if (colonIndex != -1)
                                {
                                    // Remove everything before and including the colon
                                    originalIdentifier = originalIdentifier.Substring(colonIndex + 1);
                                }

                                // Try to get the inner dictionary from materialBanksForMeshes
                                if (!skinData.materialBanksForMeshes.TryGetValue(originalIdentifier, out Dictionary<string, Mesh> currentMeshDictionary) || currentMeshDictionary == null)
                                {
                                    //Debug.LogWarning($"No material bank found for Identifier: {textureOverride.Identifier}");
                                    continue;
                                }

                                // Handle SkinnedMeshRenderer
                                if (targetOverride.Target is SkinnedMeshRenderer skinnedMeshRenderer)
                                {
                                    if (currentMeshDictionary.TryGetValue(skinnedMeshRenderer.name, out Mesh replacementMesh) && replacementMesh != null)
                                    {
                                        skinnedMeshRenderer.sharedMesh = replacementMesh;
                                        //Debug.Log($"Replaced SkinnedMeshRenderer: {skinnedMeshRenderer.name} with mesh: {replacementMesh.name}");
                                    }
                                    else
                                    {
                                        //Debug.LogWarning($"No replacement mesh found for SkinnedMeshRenderer: {skinnedMeshRenderer.name} in material bank.");
                                    }
                                }
                                // Handle MeshRenderer
                                else if (targetOverride.Target is MeshRenderer meshRenderer)
                                {
                                    if (currentMeshDictionary.TryGetValue(meshRenderer.name, out Mesh replacementMesh) && replacementMesh != null)
                                    {
                                        MeshFilter meshFilter = meshRenderer.gameObject.GetComponent<MeshFilter>();

                                        if (meshFilter != null)
                                        {
                                            meshFilter.sharedMesh = replacementMesh;
                                            //Debug.Log($"Replaced MeshRenderer: {meshRenderer.name} with mesh: {replacementMesh.name}");
                                        }
                                        else
                                        {
                                            //Debug.LogError($"MeshFilter missing on MeshRenderer: {meshRenderer.name}. Cannot apply replacement.");
                                        }
                                    }
                                    else
                                    {
                                        //Debug.LogWarning($"No replacement mesh found for MeshRenderer: {meshRenderer.name} in material bank.");
                                    }
                                }
                            }
                        }
                    }

                    Debug.LogWarning("Skin was applied for " + __instance.Codename);
                }


            }
        }

    }

    private static IEnumerator ApplyCustomVFXData(CharacterManager __instance, CustomSkinData skinData)
    {
        // Wait a bit to ensure all spawned effects and materials are initialized
        yield return new WaitForSeconds(0.5f);

        Dictionary<string, List<Material>> materialGroups = new Dictionary<string, List<Material>>();

        // --- Collect renderers from VFX spawners only ---
        for (int i = 0; i < __instance.vfxSpawners.Count; i++)
        {
            var spawner = __instance.vfxSpawners[i];
            if (spawner == null)
            {
                Plugin.Log.LogWarning("vfxspawner " + i + " IS NULL, skipping");
                continue;
            }

            var renderers = spawner.GetComponentsInChildren<Renderer>(true);
            //Plugin.Log.LogWarning($"[MaterialEditor] Found {renderers.Length} renderers in vfxSpawners[{i}] ({spawner.name})");

            foreach (var renderer in renderers)
            {
                if (renderer == null) continue;
                if (renderer is UnityEngine.UI.Graphic || renderer.GetComponent<TMPro.TextMeshPro>() != null)
                    continue;

                // --- Regular materials ---
                foreach (var mat in renderer.materials) // use .materials to allow runtime instancing
                {
                    if (mat == null) continue;

                    string matName = mat.name.Replace(" (Instance)", "");

                    if (!materialGroups.TryGetValue(matName, out var list))
                    {
                        list = new List<Material>();
                        materialGroups[matName] = list;
                    }

                    list.Add(mat);
                }

                // --- Trail materials (for particle systems) ---
                if (renderer is ParticleSystemRenderer psr && psr.trailMaterial != null)
                {
                    var trailMat = psr.trailMaterial;
                    if (trailMat == null) continue;

                    string trailMatName = trailMat.name.Replace(" (Instance)", "");

                    if (!materialGroups.TryGetValue(trailMatName, out var list))
                    {
                        list = new List<Material>();
                        materialGroups[trailMatName] = list;
                    }

                    list.Add(trailMat);
                }
            }
        }

        // --- Apply overrides from skin data ---
        var vfxOverrides = skinData.customVFXData?.VfxOverrides;
        if (vfxOverrides == null || vfxOverrides.Count == 0)
            yield break;

        foreach (var entry in vfxOverrides)
        {
            foreach (var color in entry.colorOverrides)
            {
                VFXHelpers.SetColor(color.ColorID, color.ColorValue, entry.identifier, materialGroups);
            }
            foreach (var attribute in entry.attributeOverrides)
            {
                VFXHelpers.SetFloat(attribute.AttributeID, attribute.AttributeValue, entry.identifier, materialGroups);
            }
            foreach (var texture in entry.textureOverrides)
            {
                VFXHelpers.SetTexture(texture.TextureID, texture.TextureRef, entry.identifier, materialGroups);
            }
            foreach (var vector in entry.vectorAttributeOverrides)
            {
                VFXHelpers.SetVector(vector.AttributeID, vector.AttributeValue, entry.identifier, materialGroups);
            }
        }
    }

}