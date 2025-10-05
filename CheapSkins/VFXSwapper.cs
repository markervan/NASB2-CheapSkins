using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(fileName = "VFXSwapper", menuName = "ScriptableObjects/VFXSwapper", order = 2)]
public class VFXSwapper
{
    public List<VfxOverrideGroup> VfxOverrides = new List<VfxOverrideGroup>();


    [System.Serializable]
    public class VfxOverrideGroup
    {
        public string identifier;
        public Material customMaterial;
        public List<TextureOverride> textureOverrides;
        public List<AttributeOverride> attributeOverrides;
        public List<Vector4AttributeOverride> vectorAttributeOverrides;
        public List<ColorOverride> colorOverrides;
    }

    [Serializable]
    public class TextureOverride
    {
        public string TextureID;
        public Texture2D TextureRef;
    }

    [Serializable]
    public class AttributeOverride
    {
        public enum AttributeNumberTypes
        {
            Integer = 0,
            Float = 1
        }
        public AttributeNumberTypes AttributeType;
        public string AttributeID;
        public float AttributeValue;
    }

    [Serializable]
    public class Vector4AttributeOverride
    {
        public string AttributeID;
        public Vector4 AttributeValue;
    }

    [Serializable]
    public class ColorOverride
    {
        public string ColorID;
        [ColorUsage(true, true)]
        public Color ColorValue;
    }
}
