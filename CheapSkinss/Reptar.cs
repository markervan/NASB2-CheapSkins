using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class Reptar
    {

        public static List<string> Reptar_Costume_00_Mat = new List<string>
        {
            "body_C_mesh",
            "costume00_scales_C_mesh",
            "closed_C_mesh",
            "default_C_mesh",
            "fullteethbite_C_mesh",
            "happy_C_mesh",
            "hit_C_mesh",
            "hit_inv_C_mesh",
            "teethbite_C_mesh",
            "teethbite_inv_C_mesh",
            "tongue_C_mesh",
            "tongue_inv_C_mesh",
            "downeyelid_R_mesh",
            "downeyelid_L_mesh",
            "upeyelid_R_mesh",
            "upeyelid_L_mesh"
        };
        public static List<string> Reptar_Costume_00_Expressions_Mat = new List<string>
        {
            "closed_C_mesh",
            "fullteethbite_C_mesh",
            "happy_C_mesh",
            "hit_C_mesh",
            "hit_inv_C_mesh",
            "teethbite_C_mesh",
            "teethbite_inv_C_mesh",
            "tongue_C_mesh",
            "tongue_inv_C_mesh",
            "wideopen_C_mesh"
        };
        public static List<string> Reptar_Props_Mat = new List<string>
        {
            "building_01_C_mesh",
            "ski_C_mesh",
            "building_02_C_mesh",
            "cereal_C_mesh",
            "spoon_C_mesh"
        };
        public static List<string> Reptar_Costume_00_Cheeks_Mat = new List<string>
        {
            "reptarcheeks_C_mesh",
        };
        public static List<string> Reptar_Costume_03_Mat = new List<string>
        {
            "default_hat",
            "Happy_hat",
            "second_hat",
            "wideopen_hat",
            "tongue_hat",
            "costume01_hat_C_mesh_name",
            "downeyelid_L_mesh",
            "upeyelid_L_mesh",
        };
        public static List<string> Reptar_Costume_04_Mat = new List<string>
        {
            "costume02_default_body_C_mesh",
            "costume02_closed_C_mesh",
            "costume02_default_C_mesh",
            "costume02_fullteethbite_C_mesh",
            "costume02_happy_C_mesh",
            "costume02_hit_C_mesh",
            "costume02_teethbite_C_mesh",
            "costume02_tongue_C_mesh",
            "costume02_wideopen_C_mesh"
        };

        public static Dictionary<string, List<string>> Reptar0Parts = new Dictionary<string, List<string>>
        {
            { "Reptar_Costume_00_Mat", Reptar_Costume_00_Mat },
            { "Reptar_Costume_00_Expressions_Mat", Reptar_Costume_00_Expressions_Mat },
            { "Reptar_Props_Mat", Reptar_Props_Mat },
            { "Reptar_Costume_00_Cheeks_Mat", Reptar_Costume_00_Cheeks_Mat  }
        };
        public static Dictionary<string, List<string>> Reptar1Parts = new Dictionary<string, List<string>>
        {
            { "Reptar_Costume_02_Mat", Reptar_Costume_00_Mat },
            { "Reptar_Costume_02_Expressions_Mat",  Reptar_Costume_00_Expressions_Mat},
            { "Reptar_Props_Mat", Reptar_Props_Mat },
            { "Reptar_Costume_02_Cheeks_Mat",  Reptar_Costume_00_Cheeks_Mat}

        };
        public static Dictionary<string, List<string>> Reptar2Parts = new Dictionary<string, List<string>>
        {
            { "Reptar_Costume_03_Mat",  Reptar_Costume_03_Mat},
            { "Reptar_Costume_02_Mat", Reptar_Costume_00_Mat },
            { "Reptar_Costume_02_Expressions_Mat",  Reptar_Costume_00_Expressions_Mat},
            { "Reptar_Props_Mat", Reptar_Props_Mat },
            { "Reptar_Costume_02_Cheeks_Mat",  Reptar_Costume_00_Cheeks_Mat}

        };
        public static Dictionary<string, List<string>> Reptar3Parts = new Dictionary<string, List<string>>
        {
            { "Reptar_Costume_04_Mat", Reptar_Costume_04_Mat },
            { "Reptar_Costume_04_Cheeks_Mat", Reptar_Costume_00_Cheeks_Mat },
            { "Reptar_Props_Mat", Reptar_Props_Mat },

        };
        public static Dictionary<int, Dictionary<string, List<string>>> ReptarAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, Reptar0Parts},
            { 1, Reptar1Parts},
            { 2, Reptar2Parts},
            { 3, Reptar3Parts}
        };
    }
}
