using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using static CheapSkinss.Plugin;
using UnityEngine;
using CheapSkinss;


[HarmonyPatch]
public class CharacterHUD_Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CharacterHUD), "CharacterHUDSpriteLoaded")]
    public static void CharacterHUDSpriteLoaded(CharacterHUD __instance, Sprite sprite)
    {
        if (dataManager.Online)
        {
            foreach (var player in onlineManager.GetPlayersList())
            {
                Plugin.Log.LogWarning(player.NickName);

                int baseSkin = Plugin.onlineManager.Properties.GetPlayerCharacterSkin(player);
                string customSkin = Plugin.Patches.GetPlayerCharacterCustomSkin(player);

                if (player.NickName == __instance.NicknameText.text)
                {
                    if (Plugin.dictCustomSkinDatas.TryGetValue(customSkin, out var customSkinData))
                    {
                        CustomSkinData skinData = customSkinData;

                        Texture2D texture = skinData.VSRender;
                        __instance.CharacterImage.sprite = Utility.ConvertTextureToSprite(texture);

                    }
                }
            }
        }
        else
        {
            Texture2D customSprite = null;

            if (Plugin.metaDataDict.TryGetValue(__instance.CharacterIndex, out var metaData))
            {
                if (Plugin.dictCustomSkinDatas.TryGetValue(metaData.customSkinName, out var customSkinData))
                {
                    CustomSkinData skinData = customSkinData;

                    Texture2D texture = skinData.VSRender;
                    __instance.CharacterImage.sprite = Utility.ConvertTextureToSprite(texture);
                }
            }
        }
        
    }
    [HarmonyPostfix]
    [HarmonyPatch(typeof(CharacterIndicator), "CharacterHUDSpriteLoaded")]
    public static void CharacterHUDSpriteLoaded(CharacterIndicator __instance, Sprite sprite)
    {
        if (dataManager.Online)
        {
            foreach (var player in onlineManager.GetPlayersList())
            {
                Plugin.Log.LogWarning(player.NickName);

                int baseSkin = Plugin.onlineManager.Properties.GetPlayerCharacterSkin(player);
                string customSkin = Plugin.Patches.GetPlayerCharacterCustomSkin(player);

                if (player.NickName == __instance.currentNickname)
                {
                    if (Plugin.dictCustomSkinDatas.TryGetValue(customSkin, out var customSkinData))
                    {
                        CustomSkinData skinData = customSkinData;

                        Texture2D texture = skinData.VSRender;
                        __instance.CharacterRenderImage.sprite = Utility.ConvertTextureToSprite(texture);

                        //__instance.CharacterImage.sprite = sprite;

                    }
                }
            }
        }
        else
        {
            Texture2D customSprite = null;

            if (Plugin.metaDataDict.TryGetValue(__instance.CharacterIndex, out var metaData))
            {
                if (Plugin.dictCustomSkinDatas.TryGetValue(metaData.customSkinName, out var customSkinData))
                {
                    CustomSkinData skinData = customSkinData;

                    Texture2D texture = skinData.VSRender;
                    __instance.CharacterRenderImage.sprite = Utility.ConvertTextureToSprite(texture);
                }
            }
        }

    }

}
