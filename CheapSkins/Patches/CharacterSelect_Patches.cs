using CheapSkinss;
using HarmonyLib;
using Photon.Deterministic;
using Quantum;
using System;
using System.Collections.Generic;
using System.Text;
using static CheapSkinss.Plugin;

[HarmonyPatch]
public class CharacterSelect_Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterSelect), "ReadDataManager")]
    public static bool ReadDataManager(CharacterSelect __instance)
    {
        Plugin.Log.LogWarning("READDATAMANAGER HAPPENING");

        __instance.Selectors.Clear();
        if (__instance.dataManager.PlayersData.LocalPlayers.Count == 1)
        {
            __instance.inputManager.CleanPlayersInputData();
            __instance.inputManager.AddPlayerInputData(__instance.dataManager.PlayersData.LocalPlayers[0].PlayerIndex, __instance.inputManager.LastInputDeviceWithInput, __instance.settingsManager.GlobalData.DefaultProfile);
        }
        List<RuntimePlayer> list = new List<RuntimePlayer>();
        for (int i = 0; i < __instance.dataManager.PlayersData.LocalPlayers.Count; i++)
        {
            RuntimePlayer runtimePlayer = __instance.dataManager.PlayersData.LocalPlayers[i];
            int playerInputDeviceIndex = __instance.inputManager.GetPlayerInputDeviceIndex(runtimePlayer.PlayerIndex);
            if (__instance.inputManager.GetInputDevice(playerInputDeviceIndex) == null)
            {
                __instance.RemoveInputDevice(runtimePlayer.PlayerIndex);
                list.Add(runtimePlayer);
            }
        }
        foreach (RuntimePlayer item in list)
        {
            __instance.dataManager.PlayersData.LocalPlayers.Remove(item);
        }
        if (__instance.dataManager.PlayersData.LocalPlayers.Count == 0)
        {
            GameManager.Instance.SetDefaultLocalPlayer(false);
        }
        int num = __instance.dataManager.PlayersData.LocalPlayers.Count;
        if (num > 1 && __instance.SquadStrikeSelection)
        {
            num = 2;
        }
        for (int j = 0; j < num; j++)
        {
            RuntimePlayer runtimePlayer2 = __instance.dataManager.PlayersData.LocalPlayers[j];
            if ((__instance.SquadStrikeSelection || !runtimePlayer2.IsSpectator) && (!__instance.SquadStrikeSelection || !__instance.dataManager.Online || !runtimePlayer2.IsSpectator))
            {
                int profileIndex = -1;
                if (j == 0)
                {
                    profileIndex = __instance.settingsManager.GlobalData.DefaultProfile;
                }
                __instance.inputManager.AssignInputProfile(profileIndex, runtimePlayer2.PlayerIndex);
                CharacterSelectSelector characterSelectSelector = new CharacterSelectSelector();
                characterSelectSelector.Enabled = true;
                characterSelectSelector.OnCharacterSelector = true;
                characterSelectSelector.Active = true;
                characterSelectSelector.IsCPU = false;
                characterSelectSelector.OnlineRemotePlayer = false;
                characterSelectSelector.PlayerIndex = runtimePlayer2.PlayerIndex;
                characterSelectSelector.InputDeviceIndex = __instance.inputManager.GetPlayerInputDeviceIndex(runtimePlayer2.PlayerIndex);
                characterSelectSelector.ProfileIndex = __instance.inputManager.GetPlayerProfile(runtimePlayer2.PlayerIndex);
                characterSelectSelector.PlayerNumber = runtimePlayer2.CharacterMatchData.PlayerNumber;
                if (__instance.SquadStrikeSelection && !__instance.panelChange && runtimePlayer2.CharacterMatchData.CharacterSquad != null)
                {
                    for (int k = 0; k < runtimePlayer2.CharacterMatchData.CharacterSquad.Count; k++)
                    {
                        SquadMember squadMember = runtimePlayer2.CharacterMatchData.CharacterSquad[k];
                        characterSelectSelector.Characters[k] = squadMember.Codename;
                        characterSelectSelector.Skins[k] = squadMember.Skin;
                        characterSelectSelector.RandomCharacters[j] = squadMember.Random;
                    }
                }
                else
                {
                    characterSelectSelector.Characters[characterSelectSelector.BrawlerIndex] = runtimePlayer2.CharacterMatchData.Character;
                    characterSelectSelector.Skins[characterSelectSelector.BrawlerIndex] = runtimePlayer2.CharacterMatchData.Skin;
                }
                characterSelectSelector.RandomCharacters[characterSelectSelector.BrawlerIndex] = runtimePlayer2.CharacterMatchData.RandomSelection;
                if (characterSelectSelector.IsRandom)
                {
                    characterSelectSelector.Characters[0] = __instance.matchManager.GetRandomCharacter();
                    characterSelectSelector.Skins[0] = __instance.matchManager.GetRandomCharacterSkin(characterSelectSelector.Character);
                }
                if (__instance.SquadStrikeSelection && characterSelectSelector.Characters[0] == CharacterCodename.Undefined)
                {
                    characterSelectSelector.Characters[0] = CharacterCodename.SpongeBob;
                    characterSelectSelector.Skins[0] = 0;
                }
                characterSelectSelector.Team = runtimePlayer2.CharacterMatchData.Team;
                if (characterSelectSelector.Team == CharacterTeam.None && __instance.teamsActive)
                {
                    characterSelectSelector.Team = CharacterTeam.Team1;
                }
                else if (!__instance.teamsActive)
                {
                    characterSelectSelector.Team = CharacterTeam.None;
                }
                int characterSlotIndex = __instance.GetCharacterSlotIndex(characterSelectSelector.Character);
                __instance.GridNavigator.SetCurrentIndex(characterSlotIndex, characterSelectSelector.PlayerIndex);
                __instance.Selectors.Add(characterSelectSelector);
            }
        }
        int num2 = __instance.dataManager.MatchData.MatchNPC.StartNPC.Count;
        if (num2 > 0 && __instance.SquadStrikeSelection)
        {
            num2 = 1;
        }
        for (int l = 0; l < num2; l++)
        {
            NPCData npcdata = __instance.dataManager.MatchData.MatchNPC.StartNPC[l];
            CharacterMatchData characterMatchData = npcdata.CharacterMatchData;
            CharacterSelectSelector characterSelectSelector2 = new CharacterSelectSelector();
            characterSelectSelector2.Enabled = true;
            characterSelectSelector2.OnCharacterSelector = true;
            characterSelectSelector2.IsCPU = true;
            characterSelectSelector2.OnlineRemotePlayer = false;
            if (__instance.ComingFromMatch)
            {
                characterSelectSelector2.Ready = true;
                characterSelectSelector2.OnCharacterSelector = false;
                if (__instance.SquadStrikeSelection)
                {
                    __instance.MainMenu.BattleMenu.GameMode = BattleMenu.BattleType.Squad;
                }
                else
                {
                    __instance.MainMenu.BattleMenu.GameMode = BattleMenu.BattleType.Battle;
                }
            }
            if (__instance.SquadStrikeSelection && !__instance.panelChange && characterMatchData.CharacterSquad != null)
            {
                for (int m = 0; m < characterMatchData.CharacterSquad.Count; m++)
                {
                    SquadMember squadMember2 = characterMatchData.CharacterSquad[m];
                    characterSelectSelector2.Characters[m] = squadMember2.Codename;
                    characterSelectSelector2.Skins[m] = squadMember2.Skin;
                    characterSelectSelector2.CPULevels[m] = squadMember2.NPCPresetID;
                }
            }
            else
            {
                characterSelectSelector2.Characters[characterSelectSelector2.BrawlerIndex] = characterMatchData.Character;
                characterSelectSelector2.Skins[characterSelectSelector2.BrawlerIndex] = characterMatchData.Skin;
                characterSelectSelector2.CPULevels[characterSelectSelector2.BrawlerIndex] = int.Parse(npcdata.PresetID);
            }
            characterSelectSelector2.RandomCharacters[characterSelectSelector2.BrawlerIndex] = characterMatchData.RandomSelection;
            if (characterSelectSelector2.IsRandom)
            {
                characterSelectSelector2.Characters[characterSelectSelector2.BrawlerIndex] = __instance.matchManager.GetRandomCharacter();
                characterSelectSelector2.Skins[characterSelectSelector2.BrawlerIndex] = __instance.matchManager.GetRandomCharacterSkin(characterSelectSelector2.Character);
            }
            characterSelectSelector2.Team = characterMatchData.Team;
            characterSelectSelector2.PlayerIndex = -1;
            characterSelectSelector2.PlayerNumber = -1;
            if (__instance.Selectors.Count == 0)
            {
                RuntimePlayer runtimePlayer3 = __instance.dataManager.PlayersData.LocalPlayers[0];
                characterSelectSelector2.PlayerNumber = runtimePlayer3.CharacterMatchData.PlayerNumber;
                characterSelectSelector2.PlayerIndex = 0;
                characterSelectSelector2.InputDeviceIndex = __instance.inputManager.GetPlayerInputDeviceIndex(runtimePlayer3.PlayerIndex);
                characterSelectSelector2.Active = true;
            }
            if (!__instance.SquadStrikeSelection)
            {
                __instance.Selectors.Add(characterSelectSelector2);
            }
        }
        for (int n = __instance.Selectors.Count; n < (__instance.SquadStrikeSelection ? 2 : 4); n++)
        {
            CharacterSelectSelector item2 = new CharacterSelectSelector();
            __instance.Selectors.Add(item2);
        }
        __instance.ReadedData = true;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterSelect), "SetSelectorCharacter")]
    public static bool SetSelectorCharacter(CharacterSelect __instance, CharacterCodename character, int selectorNumber, int skin = 0, bool unlocked = true)
    {
        Plugin.Log.LogWarning("CUSTOM SET SELECTOR CHARACTER");
        bool flag = selectorNumber == -1;
        bool result;
        if (flag)
        {
            result = false;
        }
        else
        {
            bool flag2 = __instance.StartCoundown && !unlocked;
            if (flag2)
            {
                result = false;
            }
            else
            {
                CharacterCodename character2 = __instance.Selectors[selectorNumber].Character;
                bool isRandom = __instance.Selectors[selectorNumber].IsRandom;
                __instance.Selectors[selectorNumber].Characters[__instance.Selectors[selectorNumber].BrawlerIndex] = character;
                __instance.Selectors[selectorNumber].RandomCharacters[__instance.Selectors[selectorNumber].BrawlerIndex] = false;
                __instance.Selectors[selectorNumber].Skins[__instance.Selectors[selectorNumber].BrawlerIndex] = skin;
                __instance.Selectors[selectorNumber].IsLocked = !unlocked;

                string localizedString = string.Empty;

                if (character != CharacterCodename.Sartana && character != CharacterCodename.VladPlasmius)
                {
                    localizedString = GameManager.Instance.GameResourcesManager.GetCharacterUIData(character).CharacterDescription.GetLocalizedString();
                }
                else
                {
                    localizedString = "Testing CharacterInfo";
                }


                RecordTracker records = GameManager.Instance.SettingsManager.GlobalData.Records;



                SinglePlayerMenu.SinglePlayerSubMenu lastArcadeMode = __instance.uiManager.LastArcadeMode;
                if (__instance.dataManager.MainMenuContext == MainMenuContext.Arcade)
                {
                    int characterBestScore;
                    double characterBestTime;
                    if (lastArcadeMode == SinglePlayerMenu.SinglePlayerSubMenu.BossRush)
                    {
                        characterBestScore = records.GetCharacterBestScore(character, RecordTracker.RecordTypes.BossRush);
                        characterBestTime = records.GetCharacterBestTime(character, RecordTracker.RecordTypes.BossRush);
                    }
                    else if (lastArcadeMode == SinglePlayerMenu.SinglePlayerSubMenu.AllStar)
                    {
                        characterBestScore = records.GetCharacterBestScore(character, RecordTracker.RecordTypes.AllStar);
                        characterBestTime = records.GetCharacterBestTime(character, RecordTracker.RecordTypes.AllStar);
                    }
                    else
                    {
                        characterBestScore = records.GetCharacterBestScore(character, RecordTracker.RecordTypes.Arcade);
                        characterBestTime = records.GetCharacterBestTime(character, RecordTracker.RecordTypes.Arcade);
                    }
                    __instance.SetArcadeInfo(characterBestTime, characterBestScore, localizedString);
                }
                if (__instance.dataManager.MainMenuContext == MainMenuContext.AllStar)
                {
                    int characterBestScore = records.GetCharacterBestScore(character, RecordTracker.RecordTypes.AllStar);
                    double characterBestTime = records.GetCharacterBestTime(character, RecordTracker.RecordTypes.AllStar);
                    __instance.SetArcadeInfo(characterBestTime, characterBestScore, localizedString);
                }
                if (__instance.dataManager.MainMenuContext == MainMenuContext.BotsMinigame)
                {
                    int characterBestScore = records.GetCharacterBestScore(character, RecordTracker.RecordTypes.BotsMiniGame);
                    __instance.SetWhackInfo(characterBestScore, localizedString);
                }
                if (__instance.dataManager.MainMenuContext == MainMenuContext.TargetsMinigame)
                {
                    double characterBestTime = records.GetCharacterBestTime(character, RecordTracker.RecordTypes.BallonsMinigame);
                    __instance.SetBlimpsInfo(characterBestTime, localizedString);
                }
                bool flag9 = __instance.dataManager.Online && !__instance.Selectors[selectorNumber].OnlineRemotePlayer && (character2 != character || isRandom);
                if (flag9)
                {
                    bool flag10 = !unlocked;
                    if (flag10)
                    {
                        __instance.onlineManager.Properties.SetPlayerCharacterIsRandom(true);
                        character = __instance.matchManager.GetRandomCharacter();
                        __instance.onlineManager.Properties.SetPlayerCharacter(character);
                        __instance.Selectors[selectorNumber].LockedCharacterReplacement = character;
                    }
                    else
                    {
                        __instance.onlineManager.Properties.SetPlayerCharacterIsRandom(false);
                    }
                    Plugin.Patches.CustomSetPlayerCharacterSkin(skin, character);
                    __instance.onlineManager.Properties.SetPlayerCharacterSkin(skin);
                    __instance.dataManager.PlayersData.LocalPlayers[0].CharacterMatchData.RandomSelection = false;
                    __instance.dataManager.PlayersData.LocalPlayers[0].CharacterMatchData.Character = character;
                    __instance.dataManager.PlayersData.LocalPlayers[0].CharacterMatchData.Skin = skin;
                }
                if (!dataManager.Online)
                {
                    if (Plugin.metaDataDict.TryGetValue(selectorNumber, out var metaData))
                    {
                        CharacterMetaData meta = new CharacterMetaData
                        {
                            playerIndex = selectorNumber,
                            skinIndex = skin,
                            customSkinName = "none",

                        };

                        Plugin.metaDataDict[selectorNumber] = meta;
                    }
                    else
                    {
                        CharacterMetaData meta = new CharacterMetaData
                        {
                            playerIndex = selectorNumber,
                            skinIndex = skin,
                            customSkinName = "none",

                        };

                        Plugin.metaDataDict.Add(selectorNumber, meta);
                    }
                }
                __instance.RefreshUI();
                result = false;
            }
        }
        return result;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterSelect), "WriteDataManager")]
    public static bool WriteDataManager(CharacterSelect __instance)
    {
        __instance.dataManager.PlayersData.LocalPlayers.Clear();
        __instance.dataManager.MatchData.MatchNPC.StartNPC.Clear();
        __instance.Selectors.Sort((CharacterSelectSelector s1, CharacterSelectSelector s2) => s1.PlayerNumber.CompareTo(s2.PlayerNumber));
        __instance.inputManager.CleanPlayersInputData();

        //int selectorIndex = -1;
        foreach (CharacterSelectSelector characterSelectSelector in __instance.Selectors)
        {
            if (characterSelectSelector.Enabled && !characterSelectSelector.Disabled && !characterSelectSelector.OnlineRemotePlayer)
            {
                if (characterSelectSelector.IsCPU)
                {
                    NPCData npcdata = new NPCData();
                    if (characterSelectSelector.IsLocked)
                    {
                        int playerCurrentSelector = __instance.GetPlayerCurrentSelector(characterSelectSelector.PlayerIndex);
                        __instance.SelectRandomCharacter(playerCurrentSelector, characterSelectSelector.PlayerIndex);
                    }
                    characterSelectSelector.BrawlerIndex = 0;
                    if (__instance.SquadStrikeSelection)
                    {
                        npcdata.CharacterMatchData.CharacterSquad = new List<SquadMember>();
                        for (int i = 0; i < characterSelectSelector.Characters.Count; i++)
                        {
                            SquadMember item = default(SquadMember);
                            item.Codename = characterSelectSelector.Characters[i];
                            item.Skin = characterSelectSelector.Skins[i];
                            item.Random = characterSelectSelector.RandomCharacters[i];
                            item.NPCPresetID = characterSelectSelector.CPULevels[i];
                            npcdata.CharacterMatchData.CharacterSquad.Add(item);
                        }
                    }
                    npcdata.CharacterMatchData.Character = characterSelectSelector.Character;

                    CharacterUIData characterUIData = __instance.gameResourcesManager.GetCharacterUIData(characterSelectSelector.Character);

                    if (characterUIData.Skins[characterSelectSelector.Skin].DebugName.Contains(":"))
                    {
                        // Get the DebugName
                        string debugName = characterUIData.Skins[characterSelectSelector.Skin].DebugName;

                        // Split at the colon and take the first part
                        string numberPart = debugName.Split(':')[0];

                        // Try to parse the number before colon
                        if (int.TryParse(numberPart, out int skinIndex))
                        {
                            npcdata.CharacterMatchData.Skin = skinIndex;
                        }
                        else
                        {
                            // Fallback to default skin if parsing fails
                            npcdata.CharacterMatchData.Skin = characterSelectSelector.Skin;
                            //Logger.LogWarning($"Failed to parse skin index from DebugName: {debugName}");
                        }
                    }
                    else
                    {
                        npcdata.CharacterMatchData.Skin = characterSelectSelector.Skin;
                    }

                    //npcdata.CharacterMatchData.Skin = characterSelectSelector.Skin;
                    npcdata.CharacterMatchData.Team = characterSelectSelector.Team;
                    npcdata.CharacterMatchData.TeamStocksMode = __instance.dataManager.MatchData.MatchRules.TeamStocksMode;
                    npcdata.CharacterMatchData.RandomSelection = characterSelectSelector.IsRandom;
                    npcdata.PresetID = characterSelectSelector.CPULevel.ToString();
                    __instance.dataManager.MatchData.MatchNPC.StartNPC.Add(npcdata);
                }
                else
                {
                    RuntimePlayer runtimePlayer = new RuntimePlayer();
                    runtimePlayer.PlayerIndex = characterSelectSelector.PlayerIndex;
                    runtimePlayer.CharacterMatchData.PlayerNumber = characterSelectSelector.PlayerNumber;
                    if (characterSelectSelector.IsLocked)
                    {
                        if (!__instance.dataManager.Online)
                        {
                            int playerCurrentSelector2 = __instance.GetPlayerCurrentSelector(characterSelectSelector.PlayerIndex);
                            __instance.SelectRandomCharacter(playerCurrentSelector2, characterSelectSelector.PlayerIndex);
                        }
                        else
                        {
                            characterSelectSelector.Characters[characterSelectSelector.BrawlerIndex] = characterSelectSelector.LockedCharacterReplacement;
                        }
                    }
                    characterSelectSelector.BrawlerIndex = 0;
                    if (__instance.SquadStrikeSelection)
                    {
                        runtimePlayer.CharacterMatchData.CharacterSquad = new List<SquadMember>();
                        for (int j = 0; j < characterSelectSelector.Characters.Count; j++)
                        {
                            SquadMember item2 = default(SquadMember);
                            item2.Codename = characterSelectSelector.Characters[j];
                            item2.Skin = characterSelectSelector.Skins[j];
                            item2.Random = characterSelectSelector.RandomCharacters[j];
                            runtimePlayer.CharacterMatchData.CharacterSquad.Add(item2);
                        }
                        if (__instance.dataManager.Online && !characterSelectSelector.OnlineRemotePlayer)
                        {
                            __instance.CharacterSelectOnlineManager.SetOnlineSquadReadyCharacter(characterSelectSelector.Characters[0], characterSelectSelector.Skins[0], 0);
                        }
                    }
                    runtimePlayer.CharacterMatchData.Character = characterSelectSelector.Character;
                    //int checkSkin = __instance.GetCharacterPanel(selectorIndex).currentSkins[characterSelectSelector.Skin];

                    Plugin.Log.LogWarning(characterSelectSelector.Skin);

                    CharacterUIData characterUIData = __instance.gameResourcesManager.GetCharacterUIData(characterSelectSelector.Character);

                    

                    if(characterUIData.Skins[characterSelectSelector.Skin].DebugName.Contains(":"))
                    {
                        // Get the DebugName
                        string debugName = characterUIData.Skins[characterSelectSelector.Skin].DebugName;

                        // Split at the colon and take the first part
                        string numberPart = debugName.Split(':')[0];

                        // Try to parse the number before colon
                        if (int.TryParse(numberPart, out int skinIndex))
                        {
                            runtimePlayer.CharacterMatchData.Skin = skinIndex;
                        }
                        else
                        {
                            // Fallback to default skin if parsing fails
                            runtimePlayer.CharacterMatchData.Skin = characterSelectSelector.Skin;
                            //Logger.LogWarning($"Failed to parse skin index from DebugName: {debugName}");
                        }
                    }
                    else
                    {
                        runtimePlayer.CharacterMatchData.Skin = characterSelectSelector.Skin;
                    }

                    
                    runtimePlayer.CharacterMatchData.RandomSelection = characterSelectSelector.IsRandom;
                    runtimePlayer.CharacterMatchData.Team = characterSelectSelector.Team;
                    runtimePlayer.CharacterMatchData.TeamStocksMode = __instance.dataManager.MatchData.MatchRules.TeamStocksMode;
                    if (__instance.dataManager.Online)
                    {
                        runtimePlayer.CharacterMatchData.Nickname = __instance.uiManager.MainUserNickname;
                    }
                    else if (characterSelectSelector.ProfileIndex != -1)
                    {
                        runtimePlayer.CharacterMatchData.Nickname = __instance.settingsManager.GlobalData.Profiles[characterSelectSelector.ProfileIndex].ProfileName;
                    }
                    else
                    {
                        runtimePlayer.CharacterMatchData.Nickname = "";
                    }
                    __instance.dataManager.PlayersData.LocalPlayers.Add(runtimePlayer);
                    __instance.inputManager.AddPlayerInputData(characterSelectSelector.PlayerIndex, characterSelectSelector.InputDeviceIndex, characterSelectSelector.ProfileIndex);
                    __instance.inputManager.AssignInputProfile(characterSelectSelector.ProfileIndex, characterSelectSelector.PlayerIndex);
                }
            }
        }

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterSelect), "SelectCharacter")]
    public static bool SelectCharacter(CharacterSelect __instance, CharacterCodename character, int selectorNumber, int playerIndex)
    {
        Plugin.Log.LogWarning("SelectCharacter HAPPENING");

        if (selectorNumber < 0)
        {
            return false;
        }
        CharacterSelectSelector characterSelectSelector = __instance.Selectors[selectorNumber];
        characterSelectSelector.Characters[characterSelectSelector.BrawlerIndex] = character;
        characterSelectSelector.OnCharacterSelector = false;
        __instance.GetCharacterPanel(selectorNumber).ShowSelectCharacterAnim();
        __instance.inputManager.CleanInputDevice(characterSelectSelector.InputDeviceIndex);
        int brawlerIndex = characterSelectSelector.BrawlerIndex;
        if (__instance.SquadStrikeSelection)
        {
            __instance.SquadsSelect.SquadSetCharacter(character, characterSelectSelector.Skin, selectorNumber, false);
            if (!__instance.SquadsSelect.SquadCompleteSet(selectorNumber) && !characterSelectSelector.IsCPU)
            {
                characterSelectSelector.OnCustomizeMenu = false;
                characterSelectSelector.OnSquadSelection = false;
                characterSelectSelector.OnCharacterSelector = true;
                if (!characterSelectSelector.OnlineRemotePlayer)
                {
                    __instance.SquadsSelect.NextCharacter(selectorNumber);
                }
            }
            else
            {
                characterSelectSelector.OnCustomizeMenu = true;
                characterSelectSelector.OnSquadSelection = false;
            }
        }
        else
        {
            characterSelectSelector.OnCustomizeMenu = true;
        }
        if ((!__instance.SquadStrikeSelection && !characterSelectSelector.IsRandom) || (__instance.SquadStrikeSelection && !characterSelectSelector.RandomCharacters[brawlerIndex]))
        {
            __instance.CharacterAnnouncerSFXSpawner.PlaySFXCategory(SFXType.General, -1, character.ToString(), FPVector3.Zero, -1, SFXSpawner.SpecialAudioBus.None, CharacterCodename.Undefined);
        }
        if (__instance.dataManager.Online && !characterSelectSelector.OnlineRemotePlayer)
        {
            if (__instance.SquadStrikeSelection)
            {
                __instance.onlineManager.Properties.SetPlayerSquadCharacterIndex(brawlerIndex);
                __instance.onlineManager.Properties.SetOnlineSquadCharacter(character, brawlerIndex);
                __instance.onlineManager.Properties.SetOnlineSquadRandom(characterSelectSelector.RandomCharacters[brawlerIndex], brawlerIndex);
                __instance.onlineManager.Properties.SetOnlineSquadSkin(characterSelectSelector.Skins[brawlerIndex], brawlerIndex);
                if (__instance.dataManager.PlayersData.LocalPlayers.Count != 0)
                {
                    SquadMember squadMember = default(SquadMember);
                    squadMember.Codename = character;
                    squadMember.Skin = characterSelectSelector.Skins[brawlerIndex];
                    if (__instance.dataManager.PlayersData.LocalPlayers[0].CharacterMatchData.CharacterSquad != null)
                    {
                        if (__instance.dataManager.PlayersData.LocalPlayers[0].CharacterMatchData.CharacterSquad.Count <= brawlerIndex)
                        {
                            __instance.dataManager.PlayersData.LocalPlayers[0].CharacterMatchData.CharacterSquad.Add(squadMember);
                        }
                        else
                        {
                            __instance.dataManager.PlayersData.LocalPlayers[0].CharacterMatchData.CharacterSquad[brawlerIndex] = squadMember;
                        }
                    }
                }
            }

            Plugin.metaDataDict.Remove(brawlerIndex);

            __instance.onlineManager.Properties.SetPlayerCharacter(character);
            if (__instance.dataManager.PlayersData.LocalPlayers.Count != 0)
            {
                __instance.dataManager.PlayersData.LocalPlayers[0].CharacterMatchData.Character = character;
            }
        }
        __instance.RefreshUI();

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CharacterSelect), "RefreshUI")]
    public static bool RefreshUI(CharacterSelect __instance)
    {

        if (__instance.SquadStrikeSelection != __instance.SquadsSelect.ActivePanel)
        {
            __instance.panelChange = true;
            foreach (CharacterSelectPlayerPosition characterSelectPlayerPosition in UnityEngine.Object.FindObjectOfType<CharacterSelectPlayerPositions>().Positions)
            {
                characterSelectPlayerPosition.Highlight.SetActive(false);
            }
            for (int i = 0; i < __instance.panelsCount; i++)
            {
                __instance.GetCharacterPanel(i).CustomizeMenu.ToggleMenu(false, false);
                __instance.GetCharacterPanel(i).CustomizeMenu.Clean();
                __instance.GetCharacterPanel(i).Disable();
            }
            for (int j = 0; j < __instance.slotsCount; j++)
            {
                __instance.GetCharacterSlot(j).CleanSquadData();
            }
            __instance.ReadedData = false;
            __instance.Initialize();
            __instance.SetCharacterSlots();
            __instance.Show();
            __instance.panelChange = false;
        }
        if (__instance.SquadStrikeSelection)
        {
            __instance.Settings.AllowChangePlayerToCPU = false;
            __instance.SquadsSelect.RefreshSlots();
        }
        for (int k = 0; k < __instance.panelsCount; k++)
        {
            CharacterSelectSelector characterSelectSelector = __instance.Selectors[k];
            CharacterPanel characterPanel = __instance.GetCharacterPanel(k);
            characterPanel.AssignInputDevice(characterSelectSelector.PlayerIndex);
            if (__instance.SquadStrikeSelection)
            {
                __instance.SquadsSelect.AssignInputDevice(characterSelectSelector.PlayerIndex, characterSelectSelector.OnlineRemotePlayer ? characterSelectSelector.PlayerNumber : k);
            }
            if (!characterSelectSelector.Active && !characterSelectSelector.Enabled && !characterSelectSelector.Ready)
            {
                characterPanel.ToggleAddPlayerMode(true);
                characterPanel.CustomizeMenu.Clean();
            }
            if (characterSelectSelector.Active && !characterSelectSelector.Ready && !characterSelectSelector.Enabled && characterSelectSelector.IsCPU)
            {
                characterPanel.ToggleAddCPUPrompt(true);
            }
            else
            {
                characterPanel.ToggleAddCPUPrompt(false);
            }
            if (characterSelectSelector.Active && characterSelectSelector.Ready)
            {
                characterPanel.ToggleEditPrompt(true);
            }
            else
            {
                characterPanel.ToggleEditPrompt(false);
            }
            if (__instance.dataManager.Online)
            {
                characterPanel.ChangeOnlineState(characterSelectSelector.LobbyLocation, characterSelectSelector.OnlineUsername, !characterSelectSelector.OnlineRemotePlayer);
            }
            else
            {
                characterPanel.DisableOnlineElements();
            }
            if (characterSelectSelector.Enabled && characterSelectSelector.Character != CharacterCodename.Undefined)
            {
                if (characterSelectSelector.IsRandom)
                {
                    characterPanel.ShowRandom();
                }
                else
                {

                    CharacterPanel characterPanel1 = __instance.GetCharacterPanel(k);

                    CharacterUIData characterUIData = __instance.gameResourcesManager.GetCharacterUIData(characterSelectSelector.Character);

                    



                    Plugin.Log.LogWarning($"Updating CharacterPanel ({k + 1}) with Skin: {characterSelectSelector.Skin}");
                    //Plugin.Log.LogWarning($"Updating CharacterPanel ({k + 1}) with Skin Modified: {characterPanel1.currentSkin}");
                    //Plugin.Log.LogWarning($"Updating CharacterPanel ({k + 1}) with Skin Modified: {characterPanel1.currentSkin}");
                    
                    if (characterSelectSelector.Skin == 0)
                    {
                        characterPanel.UpdateData(characterUIData, characterSelectSelector.Skin);
                    }
                    else
                    {
                        characterPanel.UpdateData(characterUIData, characterPanel1.currentSkin);
                    }
                    Plugin.MetaData component = characterPanel1.gameObject.GetComponent<Plugin.MetaData>();
                    if (component != null)
                    {
                        component.UpdateData(characterUIData, characterSelectSelector.Skin);
                    }



                }
                if (__instance.SquadStrikeSelection)
                {
                    __instance.SquadsSelect.SquadShowCharacter(characterSelectSelector.Character, k, characterSelectSelector.IsRandom);
                }
            }
            characterPanel.SetColor();
            if (__instance.teamsActive != characterPanel.CustomizeMenu.TeamSelector.isActiveAndEnabled)
            {
                characterPanel.CustomizeMenu.RefreshData();
            }
        }
        for (int l = 0; l < __instance.slotsCount; l++)
        {
            CharacterSelectSlot characterSlot = __instance.GetCharacterSlot(l);
            if (characterSlot.gameObject.activeInHierarchy)
            {
                characterSlot.PreviousSelectorsHighlighting = new List<int>(characterSlot.SelectorsHighlighting);
                characterSlot.SelectorsHighlighting.Clear();
                characterSlot.SelectorsActive.Clear();
                characterSlot.CleanData();
                bool flag = false;
                for (int m = 0; m < __instance.Selectors.Count; m++)
                {
                    CharacterSelectSelector characterSelectSelector2 = __instance.Selectors[m];
                    if (!characterSelectSelector2.OnlineRemotePlayer && characterSelectSelector2.Enabled)
                    {
                        bool flag2 = characterSelectSelector2.Character == characterSlot.GetCharacter() && !characterSelectSelector2.IsRandom;
                        if (__instance.SquadStrikeSelection)
                        {
                            flag2 = false;
                            if (characterSelectSelector2.Character == characterSlot.GetCharacter() && !characterSelectSelector2.IsRandom)
                            {
                                flag2 = true;
                            }
                            for (int n = 0; n < characterSelectSelector2.Characters.Count; n++)
                            {
                                CharacterCodename characterCodename = characterSelectSelector2.Characters[n];
                                if (n != characterSelectSelector2.BrawlerIndex && characterCodename == characterSlot.GetCharacter() && __instance.SquadsSelect.SquadCharacterIsSet(m, n))
                                {
                                    CharacterSelectSlot characterSlot2 = __instance.GetCharacterSlot(__instance.randomSlot);
                                    if (!characterSelectSelector2.RandomCharacters[n])
                                    {
                                        characterSlot.SetSquadsTags(m, n);
                                    }
                                    else
                                    {
                                        characterSlot2.SetSquadsTags(m, n);
                                    }
                                }
                            }
                        }
                        bool flag3 = characterSelectSelector2.IsRandom && characterSlot.IsRandom;
                        if (flag2 || flag3)
                        {
                            if (characterSelectSelector2.Active && characterSelectSelector2.OnCharacterSelector)
                            {
                                flag = true;
                                characterSlot.SelectorsHighlighting.Add(m);
                                characterSlot.Highlight();
                            }
                            else
                            {
                                characterSlot.SelectorsActive.Add(m);
                            }
                            if (characterSelectSelector2.Active)
                            {
                                __instance.GridNavigator.SetCurrentIndex(l, characterSelectSelector2.PlayerIndex);
                            }
                        }
                    }
                }
                if (!flag)
                {
                    characterSlot.UndoHighlight();
                }
                characterSlot.CheckHighlights();
            }
        }
        for (int num = 0; num < __instance.Selectors.Count; num++)
        {
            CharacterSelectSelector characterSelectSelector3 = __instance.Selectors[num];
            bool ready = characterSelectSelector3.Ready && !__instance.matchReady;
            __instance.GetCharacterPanel(num).TogglePlayerReady(ready);
            CharacterSelectCustomizeMenu customizeMenu = __instance.GetCharacterPanel(num).CustomizeMenu;
            customizeMenu.AssignInputDevice(characterSelectSelector3.PlayerIndex);
            if (characterSelectSelector3.OnCustomizeMenu)
            {
                customizeMenu.ToggleMenu(true, false);
                customizeMenu.UINavigator.TogglePlayerNavigator(characterSelectSelector3.PlayerIndex, characterSelectSelector3.Active);
                customizeMenu.RefreshSelector();
            }
            else
            {
                customizeMenu.ToggleMenu(false, false);
                customizeMenu.UINavigator.TogglePlayerNavigator(characterSelectSelector3.PlayerIndex, false);
            }
            if (characterSelectSelector3.Active)
            {
                PlayerCursor playerCursor = __instance.GetPlayerCursor(characterSelectSelector3.PlayerNumber);
                playerCursor.ToggleCursor(true);
                if (characterSelectSelector3.IsCPU && characterSelectSelector3.OnCharacterSelector)
                {
                    playerCursor.ToggleCPUTag(true);
                }
                else
                {
                    playerCursor.ToggleCPUTag(false);
                }
                if (__instance.teamsActive)
                {
                    int num2 = characterSelectSelector3.Team - CharacterTeam.Team1;
                    playerCursor.SetCursor(characterSelectSelector3.PlayerIndex + 1, __instance.uiManager.PlayerColors[num2].MainColor);
                }
                if (__instance.SquadStrikeSelection)
                {
                    if (characterSelectSelector3.OnCharacterSelector)
                    {
                        __instance.TogglePlayerGridNavigation(characterSelectSelector3.PlayerIndex, true);
                    }
                    else
                    {
                        __instance.TogglePlayerGridNavigation(characterSelectSelector3.PlayerIndex, false);
                        if (!characterSelectSelector3.OnCustomizeMenu && !characterSelectSelector3.OnSquadSelection)
                        {
                            __instance.GetCharacterPanel(num).ToggleHighlight(true);
                        }
                    }
                    __instance.TogglePlayerSquadNavigation(characterSelectSelector3.PlayerIndex, num, characterSelectSelector3.OnSquadSelection);
                }
                else if (characterSelectSelector3.OnCharacterSelector)
                {
                    __instance.TogglePlayerGridNavigation(characterSelectSelector3.PlayerIndex, true);
                }
                else
                {
                    __instance.TogglePlayerGridNavigation(characterSelectSelector3.PlayerIndex, false);
                    if (!characterSelectSelector3.OnCustomizeMenu)
                    {
                        __instance.GetCharacterPanel(num).ToggleHighlight(true);
                    }
                }
            }
            else
            {
                __instance.GetCharacterPanel(num).ToggleHighlight(false);
            }
            if (characterSelectSelector3.PlayerNumber == 0)
            {
                __instance.GridNavigator.MousePlayerIndex = characterSelectSelector3.PlayerIndex;
            }
        }
        for (int num3 = 0; num3 < (__instance.SquadStrikeSelection ? 2 : 4); num3++)
        {
            if (!__instance.PlayerNumbersIsActive(num3))
            {
                __instance.GetPlayerCursor(num3).ToggleCursor(false);
            }
        }
        __instance.CheckMatchReady();
        return false;
    }
}