using CheapSkinss;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static CheapSkinss.Plugin;

public class SkinsHolder : MonoBehaviour
{
    public Dictionary<string, CustomSkinData> dictCustomSkinDatas = new Dictionary<string, CustomSkinData>();
    public UIKey UIKey;
    //public List<Plugin.OnlineSkinSet> skinsSet = new List<Plugin.OnlineSkinSet>();
    public List<OnlineSkinSet> onlineSkinSets = new List<OnlineSkinSet>();
}