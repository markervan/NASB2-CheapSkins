using Quantum;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
    public VFXSwapper customVFXData;
    public Dictionary<string, CharacterAnimatorStateAsset> customAnimatorBehaviours = new Dictionary<string, CharacterAnimatorStateAsset>();


}