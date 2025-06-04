using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class Aaang
    {
        public static List<string> Aang0Body = new List<string>
{
    "costume01Body_C_mesh"
};

        public static List<string> Aang0Costume = new List<string>
{
    "costume01_C_mesh"
};

        public static List<string> Aang0Expressions = new List<string>
{
    "attack01_C_mesh",
    "attack02_C_mesh",
    "blow_C_mesh",
    "confidentsmile_C_mesh",
    "default_C_mesh",
    "dodge_C_mesh",
    "effort_C_mesh",
    "hit_C_mesh",
    "mouthopen_C_mesh",
    "mouthwide_C_mesh",
    "sleep_C_mesh",
    "smileteeth_C_mesh",
    "taunt01_C_mesh",
    "taunt02_C_mesh",
    "downeyelid_L_mesh",
    "downeyelid_R_mesh",
    "upeyelid_L_mesh",
    "upeyelid_R_mesh"
};

        public static List<string> Aang0Props = new List<string>
{
    "glider_close_C_mesh",
    "glider_middle_C_mesh",
    "glider_open_C_mesh",
    "glider02_close_C_mesh",
    "glider02_middle_C_mesh",
    "glider02_open_C_mesh",
    "waterskin_C_mesh"
};

        public static List<string> Aang0Lambert = new List<string>
{
    "Sphere_C_Mesh_1",
    "Sphere_C_Mesh_2",
    "Sphere_C_Mesh_3",
    "Aang_AirBall"
};

        public static List<string> Aang0Rocks = new List<string>
{
    "groundspike_C_mesh",
    "groundspikes_C_mesh",
    "iceshards_C_mesh"
};

        public static List<string> Aang1Body = new List<string>
{
    "costume02Body_C_mesh"
};

        public static List<string> Aang1Costume = new List<string>
{
    "costume02_C_mesh",
    "costume02_frontstrips_C_mesh",
    "costume02_frontstrips_inv_C_mesh",
    "costume02_inv_C_mesh",
    "costume02_sidestrips_C_mesh",
    "costume02_sidestrips_inv_C_mesh"
};

        public static List<string> Aang2Body = new List<string>
{
    "costume03Body_C_mesh",
    "costume03_C_mesh",
    "costume03_strips_C_mesh",
    "costume03_strips_inv_C_mesh"
};

        public static List<string> Aang3Body = new List<string>
{
    "costume02Body_C_mesh"
};

        public static List<string> Aang3Costume = new List<string>
        {
            "costume02_C_mesh",
            "costume02_frontstrips_inv_C_mesh",
            "costume02_inv_C_mesh",
            "costume02_sidestrips_C_mesh",
            "costume02_sidestrips_inv_C_mesh"
        };

        public static List<string> Aang3Costume2 = new List<string>
        {
            "costume02_frontstrips_C_mesh"
        };

        public static Dictionary<string, List<string>> Aang0Parts = new Dictionary<string, List<string>>
        {
            { "Aang_Body_02_Mat", Aang0Body },
            { "Aang_Costume_01B_Mat", Aang0Costume },
            { "Aang_Expressions_Mat", Aang0Expressions },
            { "Aang_Props_Mat", Aang0Props },
            { "lambert1", Aang0Lambert },
            { "Aang_Rocks_Mat", Aang0Rocks }
        };

        public static Dictionary<string, List<string>> Aang1Parts = new Dictionary<string, List<string>>
        {
            { "Aang_Body_02_Mat", Aang1Body },
            { "Aang_Costume_02B_Mat", Aang1Costume },
            { "Aang_Expressions_Mat", Aang0Expressions },
            { "Aang_Props_Mat", Aang0Props },
            { "lambert1", Aang0Lambert },
            { "Aang_Rocks_Mat", Aang0Rocks }
        };

        public static Dictionary<string, List<string>> Aang2Parts = new Dictionary<string, List<string>>
        {
            { "Aang_Body_02_Mat", Aang2Body },
            { "Aang_Expressions_Mat", Aang0Expressions },
            { "Aang_Props_Mat", Aang0Props },
            { "lambert1", Aang0Lambert },
            { "Aang_Rocks_Mat", Aang0Rocks }
        };

        public static Dictionary<string, List<string>> Aang3Parts = new Dictionary<string, List<string>>
        {
            { "Aang_Body_02_Mat", Aang3Body },
            { "Aang_Costume_03B_Mat", Aang3Costume },
            { "Aang_Costume_02B_Mat", Aang3Costume2 },
            { "Aang_Expressions_Mat", Aang0Expressions },
            { "Aang_Props_Mat", Aang0Props },
            { "lambert1", Aang0Lambert },
            { "Aang_Rocks_Mat", Aang0Rocks }
        };

        public static Dictionary<int, Dictionary<string, List<string>>> AangAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, Aang0Parts },
            { 1, Aang1Parts },
            { 2, Aang2Parts },
            { 3, Aang3Parts }
        };
    }
}
