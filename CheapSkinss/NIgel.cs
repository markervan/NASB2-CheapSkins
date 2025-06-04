using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class NIgel
    {
        public static List<string> Nigel_Mat = new List<string>
        {
            "arms_C_mesh",
            "costume00_legs_C_mesh",
            "costume00_pants_C_mesh",
            "costume00_tshirt_C_mesh",
            "default_C_mesh",
            "smile_C_mesh",
            "downeyelid_L_mesh",
            "downeyelid_R_mesh",
            "upeyelid_L_mesh",
            "upeyelid_R_mesh"
        };

        public static List<string> Nigel_Costume_02_Mat = new List<string>
        {
            "costume00_hat_C_inv_mesh",
            "costume00_hat_C_mesh",
            "costume00_hat_C_inv_mesh",
            "costume01_hat_C_mesh",
            "costume01_legs_C_mesh",
            "costume01_pants_C_mesh",
            "costume01_sweater_C_mesh",
            "default_hat",
            "excited_hat",
            "hurt_hat",
            "default_hat_inv",
            "excited_hat_inv",
            "hurt_hat_inv"
        };

        public static List<string> Nigel_Expressions_Mat = new List<string>
        {
            "excited_C_mesh",
            "hurt_C_mesh",
            "smile_C_mesh"
        };
        public static List<string> Nigel_HairMask_Mat = new List<string>
        {
            "default_C_mesh"
        };
        public static List<string> Nigel_Expressions_Hairmask_Mat = new List<string>
        {
            "excited_C_mesh",
            "hurt_C_mesh",
            "smile_C_mesh"
        };

        public static List<string> Nigel_Crocodile_Mat = new List<string>
        {
            "crocodile_C_mesh",
            "eagle_C_mesh"
        };

        public static List<string> Nigel_Elephant_Mat = new List<string>
        {
            "elephant_C_mesh"
        };

        public static List<string> Nigel_PropsRig_Mat = new List<string>
        {
            "grappling_hook_C_mesh",
            "binoculars_C_mesh",
            "boxing_glove_right_C_mesh",
            "boxing_glove_left_C_mesh",
            "buriedHead_C_mesh",
            "coconut_C_mesh",
            "kettle_C_mesh",
            "plate_C_mesh",
            "teaCup_C_mesh"
        };
        public static List<string> Nigel_Mat1 = new List<string>
        {
            "arms_C_mesh",
            "default_C_mesh",
            "smile_C_mesh",
            "downeyelid_L_mesh",
            "downeyelid_R_mesh",
            "upeyelid_L_mesh",
            "upeyelid_R_mesh"
        };

        public static List<string> Nigel_Costume_03_Mat = new List<string>
        {
            "costume02_jacket_C_inv_mesh",
            "costume02_jacket_C_mesh",
            "costume02_pants_C_mesh",
            "costume02_shoes_C_mesh",
            "costume02_shoulderPiece_C_inv_mesh",
            "costume02_shoulderPiece_C_mesh",
            "costume02_strap_C_inv_mesh",
            "costume02_strap_C_mesh"
        };

        public static List<string> Nigel_Expressions_Mat1 = new List<string>
        {
            "excited_C_mesh",
            "hurt_C_mesh",
            "smile_C_mesh"
        };

        public static Dictionary<string, List<string>> Nigel0Parts = new Dictionary<string, List<string>>
        {
            { "Nigel_Mat", Nigel_Mat },
            { "Nigel_Costume_02_Mat", Nigel_Costume_02_Mat },
            { "Nigel_Expressions_Mat", Nigel_Expressions_Mat },
            { "Nigel_Crocodile_Mat", Nigel_Crocodile_Mat },
            { "Nigel_Elephant_Mat", Nigel_Elephant_Mat },
            { "Nigel_PropsRig_Mat", Nigel_PropsRig_Mat },
        };
        public static Dictionary<string, List<string>> Nigel1Parts = new Dictionary<string, List<string>>
        {
            { "Nigel_Mat", Nigel_Mat },
            { "Nigel_Costume_02_Mat", Nigel_Costume_02_Mat },
            { "Nigel_Expressions_Mat", Nigel_Expressions_Mat },
            { "Nigel_Crocodile_Mat", Nigel_Crocodile_Mat },
            { "Nigel_Elephant_Mat", Nigel_Elephant_Mat },
            { "Nigel_PropsRig_Mat", Nigel_PropsRig_Mat },
        };
        public static Dictionary<string, List<string>> Nigel2Parts = new Dictionary<string, List<string>>
        {
            { "Nigel_Mat", Nigel_Mat },
            { "Nigel_Costume_02_Mat", Nigel_Costume_02_Mat },
            { "Nigel_HairMask_Mat",  Nigel_HairMask_Mat},
            { "Nigel_Expressions_Hairmask_Mat", Nigel_Expressions_Hairmask_Mat},
            { "Nigel_Crocodile_Mat", Nigel_Crocodile_Mat },
            { "Nigel_Elephant_Mat", Nigel_Elephant_Mat },
            { "Nigel_PropsRig_Mat", Nigel_PropsRig_Mat },
        };
        public static Dictionary<string, List<string>> Nigel3Parts = new Dictionary<string, List<string>>
        {
            { "Nigel_Mat", Nigel_Mat1 },
            { "Nigel_Costume_03_Mat", Nigel_Costume_03_Mat },
            { "Nigel_Expressions_Mat",  Nigel_Expressions_Mat1},
            { "Nigel_Crocodile_Mat", Nigel_Crocodile_Mat },
            { "Nigel_Elephant_Mat", Nigel_Elephant_Mat },
            { "Nigel_PropsRig_Mat", Nigel_PropsRig_Mat },
        };
        public static Dictionary<int, Dictionary<string, List<string>>> NigelAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, Nigel0Parts},
            { 1, Nigel1Parts},
            { 2, Nigel2Parts},
            { 3, Nigel3Parts},

        };
    }
}
