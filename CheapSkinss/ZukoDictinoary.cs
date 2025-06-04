using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class ZukoDictinoary
    {
        public static List<string> Stevia0Exp = new List<string>
        {
            "arms_C_mesh",
            "hair_C_mesh",
            "hair_inv_C_mesh",
            "angry_C_mesh",
            "angry_inv_C_mesh",
            "arrogant_C_mesh",
            "arrogant_inv_C_mesh",
            "breath_C_mesh",
            "concentrate_C_mesh",
            "concentrate_inv_C_mesh",
            "default_C_mesh",
            "default_inv_C_mesh",
            "idle_C_mesh",
            "idle_inv_C_mesh",
            "mouthwide_C_mesh",
            "mouthwide_inv_C_mesh",
            "surprise_C_mesh",
            "surprise_inv_C_mesh",
            "downeyelid_R_mesh",
            "downeyelid_L_mesh",
            "upeyelid_R_mesh",
            "upeyelid_L_mesh",
            "downeyelid_inv_L_mesh",
            "downeyelid_inv_R_mesh",
            "upeyelid_inv_L_mesh",
            "upeyelid_inv_R_mesh",
            "hands_C_mesh",
            "costume03_hair_C_mesh",
            "costume03_hair_inv_C_mesh"

        };
        public static List<string> Stevia0Costume00Mat = new List<string>
        {
            "outershirt_C_mesh",
            "outershirt_inv_C_mesh",
            "undershirt_C_mesh",
            "undershirt_inv_C_mesh",
        };
        public static List<string> Stevia1Costume01Mat = new List<string>
        {
            "costume01_C_mesh",
            "costume01_hair_C_mesh",
        };
        public static List<string> Stevia0Sword = new List<string>
        {
            "sword_C_mesh",
            "swordsheath_C_mesh",
            "swordsheath_handle_C_mesh",
        };

        public static List<string> Stevia1ExpCostume01_Mat = new List<string>
        {
            "hands_C_mesh",
        };
        public static List<string> Stevia_Costume02_Mat = new List<string>
        {
            "costume02_C_mesh",
            "costume02_inv_C_mesh",
            "costume02mask_C_mesh",
        };
        public static List<string> Stevia_Costume03_Mat = new List<string>
        {
            "costume03_C_mesh",
            "costume03_inv_C_mesh",
            "costume03_pants_C_mesh",
            "costume03_pants_inv_C_mesh"
        };

        public static List<string> Stevia_Costume02_Mat2 = new List<string>
{
    "costume02_C_mesh",
    "costume02_inv_C_mesh",
    "costume02mask_C_mesh"
};

        public static List<string> Stevia_Expressions_Mat2 = new List<string>
{
    "angry_C_mesh",
    "angry_inv_C_mesh",
    "arrogant_C_mesh",
    "arrogant_inv_C_mesh",
    "breath_C_mesh",
    "breath_inv_C_mesh",
    "concentrate_C_mesh",
    "concentrate_inv_C_mesh",
    "default_C_mesh",
    "default_inv_C_mesh",
    "idle_C_mesh",
    "idle_inv_C_mesh",
    "mouthwide_C_mesh",
    "mouthwide_inv_C_mesh",
    "surprise_C_mesh",
    "surprise_inv_C_mesh",
    "downeyelid_L_mesh",
    "downeyelid_R_mesh",
    "upeyelid_L_mesh",
    "upeyelid_R_mesh",
    "downeyelid_inv_L_mesh",
    "downeyelid_inv_R_mesh",
    "upeyelid_inv_L_mesh",
    "upeyelid_inv_R_mesh",
    "hands_C_mesh"
};

        public static Dictionary<string, List<string>> Stevia0Parts = new Dictionary<string, List<string>>
        {
            { "Stevia_Expressions_Mat", Stevia0Exp },
            { "Stevia_Costume00_Mat", Stevia0Costume00Mat },
            { "Stevia_Sword_Mat", Stevia0Sword },
        };
        public static Dictionary<string, List<string>> Stevia1Parts = new Dictionary<string, List<string>>
        {
            { "Stevia_Costume01_Mat", Stevia1Costume01Mat },
            { "Stevia_Expressions_Mat", Stevia0Exp},
            { "Stevia_Expressions_Costume01_Mat", Stevia1ExpCostume01_Mat},
            { "Stevia_Sword_Mat", Stevia0Sword }

        };
        public static Dictionary<string, List<string>> Stevia2Parts = new Dictionary<string, List<string>>
        {
            { "Stevia_Costume02_Mat", Stevia_Costume02_Mat2},
            { "Stevia_Expressions_Mat", Stevia_Expressions_Mat2 },
            { "Stevia_Sword_Mat", Stevia0Sword }

        };
        public static Dictionary<string, List<string>> Stevia3Parts = new Dictionary<string, List<string>>
        {
            { "Stevia_Costume03_Mat", Stevia1Costume01Mat },
            { "Stevia_Expressions_Mat", Stevia0Exp },
            { "Stevia_Sword_Mat", Stevia0Sword }

        };
        public static Dictionary<int, Dictionary<string, List<string>>> SteviaAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, Stevia0Parts},
            { 1, Stevia1Parts},
            { 2, Stevia2Parts},
            { 3, Stevia3Parts}
        };

    }
}
