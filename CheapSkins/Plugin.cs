using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CheapSkins.Resources.CharactersDictionaries;
using Epic.OnlineServices;
using FIMSpace.Basics;
using HarmonyLib;
using Newtonsoft.Json;
using Photon.Deterministic;
using Photon.Realtime;
using Quantum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using static CheapSkinss.Plugin;
using static Quantum.Core.FrameContext;
using static UnityEngine.EventSystems.EventTrigger;

namespace CheapSkinss
{
    [BepInDependency("markervan.nasb2.cheapnasb2", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin("markaccino.nasb2.cheapskins", "CheapSkins", "2.0")]
    internal class Plugin : BaseUnityPlugin
    {
        
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
        static public GameResourcesManager gameResourcesManager
        {
            get
            {
                return GameManager.Instance.GameResourcesManager;
            }
        }
        #endregion

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
            { "Dagget", Daggett.DaggettaltParts},
            { "Norbert", Norbet.NorbertaltParts},
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
            public List<AnimationsOverride> animationClips;
            public string sfxData;

            [System.Serializable]
            public class MaterialOverrideGroup
            {
                public string identifier;
                public string matIndex;
                public string customShader;
                public string customMaterial;
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

            [System.Serializable]
            public class AnimationsOverride
            {
                public string animationID; // Identifier for the animation
                public string animationRef; // Path or reference to the AnimationClip as a string
                public string animationVisibilty;
            }


            public bool shadowTrail;
        }
        #endregion

        internal static ManualLogSource Log;
        public static string skinsPath = Path.Combine(Paths.PluginPath, "Skins");
        //public static Dictionary<string, CharacterMaterialOverridesHandler.MaterialOverrideGroup> MOGList = new Dictionary<string, CharacterMaterialOverridesHandler.MaterialOverrideGroup>();
        public static List<CharacterMetaData> metaDataList = new List<CharacterMetaData>();
        public static string CUSTOM_SKIN_ID = "CUSTOM_SKIN_ID";
        public static string CUSTOM_SKIN_NAME = "CUSTOM_SKIN_NAME";
        private List<string> cm = new List<string>();
        public static Dictionary<string, List<string>> materialMeshGroups = new Dictionary<string, List<string>>();


        private const string OtherModGuid = "markervan.nasb2.cheapnasb2";
        

        public static Dictionary<int, CharacterMetaData> metaDataDict = new Dictionary<int, CharacterMetaData>();

        public class CharacterMetaData
        {
            public int playerIndex;
            public int skinIndex;
            public string customSkinName;
        }
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
            public Dictionary<string, Dictionary<string, Mesh>> materialBanksForMeshes;
            public Dictionary<string, Shader> shaderToUse;
            public Dictionary<string, Material> customMaterials;
            public Dictionary<string, AnimationClip> customAnimations;
            public SFXData customSFXData;
            public Dictionary<string, CharacterAnimatorStateAsset> customAnimatorBehaviours = new Dictionary<string, CharacterAnimatorStateAsset>();


        }

        public class PreviewCustomSkin
        {
            public CharacterCodename character;
            public int skinIndex;
            public string skinName;
            public string authorName;
            public string skinFilename;
            public Sprite CSSimage;
        }

        public static Dictionary<CharacterCodename, List<PreviewCustomSkin>> previewSkinsDict = new Dictionary<CharacterCodename, List<PreviewCustomSkin>>();

        public class SkinNameComparer : IComparer<CustomSkinData>
        {
            public int Compare(CustomSkinData x, CustomSkinData y)
            {
                return string.Compare(x.skinName, y.skinName, StringComparison.OrdinalIgnoreCase);
            }
        }

        public static List<CustomSkinData> customSkinDatas = new List<CustomSkinData>();

        public static Dictionary<string, CustomSkinData> dictCustomSkinDatas = new Dictionary<string, CustomSkinData>();
        #region CheapSkinLoaders

        private void LogCustomSkinData()
        {
            foreach (var skinData in customSkinDatas)
            {
                Debug.Log($"Skin ID: {skinData.skinID}");
                Debug.Log($"Character Codename: {skinData.characterCodename}");
                Debug.Log($"Skin Index: {skinData.skinIndex}");
                Debug.Log($"Skin Int Index: {skinData.skinIntIndex}");
                Debug.Log($"Skin Name: {skinData.skinName}");
                Debug.Log($"Author Name: {skinData.authorName}");

                if (skinData.VSRender != null)
                {
                    Debug.Log($"VS Render: {skinData.VSRender.name}");
                }
                else
                {
                    Debug.Log("VS Render: None");
                }

                if (skinData.stockImage != null)
                {
                    Debug.Log($"Stock Image: {skinData.stockImage.name}");
                }
                else
                {
                    Debug.Log("Stock Image: None");
                }

                if (skinData.stockImageSprite != null)
                {
                    Debug.Log($"Stock Image Sprite: {skinData.stockImageSprite.name}");
                }
                else
                {
                    Debug.Log("Stock Image Sprite: None");
                }

                // Log the material banks for meshes
                Debug.Log($"Material Banks for Meshes for Skin ID: {skinData.skinID}");
                foreach (var bankEntry in skinData.materialBanksForMeshes)
                {
                    Debug.Log($"  Key: {bankEntry.Key}");
                    foreach (var meshEntry in bankEntry.Value)
                    {
                        Debug.Log($"    Sub-Key: {meshEntry.Key}, Mesh Name: {meshEntry.Value.name}");
                    }
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

                            //Debug.Log("Loading AssetBundle from memory...");
                            AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromMemoryAsync(assetBundleData);

                            bundleRequest.completed += (asyncOperation) =>
                            {
                                AssetBundle bundle = bundleRequest.assetBundle;
                                if (bundle == null)
                                {
                                    ////Debug.LogError("Failed to load AssetBundle from memory!");
                                    return;
                                }

                                //Debug.Log("AssetBundle loaded successfully from memory.");

                                string[] assetNames = bundle.GetAllAssetNames();
                                //Debug.Log("Assets in the AssetBundle:");
                                foreach (string assetName in assetNames)
                                {
                                    //Debug.Log(assetName);
                                }

                                Debug.Log($"Loaded characterID: {package.characterID}, skinID: {package.skinID} from {Path.GetFileName(cheapskinFile)}");

                                CustomSkinData customSkinData = new CustomSkinData();
                                if (package.materialOverrideGroups != null)
                                {
                                    
                                    customSkinData.skinID = package.skinID;
                                    customSkinData.skinName = package.skinName;
                                    // Try to parse the string into the CharacterCodename enum
                                    if (Enum.TryParse(package.characterID, out CharacterCodename parsedCodename))
                                    {
                                        customSkinData.characterCodename = parsedCodename;
                                    }
                                    else
                                    {
                                        //Debug.LogError("Failed to parse characterID: " + package.characterID);
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



                                    customSkinData.materialBanksForMeshes = new Dictionary<string, Dictionary<string, Mesh>>();
                                    customSkinData.shaderToUse = new Dictionary<string, Shader>();
                                    customSkinData.customMaterials = new Dictionary<string, Material>();



                                    try
                                    {
                                        //.Log("Custom Skin Data Index: " + customSkinData.skinIntIndex);
                                        //Debug.LogWarning("Processing SFXData...");

                                        if (package.sfxData != null)
                                        {
                                            //Debug.Log("SFXData path: " + package.sfxData);

                                            if (!string.IsNullOrEmpty(package.sfxData))
                                            {
                                                SFXData sFXData = bundle.LoadAsset<SFXData>(package.sfxData);
                                                if (sFXData != null)
                                                {
                                                    //Debug.Log("Successfully loaded SFXData: " + package.sfxData);
                                                    customSkinData.customSFXData = sFXData;
                                                }
                                                else
                                                {
                                                    //Debug.LogError("Failed to load SFXData for skin: " + package.skinName);
                                                }
                                            }
                                            else
                                            {
                                                //Debug.LogWarning("SFXData path is empty for skin: " + package.skinName);
                                            }
                                        }
                                        else
                                        {
                                            //Debug.LogWarning("No SFXData provided for skin: " + package.skinName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Debug.LogError("Error processing SFXData for skin: " + package.skinName + ". Exception: " + ex.Message);
                                    }

                                    //Debug.Log("Processing material override groups.");
                                    foreach (var MOGVar in package.materialOverrideGroups)
                                    {
                                        //Debug.Log($"Processing material override group: {MOGVar.identifier}");

                                        CharacterMaterialOverridesHandler.MaterialOverrideGroup MOG = new CharacterMaterialOverridesHandler.MaterialOverrideGroup
                                        {
                                            Identifier = $"{MOGVar.matIndex}:{MOGVar.identifier}",
                                            TextureOverrides = new List<CharacterMaterialOverridesHandler.TextureOverride>() // Initialize here
                                        };
                                        Dictionary<string, Mesh> customMeshesToReplace = new Dictionary<string, Mesh>();


                                        //SHADER GETTER

                                        if (MOGVar.customShader != null)
                                        {
                                            //Debug.LogWarning("Custom Shader is used, trying to load Custom Shader...");

                                            if (!string.IsNullOrWhiteSpace(MOGVar.customShader))
                                            {
                                                Shader shader = bundle.LoadAsset<Shader>(MOGVar.customShader);
                                                if (shader != null)
                                                {
                                                    //Debug.LogWarning("Shader Found: " + shader.name);

                                                    if (!customSkinData.shaderToUse.ContainsKey(MOGVar.identifier))
                                                    {
                                                        customSkinData.shaderToUse.Add(MOGVar.matIndex + ":" + MOGVar.identifier, shader);
                                                    }

                                                }
                                            }
                                        }

                                        if (MOGVar.customMaterial != null)
                                        {
                                            //Debug.LogWarning("Custom Material is used, trying to load Custom Material...");

                                            if (!string.IsNullOrWhiteSpace(MOGVar.customMaterial))
                                            {
                                                Material material = bundle.LoadAsset<Material>(MOGVar.customMaterial);
                                                if (material != null)
                                                {
                                                    //Debug.LogWarning("Shader Found: " + material.name);

                                                    if (!customSkinData.customMaterials.ContainsKey(MOGVar.identifier))
                                                    {
                                                        customSkinData.customMaterials.Add(MOGVar.matIndex + ":" + MOGVar.identifier, material);
                                                    }

                                                }
                                            }
                                        }

                                        // TARGETS OVERRIDES


                                        // Loop through the targets
                                        //Debug.Log("Processing Targets.");
                                        foreach (var MOGTargets in MOGVar.Targets)
                                        {
                                            if (MOGTargets.materialIndex != 0)
                                            {
                                                continue;
                                            }

                                            //Debug.Log($"Attempting to load GameObject from asset bundle for target: {MOGTargets.targetName}");

                                            if (string.IsNullOrWhiteSpace(MOGTargets.targetName))
                                            {
                                                //Debug.LogWarning("IS NULL OR WHITE SPACE");

                                                Mesh nullMesh = new Mesh();
                                                if (customMeshesToReplace.ContainsKey(MOGTargets.targetMesh))
                                                {
                                                    Debug.LogWarning($"Key {MOGTargets.targetMesh} already exists in customMeshesToReplace. Skipping addition to avoid duplicates.");
                                                }
                                                else
                                                {
                                                    customMeshesToReplace.Add(MOGTargets.targetMesh, nullMesh);
                                                }
                                                continue;
                                            }

                                            GameObject fbx = bundle.LoadAsset<GameObject>(MOGTargets.targetName);

                                            if (fbx == null)
                                            {
                                                //Debug.LogError($"Failed to load GameObject from path: {MOGTargets.targetName}");
                                                continue; // Continue to the next target
                                            }

                                            //Debug.Log("GameObject found: " + fbx.name);

                                            // Find the SkinnedMeshRenderer in the FBX
                                            SkinnedMeshRenderer skinnedMesh = FindSkinnedMeshRendererInChildren(fbx);
                                            if (skinnedMesh != null)
                                            {
                                                //Debug.Log("Found SkinnedMeshRenderer in FBX: " + skinnedMesh.name);
                                                Mesh mesh = skinnedMesh.sharedMesh;

                                                if (mesh != null)
                                                {
                                                    if (customMeshesToReplace.ContainsKey(MOGTargets.targetMesh))
                                                    {
                                                        //Debug.LogWarning($"Key {MOGTargets.targetMesh} already exists in customMeshesToReplace. Skipping addition to avoid duplicates.");
                                                    }
                                                    else
                                                    {
                                                        //Debug.Log($"Adding SkinnedMeshRenderer mesh to dictionary: Target Mesh = {MOGTargets.targetMesh}, Mesh = {mesh.name}");
                                                        customMeshesToReplace.Add(MOGTargets.targetMesh, mesh);
                                                    }
                                                }
                                                else
                                                {
                                                    //Debug.LogWarning($"SkinnedMeshRenderer found, but no mesh assigned: {skinnedMesh.name}");
                                                }
                                            }
                                            else
                                            {
                                                //Debug.LogWarning("No SkinnedMeshRenderer found in FBX, trying to find MeshRenderer....");

                                                MeshRenderer meshRenderer = FindMeshRendererInChildren(fbx);
                                                if (meshRenderer != null)
                                                {
                                                    //Debug.Log("Found MeshRenderer in FBX " + meshRenderer.name);

                                                    MeshFilter meshFilter = meshRenderer.gameObject.GetComponent<MeshFilter>();
                                                    if (meshFilter != null && meshFilter.mesh != null)
                                                    {
                                                        Mesh mesh = meshFilter.mesh;
                                                        if (customMeshesToReplace.ContainsKey(MOGTargets.targetMesh))
                                                        {
                                                            //Debug.LogWarning($"Key {MOGTargets.targetMesh} already exists in customMeshesToReplace. Skipping addition to avoid duplicates.");
                                                        }
                                                        else
                                                        {
                                                            //Debug.Log($"Adding MeshRenderer mesh to dictionary: Target Mesh = {MOGTargets.targetMesh}, Mesh = {mesh.name}");
                                                            customMeshesToReplace.Add(MOGTargets.targetMesh, mesh);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Debug.LogWarning($"MeshRenderer found, but no mesh or MeshFilter: {meshRenderer.name}");
                                                    }
                                                }
                                            }

                                            // Log the dictionary contents after all meshes are loaded
                                            //Debug.Log("Logging customMeshesToReplace dictionary contents after population:");
                                            foreach (var entry in customMeshesToReplace)
                                            {
                                                //Debug.Log($"Key: {entry.Key}, Mesh name: {entry.Value.name}");
                                            }
                                        }

                                        // Check if the identifier already exists in materialBanksForMeshes before adding
                                        if (customSkinData.materialBanksForMeshes.ContainsKey(MOGVar.identifier))
                                        {
                                            //Debug.LogWarning($"Identifier {MOGVar.identifier} already exists in materialBanksForMeshes. Merging with existing data.");

                                            // Merge dictionaries to avoid overwriting
                                            foreach (var kvp in customMeshesToReplace)
                                            {
                                                if (!customSkinData.materialBanksForMeshes[MOGVar.identifier].ContainsKey(kvp.Key))
                                                {
                                                    customSkinData.materialBanksForMeshes[MOGVar.identifier].Add(kvp.Key, kvp.Value);
                                                }
                                                else
                                                {
                                                    //Debug.LogWarning($"Key {kvp.Key} already exists in materialBanksForMeshes[{MOGVar.identifier}]. Skipping.");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            customSkinData.materialBanksForMeshes.Add(MOGVar.identifier, new Dictionary<string, Mesh>(customMeshesToReplace));
                                        }
                                        // ATTRIBUTE OVERRIDES
                                        if (MOGVar.attributeOverrides != null)
                                        {
                                            //Debug.Log("Processing attribute overrides.");

                                            if (MOG.AttributeOverrides == null)
                                            {
                                                //Debug.LogError("MOG.AttributeOverrides is null! Initializing it.");
                                                MOG.AttributeOverrides = new List<CharacterMaterialOverridesHandler.AttributeOverride>();
                                            }

                                            foreach (var MOGattributeOverrides in MOGVar.attributeOverrides)
                                            {
                                                if (MOGattributeOverrides == null)
                                                {
                                                    //Debug.LogError("MOGattributeOverrides is null! Skipping this entry.");
                                                    continue;
                                                }

                                                //Debug.Log($"Processing attribute with ID: {MOGattributeOverrides.attributeID}");

                                                CharacterMaterialOverridesHandler.AttributeOverride newAttribute = new CharacterMaterialOverridesHandler.AttributeOverride
                                                {
                                                    AttributeType = (CharacterMaterialOverridesHandler.AttributeOverride.AttributeNumberTypes)MOGattributeOverrides.attributeType, // Assuming Integer type for now
                                                    AttributeID = MOGattributeOverrides.attributeID,
                                                    AttributeValue = MOGattributeOverrides.attributeValue,
                                                };

                                                //Debug.Log("Adding new attribute override.");

                                                MOG.AttributeOverrides.Add(newAttribute);
                                            }
                                        }
                                        else
                                        {
                                            //Debug.LogWarning("MOGVar.attributeOverrides is null.");
                                        }

                                        // TEXTURE OVERRIDES
                                        if (MOGVar.textureOverrides != null)
                                        {
                                            //Debug.Log("Processing texture overrides.");
                                            foreach (var MOGtextureOverrides in MOGVar.textureOverrides)
                                            {

                                                if (string.IsNullOrWhiteSpace(MOGtextureOverrides.textureRef))
                                                {
                                                    Debug.LogError($"Empty Texture2D");
                                                    CharacterMaterialOverridesHandler.TextureOverride newTexture1 = new CharacterMaterialOverridesHandler.TextureOverride
                                                    {
                                                        TextureID = MOGtextureOverrides.textureID,
                                                        TextureRef = null,
                                                    };
                                                    MOG.TextureOverrides.Add(newTexture1);
                                                    continue;
                                                }

                                                Texture2D texture2D = bundle.LoadAsset<Texture2D>(MOGtextureOverrides.textureRef);
                                                //Debug.Log("Texture2D found: " + texture2D.name);

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
                                            //Debug.Log("Processing vector attribute overrides.");

                                            // Ensure MOG.VectorAttributeOverrides is initialized
                                            if (MOG.VectorAttributeOverrides == null)
                                            {
                                                //Debug.LogError("MOG.VectorAttributeOverrides is null! Initializing it.");
                                                MOG.VectorAttributeOverrides = new List<CharacterMaterialOverridesHandler.Vector4AttributeOverride>();
                                            }

                                            foreach (var MOGattributeOverrides in MOGVar.vectorAttributeOverrides)
                                            {
                                                if (MOGattributeOverrides == null)
                                                {
                                                    //Debug.LogError("MOGattributeOverrides (vector) is null! Skipping this entry.");
                                                    continue;
                                                }

                                                //Debug.Log($"Processing vector attribute with ID: {MOGattributeOverrides.attributeID}");

                                                Vector4 vector = FloatArrayToVector4(MOGattributeOverrides.attributeValue);

                                                CharacterMaterialOverridesHandler.Vector4AttributeOverride newVectorAttribute = new CharacterMaterialOverridesHandler.Vector4AttributeOverride
                                                {
                                                    AttributeID = MOGattributeOverrides.attributeID,
                                                    AttributeValue = vector,
                                                };

                                                //Debug.Log("Adding new vector attribute override.");
                                                MOG.VectorAttributeOverrides.Add(newVectorAttribute);
                                            }
                                        }
                                        else
                                        {
                                            //Debug.LogWarning("MOGVar.vectorAttributeOverrides is null.");
                                        }

                                        // COLOR OVERRIDES
                                        if (MOGVar.colorOverrides != null)
                                        {
                                            //Debug.Log("Processing color overrides.");

                                            // Ensure MOG.ColorOverrides is initialized
                                            if (MOG.ColorOverrides == null)
                                            {
                                                //Debug.LogError("MOG.ColorOverrides is null! Initializing it.");
                                                MOG.ColorOverrides = new List<CharacterMaterialOverridesHandler.ColorOverride>();
                                            }

                                            foreach (var MOGtextureOverrides in MOGVar.colorOverrides)
                                            {
                                                if (MOGtextureOverrides == null)
                                                {
                                                    //Debug.LogError("MOGtextureOverrides (color) is null! Skipping this entry.");
                                                    continue;
                                                }

                                                //Debug.Log($"Processing color with ID: {MOGtextureOverrides.colorID}");

                                                Color color = HexToUnityColor(MOGtextureOverrides.colorValue);

                                                CharacterMaterialOverridesHandler.ColorOverride newColor = new CharacterMaterialOverridesHandler.ColorOverride
                                                {
                                                    ColorID = MOGtextureOverrides.colorID,
                                                    ColorValue = color
                                                };

                                                //Debug.Log("Adding new color override.");
                                                MOG.ColorOverrides.Add(newColor);
                                            }
                                        }
                                        else
                                        {
                                            //Debug.LogWarning("MOGVar.colorOverrides is null.");
                                        }

                                        //Debug.Log("Material override group processed.");

                                        customSkinData.CustomMOGList.Add(MOG);
                                    }


                                    //Debug.Log("trying to add customskinData to customSkinDatas");

                                    if (customSkinData.materialBanksForMeshes != null)
                                    {
                                        foreach (var entry in customSkinData.materialBanksForMeshes)
                                        {
                                            //Debug.LogWarning($"Mesh entry found: {entry.Key} -> {entry.Value}");
                                        }
                                    }
                                    if (!Plugin.customSkinDatas.Any(x => x.skinID == customSkinData.skinID))
                                    {
                                        Plugin.customSkinDatas.Add(customSkinData);

                                        Plugin.dictCustomSkinDatas.Add(customSkinData.skinID, customSkinData);

                                        customSkinDatas.Sort(new SkinNameComparer());
                                        //Plugin.Log.LogWarning("dont sorting");

                                        foreach (var cheapskinFile in customSkinDatas)
                                        {
                                            // Process each .cheapskin file
                                            //Plugin.Log.LogWarning(cheapskinFile.skinName);
                                        }
                                    }

                                    foreach (var entry in customSkinData.shaderToUse)
                                    {
                                        //Debug.LogWarning($"Custom Shader entry found: {entry.Key} -> {entry.Value}");
                                    }
                                    foreach (var entry in customSkinData.customMaterials)
                                    {
                                        //Debug.LogWarning($"Custom Material entry found: {entry.Key} -> {entry.Value}");
                                    }
                                }
                                else
                                {
                                    //Debug.LogWarning("No material override groups found.");
                                }

                                if (package.animationClips != null)
                                {
                                    Debug.Log($"Found {package.animationClips} animation clips in package.");
                                    customSkinData.customAnimatorBehaviours = new Dictionary<string, CharacterAnimatorStateAsset>();

                                    foreach (var entry in package.animationClips)
                                    {
                                        Debug.Log($"Processing AnimationClip ID: {entry.animationID}");

                                        if (!string.IsNullOrEmpty(entry.animationVisibilty))
                                        {
                                            Debug.Log($"Attempting to load CharacterAnimatorBehaviourAsset: {entry.animationVisibilty}");

                                            CharacterAnimatorStateAsset state = bundle.LoadAsset<CharacterAnimatorStateAsset>(entry.animationVisibilty);

                                            if (state != null)
                                            {
                                                Debug.Log($"Successfully loaded CharacterAnimatorBehaviourAsset: {entry.animationVisibilty} for AnimationClip ID: {entry.animationID}");

                                                if (!customSkinData.customAnimatorBehaviours.ContainsKey(entry.animationID))
                                                {
                                                    customSkinData.customAnimatorBehaviours.Add(entry.animationID.ToLower(), state);
                                                    Debug.Log($"Added new entry - Key: {entry.animationID}, Value: {state.name}");
                                                }
                                                else
                                                {
                                                    Debug.LogWarning($"Duplicate AnimationClip ID detected: {entry.animationID}. Skipping addition.");
                                                }
                                            }
                                            else
                                            {
                                                Debug.LogError($"Failed to load CharacterAnimatorBehaviourAsset: {entry.animationVisibilty}");
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogWarning($"AnimationClip ID {entry.animationID} has no associated animationVisibilty path.");
                                        }
                                    }

                                    // Log final contents of the dictionary
                                    Debug.Log($"Final customAnimatorBehaviours count: {customSkinData.customAnimatorBehaviours.Count}");
                                    foreach (var entry in customSkinData.customAnimatorBehaviours)
                                    {
                                        Debug.Log($"AnimationClip Entry - Key: {entry.Key}, Value: {entry.Value?.name ?? "NULL"}");
                                    }
                                }
                                /*else
                                {
                                    Debug.LogWarning("package.animationClips is null. No animations were processed.");
                                }*/




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
        public static Sprite ConvertTextureToSprite(Texture2D texture)
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

        #endregion
        public void Awake()
        {
            Plugin.Log = base.Logger;
            
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
            string configPath = Paths.ConfigPath;
            string path = Path.Combine(configPath, "BepInEx.cfg");
            string[] array = File.ReadAllLines(path);
            for (int i = 0; i < array.Length; i++)
            {
                bool flag = array[i].Trim().StartsWith("HideManagerGameObject");
                if (flag)
                {
                    array[i] = "HideManagerGameObject = true";
                    break;
                }
            }
            var harmony = new Harmony("markaccino.nasb2.cheapskins");

            try
            {
                // First unpatch conflicting methods from other mod
                UnpatchConflictingMethods(harmony);

                // Then apply our patches
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                base.Logger.LogInfo("Mod loaded successfully!");
            }
            catch (Exception ex)
            {
                base.Logger.LogError($"Error during patching: {ex}");
            }
            File.WriteAllLines(path, array);
            Debug.Log("HideManagerGameObject is set to true.");
            LoadAllCheapskins();

        }

        private void UnpatchConflictingMethods(Harmony harmony)
        {
            try
            {
                // Example - unpatch a specific method from another mod
                // Replace with actual types/methods you want to unpatch
                var targetType = typeof(CharacterPanel); // The class containing the method
                var targetMethod = targetType.GetMethod("LoadCharacterGameObject"); // The method name

                if (targetMethod != null)
                {
                    // Unpatch all prefixes from the other mod
                    harmony.Unpatch(targetMethod, HarmonyPatchType.Prefix, OtherModGuid);

                    // Optionally unpatch other patch types if needed
                    harmony.Unpatch(targetMethod, HarmonyPatchType.Postfix, OtherModGuid);
                    harmony.Unpatch(targetMethod, HarmonyPatchType.Transpiler, OtherModGuid);

                    Log.LogInfo($"Successfully unpatched conflicting methods from {OtherModGuid}");
                }
                else
                {
                    Log.LogWarning("Target method not found for unpatching");
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Failed to unpatch conflicting methods: {ex}");
            }
        }

        

        public class SsaoConfigurator
        {
            private readonly object _ssaoSettings;
            private readonly FieldInfo _fRadius;
            private readonly FieldInfo _fAfterOpaque;
            private readonly FieldInfo _fIntensity;
            private readonly FieldInfo _fFalloff;

            public SsaoConfigurator()
            {
                // Method to find a specific render feature by type
                static ScriptableRendererFeature findRenderFeature(Type type)
                {
                    FieldInfo field = reflectField(typeof(ScriptableRenderer), "m_RendererFeatures");
                    ScriptableRenderer renderer = UniversalRenderPipeline.asset.scriptableRenderer;
                    var list = (List<ScriptableRendererFeature>)field.GetValue(renderer);
                    foreach (ScriptableRendererFeature feature in list)
                        if (feature.GetType() == type)
                            return feature;
                    throw new Exception($"Could not find instance of {type.AssemblyQualifiedName} in the renderer features list");
                }

                // Method to reflect private fields
                static FieldInfo reflectField(Type type, string name) =>
                    type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic) ??
                    throw new Exception($"Could not reflect field [{type.AssemblyQualifiedName}].{name}");

                // Get the SSAO feature type and settings
                Type tSsaoFeature = Type.GetType("UnityEngine.Rendering.Universal.ScreenSpaceAmbientOcclusion, Unity.RenderPipelines.Universal.Runtime", true);
                FieldInfo fSettings = reflectField(tSsaoFeature, "m_Settings");
                ScriptableRendererFeature ssaoFeature = findRenderFeature(tSsaoFeature);
                _ssaoSettings = fSettings.GetValue(ssaoFeature) ?? throw new Exception("ssaoFeature.m_Settings was null");

                // Reflect SSAO settings fields
                _fRadius = reflectField(_ssaoSettings.GetType(), "Radius");
                _fAfterOpaque = reflectField(_ssaoSettings.GetType(), "AfterOpaque");
                _fIntensity = reflectField(_ssaoSettings.GetType(), "Intensity");
                _fFalloff = reflectField(_ssaoSettings.GetType(), "Falloff");
            }

            // Property for Radius
            public float Radius
            {
                get => (float)_fRadius.GetValue(_ssaoSettings);
                set => _fRadius.SetValue(_ssaoSettings, value);
            }

            // Property for AfterOpaque
            public bool AfterOpaque
            {
                get => (bool)_fAfterOpaque.GetValue(_ssaoSettings);
                set => _fAfterOpaque.SetValue(_ssaoSettings, value);
            }

            // Property for Intensity
            public float Intensity
            {
                get => (float)_fIntensity.GetValue(_ssaoSettings);
                set => _fIntensity.SetValue(_ssaoSettings, value);
            }

            // Property for Falloff
            public float Falloff
            {
                get => (float)_fFalloff.GetValue(_ssaoSettings);
                set => _fFalloff.SetValue(_ssaoSettings, value);
            }
        }

        public class MetaData : MonoBehaviour
        {
            private CharacterPanel characterPanel;

            private CharacterCodename currentCharacter;

            private string customSkinName;

            private int customSkinIndex;

            private int skinListIndex;

            private int currentCustomSkinID;

            private string currentSkinFileName;

            public List<CustomSkinData> customSkinsID = new List<CustomSkinData>();

            public List<PreviewCustomSkin> previewSkinsList = new List<PreviewCustomSkin>();

            private TMPro.TextMeshProUGUI mainTitleSkin;
            private TMPro.TextMeshProUGUI authorTitle;
            private UnityEngine.UI.Image skinImage;

            public GameObject CustominfoSection;
            public GameObject dummyInfo;
            public Sprite FPLImage;

            private void Awake()
            {
                characterPanel = this.gameObject.GetComponent<CharacterPanel>();
                Debug.Log(characterPanel.gameObject.name);

                SetUpPanel();
            }
            private void SetUpPanel()
            {
                Transform childTransform = characterPanel.gameObject.transform.Find("InfoSection");

                CustominfoSection = GameObject.Instantiate(childTransform.gameObject);

                CustominfoSection.transform.SetParent(characterPanel.gameObject.transform, true);
                CustominfoSection.name = "CustomSkinInfo";

                CustominfoSection.transform.localPosition = new Vector3(-38.7277f, -411.6458f, 0);
                CustominfoSection.transform.localScale = new Vector3(1, 1, 1);

                Transform mainTextTransform = CustominfoSection.transform.Find("MainTitleText");
                mainTitleSkin = mainTextTransform.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                mainTitleSkin.alignment = TextAlignmentOptions.Center;
                mainTitleSkin.gameObject.transform.localPosition = new Vector3(46.8699f, -24.9899f, 0);
                mainTitleSkin.text = "DEFAULT";
                CustominfoSection.SetActive(false);

                Transform mainTextTransform11 = CustominfoSection.transform.Find("SecondaryTitle");
                authorTitle = mainTextTransform11.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                authorTitle.text = "FAIR PLAY LABS";

                Transform mainTextTransform12 = CustominfoSection.transform.Find("PlayerTag");
                skinImage = mainTextTransform12.gameObject.GetComponentInChildren<UnityEngine.UI.Image>();
                skinImage.preserveAspect = true;
                skinImage.material = null;
                skinImage.color = Color.white;
                Texture2D fplLogoTexture = Patches.getTexture2D();

                Sprite fplLogo = ConvertTextureToSprite(fplLogoTexture);
                skinImage.sprite = fplLogo;
                FPLImage = fplLogo;

                TMPro.TextMeshProUGUI textComponent = mainTextTransform12.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                GameObject textP1 = textComponent.gameObject;
                textP1.SetActive(false);

                // Instantiate dummyInfo after setting up CustominfoSection
                dummyInfo = GameObject.Instantiate(CustominfoSection);
                dummyInfo.transform.SetParent(characterPanel.gameObject.transform, true);
                dummyInfo.name = "DUMMYINFO";
                dummyInfo.transform.localPosition = new Vector3(-38.7277f, -411.6458f, 0);
                dummyInfo.transform.localScale = new Vector3(1, 1, 1);
                Transform mainTextTransform1 = dummyInfo.transform.Find("MainTitleText");

                TextMeshProUGUI mainTitleSkin1;
                mainTitleSkin1 = mainTextTransform1.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                mainTitleSkin1.alignment = TextAlignmentOptions.Center;
                mainTitleSkin1.gameObject.transform.localPosition = new Vector3(46.8699f, -24.9899f, 0);
                mainTitleSkin1.text = "DEFAULT";

                // Ensure CustominfoSection is rendered in front of dummyInfo
                dummyInfo.transform.SetSiblingIndex(CustominfoSection.transform.GetSiblingIndex() - 1);
            }
            public void UpdateData(CharacterUIData characterUI, int skin)
            {
                /*NewUpdate();
                SetSkin(skinListIndex);*/

                if (Plugin.previewSkinsDict.TryGetValue(characterUI.CharacterCodename, out var skins))
                {
                    previewSkinsList = skins; // skins contains the List<PreviewCustomSkin> if found
                }

                SetSkin(skin);

            }
            public void NewUpdate()
            {
                // Cache frequently accessed values
                var currentChar = characterPanel.currentCharacter;
                var currentSkinIndex = characterPanel?.currentSkin ?? -1;
                var currentSkinsList = characterPanel?.currentSkins;

                // Only update if changes detected (with full safety checks)
                bool needsUpdate =
                    currentCharacter != currentChar ||
                    (previewSkinsList != null &&
                     skinListIndex >= 0 &&
                     skinListIndex < previewSkinsList.Count &&
                     customSkinName != previewSkinsList[skinListIndex]?.skinName) ||
                    (currentSkinsList != null &&
                     currentSkinIndex >= 0 &&
                     currentSkinIndex < currentSkinsList.Count &&
                     customSkinIndex != currentSkinsList[currentSkinIndex]);

                if (needsUpdate)
                {
                    // Update character data if changed
                    if (currentCharacter != currentChar)
                    {
                        previewSkinsList = Plugin.previewSkinsDict.TryGetValue(currentChar, out var skins)
                            ? skins
                            : new List<PreviewCustomSkin>();
                        currentCharacter = currentChar;
                    }

                    // Update skin index with validation
                    skinListIndex = currentSkinIndex >= 0 ? currentSkinIndex : 0;

                    // Update skin name with null checks
                    customSkinName = (previewSkinsList != null &&
                                     skinListIndex >= 0 &&
                                     skinListIndex < previewSkinsList.Count)
                        ? previewSkinsList[skinListIndex]?.skinName ?? string.Empty
                        : string.Empty;

                    // Update custom skin index with validation
                    customSkinIndex = (currentSkinsList != null &&
                                      currentSkinIndex >= 0 &&
                                      currentSkinIndex < currentSkinsList.Count)
                        ? currentSkinsList[currentSkinIndex]
                        : 0; // Default value


                    currentSkinFileName = previewSkinsList[skinListIndex].skinFilename;
                    //Debug.Log($"Updated skin data: {customSkinName}");
                }
            }
            public void Update()
            {
                if (characterPanel.playerIsReady)
                {
                    CustominfoSection.transform.localPosition = new Vector3(-38.7277f, -236.8675f, 0f);
                }
                else
                {
                    CustominfoSection.transform.localPosition = new Vector3(-38.7277f, -411.6458f, 0f);
                }
            }

            public void SetSkin(int skinIndex)
            {
                mainTitleSkin.text = previewSkinsList[skinIndex].skinName;
                authorTitle.text = previewSkinsList[skinIndex].authorName;
                skinImage.sprite = previewSkinsList[skinIndex].CSSimage;

                if (previewSkinsList[skinIndex].skinFilename.Contains("base"))
                {
                    CustominfoSection.SetActive(false);
                }
                else
                {
                    CustominfoSection.SetActive(true);
                }
            }
        }

        [HarmonyPatch(typeof(MainMenu), "LoadingFinished")]
        public class Patches
        {

            

            [HarmonyPostfix]
            [HarmonyPatch(typeof(MainMenu), "LoadingFinished")]
            public static void LoadingFinished(MainMenu __instance)
            {
                Plugin.metaDataDict.Clear();

                List<CharacterUIData> UIDatas = Plugin.gameResourcesManager.GetAllCharactersUIData();

                foreach (var character in UIDatas)
                {
                    List<PreviewCustomSkin> prevChar = new List<PreviewCustomSkin>();

                    for (int i = 0; i < character.Skins.Count; i++)
                    {
                        PreviewCustomSkin prev = new PreviewCustomSkin
                        {
                            authorName = "FAIR PLAY LABS",
                            character = character.CharacterCodename,
                            skinIndex = i,
                            skinName = character.Skins[i].DebugName,
                            skinFilename = "base",
                            CSSimage = null,
                        };

                        prevChar.Add(prev);
                    }

                    foreach (var skin1 in customSkinDatas)
                    {
                        if (character.CharacterCodename == skin1.characterCodename)
                        {
                            string debugName = skin1.skinIndex + ":" + skin1.skinID;

                            PreviewCustomSkin prev = new PreviewCustomSkin
                            {
                                authorName = skin1.authorName,
                                character = character.CharacterCodename,
                                skinIndex = skin1.skinIntIndex,
                                skinName = skin1.skinName,
                                skinFilename = skin1.skinID,
                                CSSimage = skin1.stockImageSprite,
                            };

                            prevChar.Add(prev);


                            // Check if a skin with this DebugName already exists
                            if (!character.Skins.Any(s => s.DebugName == debugName))
                            {
                                CharacterUIData.SkinData skinData = new CharacterUIData.SkinData
                                {
                                    DebugName = debugName,
                                    Disabled = false,
                                    DLCIdentifier = null,
                                    CostumeType = CostumeType.None,
                                };
                                //customSkinlist.Add(skin1.skinIndex);
                                character.Skins.Add(skinData);
                            }
                        }
                    }

                    Plugin.previewSkinsDict.Add(character.CharacterCodename, prevChar);
                }


            }


            

            [HarmonyPrefix]
            [HarmonyPriority(Priority.First)]
            [HarmonyPatch(typeof(CharacterPanel), "LoadCharacterGameObject")]
            public static bool LoadCharacterGameObject(CharacterPanel __instance, CharacterCodename codename, int skinID)
            {
                __instance.loadingMesh = true;
                __instance.ToggleLoadingIcon(true);
                string str = "QuantumDB/Characters/" + codename.ToString() + "/";
                string str2 = codename.ToString() + " Costume0" + skinID.ToString() + " Simple Prefab";

                Debug.LogWarning("Trying to load: " + str + str2);
                UnityUtils.LoadAsync<GameObject>(str + str2, delegate (GameObject mesh)
                {
                    if (__instance.sceneLoadManager.Loading)
                    {
                        return;
                    }
                    __instance.loadingMesh = false;
                    __instance.ToggleLoadingIcon(false);
                    if (mesh == null)
                    {
                        return;
                    }
                    __instance.currentCharacterMesh = UnityEngine.Object.Instantiate<GameObject>(mesh, __instance.characterPosition);
                    __instance.currentCharacterMesh.SetActive(true);
                    if (__instance.currentCharacter != codename || __instance.currentSkins[__instance.currentSkin] != skinID)
                    {
                        __instance.UnloadMesh(codename);
                        __instance.LoadCharacterGameObject(__instance.currentCharacter, __instance.currentSkin);
                        return;
                    }
                    if (__instance.randomMode)
                    {
                        __instance.UnloadMesh(codename);
                        return;
                    }
                    if (!__instance.showIntroAnimation)
                    {
                        CharacterAnimationSequence component = __instance.currentCharacterMesh.GetComponent<CharacterAnimationSequence>();
                        if (component == null)
                        {
                            return;
                        }
                        component.StartSequence("idle");
                    }
                });

                return false;
            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(CharacterPanel), "ChangeSkin")]
            public static bool ChangeSkin(CharacterPanel __instance)
            {
                __instance.showIntroAnimation = false;

                __instance.characterSelect.SelectSkin(__instance.currentSkin, __instance.selectorNumber);
                __instance.uiManager.PlaySound(__instance.characterSelect.ChangeSkinSFX);
                if (!__instance.loadingMesh)
                {
                    __instance.UnloadMesh(__instance.currentCharacter);
                    __instance.LoadCharacterGameObject(__instance.currentCharacter, __instance.currentSkins[__instance.currentSkin]);
                }

                return false;
            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(CharacterSelect), "SelectSkin")]
            public static bool SelectSkin(CharacterSelect __instance, int skin, int selectorIndex)
            {
                Debug.LogWarning($"MODDED SelectSkin:: CharacterPanel({selectorIndex}) : {skin}");

                int checkSkin = __instance.GetCharacterPanel(selectorIndex).currentSkins[skin];

                CharacterSelectSelector characterSelectSelector = __instance.Selectors[selectorIndex];
                if (characterSelectSelector.Skins[characterSelectSelector.BrawlerIndex] == skin)
                {
                    return false;
                }

                characterSelectSelector.Skins[characterSelectSelector.BrawlerIndex] = skin;
                if (__instance.dataManager.Online && !characterSelectSelector.OnlineRemotePlayer)
                {
                    __instance.dataManager.PlayersData.LocalPlayers[0].CharacterMatchData.Skin = checkSkin;
                    if (!__instance.inCooldown)
                    {
                        __instance.inCooldown = true;
                        __instance.StartCoroutine(__instance.SelectOnlineSkin(selectorIndex, characterSelectSelector.BrawlerIndex));
                    }
                }

                if (!__instance.dataManager.Online)
                {
                    CharacterUIData characterUIData = Plugin.gameResourcesManager.GetCharacterUIData(characterSelectSelector.Character);

                    string customSkinName = string.Empty;

                    if (characterUIData.Skins[skin].DebugName.Contains(":"))
                    {
                        // Get the DebugName
                        string debugName = characterUIData.Skins[skin].DebugName;

                        string[] parts = debugName.Split(':');

                        if (parts.Length > 1)
                        {
                            customSkinName = parts[1];
                        }
                    }
                    else
                    {
                        customSkinName = "none";
                    }

                    if (Plugin.metaDataDict.TryGetValue(selectorIndex, out var metaData))
                    {
                        CharacterMetaData meta = new CharacterMetaData
                        {
                            playerIndex = selectorIndex,
                            skinIndex = checkSkin,
                            customSkinName = customSkinName,
                            
                        };

                        Plugin.metaDataDict[selectorIndex] = meta;
                    }
                    else
                    {
                        CharacterMetaData meta = new CharacterMetaData
                        {
                            playerIndex = selectorIndex,
                            skinIndex = checkSkin,
                            customSkinName = customSkinName,

                        };

                        Plugin.metaDataDict.Add(selectorIndex, meta);
                    }

                    if (Plugin.metaDataDict.TryGetValue(selectorIndex, out var metaData1))
                    {

                        Debug.Log("METADATA LIST CURRENTLY - SELECTOR : " + selectorIndex + " METADATA SKIN NAME: " + metaData1.customSkinName);
                    }
                }

                

                return false;
            }

            [HarmonyPatch(typeof(CharacterSelect), nameof(CharacterSelect.SelectOnlineSkin))]
            static class SelectOnlineSkinReplacer
            {
                static bool Prefix(CharacterSelect __instance, int selectorIndex, int brawlerIndex, ref IEnumerator __result)
                {
                    // Completely replace the original coroutine
                    __result = CustomSelectOnlineSkin(__instance, selectorIndex, brawlerIndex);

                    return false; // Skip original method
                }

                static IEnumerator CustomSelectOnlineSkin(CharacterSelect __instance, int selectorIndex, int brawlerIndex)
                {
                    // Your custom implementation
                    Debug.Log("CUSTOM ONLINE SKIN");

                    yield return new WaitForSeconds(0.1f); // Modified wait time

                    __instance.inCooldown = false;
                    CharacterSelectSelector characterSelectSelector = __instance.Selectors[selectorIndex];
                    int num = characterSelectSelector.Skins[characterSelectSelector.BrawlerIndex];

                    int checkSkin = __instance.GetCharacterPanel(selectorIndex).currentSkins[num];

                    

                    __instance.onlineManager.Properties.SetPlayerSquadCharacterIndex(brawlerIndex);

                    //__instance.onlineManager.Properties.SetPlayerCharacterSkin(num);
                    CustomSetPlayerCharacterSkin(num, characterSelectSelector.Character);

                    __instance.onlineManager.Properties.SetOnlineSquadSkin(checkSkin, brawlerIndex);
                    yield break;
                }
            }

            public static void CustomSetPlayerCharacterSkin(int skin, CharacterCodename character)
            {

                Debug.LogWarning("CUSTOM SET PLAYER CHARACTER SKIN HAPPENING");
                CharacterUIData characterUIData = Plugin.gameResourcesManager.GetCharacterUIData(character);

                int checkSkin;
                string skinName = "none";

                if (characterUIData.Skins[skin].DebugName.Contains(":"))
                {
                    // Get the DebugName
                    string debugName = characterUIData.Skins[skin].DebugName;

                    string[] parts = debugName.Split(':');

                    // Split at the colon and take the first part
                    string numberPart = debugName.Split(':')[0];

                    // Try to parse the number before colon
                    if (int.TryParse(numberPart, out int skinIndex))
                    {
                        checkSkin = skinIndex;
                    }
                    else
                    {
                        checkSkin = skin;
                    }
                    if (parts.Length > 1)
                    {
                        skinName = parts[1];
                    }
                }
                else
                {
                    checkSkin = skin;
                    skinName = "none";
                }

                Plugin.onlineManager.Properties.SetPlayerRoomProperty(Plugin.onlineManager.Properties.PlayerCharacterSkinID, checkSkin);

                Plugin.onlineManager.Properties.SetPlayerRoomProperty(Plugin.CUSTOM_SKIN_ID, skinName);

                Debug.Log($"Using skin index: {checkSkin}, additional data: '{skinName}'");

            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(OnlineProperties), "SetPlayerCharacterSkin")]
            public static bool SetPlayerCharacterSkin(OnlineProperties __instance, int skin)
            {
                Debug.LogWarning($"MODDED SetPlayerCharacterSkin");

                __instance.SetPlayerRoomProperty(__instance.PlayerCharacterSkinID, skin);

                return false;
            }

            [HarmonyPrefix]
            [HarmonyPatch(typeof(CharacterPanel), "Update")]
            public static bool Update(CharacterPanel __instance)
            {
                if (__instance.inputDeviceIndex == -1)
                {
                    return false;
                }
                if (__instance.characterSelect.StartCoundown)
                {
                    return false;
                }
                if (!__instance.currentSelector.Active)
                {
                    return false;
                }
                if (!__instance.currentSelector.Enabled)
                {
                    return false;
                }
                if (!__instance.randomMode && __instance.inputManager.GetUIInputDown(__instance.inputDeviceIndex, UIKey.BumperR, "", false) && __instance.unlocked)
                {
                    __instance.currentSkin++;
                    if (__instance.currentSkin > __instance.currentSkins.Count - 1)
                    {
                        __instance.currentSkin = 0;
                    }
                    __instance.ChangeSkin();
                }
                if (!__instance.randomMode && __instance.inputManager.GetUIInputDown(__instance.inputDeviceIndex, UIKey.BumperL, "", false) && __instance.unlocked)
                {
                    __instance.currentSkin--;
                    if (__instance.currentSkin < 0)
                    {
                        __instance.currentSkin = __instance.currentSkins.Count - 1;
                    }
                    __instance.ChangeSkin();
                }

                return false;
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(CharacterPanel), "Init")]
            public static void Init(CharacterPanel __instance)
            {
                //Debug.Log("Init happening");
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
        
            [HarmonyPostfix]
            [HarmonyPatch(typeof(CharacterPanel), "ChangeSkin")]
            public static void ChangeSkin_post(CharacterPanel __instance)
            {
                try
                {
                    Plugin.MetaData component = __instance.gameObject.GetComponent<Plugin.MetaData>();
                    bool flag = component != null;
                    if (flag)
                    {
                        component.SetSkin(__instance.currentSkin);
                    }
                }

                catch (Exception ex)
                {
                    // Maneja la excepción, por ejemplo, mostrando un mensaje de error en el log
                    Plugin.Log.LogError($"Error en POSTUpdateData: {ex.Message}");
                }
            }

            public static string GetPlayerCharacterCustomSkin(Player player)
            {
                try
                {
                    object playerRoomProperty = Plugin.onlineManager.Properties.GetPlayerRoomProperty(player, Plugin.CUSTOM_SKIN_ID);
                    if (playerRoomProperty == null)
                    {
                        return "none";
                    }
                    return playerRoomProperty.ToString();
                }
                catch (Exception e)
                {
                    Plugin.Log.LogError($"Error getting custom skin: {e}");
                    return "none";
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(CharacterManager), "OnInstantiated")]
            public static void OnInstantiated(CharacterManager __instance, QuantumGame game)
            {
                Debug.Log("INSTANTIATING CHARACTER...");

                if (!dataManager.Online)
                {
                    if(Plugin.metaDataDict.TryGetValue(__instance.Data.Character.index, out var metaData))
                    {
                        if (Plugin.dictCustomSkinDatas.TryGetValue(metaData.customSkinName, out var customSkinData))
                        {
                            CustomSkinData skinData = customSkinData;
                            CharacterMaterialOverridesHandler characterMaterialOverridesHandler = GetCharacterMaterialOverridesHandler(__instance);
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

                            foreach (CharacterMaterialOverridesHandler.MaterialOverrideGroup originalMOGroup in skinData.CustomMOGList)
                            {
                                if (originalMOGroup == null) continue;

                                var MOGroup = CloneMOGroup(originalMOGroup);
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

                            return;
                        }
                    }
                }

                foreach (var player in onlineManager.GetPlayersList())
                {
                    Plugin.Log.LogWarning(player.NickName);

                    int baseSkin = Plugin.onlineManager.Properties.GetPlayerCharacterSkin(player);
                    string customSkin = Plugin.Patches.GetPlayerCharacterCustomSkin(player);


                    Plugin.Log.LogWarning("Character Skin: " + baseSkin + " - Custom Skin Name: " + customSkin);


                    if(player.NickName == __instance.Data.Character.nickName)
                    {
                        if(Plugin.dictCustomSkinDatas.TryGetValue(customSkin, out var customSkinData))
                        {
                            Plugin.Log.LogWarning("Skin: " + customSkin + " found for Player: " + player.NickName);

                            CustomSkinData skinData = customSkinData;
                            CharacterMaterialOverridesHandler characterMaterialOverridesHandler = GetCharacterMaterialOverridesHandler(__instance);
                            if (characterMaterialOverridesHandler == null)
                            {
                                Debug.LogError("CharacterMaterialOverridesHandler is null!");
                                return;
                            }

                            if (skinData.CustomMOGList == null) return;

                            foreach (CharacterMaterialOverridesHandler.MaterialOverrideGroup originalMOGroup in skinData.CustomMOGList)
                            {
                                if (originalMOGroup == null) continue;

                                var MOGroup = CloneMOGroup(originalMOGroup);
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
                if(materialOverrideID == "Shield_Default" || materialOverrideID == "Shield_PerfectBlock" || materialOverrideID == "Possessed" || materialOverrideID == "Clone")
                {
                    return true;
                }

                //Plugin.Log.LogInfo("Apply");

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


                CharacterManager characterManager;

                GameObject gameObject = __instance.gameObject.transform.parent.gameObject;
                Transform gameObject1 = gameObject.transform.Find("Character");
                characterManager = gameObject1.gameObject.GetComponent<CharacterManager>();


                foreach (var textureOverrideTarget in materialOverrideGroup.Targets)
                {
                    if (textureOverrideTarget.Target == null)
                    {
                        
                        continue;
                    }

                    try
                    {
                        string originalIdentifier1 = materialOverrideID;
                        //Debug.Log($"Original Material Override ID: {materialOverrideID}");

                        // Check if the identifier contains a colon and remove the prefix if it's there
                        int colonIndex = originalIdentifier1.IndexOf(':');
                        if (colonIndex != -1)
                        {
                            //Debug.Log($"Colon found at index {colonIndex}. Removing prefix.");
                            // Remove everything before and including the colon
                            originalIdentifier1 = originalIdentifier1.Substring(colonIndex + 1);
                            //Debug.Log($"Material Override ID after prefix removal: {originalIdentifier1}");
                        }
                        else
                        {
                            //Debug.Log("No colon found in Material Override ID.");
                        }

                        // Get the current material name and remove "(Instance)" suffix if it exists
                        try
                        {
                            var materialName = textureOverrideTarget.Target.sharedMaterials[textureOverrideTarget.MaterialIndex].name;
                            if (materialName.EndsWith(" (Instance)"))
                            {
                                materialName = materialName.Substring(0, materialName.Length - 11); // Remove the last 11 characters
                                //Debug.Log($"Material name after removing (Instance): {materialName}");
                            }
                            else
                            {
                                //Debug.Log($"Material name does not contain (Instance): {materialName}");
                            }

                            //Debug.Log($"Material Index: {textureOverrideTarget.MaterialIndex}");
                            //Debug.Log($"Shared Materials Count: {textureOverrideTarget.Target.sharedMaterials.Length}");

                            // Perform the comparison
                            if (materialName != originalIdentifier1)
                            {
                                //Debug.LogWarning($"Material name '{materialName}' does not match the override ID '{originalIdentifier1}'. Returning false.");
                                continue;
                            }
                            else
                            {
                                //Debug.Log($"Material name '{materialName}' matches the override ID '{originalIdentifier1}'.");
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }

                        if (dataManager.Online)
                        {
                            foreach (var player in onlineManager.GetPlayersList())
                            {
                                Plugin.Log.LogWarning(player.NickName);

                                int baseSkin = Plugin.onlineManager.Properties.GetPlayerCharacterSkin(player);
                                string customSkin = Plugin.Patches.GetPlayerCharacterCustomSkin(player);

                                if (player.NickName == characterManager.Data.Character.nickName)
                                {
                                    if (Plugin.dictCustomSkinDatas.TryGetValue(customSkin, out var customSkinData))
                                    {
                                        CustomSkinData skinData = customSkinData;

                                        //Debug.Log("Skin index matched, trying to change shader");
                                        if (skinData.shaderToUse.ContainsKey(materialOverrideID))
                                        {
                                            //Debug.Log($"Shader found for identifier: {materialOverrideID}");
                                            Shader shader = skinData.shaderToUse[materialOverrideID];

                                            if (char.IsDigit(materialOverrideID[0]))
                                            {
                                                try
                                                {
                                                    // Convert the first character to an integer
                                                    int firstDigit = int.Parse(materialOverrideID[0].ToString());
                                                    //Debug.Log($"First digit as int: {firstDigit}");
                                                    //Debug.Log($"Applying shader: {shader.name}");

                                                    // Log the length of the materials array
                                                    //Debug.Log($"Materials array length: {textureOverrideTarget.Target.materials.Length}");

                                                    // Ensure firstDigit is within bounds of the materials array
                                                    if (firstDigit < textureOverrideTarget.Target.materials.Length)
                                                    {

                                                        Shader baseShader = Shader.Find(shader.name);
                                                        if (baseShader != null)
                                                        {
                                                            textureOverrideTarget.Target.materials[firstDigit].shader = baseShader;
                                                            //Debug.Log($"OG Shader applied to material index: {firstDigit}");
                                                        }
                                                        else
                                                        {
                                                            textureOverrideTarget.Target.materials[firstDigit].shader = shader;
                                                            //Debug.Log($"Custom Shader applied to material index: {firstDigit}");
                                                        }

                                                    }
                                                    else
                                                    {
                                                        //Debug.LogError($"Material index {firstDigit} is out of range. Materials length: {textureOverrideTarget.Target.materials.Length}");
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    //Debug.LogException(e);
                                                }
                                            }
                                        }
                                        if (skinData.customMaterials.ContainsKey(materialOverrideID))
                                        {
                                            //Debug.Log($"Custom Material found for identifier: {materialOverrideID}");
                                            Material material = skinData.customMaterials[materialOverrideID];
                                            Shader baseShader = Shader.Find(material.shader.name);
                                            if (baseShader != null)
                                            {
                                                //Debug.LogWarning("SHADER FOUND, SWAPPING...");
                                                material.shader = baseShader;

                                            }
                                            else
                                            {
                                                //Debug.LogWarning("Shader not found with name: " + material.shader.name);
                                            }
                                            if (char.IsDigit(materialOverrideID[0]))
                                            {
                                                try
                                                {
                                                    // Convert the first character to an integer
                                                    int firstDigit = int.Parse(materialOverrideID[0].ToString());
                                                    //Debug.Log($"First digit as int: {firstDigit}");
                                                    //Debug.Log($"Applying shader: {material.name}");

                                                    // Log the length of the materials array
                                                    //Debug.Log($"Materials array length: {textureOverrideTarget.Target.materials.Length}");

                                                    // Ensure firstDigit is within bounds of the materials array
                                                    if (firstDigit < textureOverrideTarget.Target.materials.Length)
                                                    {
                                                        Material[] sharedMaterialsArray = textureOverrideTarget.Target.sharedMaterials;

                                                        // Modify the specific material in the array
                                                        sharedMaterialsArray[firstDigit] = material;

                                                        // Assign the modified array back to the renderer
                                                        textureOverrideTarget.Target.sharedMaterials = sharedMaterialsArray;

                                                        //Debug.Log($"Material successfully applied to index: {firstDigit}");
                                                    }
                                                    else
                                                    {
                                                        //Debug.LogError($"Material index {firstDigit} is out of range. Materials length: {textureOverrideTarget.Target.materials.Length}");
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    //Debug.LogException(e);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Plugin.metaDataDict.TryGetValue(characterManager.Data.Character.index, out var metaData))
                            {
                                if (Plugin.dictCustomSkinDatas.TryGetValue(metaData.customSkinName, out var customSkinData))
                                {
                                    CustomSkinData skinData = customSkinData;
                                    //Debug.Log("Skin index matched, trying to change shader");
                                    if (skinData.shaderToUse.ContainsKey(materialOverrideID))
                                    {
                                        //Debug.Log($"Shader found for identifier: {materialOverrideID}");
                                        Shader shader = skinData.shaderToUse[materialOverrideID];

                                        if (char.IsDigit(materialOverrideID[0]))
                                        {
                                            try
                                            {
                                                // Convert the first character to an integer
                                                int firstDigit = int.Parse(materialOverrideID[0].ToString());
                                                //Debug.Log($"First digit as int: {firstDigit}");
                                                //Debug.Log($"Applying shader: {shader.name}");

                                                // Log the length of the materials array
                                                //Debug.Log($"Materials array length: {textureOverrideTarget.Target.materials.Length}");

                                                // Ensure firstDigit is within bounds of the materials array
                                                if (firstDigit < textureOverrideTarget.Target.materials.Length)
                                                {

                                                    Shader baseShader = Shader.Find(shader.name);
                                                    if (baseShader != null)
                                                    {
                                                        textureOverrideTarget.Target.materials[firstDigit].shader = baseShader;
                                                        //Debug.Log($"OG Shader applied to material index: {firstDigit}");
                                                    }
                                                    else
                                                    {
                                                        textureOverrideTarget.Target.materials[firstDigit].shader = shader;
                                                        //Debug.Log($"Custom Shader applied to material index: {firstDigit}");
                                                    }

                                                }
                                                else
                                                {
                                                    //Debug.LogError($"Material index {firstDigit} is out of range. Materials length: {textureOverrideTarget.Target.materials.Length}");
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                //Debug.LogException(e);
                                            }
                                        }
                                    }
                                    if (skinData.customMaterials.ContainsKey(materialOverrideID))
                                    {
                                        //Debug.Log($"Custom Material found for identifier: {materialOverrideID}");
                                        Material material = skinData.customMaterials[materialOverrideID];
                                        Shader baseShader = Shader.Find(material.shader.name);
                                        if (baseShader != null)
                                        {
                                            //Debug.LogWarning("SHADER FOUND, SWAPPING...");
                                            material.shader = baseShader;

                                        }
                                        else
                                        {
                                            //Debug.LogWarning("Shader not found with name: " + material.shader.name);
                                        }
                                        if (char.IsDigit(materialOverrideID[0]))
                                        {
                                            try
                                            {
                                                // Convert the first character to an integer
                                                int firstDigit = int.Parse(materialOverrideID[0].ToString());
                                                //Debug.Log($"First digit as int: {firstDigit}");
                                                //Debug.Log($"Applying shader: {material.name}");

                                                // Log the length of the materials array
                                                //Debug.Log($"Materials array length: {textureOverrideTarget.Target.materials.Length}");

                                                // Ensure firstDigit is within bounds of the materials array
                                                if (firstDigit < textureOverrideTarget.Target.materials.Length)
                                                {
                                                    Material[] sharedMaterialsArray = textureOverrideTarget.Target.sharedMaterials;

                                                    // Modify the specific material in the array
                                                    sharedMaterialsArray[firstDigit] = material;

                                                    // Assign the modified array back to the renderer
                                                    textureOverrideTarget.Target.sharedMaterials = sharedMaterialsArray;

                                                    //Debug.Log($"Material successfully applied to index: {firstDigit}");
                                                }
                                                else
                                                {
                                                    //Debug.LogError($"Material index {firstDigit} is out of range. Materials length: {textureOverrideTarget.Target.materials.Length}");
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                //Debug.LogException(e);
                                            }
                                        }
                                    }

                                }

                            }
                        }

                        

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
                    catch(Exception e) 
                    {
                        Debug.LogException(e);
                    }

                    
                }

                __result = true;
                return false;
            }

            public static CharacterMaterialOverridesHandler GetCharacterMaterialOverridesHandler(CharacterManager characterManager)
            {
                CharacterMaterialOverridesHandler characterMaterialOverride;

                GameObject parentObject = characterManager.gameObject.transform.parent.gameObject;
                //Debug.Log("Parent GameObject: " + parentObject.name);

                Transform childTransform = parentObject.transform.Find("GeneralMaterialOverrides");
                if(childTransform != null)
                {
                    GameObject childObject = childTransform.gameObject;
                    //Debug.Log("GeneralMaterialOverrides GameObject found: " + childObject.name);

                    characterMaterialOverride = childObject.GetComponent<CharacterMaterialOverridesHandler>();
                    if (characterMaterialOverride == null)
                    {
                        //Debug.LogWarning("CharacterMaterialOverridesHandler not found on GeneralMaterialOverrides!");
                        return null;
                    }
                    else
                    {
                        return characterMaterialOverride;
                    }
                }
                else
                {
                    //Debug.Log("cannot find GeneralMaterialOverrides");
                }

                return null;
            }

            public static Texture2D getTexture2D()
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                Texture2D texture2D = new Texture2D(1, 1);
                using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("CheapSkins.Resources.fpl.png"))
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

            /*[HarmonyPrefix]
            [HarmonyPatch(typeof(CharacterMaterialOverridesHandler), "Awake")]
            public static bool Awake(CharacterMaterialOverridesHandler __instance)
            {
                if (__instance.mpb == null)
                {
                    __instance.mpb = new MaterialPropertyBlock();
                }
                if (!string.IsNullOrEmpty(__instance.InitialOverride))
                {
                    __instance.ApplyMaterialOverride(__instance.InitialOverride);
                }

                return false;
            }*/

            public static void ProcessRenderers(GameObject obj)
            {
                // Find all SkinnedMeshRenderers and MeshRenderers in the target object and its children
                SkinnedMeshRenderer[] skinnedMeshRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>(true);
                CharacterObjects characterObjects = obj.GetComponentInChildren<CharacterObjects>();

                foreach(var obj1 in characterObjects.MultipleConstraintObjectsList)
                {
                    Plugin.Log.LogWarning(obj1.VisibleObjects[0].name);
                }

                // Process SkinnedMeshRenderers
                /*foreach (SkinnedMeshRenderer skinnedRenderer in skinnedMeshRenderers)
                {
                    ProcessMaterials(skinnedRenderer.sharedMaterials, skinnedRenderer.name);
                }

                // Process MeshRenderers
                foreach (MeshRenderer meshRenderer in meshRenderers)
                {
                    ProcessMaterials(meshRenderer.sharedMaterials, meshRenderer.name);
                }*/
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


