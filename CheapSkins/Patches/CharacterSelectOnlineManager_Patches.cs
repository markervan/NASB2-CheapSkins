using CheapSkinss;
using HarmonyLib;
using Photon.Deterministic;
using Photon.Realtime;
using Quantum;
using System;
using System.Collections.Generic;
using System.Text;

[HarmonyPatch]
public class CharacterSelectOnlineManager_Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterSelectOnlineManager), "PhotomRoomInfoUpdate")]
    public static bool PhotomRoomInfoUpdate(CharacterSelectOnlineManager __instance)
    {
        Plugin.Log.LogWarning("PhotomRoomInfoUpdate HAPPENING");

        __instance.onlineManager.UpdatePlayersData();
        __instance.currentPlatformManager.ActivitiesSystem.RefreshPlayersData();
        if (__instance.onlineManager.CheckBlocklistPending)
        {
            __instance.onlineManager.CheckBlocklistPending = false;
            __instance.onlineManager.CheckBlocklist();
        }
        if (__instance.onlineManager.Properties.GetMatchActive())
        {
            return false;
        }
        int num = 0;
        List<Player> playersList = __instance.onlineManager.GetPlayersList();
        foreach (Player player in playersList)
        {
            if (__instance.onlineManager.IsPlayerMatchBrawler(player))
            {
                PlayerLobbyLocation playerLobbyLocation = __instance.onlineManager.Properties.GetPlayerLobbyLocation(player);
                CharacterSelectSelector characterSelectSelector = __instance.CharacterSelect.Selectors[num];
                characterSelectSelector.PlayerNumber = num;
                characterSelectSelector.LobbyLocation = playerLobbyLocation;
                characterSelectSelector.OnlineUsername = __instance.onlineManager.GetPlayerNickname(player, true);
                characterSelectSelector.Enabled = true;
                characterSelectSelector.IsCPU = false;
                if (player == __instance.onlineManager.CurrentPlayer)
                {
                    Plugin.Log.LogMessage("player is current player");
                    if (!__instance.initLocalPlayer)
                    {
                        __instance.initLocalPlayer = true;
                        RuntimePlayer runtimePlayer = __instance.dataManager.PlayersData.LocalPlayers[0];
                        characterSelectSelector.Active = true;
                        characterSelectSelector.PlayerIndex = runtimePlayer.PlayerIndex;
                        characterSelectSelector.ProfileIndex = __instance.settingsManager.GlobalData.DefaultProfile;
                        characterSelectSelector.InputDeviceIndex = __instance.inputManager.GetPlayerInputDeviceIndex(runtimePlayer.PlayerIndex);
                        if (__instance.onlineManager.Properties.GetGameMode() == GameModes.SquadStrike)
                        {
                            if (runtimePlayer.CharacterMatchData.CharacterSquad != null)
                            {
                                for (int i = 0; i < runtimePlayer.CharacterMatchData.CharacterSquad.Count; i++)
                                {
                                    SquadMember squadMember = runtimePlayer.CharacterMatchData.CharacterSquad[i];
                                    characterSelectSelector.Characters[i] = squadMember.Codename;
                                    characterSelectSelector.Skins[i] = squadMember.Skin;
                                }
                            }
                        }
                        else
                        {
                            characterSelectSelector.Characters[characterSelectSelector.BrawlerIndex] = runtimePlayer.CharacterMatchData.Character;

                            Plugin.Log.LogMessage("(current player) current Skin: " + characterSelectSelector.Skins[characterSelectSelector.BrawlerIndex] + "setting it to " + runtimePlayer.CharacterMatchData.Skin);

                            characterSelectSelector.Skins[characterSelectSelector.BrawlerIndex] = runtimePlayer.CharacterMatchData.Skin;
                            characterSelectSelector.RandomCharacters[characterSelectSelector.BrawlerIndex] = runtimePlayer.CharacterMatchData.RandomSelection;
                        }
                        characterSelectSelector.Team = runtimePlayer.CharacterMatchData.Team;
                        characterSelectSelector.OnCharacterSelector = true;
                        characterSelectSelector.OnlineRemotePlayer = false;
                        characterSelectSelector.LobbyLocation = PlayerLobbyLocation.CharacterSelect;
                        if (characterSelectSelector.IsRandom)
                        {
                            characterSelectSelector.Characters[characterSelectSelector.BrawlerIndex] = __instance.matchManager.GetRandomCharacter();
                            characterSelectSelector.Skins[characterSelectSelector.BrawlerIndex] = __instance.matchManager.GetRandomCharacterSkin(characterSelectSelector.Character);
                            __instance.onlineManager.Properties.SetPlayerSquadCharacterIndex(characterSelectSelector.BrawlerIndex);
                            __instance.onlineManager.Properties.SetOnlineSquadCharacter(characterSelectSelector.Character, characterSelectSelector.BrawlerIndex);
                            __instance.onlineManager.Properties.SetOnlineSquadRandom(characterSelectSelector.IsRandom, characterSelectSelector.BrawlerIndex);
                            __instance.onlineManager.Properties.SetOnlineSquadSkin(characterSelectSelector.Skin, characterSelectSelector.BrawlerIndex);
                            __instance.onlineManager.Properties.SetPlayerCharacter(characterSelectSelector.Character);
                            __instance.onlineManager.Properties.SetPlayerCharacterSkin(characterSelectSelector.Skin);
                        }
                    }
                }
                else
                {
                    Plugin.Log.LogMessage("player is remote");
                    characterSelectSelector.OnlineRemotePlayer = true;
                    characterSelectSelector.PlayerIndex = -1;
                    if (playerLobbyLocation == PlayerLobbyLocation.CharacterSelect)
                    {
                        characterSelectSelector.BrawlerIndex = __instance.onlineManager.Properties.GetPlayerSquadCharacterIndex(player);
                        if (__instance.onlineManager.Properties.GetGameMode() == GameModes.SquadStrike)
                        {
                            characterSelectSelector.Characters = __instance.GetOnlineSquadCharacter(player);
                            characterSelectSelector.Skins = __instance.GetOnlineSquadSkin(player);
                            characterSelectSelector.RandomCharacters = __instance.GetOnlineSquadRandom(player);
                        }
                        else
                        {
                            characterSelectSelector.Characters[characterSelectSelector.BrawlerIndex] = __instance.onlineManager.Properties.GetPlayerCharacter(player);

                            Plugin.Log.LogMessage("(remote player) current skin: " + characterSelectSelector.Skins[characterSelectSelector.BrawlerIndex] + "setting it to : " + __instance.onlineManager.Properties.GetPlayerCharacterSkin(player));
                            characterSelectSelector.Skins[characterSelectSelector.BrawlerIndex] = __instance.onlineManager.Properties.GetPlayerCharacterSkin(player);
                            characterSelectSelector.RandomCharacters[characterSelectSelector.BrawlerIndex] = __instance.onlineManager.Properties.GetPlayerCharacterIsRandom(player);
                        }
                        characterSelectSelector.Team = __instance.onlineManager.Properties.GetPlayerTeam(player);
                    }
                    else
                    {
                        characterSelectSelector.Characters[characterSelectSelector.BrawlerIndex] = CharacterCodename.Undefined;
                    }
                    bool playerReady = __instance.onlineManager.Properties.GetPlayerReady(player);
                    characterSelectSelector.Ready = playerReady;
                }
                __instance.CharacterSelect.Selectors[num] = characterSelectSelector;
                num++;
            }
        }
        if (__instance.currentBrawlersAmount != -1 && __instance.currentBrawlersAmount != num)
        {
            __instance.currentBrawlersAmount = num;
            __instance.CleanSelectors();
            __instance.PhotomRoomInfoUpdate();
            return false;
        }
        __instance.currentBrawlersAmount = num;
        if (playersList.Count == 1 || num == 1)
        {
            if (__instance.onlineManager.Properties.GetLobbyType() == OnlineLobbyType.Default)
            {
                __instance.Clean();
                __instance.CharacterSelect.MainMenu.ChangeScreen(GameScreenID.LobbyMenu);
            }
            else
            {
                CharacterSelectSelector characterSelectSelector2 = __instance.CharacterSelect.Selectors[num];
                characterSelectSelector2.PlayerNumber = num;
                characterSelectSelector2.Enabled = true;
                characterSelectSelector2.LobbyLocation = PlayerLobbyLocation.WaitingToJoin;
                characterSelectSelector2.OnlineRemotePlayer = true;
                characterSelectSelector2.Characters[characterSelectSelector2.BrawlerIndex] = CharacterCodename.Undefined;
            }
        }
        Ruleset ruleset = __instance.onlineManager.SyncOnlineRules();
        __instance.CharacterSelect.RulesetPreview.SetRuleset(ruleset);
        bool flag = __instance.dataManager.MatchData.MatchRules.GameMode == GameModes.SquadStrike;
        if (__instance.CharacterSelect.EnabledSquadsUI != flag)
        {
            __instance.CharacterSelect.EnabledSquadsUI = flag;
        }
        __instance.CharacterSelect.RefreshUI();

        return false;
    }

}
