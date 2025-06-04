using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class DannyDictionaries
    {
        public static List<string> Danny0body = new List<string>
        {
            "body_C_mesh",
            "legs_C_mesh",
            "hands_C_mesh",
            "fist_C_mesh"
        };
        public static List<string> Danny1body = new List<string>
        {
            "costume01_body_C_mesh",
            "costume01_fist_C_mesh",
            "costume01_hands_C_mesh",
            "costume01_legs_C_mesh"
        };

        public static List<string> Danny0expressions = new List<string>
        {
            "attack01_C_mesh",
            "attack02_C_mesh",
            "attack03_front_C_mesh",
            "attack04_C_mesh",
            "default_C_mesh",
            "down_C_mesh",
            "head_front_C_mesh",
            "hurt1_C_mesh",
            "hurt02_C_mesh",
            "idle_C_mesh",
            "mad_C_mesh",
            "openmouth_C_mesh",
            "sleeplose_C_mesh",
            "taunt_C_mesh",
            "neck_default_C_mesh",
            "downeyelid_R_mesh",
            "downeyelid_L_mesh",
            "upeyelid_R_mesh",
            "upeyelid_L_mesh",
            "hair_attack03_front_C_mesh",
            "hair_C_mesh",
            "hair_front_C_mesh"

        };


        public static List<string> Danny1expressions = new List<string>
        {
            "attack01_C_mesh",
            "attack02_C_mesh",
            "attack03_front_C_mesh",
            "attack04_C_mesh",
            "default_C_mesh",
            "down_C_mesh",
            "head_front_C_mesh",
            "hurt1_C_mesh",
            "hurt02_C_mesh",
            "idle_C_mesh",
            "mad_C_mesh",
            "openmouth_C_mesh",
            "sleeplose_C_mesh",
            "taunt_C_mesh",
            "hairdefault_costume03_C_mesh",
            "hairattack03_costume03_C_mesh",
            "downeyelid_R_mesh",
            "downeyelid_L_mesh",
            "upeyelid_R_mesh",
            "upeyelid_L_mesh",
            "hair_attack03_front_C_mesh",
            "hair_C_mesh",
            "hair_front_C_mesh"

        };

        public static List<string> Danny3expressions = new List<string>
        {
            "attack01_C_mesh",
            "attack02_C_mesh",
            "attack03_front_C_mesh",
            "attack04_C_mesh",
            "default_C_mesh",
            "down_C_mesh",
            "head_front_C_mesh",
            "hurt1_C_mesh",
            "hurt02_C_mesh",
            "idle_C_mesh",
            "mad_C_mesh",
            "openmouth_C_mesh",
            "sleeplose_C_mesh",
            "taunt_C_mesh",
            "neck_default_C_mesh",
            "hairdefault_costume03_C_mesh",
            "hairattack03_costume03_C_mesh",
            "downeyelid_R_mesh",
            "downeyelid_L_mesh",
            "upeyelid_R_mesh",
            "upeyelid_L_mesh"
        };
        public static List<string> Danny1visor = new List<string>
        {
            "costume01_glassesdefault_C_inv_mesh",
            "costume01_glassesdefault_C_mesh",
            "costume01_glassesfront_C_mesh"
        };
        public static List<string> Danny1neck = new List<string>
        {
            "neck_default_C_mesh",
        };


        public static Dictionary<string, List<string>> Danny0Parts = new Dictionary<string, List<string>>
        {
            { "DannyPhantom_Body_Mat", Danny0body },
            { "DannyPhantom_Expressions_Mat", Danny0expressions }
        };
        public static Dictionary<string, List<string>> Danny1Parts = new Dictionary<string, List<string>>
        {
            { "DannyPhantom_Costume_04_Body_Mat", Danny1body },
            { "DannyPhantom_Expressions_Mat", Danny3expressions },
            { "DannyPhantom_Neck_Costume04_Mat",  Danny1expressions},
            { "DannyPhantom_Costume_04_Visor_Mat",  Danny1visor}
        };
        public static Dictionary<string, List<string>> Danny3Parts = new Dictionary<string, List<string>>
        {
            { "DannyPhantom_Body_Mat", Danny0body },
            { "DannyPhantom_Expressions_Costume03_Mat", Danny3expressions }

        };
        public static Dictionary<int, Dictionary<string, List<string>>> DannyAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, Danny0Parts},
            { 1, Danny1Parts},
            { 2, Danny0Parts},
            { 3, Danny3Parts}
        };
    }
}
