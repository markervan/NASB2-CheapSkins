using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class Azula
    {
        public static List<string> Azula_Costume_01_Mat = new List<string>
        {
            "costume00_cloth_C_mesh",
            "costume00_skirt_C_mesh"
        };

        public static List<string> Azula_Expressions_Mat = new List<string>
        {
            "arrogant_C_mesh",
            "attack01_C_mesh",
            "attack03_C_mesh",
            "attack04_C_mesh",
            "default_C_mesh",
            "hurt01_C_mesh",
            "hurt02_C_mesh",
            "idle_C_mesh",
            "sidesmile_C_mesh",
            "sleep_C_mesh",
            "smileteeth_C_mesh",
            "eyelash_L_mesh",
            "eyelash_R_mesh",
            "downeyelid_L_mesh",
            "downeyelid_R_mesh",
            "upeyelid_L_mesh",
            "upeyelid_R_mesh",
            "hair_C_mesh",
            "hand_C_mesh"
        };

        public static Dictionary<string, List<string>> Azula0Parts = new Dictionary<string, List<string>>()
        {
            { "Azula_Costume_01_Mat", Azula_Costume_01_Mat },
            { "Azula_Expressions_Mat", Azula_Expressions_Mat }
        };
        //1

        public static List<string> Azula_Costume_02_Mat1 = new List<string>
        {
            "costume01_apron_C_mesh",
            "costume01_cloth_C_mesh",
            "costume01_skirt_C_mesh"
        };

        public static List<string> Azula_Expressions_Mat1 = new List<string>
        {
            "costume01_arm_C_mesh",
            "arrogant_C_mesh",
            "attack01_C_mesh",
            "attack03_C_mesh",
            "attack04_C_mesh",
            "default_C_mesh",
            "hurt01_C_mesh",
            "hurt02_C_mesh",
            "idle_C_mesh",
            "sidesmile_C_mesh",
            "sleep_C_mesh",
            "smileteeth_C_mesh",
            "eyelash_L_mesh",
            "eyelash_R_mesh",
            "downeyelid_L_mesh",
            "downeyelid_R_mesh",
            "upeyelid_L_mesh",
            "upeyelid_R_mesh",
            "hair_C_mesh"
        };

        public static List<string> Azula_Expressions_HandMask_Mat1 = new List<string>
        {
            "hand_C_mesh"
        };

        public static Dictionary<string, List<string>> Azula1Parts = new Dictionary<string, List<string>>()
        {
            { "Azula_Costume_02_Mat", Azula_Costume_02_Mat1 },
            { "Azula_Expressions_Mat", Azula_Expressions_Mat1 },
            { "Azula_Expressions_HandMask_Mat", Azula_Expressions_HandMask_Mat1 }
        };

        //2
        public static List<string> Azula_Costume_03_Mat2 = new List<string>
{
    "costume02_C_mesh"
};

        public static List<string> Azula_Expressions_Mat2 = new List<string>
{
    "arrogant_C_mesh",
    "attack01_C_mesh",
    "attack03_C_mesh",
    "attack04_C_mesh",
    "default_C_mesh",
    "hurt01_C_mesh",
    "hurt02_C_mesh",
    "idle_C_mesh",
    "sidesmile_C_mesh",
    "sleep_C_mesh",
    "smileteeth_C_mesh",
    "eyelash_L_mesh",
    "eyelash_R_mesh",
    "downeyelid_L_mesh",
    "downeyelid_R_mesh",
    "upeyelid_L_mesh",
    "upeyelid_R_mesh",
    "hair_C_mesh"
};

        public static List<string> Azula_Expressions_HandMask_Mat2 = new List<string>
{
    "hand_C_mesh"
};

        public static Dictionary<string, List<string>> Azula2Parts = new Dictionary<string, List<string>>()
        {
            { "Azula_Costume_03_Mat", Azula_Costume_03_Mat2 },
            { "Azula_Expressions_Mat", Azula_Expressions_Mat2 },
            { "Azula_Expressions_HandMask_Mat", Azula_Expressions_HandMask_Mat2 }
        };
        //3
        public static List<string> Azula_Costume_04_Mat3 = new List<string>
{
    "costume03_cloth_C_mesh",
    "costume03_hair_C_mesh"
};

        public static List<string> Azula_Expressions_Costume_04_Mat3 = new List<string>
{
    "arrogant_C_mesh",
    "attack01_C_mesh",
    "attack03_C_mesh",
    "attack04_C_mesh",
    "default_C_mesh",
    "hurt01_C_mesh",
    "hurt02_C_mesh",
    "idle_C_mesh",
    "sidesmile_C_mesh",
    "sleep_C_mesh",
    "smileteeth_C_mesh",
    "eyelash_L_mesh",
    "eyelash_R_mesh",
    "downeyelid_L_mesh",
    "downeyelid_R_mesh",
    "upeyelid_L_mesh",
    "upeyelid_R_mesh"
};

        public static Dictionary<string, List<string>> Azula3Parts = new Dictionary<string, List<string>>()
{
    { "Azula_Costume_04_Mat", Azula_Costume_04_Mat3 },
    { "Azula_Expressions_Costume_04_Mat", Azula_Expressions_Costume_04_Mat3 }
};

        public static Dictionary<int, Dictionary<string, List<string>>> AzulaAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, Azula0Parts},
            { 1, Azula1Parts},
            { 2, Azula2Parts},
            { 3, Azula3Parts},

        };
    }
}
