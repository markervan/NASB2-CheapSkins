using CheapSkinss;
using HarmonyLib;
using Photon.Realtime;
using Quantum;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
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
}