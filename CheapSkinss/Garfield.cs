using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class Garfield
    {
        public static List<string> Garfield0Body = new List<string>
{
    "body_C_mesh"
};

        public static List<string> Garfield0Expressions = new List<string>
{
    "attack1_C_mesh",
    "attack2_C_mesh",
    "attack3_C_mesh",
    "backears_C_mesh",
    "default_C_mesh",
    "default_front_C_mesh",
    "default_front_closed_C_mesh",
    "eat2_C_mesh",
    "getup_C_mesh",
    "openwide_C_mesh",
    "smallwhisker_C_mesh",
    "smallwhiskerbe_C_mesh",
    "sugarrush_C_mesh",
    "tapdancefront_C_mesh",
    "taunt1_C_mesh",
    "taunt2_C_mesh",
    "downeyelid_L_mesh",
    "downeyelid_R_mesh",
    "upeyelid_L_mesh",
    "upeyelid_R_mesh",
    "downeyelid_front_L_mesh",
    "downeyelid_front_R_mesh",
    "upeyelid_front_L_mesh",
    "upeyelid_front_R_mesh"
};

        public static List<string> Garfield0Props = new List<string>
{
    "bed_C_mesh",
    "icecream_C_mesh",
    "pillow_C_mesh",
    "sandwich_C_mesh",
    "chickenleg_mesh",
    "coffeemug_mesh",
    "donut_mesh",
    "bowtie_C_mesh",
    "hat_C_mesh",
    "stick_C_mesh",
    "lasagnabowl_mesh",
    "lasagnamix_mesh",
    "lasagnatopping_mesh",
    "oddiesfoodbowl_mesh",
    "piebowl_mesh",
    "pooky_mesh",
    "popcorn_mesh",
    "spatula_mesh"
};

        public static List<string> GarfieldOld = new List<string>
{
    "PropSpawned_Garfield_V02_mesh"
};

        public static List<string> Garfield1Costume = new List<string>
{
    "costume01_C_mesh"
};

        public static List<string> Garfield2Costume = new List<string>
{
    "costume00_handkerchief_C_mesh"
};

        public static List<string> Garfield3Costume = new List<string>
{
    "costume02_hat_c_mesh",
    "costume02_pant_c_mesh"
};

        public static Dictionary<string, List<string>> Garfield0Parts = new Dictionary<string, List<string>>
        {
            { "Garfield_Costume00_Body_Mat", Garfield0Body },
            { "Garfield_Costume00_Expressions_Mat", Garfield0Expressions },
            { "Garfield_Costume00_Props_Mat", Garfield0Props },
            { "Garfield_Costume00_Props_Oldschool_Mat", GarfieldOld },
            { "lambert14", Garfield1Costume },
            { "Garfield_Costume00_Costumes_Mat", Garfield2Costume }
        };

        public static Dictionary<string, List<string>> Garfield1Parts = new Dictionary<string, List<string>>
        {
            { "Garfield_Costume00_BodyMask_Mat", Garfield0Body },
            { "Garfield_Costume00_Expressions_Mat", Garfield0Expressions },
            { "Garfield_Costume00_Props_Mat", Garfield0Props },
            { "Garfield_Costume00_Props_Oldschool_Mat", GarfieldOld },
            { "Garfield_Costume_02_Mat", Garfield1Costume }
        };

        public static Dictionary<string, List<string>> Garfield2Parts = new Dictionary<string, List<string>>
        {
            { "Garfield_Costume00_Body_Mat", Garfield0Body },
            { "Garfield_Costume00_Expressions_Mat", Garfield0Expressions },
            { "Garfield_Costume00_Props_Mat", Garfield0Props },
            { "Garfield_Costume00_Props_Oldschool_Mat", GarfieldOld },
            { "lambert14", Garfield1Costume },
            { "Garfield_Costume00_Costumes_Mat", Garfield2Costume }
        };

        public static Dictionary<string, List<string>> Garfield3Parts = new Dictionary<string, List<string>>
        {
            { "Garfield_Costume00_Body_Mat", Garfield0Body },
            { "Garfield_Costume00_Expressions_Mat", Garfield0Expressions },
            { "Garfield_Costume00_Props_Mat", Garfield0Props },
            { "Garfield_Costume00_Props_Oldschool_Mat", GarfieldOld },
            { "Garfield_Costume00_Costumes_Mat", Garfield3Costume }
        };

        public static Dictionary<int, Dictionary<string, List<string>>> GarfieldAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, Garfield0Parts },
            { 1, Garfield1Parts },
            { 2, Garfield2Parts },
            { 3, Garfield3Parts }
        };
        public static Dictionary<string, Dictionary<int, Dictionary<string, List<string>>>> characterCodenames = new Dictionary<string, Dictionary<int, Dictionary<string, List<string>>>>
        {
            { "Garfield", GarfieldAltParts }
        };
    }
}
