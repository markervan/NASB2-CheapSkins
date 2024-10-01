using BepInEx;
using BepInEx.Logging;
using Epic.OnlineServices;
using HarmonyLib;
using Newtonsoft.Json;
using Photon.Realtime;
using Quantum;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheapSkinss
{
    [BepInPlugin("markaccino.nasb2.cheapskins", "CheapSkins", "1.0")]
    internal class Plugin : BaseUnityPlugin
    {
        public string nameText = "1";
        internal static ManualLogSource Log;
        public static string skinsPath = Path.Combine(Paths.PluginPath, "Skins");

        #region callers
        static public OnlineManager onlineManager
        {
            get
            {
                return GameManager.Instance.OnlineManager;
            }
        }

        static public UIManager uiManager
        {
            get
            {
                return GameManager.Instance.UIManager;
            }
        }

        static public NetworkClient client
        {
            get
            {
                return GameManager.Instance.OnlineManager.Client;
            }
        }

        static public DataManager dataManager
        {
            get
            {
                return GameManager.Instance.DataManager;
            }
        }
        static public InputManager inputManager
        {
            get
            {
                return GameManager.Instance.InputManager;
            }
        }
        #endregion

        public static Dictionary<string, CharacterMaterialOverridesHandler.MaterialOverrideGroup> MOGList = new Dictionary<string, CharacterMaterialOverridesHandler.MaterialOverrideGroup>();
        public static List<CharacterMetaData> metaDataList = new List<CharacterMetaData>();
        public static string CUSTOM_SKIN_ID = "CUSTOM_SKIN_ID";
        public static string CUSTOM_SKIN_NAME = "CUSTOM_SKIN_NAME";
        public static Dictionary<string, List<string>> materialMeshGroups = new Dictionary<string, List<string>>();
        public class CharacterMetaData
        {
            public int playerRef;
            public CharacterCodename characterCodename;
            public int skinIndex;
            public int customSkinID;
        }

        private class MetaData : MonoBehaviour
        {
            private CharacterPanel characterPanel;
            private CharacterSelectReadyBanner banner;
            public CharacterSelect characterSelect;
            private int playerRef;
            private CharacterCodename characterCodename;
            private CharacterCodename previousCharacterCodename;
            private int skinIndex1;
            private int previousSkinIndex;
            private int customSkinID;
            public List<CustomSkinData> customSkinsID = new List<CustomSkinData>();
            private int inputDeviceIndex;
            private int currentCustomSkinID;
            private TMPro.TextMeshProUGUI mainTitleSkin;
            private TMPro.TextMeshProUGUI authorTitle;
            private GameObject CustominfoSection;
            private UnityEngine.UI.Image skinImage;

            private void Awake()
            {
                characterPanel = this.gameObject.GetComponent<CharacterPanel>();
                Debug.Log(characterPanel.gameObject.name);

                CharacterSelectReadyBanner[] banners = FindObjectsOfType<CharacterSelectReadyBanner>(true);
                if (banners.Length > 0)
                {
                    banner = banners[0];
                    Debug.Log("Found CharacterSelectReadyBanner: " + banner.gameObject.name);
                }

                inputDeviceIndex = inputManager.GetPlayerInputDeviceIndex(characterPanel.playerIndex);

                // Initialize previous values
                previousCharacterCodename = characterPanel.currentCharacter;
                previousSkinIndex = characterPanel.currentSkin;
                SetUpPanel();
                if (dataManager.Online)
                {
                    Player currentPlayer1 = Plugin.onlineManager.CurrentPlayer;
                    Plugin.onlineManager.Properties.SetPlayerRoomProperty(currentPlayer1, Plugin.CUSTOM_SKIN_ID, 0);
                }
            }
            private void SetUpPanel()
            {
                Transform childTransform = characterPanel.gameObject.transform.Find("InfoSection");

                CustominfoSection = GameObject.Instantiate(childTransform.gameObject);

                CustominfoSection.transform.SetParent(characterPanel.gameObject.transform, true);
                //CustominfoSection.SetActive(true);
                CustominfoSection.name = "CustomSkinInfo";
                CustominfoSection.transform.localPosition = new Vector3(-38.7277f, -411.6458f, 0);
                CustominfoSection.transform.localScale = new Vector3(1, 1, 1);

                Transform mainTextTransform = CustominfoSection.transform.Find("MainTitleText");
                mainTitleSkin = mainTextTransform.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                mainTitleSkin.alignment = TextAlignmentOptions.Center;
                mainTitleSkin.gameObject.transform.localPosition = new Vector3(46.8699f, -24.9899f, 0);
                mainTitleSkin.text = "TEST";


                Transform mainTextTransform11 = CustominfoSection.transform.Find("SecondaryTitle");
                authorTitle = mainTextTransform11.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                authorTitle.text = "test1";

                Transform mainTextTransform12 = CustominfoSection.transform.Find("PlayerTag");
                skinImage = mainTextTransform12.gameObject.GetComponentInChildren<UnityEngine.UI.Image>();
                skinImage.preserveAspect = true;
                skinImage.material = null;
                skinImage.color = Color.white;

                TMPro.TextMeshProUGUI textComponent = mainTextTransform12.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                GameObject textP1 = textComponent.gameObject;
                textP1.SetActive(false);
            }
            public void UpdateData()
            {
                if (banner.isReady)
                {
                    return;
                }

                // Update only if the current character or skinIndex has changed
                if (characterCodename == previousCharacterCodename && skinIndex1 == previousSkinIndex)
                {
                    return;
                }

                //Debug.Log("UpdateData from MetaData");



                characterCodename = characterPanel.currentCharacter;
                skinIndex1 = characterPanel.currentSkin;

                // Store the current values as the previous ones for the next comparison
                previousCharacterCodename = characterCodename;
                previousSkinIndex = skinIndex1;

                customSkinsID.Clear();
                customSkinID = 0;
                currentCustomSkinID = 0;
                if (customSkinID == 0)
                {
                    CustominfoSection.SetActive(false);
                }
                // Add default skin
                CustomSkinData defaultSkin = new CustomSkinData
                {
                    skinID = null,
                    skinIndex = 0,
                    characterCodename = characterPanel.currentCharacter,
                    skinIntIndex = 0,
                    stockImage = null,
                    VSRender = null,
                    //customMeshsesToReplace = null,
                    CustomMOGList = null
                };
                customSkinsID.Add(defaultSkin);

                // Add matching custom skins
                foreach (CustomSkinData skinData in customSkinDatas)
                {
                    if (skinData.characterCodename == characterPanel.currentCharacter && skinData.skinIndex == characterPanel.currentSkin)
                    {
                        customSkinsID.Add(skinData);
                    }
                }
            }

            private void Update()
            {
                characterCodename = characterPanel.currentCharacter;
                skinIndex1 = characterPanel.currentSkin;
                inputDeviceIndex = inputManager.GetPlayerInputDeviceIndex(characterPanel.playerIndex);


                


                if (characterPanel.playerIsReady)
                {
                    CustominfoSection.transform.localPosition = new Vector3(-38.7277f, -236.8675f, 0);
                    
                }
                else
                {
                    CustominfoSection.transform.localPosition = new Vector3(-38.7277f, -411.6458f, 0);
                }


                if (banner.isReady)
                {
                    CustominfoSection.SetActive(false);
                    //Debug.Log("Player Ref: " + characterPanel.currentSelector + " - Banner is READY");
                    return;
                }
                else
                {

                    if (!CustominfoSection.activeSelf && currentCustomSkinID != 0) // activeSelf returns true if the GameObject is active
                    {
                        CustominfoSection.SetActive(true);
                    }
                }

                if (inputDeviceIndex == -1 || characterPanel.characterSelect.StartCoundown ||
                    !characterPanel.currentSelector.Active || !characterPanel.currentSelector.Enabled)
                {
                    return;
                }

                // Handle R trigger input for cycling skins forward
                if (!characterPanel.randomMode && inputManager.GetUIInputDown(inputDeviceIndex, UIKey.TriggerR, "", false))
                {
                    Debug.Log($"R trigger from {characterPanel.gameObject.name}");
                    currentCustomSkinID = (currentCustomSkinID + 1) % customSkinsID.Count; // Cycle forward
                    SetCharacterSkin();
                }

                // Handle L trigger input for cycling skins backward
                if (!characterPanel.randomMode && inputManager.GetUIInputDown(inputDeviceIndex, UIKey.TriggerL, "", false))
                {
                    Debug.Log($"L trigger from {characterPanel.gameObject.name}");
                    currentCustomSkinID = (currentCustomSkinID - 1 + customSkinsID.Count) % customSkinsID.Count; // Cycle backward
                    SetCharacterSkin();
                }

                // Check if character or skin has changed, and update accordingly
                if (characterCodename != previousCharacterCodename || skinIndex1 != previousSkinIndex)
                {
                    UpdateData();
                }
            }

            public void SetCharacterSkin()
            {
                if (customSkinsID.Count > 1)
                {
                    uiManager.PlaySound(characterSelect.ChangeSkinSFX);
                }


                characterCodename = customSkinsID[currentCustomSkinID].characterCodename;
                customSkinID = customSkinsID[currentCustomSkinID].skinIntIndex;
                Debug.Log(playerRef);
                Debug.Log(currentCustomSkinID);
                Debug.Log(customSkinsID[currentCustomSkinID].skinID);
                Debug.Log(customSkinID);

                if (customSkinsID[currentCustomSkinID].skinID == null || customSkinsID[currentCustomSkinID].authorName == null)
                {
                    CustominfoSection.SetActive(false);
                    return;
                }


                mainTitleSkin.text = customSkinsID[currentCustomSkinID].skinName.ToString();
                authorTitle.text = customSkinsID[currentCustomSkinID].authorName;
                skinImage.sprite = customSkinsID[currentCustomSkinID].stockImageSprite;
                if (dataManager.Online)
                {
                    Player currentPlayer = Plugin.onlineManager.CurrentPlayer;
                    Plugin.onlineManager.Properties.SetPlayerRoomProperty(currentPlayer, Plugin.CUSTOM_SKIN_ID, this.customSkinID);
                    Plugin.onlineManager.Properties.SetPlayerRoomProperty(currentPlayer, Plugin.CUSTOM_SKIN_NAME, uiManager.MainUserNickname);
                }


                if (customSkinID == 0)
                {
                    CustominfoSection.SetActive(false);
                }
                else
                {
                    CustominfoSection.SetActive(true);
                }
            }

            public void AddMetaData()
            {
                Debug.Log($"AddMetaData from Player index: {characterPanel.selectorNumber} - Codename: {characterPanel.currentCharacter}" );
                //Player currentPlayer1 = Plugin.onlineManager.CurrentPlayer;
                //Plugin.onlineManager.Properties.SetPlayerRoomProperty(currentPlayer1, Plugin.CUSTOM_SKIN_ID, 0);

                bool flag = this.customSkinID == 0;
                if (!flag)
                {
                    int new2 = 0;
                    //Player currentPlayer1 = Plugin.onlineManager.CurrentPlayer;
                    //Plugin.onlineManager.Properties.SetPlayerRoomProperty(currentPlayer1, Plugin.CUSTOM_SKIN_ID, new2);

                    foreach (Plugin.CharacterMetaData characterMetaData in Plugin.metaDataList)
                    {
                        bool flag2 = characterMetaData.playerRef == this.characterPanel.selectorNumber;
                        if (flag2)
                        {
                            return;
                        }
                    }
                    bool flag3 = !Plugin.dataManager.Online;
                    if (flag3)
                    {
                        Debug.Log($"Adding MetaData of Player index: {characterPanel.selectorNumber} - Codename: {characterPanel.currentCharacter} - CustomSkinID: {customSkinID}");
                        Plugin.CharacterMetaData characterMetaData2 = new Plugin.CharacterMetaData
                        {
                            skinIndex = this.characterPanel.currentSkin,
                            characterCodename = this.characterPanel.currentCharacter,
                            playerRef = this.characterPanel.selectorNumber,
                            customSkinID = this.customSkinID
                        };
                        Plugin.metaDataList.Add(characterMetaData2);
                    }
                    else
                    {
                        //Player currentPlayer = Plugin.onlineManager.CurrentPlayer;
                        //Plugin.onlineManager.Properties.SetPlayerRoomProperty(currentPlayer, Plugin.CUSTOM_SKIN_ID, this.customSkinID);
                    }
                }
            }

        }



        #region directories
        public List<CharacterMetaData> characterMetaDatas = new List<CharacterMetaData>();

        public static Dictionary<string, Dictionary<int, Dictionary<string, List<string>>>> characterCodenames = new Dictionary<string, Dictionary<int, Dictionary<string, List<string>>>>
        {
            { "SpongeBob", SpongeBob.SpongeBobAltParts},
            { "Patrick", PatrickDictionaries.PatrickAltParts},
            { "Squidward", SquidwarDictionaries.SquidwardAltParts},
            { "MechaPlankton", MechaPlanktonDictionary.MechaPlanktonAltParts},
            { "ElTigre",ElTigre.ElTigreAltParts},

            { "Rocko", Rocko.RockoAltParts},
            { "Jimmy", JimmyDictionaries.JimmyAltParts},
            { "Lucy", Lucy.LucyAltParts},
            //beavers
            { "Garfield", Garfield.GarfieldAltParts},

            { "Aang", Aaang.AangAltParts},
            { "Korra", Korra.KorraAltParts},
            { "Azula" , Azula.AzulaAltParts},
            { "Raphael", Raphael.RaphaelAltParts},
            { "Donatello", Donatello.DonatelloAltParts},

            { "April", AprilDictionaries.AprilAltParts},
            { "Danny", DannyDictionaries.DannyAltParts},
            { "Ember", EmberDictionary.EmberAltParts},
            { "GrandmaGertie", Gertie.GrandmaGertieAltParts},
            { "Gerald", Gerald.GeraldAltParts},

            { "Nigel", NIgel.NigelAltParts},
            { "Zim" , Zim.ZimAltParts},
            { "Jenny", JennyDictionary.JennyAltParts},
            { "Reptar", Reptar.ReptarAltParts},
            { "RenStimpy", RenStimpy.RenStimpyParts},

            { "Stevia", ZukoDictinoary.SteviaAltParts},
            { "Sushi", Sushi.SushiAltParts},
            { "Teacher", Teacher.TeacherAltParts},
            { "Headbanger", Headbanger.HeadbangerAltParts},
        };
        #endregion

        #region packageJSON
        public class PackageJSON
        {
            public int version;
            public string characterID;
            public int characterSkinIndex;
            public string skinID;
            public string skinName;
            public string authorName;
            public string assetBundlePath;
            public string VSRenderPath;
            public string StockIconPath;
            public List<MaterialOverrideGroup> materialOverrideGroups;

            [System.Serializable]
            public class MaterialOverrideGroup
            {
                public string identifier;
                public string matIndex;
                public List<TextureOverrideTarget> Targets;
                public List<TextureOverride> textureOverrides;
                public List<AttributeOverride> attributeOverrides;
                public List<Vector4AttributeOverride> vectorAttributeOverrides;
                public List<ColorOverride> colorOverrides;
            }

            [System.Serializable]
            public class TextureOverrideTarget
            {
                public string targetMesh;
                public string targetName;  // You may adjust this based on your game objects
                public int materialIndex;
            }

            [System.Serializable]
            public class TextureOverride
            {
                public string textureID;
                public string textureRef;
            }

            [System.Serializable]
            public class AttributeOverride
            {
                public int attributeType;
                public string attributeID;
                public float attributeValue;
            }

            [System.Serializable]
            public class Vector4AttributeOverride
            {

                public string attributeID;
                public float[] attributeValue;
            }

            [System.Serializable]
            public class ColorOverride
            {
                public string colorID;
                public string colorValue; // In hex format
            }
        }
        #endregion
        public class CustomSkinData
        {
            public string skinID;
            public CharacterCodename characterCodename;
            public int skinIndex;
            public int skinIntIndex;
            public string skinName;
            public string authorName;
            public Texture2D VSRender;
            public Texture2D stockImage;
            public Sprite stockImageSprite;
            public List<CharacterMaterialOverridesHandler.MaterialOverrideGroup> CustomMOGList;
            //public List
            public Dictionary<string, Mesh> customMeshesToReplace;

        }

        public static List<CustomSkinData> customSkinDatas = new List<CustomSkinData>();

        public void Awake()
        {
            Plugin.Log = base.Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
            base.Logger.LogInfo("Mod loaded!");
            if (!Directory.Exists(Plugin.skinsPath))
            {
                Debug.LogWarning("Skins folder not found: " + Plugin.skinsPath);
                try
                {
                    Plugin.Log.LogWarning("Creating Skins folder in Main Path...");
                    Directory.CreateDirectory(Plugin.skinsPath);
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError(ex);
                }
            }
            LoadAllCheapskins();

        }
        void Update()
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                foreach (var entry in metaDataList)
                {
                    Debug.Log("Found");
                    Debug.Log("CustomSkinID: " + entry.customSkinID + " Skin index: " + entry.skinIndex + " Character Codename: " + entry.characterCodename + " PlayweRef: " + entry.playerRef);
                }
            }
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                if (dataManager.Online)
                {
                    foreach (var player in onlineManager.GetPlayersList())
                    {
                        var propertyValue = onlineManager.Properties.GetPlayerRoomProperty(player, CUSTOM_SKIN_ID);
                        if (propertyValue == null)
                        {
                            return;
                        }

                        int new1 = (int)propertyValue;


                        Debug.Log($"Player Index is: {player.ActorNumber - 1} - Current CustomSkinID: {new1}");
                    }
                    
                    //Plugin.onlineManager.Properties.SetPlayerRoomProperty(currentPlayer, Plugin.CUSTOM_SKIN_ID, this.customSkinID);


                }

            }


        }
        void LoadAllCheapskins()
        {
            // Find all files with the ".cheapskin" extension in the directory
            string[] cheapskinFiles = Directory.GetFiles(skinsPath, "*.cheapskin");

            foreach (var cheapskinFile in cheapskinFiles)
            {
                // Process each .cheapskin file
                LoadCheapskin(cheapskinFile);
            }
        }
        void LoadCheapskin(string cheapskinFile)
        {
            try
            {
                using (FileStream zipStream = new FileStream(cheapskinFile, FileMode.Open))
                using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    // Find the "package" entry in the zip (assuming it's the package.json file)
                    var packageEntry = archive.GetEntry("package.json");

                    if (packageEntry != null)
                    {
                        using (StreamReader reader = new StreamReader(packageEntry.Open()))
                        {
                            // Read the JSON content
                            string json = reader.ReadToEnd();

                            // Deserialize the JSON into the PackageJSON class
                            PackageJSON package = JsonConvert.DeserializeObject<PackageJSON>(json, (JsonSerializerSettings?)null);

                            Debug.Log($"Loaded package: {package.characterID}, asset bundle path: {package.assetBundlePath}");

                            var assetbundleEntry = archive.GetEntry(package.assetBundlePath);
                            if (assetbundleEntry == null)
                            {
                                Debug.LogError($"AssetBundle '{package.assetBundlePath}' not found in cheapskin.");
                                return;
                            }

                            byte[] assetBundleData;
                            using (var entryStream = assetbundleEntry.Open())
                            using (var memoryStream = new MemoryStream())
                            {
                                entryStream.CopyTo(memoryStream);
                                assetBundleData = memoryStream.ToArray();
                            }

                            Debug.Log("Loading AssetBundle from memory...");
                            AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromMemoryAsync(assetBundleData);

                            bundleRequest.completed += (asyncOperation) =>
                            {
                                AssetBundle bundle = bundleRequest.assetBundle;
                                if (bundle == null)
                                {
                                    Debug.LogError("Failed to load AssetBundle from memory!");
                                    return;
                                }

                                Debug.Log("AssetBundle loaded successfully from memory.");

                                string[] assetNames = bundle.GetAllAssetNames();
                                Debug.Log("Assets in the AssetBundle:");
                                foreach (string assetName in assetNames)
                                {
                                    Debug.Log(assetName);
                                }

                                Debug.Log($"Loaded characterID: {package.characterID}, skinID: {package.skinID} from {Path.GetFileName(cheapskinFile)}");

                                if (package.materialOverrideGroups != null)
                                {
                                    CustomSkinData customSkinData = new CustomSkinData();
                                    customSkinData.skinID = package.skinID;
                                    customSkinData.skinName = package.skinName;
                                    // Try to parse the string into the CharacterCodename enum
                                    if (Enum.TryParse(package.characterID, out CharacterCodename parsedCodename))
                                    {
                                        customSkinData.characterCodename = parsedCodename;
                                    }
                                    else
                                    {
                                        Debug.LogError("Failed to parse characterID: " + package.characterID);
                                    }
                                    customSkinData.skinIndex = package.characterSkinIndex;
                                    customSkinData.CustomMOGList = new List<CharacterMaterialOverridesHandler.MaterialOverrideGroup>();
                                    using (SHA256 sha256Hash = SHA256.Create())
                                    {
                                        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(package.skinID.ToLower()));
                                        int hashValue = BitConverter.ToInt32(bytes, 0);
                                        customSkinData.skinIntIndex += hashValue;
                                    }

                                    customSkinData.VSRender = Patches.getTexture2DFromBundle(bundle, package.VSRenderPath.ToLower());

                                    Texture2D stockSprite = Patches.getTexture2DFromBundle(bundle, package.StockIconPath.ToLower());
                                    Sprite sprite = ConvertTextureToSprite(stockSprite);
                                    customSkinData.stockImageSprite = sprite;


                                    customSkinData.stockImage = Patches.getTexture2DFromBundle(bundle, package.StockIconPath.ToLower());
                                    customSkinData.authorName = package.authorName;



                                    customSkinData.customMeshesToReplace = new Dictionary<string, Mesh>();



                                    Debug.Log(customSkinData.skinIntIndex);

                                    Debug.Log("Processing material override groups.");
                                    foreach (var MOGVar in package.materialOverrideGroups)
                                    {
                                        Debug.Log($"Processing material override group: {MOGVar.identifier}");

                                        CharacterMaterialOverridesHandler.MaterialOverrideGroup MOG = new CharacterMaterialOverridesHandler.MaterialOverrideGroup
                                        {
                                            Identifier = $"{MOGVar.matIndex}:{MOGVar.identifier}",
                                            TextureOverrides = new List<CharacterMaterialOverridesHandler.TextureOverride>() // Initialize here
                                        };


                                        // TARGETS OVERRIDES
                                        

                                        // Loop through the targets
                                        Debug.Log("Processing Targets.");
                                        foreach (var MOGTargets in MOGVar.Targets)
                                        {
                                            Debug.Log($"Attempting to load GameObject from asset bundle for target: {MOGTargets.targetName}");

                                            if (string.IsNullOrWhiteSpace(MOGTargets.targetName))
                                            {
                                                Debug.LogWarning("IS NULL OR WHITE SPACE");

                                                Mesh nullMesh = new Mesh();
                                                customSkinData.customMeshesToReplace.Add(MOGTargets.targetMesh, nullMesh);
                                                continue;
                                            }

                                            GameObject fbx = bundle.LoadAsset<GameObject>(MOGTargets.targetName);

                                            if (fbx == null)
                                            {
                                                Debug.LogError($"Failed to load GameObject from path: {MOGTargets.targetName}");
                                                continue; // Continue to the next target
                                            }

                                            Debug.Log("GameObject found: " + fbx.name);

                                            // Find the SkinnedMeshRenderer in the FBX
                                            SkinnedMeshRenderer skinnedMesh = FindSkinnedMeshRendererInChildren(fbx);
                                            if (skinnedMesh != null)
                                            {
                                                Debug.Log("Found SkinnedMeshRenderer in FBX: " + skinnedMesh.name);
                                                Mesh mesh = skinnedMesh.sharedMesh;

                                                if (mesh != null)
                                                {
                                                    Debug.Log($"Adding SkinnedMeshRenderer mesh to dictionary: Target Mesh = {MOGTargets.targetMesh}, Mesh = {mesh.name}");
                                                    customSkinData.customMeshesToReplace.Add(MOGTargets.targetMesh, mesh);
                                                }
                                                else
                                                {
                                                    Debug.LogWarning($"SkinnedMeshRenderer found, but no mesh assigned: {skinnedMesh.name}");
                                                }
                                            }
                                            else
                                            {
                                                Debug.LogWarning("No SkinnedMeshRenderer found in FBX, trying to find MeshRenderer....");

                                                MeshRenderer meshRenderer = FindMeshRendererInChildren(fbx);
                                                if (meshRenderer != null)
                                                {
                                                    Debug.Log("Found MeshRenderer in FBX " + meshRenderer.name);

                                                    MeshFilter meshFilter = meshRenderer.gameObject.GetComponent<MeshFilter>();
                                                    if (meshFilter != null && meshFilter.mesh != null)
                                                    {
                                                        Mesh mesh = meshFilter.mesh;
                                                        Debug.Log($"Adding MeshRenderer mesh to dictionary: Target Mesh = {MOGTargets.targetMesh}, Mesh = {mesh.name}");
                                                        customSkinData.customMeshesToReplace.Add(MOGTargets.targetMesh, mesh);
                                                    }
                                                    else
                                                    {
                                                        Debug.LogWarning($"MeshRenderer found, but no mesh or MeshFilter: {meshRenderer.name}");
                                                    }
                                                }
                                            }
                                        }

                                        // Log the dictionary contents after all meshes are loaded
                                        Debug.Log("Logging customMeshesToReplace dictionary contents after population:");
                                        foreach (var entry in customSkinData.customMeshesToReplace)
                                        {
                                            Debug.Log($"Key: {entry.Key}, Mesh name: {entry.Value.name}");
                                        }



                                        // ATTRIBUTE OVERRIDES
                                        if (MOGVar.attributeOverrides != null)
                                        {
                                            Debug.Log("Processing attribute overrides.");

                                            if (MOG.AttributeOverrides == null)
                                            {
                                                Debug.LogError("MOG.AttributeOverrides is null! Initializing it.");
                                                MOG.AttributeOverrides = new List<CharacterMaterialOverridesHandler.AttributeOverride>();
                                            }

                                            foreach (var MOGattributeOverrides in MOGVar.attributeOverrides)
                                            {
                                                if (MOGattributeOverrides == null)
                                                {
                                                    Debug.LogError("MOGattributeOverrides is null! Skipping this entry.");
                                                    continue;
                                                }

                                                Debug.Log($"Processing attribute with ID: {MOGattributeOverrides.attributeID}");

                                                CharacterMaterialOverridesHandler.AttributeOverride newAttribute = new CharacterMaterialOverridesHandler.AttributeOverride
                                                {
                                                    AttributeType = (CharacterMaterialOverridesHandler.AttributeOverride.AttributeNumberTypes)MOGattributeOverrides.attributeType, // Assuming Integer type for now
                                                    AttributeID = MOGattributeOverrides.attributeID,
                                                    AttributeValue = MOGattributeOverrides.attributeValue,
                                                };

                                                Debug.Log("Adding new attribute override.");

                                                MOG.AttributeOverrides.Add(newAttribute);
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogWarning("MOGVar.attributeOverrides is null.");
                                        }

                                        // TEXTURE OVERRIDES
                                        if (MOGVar.textureOverrides != null)
                                        {
                                            Debug.Log("Processing texture overrides.");
                                            foreach (var MOGtextureOverrides in MOGVar.textureOverrides)
                                            {
                                                Texture2D texture2D = bundle.LoadAsset<Texture2D>(MOGtextureOverrides.textureRef);
                                                if (texture2D == null)
                                                {
                                                    Debug.LogError($"Failed to load Texture2D from path: {MOGtextureOverrides.textureRef}");
                                                    continue; // Continue to the next texture
                                                }

                                                Debug.Log("Texture2D found: " + texture2D.name);

                                                CharacterMaterialOverridesHandler.TextureOverride newTexture = new CharacterMaterialOverridesHandler.TextureOverride
                                                {
                                                    TextureID = MOGtextureOverrides.textureID,
                                                    TextureRef = texture2D
                                                };

                                                MOG.TextureOverrides.Add(newTexture);

                                            }
                                        }

                                        // VECTOR ATTRIBUTE OVERRIDES
                                        if (MOGVar.vectorAttributeOverrides != null)
                                        {
                                            Debug.Log("Processing vector attribute overrides.");

                                            // Ensure MOG.VectorAttributeOverrides is initialized
                                            if (MOG.VectorAttributeOverrides == null)
                                            {
                                                Debug.LogError("MOG.VectorAttributeOverrides is null! Initializing it.");
                                                MOG.VectorAttributeOverrides = new List<CharacterMaterialOverridesHandler.Vector4AttributeOverride>();
                                            }

                                            foreach (var MOGattributeOverrides in MOGVar.vectorAttributeOverrides)
                                            {
                                                if (MOGattributeOverrides == null)
                                                {
                                                    Debug.LogError("MOGattributeOverrides (vector) is null! Skipping this entry.");
                                                    continue;
                                                }

                                                Debug.Log($"Processing vector attribute with ID: {MOGattributeOverrides.attributeID}");

                                                Vector4 vector = FloatArrayToVector4(MOGattributeOverrides.attributeValue);

                                                CharacterMaterialOverridesHandler.Vector4AttributeOverride newVectorAttribute = new CharacterMaterialOverridesHandler.Vector4AttributeOverride
                                                {
                                                    AttributeID = MOGattributeOverrides.attributeID,
                                                    AttributeValue = vector,
                                                };

                                                Debug.Log("Adding new vector attribute override.");
                                                MOG.VectorAttributeOverrides.Add(newVectorAttribute);
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogWarning("MOGVar.vectorAttributeOverrides is null.");
                                        }

                                        // COLOR OVERRIDES
                                        if (MOGVar.colorOverrides != null)
                                        {
                                            Debug.Log("Processing color overrides.");

                                            // Ensure MOG.ColorOverrides is initialized
                                            if (MOG.ColorOverrides == null)
                                            {
                                                Debug.LogError("MOG.ColorOverrides is null! Initializing it.");
                                                MOG.ColorOverrides = new List<CharacterMaterialOverridesHandler.ColorOverride>();
                                            }

                                            foreach (var MOGtextureOverrides in MOGVar.colorOverrides)
                                            {
                                                if (MOGtextureOverrides == null)
                                                {
                                                    Debug.LogError("MOGtextureOverrides (color) is null! Skipping this entry.");
                                                    continue;
                                                }

                                                Debug.Log($"Processing color with ID: {MOGtextureOverrides.colorID}");

                                                Color color = HexToUnityColor(MOGtextureOverrides.colorValue);

                                                CharacterMaterialOverridesHandler.ColorOverride newColor = new CharacterMaterialOverridesHandler.ColorOverride
                                                {
                                                    ColorID = MOGtextureOverrides.colorID,
                                                    ColorValue = color
                                                };

                                                Debug.Log("Adding new color override.");
                                                MOG.ColorOverrides.Add(newColor);
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogWarning("MOGVar.colorOverrides is null.");
                                        }

                                        Debug.Log("Material override group processed.");

                                        customSkinData.CustomMOGList.Add(MOG);
                                    }


                                    Debug.Log("trying to add customskinData to customSkinDatas");

                                    if (customSkinData.customMeshesToReplace != null)
                                    {
                                        foreach (var entry in customSkinData.customMeshesToReplace)
                                        {
                                            Debug.LogWarning($"Mesh entry found: {entry.Key} -> {entry.Value.name}");
                                        }
                                    }
                                    Plugin.customSkinDatas.Add(customSkinData);
                                }
                                else
                                {
                                    Debug.LogWarning("No material override groups found.");
                                }

                                bundle.Unload(false);
                            };

                            // Log the characterID and skinID for testing
                            
                        }


                    }
                    else
                    {
                        Debug.LogWarning($"No 'package' entry found in {cheapskinFile}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load {cheapskinFile}: {ex.Message}");
            }
        }
        private async Task<AssetBundle> LoadAssetBundleFromStreamAsync(Stream assetBundleStream)
        {
            // Use LoadFromStreamAsync to load the AssetBundle
            AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromStreamAsync(assetBundleStream);

            // Await the completion of the bundle loading
            await Task.Run(() => {
                while (!bundleRequest.isDone) { }  // Wait for the bundle to finish loading
            });

            AssetBundle bundle = bundleRequest.assetBundle;
            if (bundle == null)
            {
                Debug.LogError("Failed to load AssetBundle from stream!");
            }

            return bundle;
        }
        public Sprite ConvertTextureToSprite(Texture2D texture)
        {
            // Define the sprite's rect and pivot point (center of the texture)
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f); // Center of the texture

            // Create the sprite from the texture
            Sprite newSprite = Sprite.Create(texture, rect, pivot);
            return newSprite;
        }
        SkinnedMeshRenderer FindSkinnedMeshRendererInChildren(GameObject parentObject)
        {
            // Check if the parent itself has the SkinnedMeshRenderer
            SkinnedMeshRenderer skinnedMeshRenderer = parentObject.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                return skinnedMeshRenderer;
            }

            // Recursively search through all the children
            foreach (Transform child in parentObject.transform)
            {
                skinnedMeshRenderer = FindSkinnedMeshRendererInChildren(child.gameObject);
                if (skinnedMeshRenderer != null)
                {
                    return skinnedMeshRenderer;
                }
            }

            // If no SkinnedMeshRenderer is found
            return null;
        }
        MeshRenderer FindMeshRendererInChildren(GameObject parentObject)
        {
            // Check if the parent itself has a MeshRenderer
            MeshRenderer meshRenderer = parentObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                return meshRenderer;
            }

            // Recursively search through all the children
            foreach (Transform child in parentObject.transform)
            {
                meshRenderer = FindMeshRendererInChildren(child.gameObject);
                if (meshRenderer != null)
                {
                    return meshRenderer;
                }
            }

            // If no MeshRenderer is found, return null
            return null;
        }
        Color HexToUnityColor(string hex)
        {
            // Ensure the hex string has 6 characters (RRGGBB)
            if (hex.Length != 6)
            {
                throw new ArgumentException($"Invalid hex color: {hex}. Must be 6 digits (RRGGBB).");
            }

            // Parse the hex string to get the RGB values
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            // Return the color with values normalized between 0 and 1
            return new Color(r / 255f, g / 255f, b / 255f);
        }
        Vector4 FloatArrayToVector4(float[] array)
        {
            // Make sure the array has exactly 4 elements
            if (array.Length != 4)
            {
                Debug.LogError("Array must have exactly 4 elements to convert to Vector4.");
                return Vector4.zero;
            }

            // Return a new Vector4 with the array values
            return new Vector4(array[0], array[1], array[2], array[3]);
        }

        [HarmonyPatch(typeof(MainMenu), "LoadingFinished")]
        public class Patches
        {
            [HarmonyPostfix]
            [HarmonyPatch(typeof(CharacterManager), "OnInstantiated")]
            public static void OnInstantiated(CharacterManager __instance, QuantumGame game)
            {
                try
                {
                    if (dataManager.Online)
                    {
                        Debug.LogWarning($"Character {__instance.Codename} : GAME IS ONLINE!");
                        foreach (var player in onlineManager.GetPlayersList())
                        {

                            //Debug.Log("Actor number is : " + (player.ActorNumber - 1) + " - UserID: " + player.UserId + " - NickName: " + player.NickName);
                            if (__instance.Data.Character.playerNumber == player.ActorNumber - 1)
                            {
                                var propertyValue = onlineManager.Properties.GetPlayerRoomProperty(player, CUSTOM_SKIN_ID);
                                if (propertyValue == null)
                                {
                                    return;
                                }

                                int new1 = (int)propertyValue;
                                if (new1 == 0)
                                {
                                    return;
                                }

                                foreach (CustomSkinData skinData in Plugin.customSkinDatas)
                                {
                                    if (skinData.skinIntIndex == new1)
                                    {
                                        if(skinData.skinIndex != __instance.Data.Character.skinIndex)
                                        {
                                            return;
                                        }

                                        //Debug.Log("CustomSkin Found - PlayerRef: " + __instance.Data.CharacterIndex + " CustomSkinInt: " + new1);

                                        CharacterMaterialOverridesHandler characterMaterialOverridesHandler = GetCharacterMaterialOverridesHandler(__instance);
                                        if (characterMaterialOverridesHandler == null)
                                        {
                                            //Debug.LogError("CharacterMaterialOverridesHandler is null!");
                                            return;
                                        }

                                        foreach (CharacterMaterialOverridesHandler.MaterialOverrideGroup originalMOGroup in skinData.CustomMOGList)
                                        {
                                            // Clone the MOGroup using the helper method
                                            var MOGroup = CloneMOGroup(originalMOGroup);

                                            foreach (var materialObject in __instance.CharacterRenderer.Objects)
                                            {
                                                if (materialObject.ObjectRenderer == null)
                                                {
                                                    //Debug.LogWarning("MaterialObject's ObjectRenderer is null!");
                                                    continue;
                                                }

                                                string originalIdentifier = MOGroup.Identifier;

                                                // Check if the identifier contains a colon and remove the prefix if it's there
                                                int colonIndex = originalIdentifier.IndexOf(':');
                                                if (colonIndex != -1)
                                                {
                                                    // Remove everything before and including the colon
                                                    originalIdentifier = originalIdentifier.Substring(colonIndex + 1);
                                                }

                                                if (characterCodenames.TryGetValue(skinData.characterCodename.ToString(), out var altParts) &&
                                                    altParts.TryGetValue(skinData.skinIndex, out var parts) &&
                                                    parts.TryGetValue(originalIdentifier.ToString(), out var partList))
                                                {
                                                    if (partList.Any(part => materialObject.ObjectRenderer.name == part))
                                                    {
                                                        //Debug.Log($"Renderer '{materialObject.ObjectRenderer.name}' matches a part of {skinData.characterCodename}.");

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

                                            characterMaterialOverridesHandler.TextureOverrides.Add(MOGroup);
                                            characterMaterialOverridesHandler.ApplyMaterialOverride(MOGroup.Identifier);
                                        }

                                        foreach (var textureOverride in characterMaterialOverridesHandler.TextureOverrides)
                                        {
                                            foreach (var targetOverride in textureOverride.Targets)
                                            {
                                                //Debug.Log($"Processing Target: {targetOverride.Target.name}");

                                                if (targetOverride.MaterialIndex != 0)
                                                {
                                                    //Debug.Log("Skipping due to MaterialIndex being non-zero.");
                                                    continue;
                                                }

                                                if (targetOverride.Target is SkinnedMeshRenderer skinnedMeshRenderer)
                                                {
                                                    //Debug.Log($"Processing SkinnedMeshRenderer: {skinnedMeshRenderer.name}");
                                                    if (skinData.customMeshesToReplace.TryGetValue(skinnedMeshRenderer.name, out Mesh replacementMesh))
                                                    {
                                                        //Debug.Log($"SkinnedMeshRenderer name matched and replaced: {skinnedMeshRenderer.name} with mesh: {replacementMesh.name}");
                                                        skinnedMeshRenderer.sharedMesh = replacementMesh;
                                                    }
                                                    else
                                                    {
                                                        //Debug.LogWarning($"No replacement mesh found for SkinnedMeshRenderer: {skinnedMeshRenderer.name}. Available keys in customMeshesToReplace:");
                                                        foreach (var key in skinData.customMeshesToReplace.Keys)
                                                        {
                                                            Debug.Log($"Available key: {key}");
                                                        }
                                                    }
                                                }
                                                else if (targetOverride.Target is MeshRenderer meshRenderer)
                                                {
                                                    // Debug.Log($"Processing MeshRenderer: {meshRenderer.name}");

                                                    // Log all entries in the customMeshesToReplace dictionary before the check
                                                    Debug.Log("Iterating through customMeshesToReplace dictionary:");
                                                    foreach (var entry in skinData.customMeshesToReplace)
                                                    {
                                                        //Debug.Log($"Key (Target Mesh Name): {entry.Key}, Mesh Name: {entry.Value.name}");
                                                    }

                                                    // Try to find a matching mesh for the current MeshRenderer
                                                    if (skinData.customMeshesToReplace.TryGetValue(meshRenderer.name, out Mesh replacementMesh))
                                                    {
                                                        //Debug.Log($"MeshRenderer name matched and replaced: {meshRenderer.name} with mesh: {replacementMesh.name}");
                                                        MeshFilter meshFilter = meshRenderer.gameObject.GetComponent<MeshFilter>();

                                                        if (meshFilter != null)
                                                        {
                                                            meshFilter.sharedMesh = replacementMesh;
                                                            //Debug.Log("Replacement successful.");
                                                        }
                                                        else
                                                        {
                                                            //Debug.LogError($"MeshFilter missing on MeshRenderer: {meshRenderer.name}. Cannot apply replacement.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Debug.LogWarning($"No replacement mesh found for MeshRenderer: {meshRenderer.name}. Available keys in customMeshesToReplace:");
                                                        foreach (var key in skinData.customMeshesToReplace.Keys)
                                                        {
                                                            //Debug.Log($"Available key: {key}");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Debug.LogWarning($"Character {__instance.Codename} : GAME IS OFFLINE!");
                        foreach (CharacterMetaData MData in metaDataList)
                        {
                            if (MData.playerRef == __instance.Data.Character.index)
                            {
                                //Debug.Log($"Current Character: {__instance.Codename} Match Found, MData Player Ref: {MData.playerRef} and Character.PlayerNumBER: {__instance.Data.Character.index}");

                                foreach (CustomSkinData skinData in Plugin.customSkinDatas)
                                {
                                    if (skinData.skinIntIndex == MData.customSkinID)
                                    {
                                        if (skinData.skinIndex != __instance.Data.Character.skinIndex)
                                        {
                                            return;
                                        }

                                        //Debug.Log("CustomSkin Found - PlayerRef: " + __instance.Data.CharacterIndex + " CustomSkinInt: " + MData.customSkinID + " CustomPlayerRef: " + MData.playerRef);

                                        CharacterMaterialOverridesHandler characterMaterialOverridesHandler = GetCharacterMaterialOverridesHandler(__instance);
                                        if (characterMaterialOverridesHandler == null)
                                        {
                                            //Debug.LogError("CharacterMaterialOverridesHandler is null!");
                                            return;
                                        }

                                        foreach (CharacterMaterialOverridesHandler.MaterialOverrideGroup originalMOGroup in skinData.CustomMOGList)
                                        {
                                            // Clone the MOGroup using the helper method
                                            var MOGroup = CloneMOGroup(originalMOGroup);

                                            foreach (var materialObject in __instance.CharacterRenderer.Objects)
                                            {
                                                if (materialObject.ObjectRenderer == null)
                                                {
                                                    //Debug.LogWarning("MaterialObject's ObjectRenderer is null!");
                                                    continue;
                                                }


                                                string originalIdentifier = MOGroup.Identifier;

                                                // Check if the identifier contains a colon and remove the prefix if it's there
                                                int colonIndex = originalIdentifier.IndexOf(':');
                                                if (colonIndex != -1)
                                                {
                                                    // Remove everything before and including the colon
                                                    originalIdentifier = originalIdentifier.Substring(colonIndex + 1);
                                                }

                                                if (characterCodenames.TryGetValue(skinData.characterCodename.ToString(), out var altParts) &&
                                                    altParts.TryGetValue(skinData.skinIndex, out var parts) &&
                                                    parts.TryGetValue(originalIdentifier.ToString(), out var partList))
                                                {
                                                    if (partList.Any(part => materialObject.ObjectRenderer.name == part))
                                                    {
                                                        //Debug.Log($"Renderer '{materialObject.ObjectRenderer.name}' matches a part of {skinData.characterCodename}.");

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

                                            characterMaterialOverridesHandler.TextureOverrides.Add(MOGroup);
                                            characterMaterialOverridesHandler.ApplyMaterialOverride(MOGroup.Identifier);
                                        }

                                        //Debug.Log("Processing characterMaterialOverridesHandler.");
                                        foreach (var textureOverride in characterMaterialOverridesHandler.TextureOverrides)
                                        {
                                            foreach (var targetOverride in textureOverride.Targets)
                                            {
                                                //Debug.Log($"Processing Target: {targetOverride.Target.name}");

                                                if (targetOverride.MaterialIndex != 0)
                                                {
                                                    //Debug.Log("Skipping due to MaterialIndex being non-zero.");
                                                    continue;
                                                }

                                                if (targetOverride.Target is SkinnedMeshRenderer skinnedMeshRenderer)
                                                {
                                                    //Debug.Log($"Processing SkinnedMeshRenderer: {skinnedMeshRenderer.name}");
                                                    if (skinData.customMeshesToReplace.TryGetValue(skinnedMeshRenderer.name, out Mesh replacementMesh))
                                                    {
                                                        //Debug.Log($"SkinnedMeshRenderer name matched and replaced: {skinnedMeshRenderer.name} with mesh: {replacementMesh.name}");

                                                        //if(replacementMesh)

                                                        skinnedMeshRenderer.sharedMesh = replacementMesh;

                                                    }
                                                    else
                                                    {
                                                        //Debug.LogWarning($"No replacement mesh found for SkinnedMeshRenderer: {skinnedMeshRenderer.name}. Available keys in customMeshesToReplace:");
                                                        foreach (var key in skinData.customMeshesToReplace.Keys)
                                                        {
                                                            Debug.Log($"Available key: {key}");
                                                        }
                                                    }
                                                }
                                                else if (targetOverride.Target is MeshRenderer meshRenderer)
                                                {
                                                    // Debug.Log($"Processing MeshRenderer: {meshRenderer.name}");

                                                    // Log all entries in the customMeshesToReplace dictionary before the check
                                                    Debug.Log("Iterating through customMeshesToReplace dictionary:");
                                                    foreach (var entry in skinData.customMeshesToReplace)
                                                    {
                                                        //Debug.Log($"Key (Target Mesh Name): {entry.Key}, Mesh Name: {entry.Value.name}");
                                                    }

                                                    // Try to find a matching mesh for the current MeshRenderer
                                                    if (skinData.customMeshesToReplace.TryGetValue(meshRenderer.name, out Mesh replacementMesh))
                                                    {
                                                        //Debug.Log($"MeshRenderer name matched and replaced: {meshRenderer.name} with mesh: {replacementMesh.name}");
                                                        MeshFilter meshFilter = meshRenderer.gameObject.GetComponent<MeshFilter>();

                                                        if (meshFilter != null)
                                                        {
                                                            meshFilter.sharedMesh = replacementMesh;
                                                            //Debug.Log("Replacement successful.");
                                                        }
                                                        else
                                                        {
                                                            //Debug.LogError($"MeshFilter missing on MeshRenderer: {meshRenderer.name}. Cannot apply replacement.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Debug.LogWarning($"No replacement mesh found for MeshRenderer: {meshRenderer.name}. Available keys in customMeshesToReplace:");
                                                        foreach (var key in skinData.customMeshesToReplace.Keys)
                                                        {
                                                            //Debug.Log($"Available key: {key}");
                                                        }
                                                    }
                                                }
                                                
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Debug.Log($"No match found for Player : {__instance.Data.CharacterIndex} - Current MetaData Player Ref: {MData.playerRef}");
                            }
                        }

                    }
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
                
            }
            private static CharacterMaterialOverridesHandler.MaterialOverrideGroup CloneMOGroup(CharacterMaterialOverridesHandler.MaterialOverrideGroup original)
            {
                // Create a new instance of MaterialOverrideGroup
                var clone = new CharacterMaterialOverridesHandler.MaterialOverrideGroup
                {
                    Identifier = original.Identifier,
                    Targets = new List<CharacterMaterialOverridesHandler.TextureOverrideTarget>(),
                    TextureOverrides = new List<CharacterMaterialOverridesHandler.TextureOverride>(),
                    AttributeOverrides = new List<CharacterMaterialOverridesHandler.AttributeOverride>(),
                    VectorAttributeOverrides = new List<CharacterMaterialOverridesHandler.Vector4AttributeOverride>(),
                    ColorOverrides = new List<CharacterMaterialOverridesHandler.ColorOverride>()
                };

                // Clone Targets
                if (original.Targets != null)
                {
                    foreach (var target in original.Targets)
                    {
                        clone.Targets.Add(new CharacterMaterialOverridesHandler.TextureOverrideTarget
                        {
                            
                            Target = target.Target, // Handle appropriately if you need a deep copy
                            MaterialIndex = target.MaterialIndex
                        });
                    }
                }

                // Clone TextureOverrides
                if (original.TextureOverrides != null)
                {
                    foreach (var textureOverride in original.TextureOverrides)
                    {
                        clone.TextureOverrides.Add(new CharacterMaterialOverridesHandler.TextureOverride
                        {

                            TextureID = textureOverride.TextureID,
                            TextureRef = textureOverride.TextureRef,
                        });

                    }
                }

                // Clone AttributeOverrides
                if (original.AttributeOverrides != null)
                {
                    foreach (var attributeOverride in original.AttributeOverrides)
                    {
                        clone.AttributeOverrides.Add(new CharacterMaterialOverridesHandler.AttributeOverride
                        {
                            AttributeID = attributeOverride.AttributeID,
                            AttributeType = attributeOverride.AttributeType,
                            AttributeValue = attributeOverride.AttributeValue,
                        });

                    }
                }

                // Clone VectorAttributeOverrides
                if (original.VectorAttributeOverrides != null)
                {
                    foreach (var vectorOverride in original.VectorAttributeOverrides)
                    {
                        clone.VectorAttributeOverrides.Add(new CharacterMaterialOverridesHandler.Vector4AttributeOverride
                        {
                            AttributeID = vectorOverride.AttributeID,
                            AttributeValue = vectorOverride.AttributeValue,
                        });

                    }
                }

                // Clone ColorOverrides
                if (original.ColorOverrides != null)
                {
                    foreach (var colorOverride in original.ColorOverrides)
                    {
                        clone.ColorOverrides.Add(new CharacterMaterialOverridesHandler.ColorOverride
                        {
                            ColorID = colorOverride.ColorID,    
                            ColorValue = colorOverride.ColorValue,
                        });

                    }
                }

                return clone;
            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(CharacterMaterialOverridesHandler), "ApplyMaterialOverride")]
            public static bool ApplyMaterialOverride(CharacterMaterialOverridesHandler __instance, string materialOverrideID, ref bool __result)
            {
                //Plugin.Log.LogInfo("Apply");

                if(materialOverrideID == "Shield_Default" || materialOverrideID == "Shield_PerfectBlock")
                {
                    return true;
                }

                if (__instance.TextureOverrides == null)
                {
                    //Debug.LogError("TextureOverrides list is null!");
                    __result = false;
                    return false;
                }

                // Find the matching MaterialOverrideGroup
                var materialOverrideGroup = __instance.TextureOverrides.FirstOrDefault(group => group.Identifier == materialOverrideID);
                if (materialOverrideGroup == null)
                {
                    //Debug.LogError("No material override found with the ID: " + materialOverrideID);
                    __result = false;
                    return false;
                }

                if (materialOverrideGroup.Targets == null)
                {
                    //Debug.LogError("Targets in MaterialOverrideGroup are null!");
                    __result = false;
                    return false;
                }

                foreach (var textureOverrideTarget in materialOverrideGroup.Targets)
                {
                    if (textureOverrideTarget.Target == null)
                    {
                        //Debug.LogError($"TextureOverrideTarget.Target is null! in: {materialOverrideID}, Target index: {materialOverrideGroup.Targets.IndexOf(textureOverrideTarget)}");
                        //return false;
                        continue; // Skip this target
                    }

                    //Debug.Log($"Applying material override for target: {textureOverrideTarget.Target.name}, Material Index: {textureOverrideTarget.MaterialIndex}");

                    textureOverrideTarget.Target.GetPropertyBlock(__instance.mpb, textureOverrideTarget.MaterialIndex);

                    if (materialOverrideGroup.TextureOverrides != null)
                    {
                        foreach (var textureOverride in materialOverrideGroup.TextureOverrides)
                        {
                            __instance.mpb.SetTexture(textureOverride.TextureID, textureOverride.TextureRef);
                        }
                    }

                    if (materialOverrideGroup.AttributeOverrides != null)
                    {
                        foreach (var attributeOverride in materialOverrideGroup.AttributeOverrides)
                        {
                            if (attributeOverride.AttributeType == CharacterMaterialOverridesHandler.AttributeOverride.AttributeNumberTypes.Integer)
                            {
                                __instance.mpb.SetInteger(attributeOverride.AttributeID, (int)attributeOverride.AttributeValue);
                            }
                            else if (attributeOverride.AttributeType == CharacterMaterialOverridesHandler.AttributeOverride.AttributeNumberTypes.Float)
                            {
                                __instance.mpb.SetFloat(attributeOverride.AttributeID, attributeOverride.AttributeValue);
                            }
                        }
                    }

                    if (materialOverrideGroup.VectorAttributeOverrides != null)
                    {
                        foreach (var vector4AttributeOverride in materialOverrideGroup.VectorAttributeOverrides)
                        {
                            __instance.mpb.SetVector(vector4AttributeOverride.AttributeID, vector4AttributeOverride.AttributeValue);
                        }
                    }

                    if (materialOverrideGroup.ColorOverrides != null)
                    {
                        foreach (var colorOverride in materialOverrideGroup.ColorOverrides)
                        {
                            __instance.mpb.SetColor(colorOverride.ColorID, colorOverride.ColorValue);
                        }
                    }

                    textureOverrideTarget.Target.SetPropertyBlock(__instance.mpb, textureOverrideTarget.MaterialIndex);
                }

                __result = true;
                return false;
            }

            public static CharacterMaterialOverridesHandler GetCharacterMaterialOverridesHandler(CharacterManager characterManager)
            {
                CharacterMaterialOverridesHandler characterMaterialOverride;

                GameObject parentObject = characterManager.gameObject.transform.parent.gameObject;
                Debug.Log("Parent GameObject: " + parentObject.name);

                Transform childTransform = parentObject.transform.Find("GeneralMaterialOverrides");
                if(childTransform != null)
                {
                    GameObject childObject = childTransform.gameObject;
                    Debug.Log("GeneralMaterialOverrides GameObject found: " + childObject.name);

                    characterMaterialOverride = childObject.GetComponent<CharacterMaterialOverridesHandler>();
                    if (characterMaterialOverride == null)
                    {
                        Debug.LogWarning("CharacterMaterialOverridesHandler not found on GeneralMaterialOverrides!");
                        return null;
                    }
                    else
                    {
                        return characterMaterialOverride;
                    }
                }
                else
                {
                    Debug.Log("cannot find GeneralMaterialOverrides");
                }

                return null;
            }

            public static Texture2D getTexture2D(string path)
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                Texture2D texture2D = new Texture2D(1, 1);
                using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("CheapSkinss.Resources.danny2.png"))
                {
                    byte[] array = new byte[manifestResourceStream.Length];
                    manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
                    ImageConversion.LoadImage(texture2D, array);
                    texture2D.wrapMode = TextureWrapMode.Clamp;
                }

                return texture2D;
            }

            public static Texture2D getTexture2DFromBundle(AssetBundle bundle, string path)
            {
                Texture2D texture2D = bundle.LoadAsset<Texture2D>(path);
                if (texture2D == null)
                {
                    Debug.LogError($"Failed to load Texture2D from path: {path}");
                    return null; // Continue to the next texture
                }
                return texture2D;
            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(CharacterMaterialOverridesHandler), "Awake")]
            public static bool Awake(CharacterMaterialOverridesHandler __instance)
            {
                //Plugin.Log.LogInfo("Awake");
                if (__instance.mpb == null)
                {
                    __instance.mpb = new MaterialPropertyBlock();
                }
                if (!string.IsNullOrEmpty(__instance.InitialOverride))
                {
                    __instance.ApplyMaterialOverride(__instance.InitialOverride);
                }

                return false;
            }

            private static List<CharacterPanel> characterPanels = new List<CharacterPanel>();
            [HarmonyPostfix]
            [HarmonyPatch(typeof(CharacterSelectReadyBanner), "ToggleMatchReady")]
            public static void ToggleMatchReady(CharacterSelectReadyBanner __instance, bool ready)
            {
                if (dataManager.Online)
                {
                    return;
                }

                if (ready)
                {

                    // Only cache the panels if the list is empty (first time) or reset between ready states
                    if (characterPanels.Count == 0)
                    {
                        CharacterPanel[] panelsArray = FindObjectsOfType<CharacterPanel>(true);
                        foreach (CharacterPanel panel in panelsArray)
                        {
                            if (panel.gameObject.activeInHierarchy)
                            {
                                Plugin.MetaData metaData = panel.GetComponent<Plugin.MetaData>();
                                if (metaData != null)
                                {
                                    characterPanels.Add(panel);  // Cache active panels with MetaData
                                }
                            }
                        }
                    }

                    // Iterate over the cached panels and call AddMetaData only if needed
                    foreach (CharacterPanel cachedPanel in characterPanels)
                    {
                        Plugin.MetaData component = cachedPanel.GetComponent<Plugin.MetaData>();
                        if (component != null)
                        {
                            component.AddMetaData();
                        }
                    }
                }
                else
                {
                    // Clear the cache and metadata list when ready is false
                    Plugin.metaDataList.Clear();
                    characterPanels.Clear();
                }
            }
            [HarmonyPostfix]
            [HarmonyPatch(typeof(CharacterPanel), "Init")]
            public static void Init(CharacterPanel __instance)
            {
                Debug.Log("Init happening");
                bool flag = __instance.gameObject.GetComponent<Plugin.MetaData>() == null;
                if (flag)
                {
                    bool flag2 = __instance.playerIndex == 0;
                    if (flag2)
                    {
                        Debug.Log("playerindex is 0");
                        __instance.gameObject.AddComponent<Plugin.MetaData>();
                    }
                }
            }
            [HarmonyPrefix]
            [HarmonyPatch(typeof(MatchLoadingCharacter), "LoadSlotImage")]
            public static bool LoadSlotImage1(MatchLoadingCharacter __instance, bool forceDefault)
            {
                if (dataManager.Online)
                {
                    foreach (var player in onlineManager.GetPlayersList())
                    {
                        if(__instance.CharacterIndex == player.ActorNumber - 1)
                        {
                            Debug.Log(onlineManager.Properties.GetPlayerRoomProperty(player, CUSTOM_SKIN_ID));

                            var propertyValue = onlineManager.Properties.GetPlayerRoomProperty(player, CUSTOM_SKIN_ID);
                            if (propertyValue == null)
                            {
                                return true;
                            }

                            int new1 = (int)propertyValue;
                            if (new1 == 0)
                            {
                                return true;
                            }
                            

                            foreach (var skinData in customSkinDatas)
                            {
                                //Debug.Log($"Checking customSkinData: SkinIntIndex = {skinData.skinIntIndex}");

                                if (skinData.skinIntIndex == new1)
                                {
                                    if (skinData.skinIndex != __instance.skin)
                                    {
                                        return true;
                                    }

                                    //Debug.Log($"Matching customSkinID found: {entry.customSkinID}, skinData.VSRender = {skinData.VSRender}");

                                    Texture2D texture = skinData.VSRender;

                                    if (texture != null)
                                    {
                                        // Logging the texture details before converting to sprite
                                        //Debug.Log($"Texture2D found. Width: {texture.width}, Height: {texture.height}, name: {texture.name}");

                                        // Create the sprite from the texture
                                        Rect rect = new Rect(0, 0, texture.width, texture.height);
                                        Vector2 pivot = new Vector2(0.5f, 0.5f); // Center of the texture

                                        Sprite newSprite = Sprite.Create(texture, rect, pivot);
                                        newSprite.name = texture.name; // Assign a name to the sprite
                                                                       //Debug.Log($"Sprite created from texture: {newSprite.name}");

                                        // Assign the sprite to the CharacterImage
                                        __instance.CharacterImage.sprite = newSprite;
                                        //Debug.Log($"Assigned new sprite to CharacterImage: {__instance.CharacterImage.sprite.name}");

                                        // Handle positioning and sizing logic (unchanged)
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
                                                renderOffsetData = (__instance.CharacterIndex == 0) ? characterUIData.RenderOffsetData.Versus2Player1 : characterUIData.RenderOffsetData.Versus2Player2;
                                                break;
                                            case 2:
                                                renderOffsetData = (__instance.CharacterIndex == 0) ? characterUIData.RenderOffsetData.Versus3Player1 :
                                                                   (__instance.CharacterIndex == 1) ? characterUIData.RenderOffsetData.Versus3Player2 :
                                                                                                       characterUIData.RenderOffsetData.Versus3Player3;
                                                break;
                                            case 3:
                                                renderOffsetData = (__instance.CharacterIndex == 0) ? characterUIData.RenderOffsetData.Versus4Player1 :
                                                                   (__instance.CharacterIndex == 1) ? characterUIData.RenderOffsetData.Versus4Player2 :
                                                                   (__instance.CharacterIndex == 2) ? characterUIData.RenderOffsetData.Versus4Player3 :
                                                                                                       characterUIData.RenderOffsetData.Versus4Player4;
                                                break;
                                        }

                                        if (flag)
                                        {
                                            __instance.CharacterImage.transform.localPosition = renderOffsetData.Position;
                                            __instance.CharacterImage.GetComponent<RectTransform>().sizeDelta = renderOffsetData.Size;
                                        }
                                        __instance.CharacterImage.gameObject.SetActive(true);
                                        return false; // Skip original method if match is found
                                    }
                                    else
                                    {
                                        //Debug.LogWarning("Texture2D is null! Cannot create a sprite.");
                                    }
                                }
                            }

                            //Debug.Log("No matching customSkinID found for: " + __instance.characterCodename);
                            return true;

                        }
                    }
                }
                else
                {
                    foreach (var entry in metaDataList)
                    {
                        //Debug.Log($"Checking metaDataList entry: characterCodename = {entry.characterCodename}, skinIndex = {entry.skinIndex}, playerRef = {entry.playerRef}");

                        if (__instance.characterCodename == entry.characterCodename &&
                            __instance.skin == entry.skinIndex &&
                            __instance.CharacterIndex == entry.playerRef)
                        {
                            //Debug.Log($"Match found for CharacterCodename: {__instance.characterCodename}, Skin: {__instance.skin}, CharacterIndex: {__instance.CharacterIndex}");

                            foreach (var skinData in customSkinDatas)
                            {
                                //Debug.Log($"Checking customSkinData: SkinIntIndex = {skinData.skinIntIndex}");

                                if (entry.customSkinID == skinData.skinIntIndex)
                                {
                                    //Debug.Log($"Matching customSkinID found: {entry.customSkinID}, skinData.VSRender = {skinData.VSRender}");

                                    Texture2D texture = skinData.VSRender;

                                    if (texture != null)
                                    {
                                        // Logging the texture details before converting to sprite
                                        //Debug.Log($"Texture2D found. Width: {texture.width}, Height: {texture.height}, name: {texture.name}");

                                        // Create the sprite from the texture
                                        Rect rect = new Rect(0, 0, texture.width, texture.height);
                                        Vector2 pivot = new Vector2(0.5f, 0.5f); // Center of the texture

                                        Sprite newSprite = Sprite.Create(texture, rect, pivot);
                                        newSprite.name = texture.name; // Assign a name to the sprite
                                                                       //Debug.Log($"Sprite created from texture: {newSprite.name}");

                                        // Assign the sprite to the CharacterImage
                                        __instance.CharacterImage.sprite = newSprite;
                                        //Debug.Log($"Assigned new sprite to CharacterImage: {__instance.CharacterImage.sprite.name}");

                                        // Handle positioning and sizing logic (unchanged)
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
                                                renderOffsetData = (__instance.CharacterIndex == 0) ? characterUIData.RenderOffsetData.Versus2Player1 : characterUIData.RenderOffsetData.Versus2Player2;
                                                break;
                                            case 2:
                                                renderOffsetData = (__instance.CharacterIndex == 0) ? characterUIData.RenderOffsetData.Versus3Player1 :
                                                                   (__instance.CharacterIndex == 1) ? characterUIData.RenderOffsetData.Versus3Player2 :
                                                                                                       characterUIData.RenderOffsetData.Versus3Player3;
                                                break;
                                            case 3:
                                                renderOffsetData = (__instance.CharacterIndex == 0) ? characterUIData.RenderOffsetData.Versus4Player1 :
                                                                   (__instance.CharacterIndex == 1) ? characterUIData.RenderOffsetData.Versus4Player2 :
                                                                   (__instance.CharacterIndex == 2) ? characterUIData.RenderOffsetData.Versus4Player3 :
                                                                                                       characterUIData.RenderOffsetData.Versus4Player4;
                                                break;
                                        }

                                        if (flag)
                                        {
                                            __instance.CharacterImage.transform.localPosition = renderOffsetData.Position;
                                            __instance.CharacterImage.GetComponent<RectTransform>().sizeDelta = renderOffsetData.Size;
                                        }
                                        __instance.CharacterImage.gameObject.SetActive(true);
                                        return false; // Skip original method if match is found
                                    }
                                    else
                                    {
                                        //Debug.LogWarning("Texture2D is null! Cannot create a sprite.");
                                    }
                                }
                            }

                            //Debug.Log("No matching customSkinID found for: " + __instance.characterCodename);
                            return true; // Allow original method to run if no matching skinData
                        }
                    }
                    return true;
                }



                //Debug.Log("No match found in metaDataList, proceeding with original method.");
                return false ; // No match found, let original method run
            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(CharacterHUD), "CharacterHUDSpriteLoaded")]
            public static bool CharacterHUDSpriteLoaded(CharacterHUD __instance, Sprite sprite)
            {
                if (dataManager.Online)
                {
                    if (!(__instance.CharacterImage == null))
                    {
                        foreach (var player in onlineManager.GetPlayersList())
                        {
                            //Debug.LogWarning("Processing HUD:: " + __instance.CharacterIndex);

                            var propertyValue1 = onlineManager.Properties.GetPlayerRoomProperty(player, CUSTOM_SKIN_NAME);
                            if (propertyValue1 == null)
                            {
                                return true;
                            }

                            string nameToCheck = (string)propertyValue1;

                            if (nameToCheck == __instance.currentNickname)
                            {
                                //Debug.LogWarning("CharacterHUD:: Match found for Character Index: " + __instance.CharacterIndex + " - Player Actor is: " + (player.ActorNumber - 1) + " - Position Index is: " + __instance.PositionIndex);

                                var propertyValue = onlineManager.Properties.GetPlayerRoomProperty(player, CUSTOM_SKIN_ID);
                                if (propertyValue == null)
                                {
                                    return true;
                                }

                                int new1 = (int)propertyValue;

                                foreach (var entry2 in customSkinDatas)
                                {
                                    if (entry2.skinIntIndex == new1)
                                    {
                                        /*if (entry2.skinIndex != __instance.initialData.character.skinIndex)
                                        {
                                            return true;
                                        }*/


                                        Texture2D texture = entry2.VSRender;

                                        if (texture != null)
                                        {
                                            Rect rect = new Rect(0, 0, texture.width, texture.height);
                                            Vector2 pivot = new Vector2(0.5f, 0.5f);

                                            Sprite newSprite = Sprite.Create(texture, rect, pivot);
                                            newSprite.name = texture.name;

                                            //Debug.LogWarning($"CharacterHUD:: Assigned Custom Texture: {entry2.skinName} to Character Index: {__instance.CharacterIndex} - Actor Number: {player.ActorNumber - 1} ");

                                            // Assign the new texture and update UI elements
                                            __instance.CharacterImage.sprite = newSprite;
                                            __instance.CharacterImage.transform.localPosition = __instance.currentCharacterUIData.RenderOffsetData.CharacterHud.Position;
                                            __instance.CharacterImage.GetComponent<RectTransform>().sizeDelta = __instance.currentCharacterUIData.RenderOffsetData.CharacterHud.Size;

                                            __instance.EliminatedCharacterImage.sprite = newSprite;
                                            __instance.EliminatedCharacterImage.transform.localPosition = __instance.currentCharacterUIData.RenderOffsetData.CharacterHud.Position;
                                            __instance.EliminatedCharacterImage.GetComponent<RectTransform>().sizeDelta = __instance.currentCharacterUIData.RenderOffsetData.CharacterHud.Size;

                                            if (__instance.refreshAnimation)
                                            {
                                                __instance.refreshAnimation = false;
                                                __instance.CharacterImageAnimator.SetTrigger("Refresh");
                                            }

                                            // Exit the loops once a match and assignment is done to avoid redundant processing
                                            return false; // Skip the original method if a match is found
                                        }
                                        else
                                        {
                                            Debug.LogWarning("CharacterHUD:: Texture is null for " + entry2.skinName);
                                            return true; // If the texture is null, exit early
                                        }
                                    }
                                }

                                // Log if no matching custom skin was found
                                //Debug.LogWarning($"CharacterHUD:: No Assigned Custom Texture for Character Index: {__instance.CharacterIndex} - Actor Number: {player.ActorNumber - 1} ");
                                return true; // Exit after processing the player

                            }

                        }
                    }
                    return true;
                }
                else
                {
                    if (!(__instance.CharacterImage == null))
                    {
                        //Debug.LogWarning("image is not null");
                        foreach (var entry in metaDataList)
                        {
                            //Debug.Log($"Checking metaDataList entry: characterCodename = {entry.characterCodename}, skinIndex = {entry.skinIndex}, playerRef = {entry.playerRef}");

                            if (__instance.CharacterIndex == entry.playerRef)
                            {
                                foreach (var entry2 in customSkinDatas)
                                {
                                    if (entry.customSkinID == entry2.skinIntIndex)
                                    {
                                        Texture2D texture = entry2.VSRender;

                                        if (texture != null)
                                        {
                                            // Create the sprite from the texture
                                            Rect rect = new Rect(0, 0, texture.width, texture.height);
                                            Vector2 pivot = new Vector2(0.5f, 0.5f); // Center of the texture

                                            Sprite newSprite = Sprite.Create(texture, rect, pivot);
                                            newSprite.name = texture.name; // Assign a name to the sprite
                                                                           //Debug.Log($"Sprite created from texture: {newSprite.name}");

                                            // Assign the sprite to the CharacterImage
                                            __instance.CharacterImage.sprite = newSprite;
                                            __instance.CharacterImage.transform.localPosition = __instance.currentCharacterUIData.RenderOffsetData.CharacterHud.Position;
                                            __instance.CharacterImage.GetComponent<RectTransform>().sizeDelta = __instance.currentCharacterUIData.RenderOffsetData.CharacterHud.Size;
                                            __instance.EliminatedCharacterImage.sprite = newSprite;
                                            __instance.EliminatedCharacterImage.transform.localPosition = __instance.currentCharacterUIData.RenderOffsetData.CharacterHud.Position;
                                            __instance.EliminatedCharacterImage.GetComponent<RectTransform>().sizeDelta = __instance.currentCharacterUIData.RenderOffsetData.CharacterHud.Size;
                                            if (__instance.refreshAnimation)
                                            {
                                                __instance.refreshAnimation = false;
                                                __instance.CharacterImageAnimator.SetTrigger("Refresh");
                                            }

                                            return false; // Skip original method if match is found
                                        }
                                        else
                                        {
                                            //Debug.LogWarning("Texture2D is null! Cannot create a sprite.");
                                        }
                                    }

                                }
                            }
                        }
                    }

                    return true;
                }



                //Debug.LogWarning("CharacterHUDSpriteLoaded");
                return false;
            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(CharacterPanel), "UpdateData")]
            public static bool UpdateData(CharacterPanel __instance, CharacterUIData characterUIData, int skin)
            {
                bool playerIsReady = __instance.playerIsReady;
                if (playerIsReady)
                {
                }
                bool flag = !__instance.randomMode && __instance.currentCharacter == characterUIData.CharacterCodename && __instance.currentSkin == skin;
                bool result;
                if (flag)
                {
                    result = false;
                }
                else
                {
                    bool flag2 = __instance.currentCharacter == characterUIData.CharacterCodename;
                    if (flag2)
                    {
                        __instance.showIntroAnimation = false;
                    }
                    else
                    {
                        __instance.showIntroAnimation = true;
                    }
                    __instance.unlocked = true;
                    bool flag3 = !string.IsNullOrEmpty(characterUIData.DLCIdentifier);
                    if (flag3)
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
                    __instance.currentSkins = __instance.uiManager.GetCharacterSkins(characterUIData);
                    bool flag4 = !__instance.loadingMesh;
                    if (flag4)
                    {
                        __instance.LoadCharacterGameObject(characterUIData.CharacterCodename, __instance.currentSkin);
                    }
                    result = false;
                }
                return result;
            }
            [HarmonyPostfix]
            [HarmonyPatch(typeof(CharacterPanel), "UpdateData")]
            public static void POSTUpdateData(CharacterPanel __instance, CharacterUIData characterUIData, int skin)
            {
                Plugin.MetaData component = __instance.gameObject.GetComponent<Plugin.MetaData>();
                bool flag = component != null;
                if (flag)
                {
                    component.UpdateData();
                    component.characterSelect = __instance.characterSelect;
                }
            }
            [HarmonyPostfix]
            [HarmonyPatch(typeof(CharacterManager), "OnInstantiated")]
            public static void OnInstantiated1(CharacterManager __instance, QuantumGame game)
            {
                if (__instance.Data.CharacterIndex == 0)
                {
                    GameObject targetObject = __instance.gameObject;
                    if (targetObject != null)
                    {
                        // Process the target object and its children
                        ProcessRenderers(targetObject);

                        // Print the grouped meshes by material
                        PrintMaterialGroups();

                        materialMeshGroups.Clear();
                    }
                    else
                    {
                        Debug.LogError("No target object assigned.");
                    }
                }
            }
            public static void ProcessRenderers(GameObject obj)
            {
                // Find all SkinnedMeshRenderers and MeshRenderers in the target object and its children
                SkinnedMeshRenderer[] skinnedMeshRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>(true);

                // Process SkinnedMeshRenderers
                foreach (SkinnedMeshRenderer skinnedRenderer in skinnedMeshRenderers)
                {
                    ProcessMaterials(skinnedRenderer.sharedMaterials, skinnedRenderer.name);
                }

                // Process MeshRenderers
                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    ProcessMaterials(meshRenderer.sharedMaterials, meshRenderer.name);
                }
            }

            public static void ProcessMaterials(Material[] materials, string rendererName)
            {
                // Loop through the materials array
                foreach (Material mat in materials)
                {
                    if (mat != null)
                    {
                        // Get the material name
                        string materialName = mat.name;

                        // If the material group doesn't exist, create it
                        if (!materialMeshGroups.ContainsKey(materialName))
                        {
                            materialMeshGroups[materialName] = new List<string>();
                        }

                        // Add the renderer name to the material group
                        if (!materialMeshGroups[materialName].Contains(rendererName))
                        {
                            materialMeshGroups[materialName].Add(rendererName);
                        }
                    }
                }
            }

            public static void PrintMaterialGroups()
            {
                // Print the results
                foreach (var materialGroup in materialMeshGroups)
                {
                    Debug.Log($"{materialGroup.Key} has {materialGroup.Value.Count} meshes:");

                    foreach (string rendererName in materialGroup.Value)
                    {
                        Debug.Log(rendererName);
                    }

                    Debug.Log(""); // Print an empty line for readability
                }
            }
        }


    }
}


