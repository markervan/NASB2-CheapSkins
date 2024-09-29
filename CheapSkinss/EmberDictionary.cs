using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class EmberDictionary
    {
        public static List<string> Ember1body = new List<string>
        {
            "costume01_hand_default_L_mesh",
            "costume01_hand_default_R_mesh",
            "costume01_piratearrings_C_mesh",
            "costume01_piratebody_C_mesh",
            "costume01_piratecape_C_mesh",
            "costume01_piratehair_C_mesh",
            "costume01_piratehairtail_C_mesh",
            "costume01_piratehat_C_mesh",
            "downeyelid_L_mesh",
            "downeyelid_R_mesh",
            "upeyelid_L_mesh",
            "upeyelid_R_mesh"
        };
        public static List<string> Ember1madexpression = new List<string>
        {
            "mad_C_mesh",
        };
        public static List<string> Ember0body = new List<string>
        {
            "costume00_body_C_mesh",
            "costume00_earrings_C_mesh",
            "costume00_hair_C_mesh",
            "costume00_hairTail_C_mesh",
            "costume00_hand_default_L_mesh",
            "mad_C_mesh",
            "costume00_hand_default_R_mesh",
            "downeyelid_L_mesh",
            "downeyelid_R_mesh",
            "upeyelid_L_mesh",
            "upeyelid_R_mesh"
        };
        public static List<string> Ember0expressions = new List<string>
        {
            "attack1_C_mesh",
            "attack2_C_mesh",
            "default_C_mesh",
            "hurt_C_mesh",
            "idle_C_mesh",
            "lose_C_mesh",
            "smile_C_mesh",
            "taunt_C_mesh"
        };
        public static List<string> Ember0guitar = new List<string>
        {
            "guitar_C_mesh",
        };
        public static List<string> Ember0eyes = new List<string>
        {
            "pupil_L_mesh",
            "pupil_R_mesh"
        };
        public static Dictionary<string, List<string>> Ember0Parts = new Dictionary<string, List<string>>
        {
            { "Ember_Costume_01_Mat", Ember0body},
            { "Ember_Costume_03_Mat", Ember0expressions},
            { "Ember_Guitar_Mat", Ember0guitar},
            { "Ember_Eye_Mat", Ember0eyes}
        };
        public static Dictionary<string, List<string>> Ember1Parts = new Dictionary<string, List<string>>
        {
            { "Ember_Costume_02_Mat", Ember1body},
            { "Ember_Costume_03_Mat", Ember0expressions},
            { "Ember_Costume_01_Mat", Ember1madexpression},
            { "Ember_Guitar_Mat", Ember0guitar},
            { "Ember_Eye_Mat", Ember0eyes}
        };
        public static Dictionary<int, Dictionary<string, List<string>>> EmberAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, Ember0Parts},
            { 1, Ember1Parts},
            { 4, Ember0Parts},
            { 5, Ember0Parts}
        };





    }
}
