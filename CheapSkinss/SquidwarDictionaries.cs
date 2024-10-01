using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class SquidwarDictionaries
    {
        public static List<string> Squidward0Costume = new List<string>
{
    "body_C_mesh",
    "bodyVar_C_mesh",
};

        public static List<string> Squidward0Base = new List<string>
{
    "costume01_shirt_C_mesh",
    "bodyVar_C_mesh",
    "legs01_C_mesh",
    "legs02_C_mesh"
};

        public static List<string> Squidward0Expressions = new List<string>
{
    "attack01_C_mesh",
    "attack02_C_mesh",
    "attack03_C_mesh",
    "attack04_C_mesh",
    "attack05_C_mesh",
    "attack06_C_mesh",
    "default_C_mesh",
    "defaultNose_C_mesh",
    "happy_C_mesh",
    "happyvariation_C_mesh",
    "idle_C_mesh",
    "mad_C_mesh",
    "mouthclosed_C_mesh",
    "mouthopen_C_mesh",
    "smear_C_mesh",
    "smile_C_mesh",
    "downeyelid_L_mesh",
    "downeyelid_R_mesh",
    "upeyelid_L_mesh",
    "upeyelid_R_mesh"
};

        public static List<string> Squidward0Props1 = new List<string>
{
    "aquaticBike_C_mesh",
    "boat_C_mesh",
    "door_C_mesh",
    "reefBlower_C_mesh",
    "wheelChair_C_mesh",
    "clary_C_mesh",
    "sunbathingchair_C_mesh",
    "sunglasses_C_mesh"
};

        public static List<string> Squidward0Props2 = new List<string>
{
    "bathingtube_C_mesh",
    "magmashat_C_mesh",
    "coffeemaker_C_mesh",
    "coffeevase_C_mesh",
    "pizzabox_C_mesh",
    "sunbathingscreen_C_mesh"
};

        public static List<string> Squidward0Props3 = new List<string>
{
    "conductorbaton_C_mesh",
    "marble_C_mesh",
    "paintboard_C_mesh",
    "paintbrush_C_mesh",
    "painting01_C_mesh",
    "painting02_C_mesh",
    "painting03_C_mesh",
    "shoe_C_mesh",
    "statue_C_mesh",
    "sunglasses_C_mesh"
};

        public static List<string> Squidward1Base = new List<string>
{
    "bodyVar_C_mesh",
    "legs01_C_mesh",
    "legs02_C_mesh"
};

        public static List<string> Squidward1Body = new List<string>
{
    "body_C_mesh"
};

        public static List<string> Squidward1Costume = new List<string>
{
    "costume02_body_C_mesh",
    "costume02_hat_C_mesh",
    "bodyVar_C_mesh"
};

        public static List<string> Squidward2Costume = new List<string>
        {
            "body_C_mesh",
            "bodyVar_C_mesh",
            "costume03_armor_C_mesh",
            "costume03_attack01_C_mesh",
            "costume03_attack03_C_mesh",
            "costume03_attack04_C_mesh",
            "costume03_attack05_C_mesh",
            "costume03_attack06_C_mesh",
            "costume03_happy_C_mesh",
            "costume03_helmet_C_mesh",
            "costume03_mouthclosed_C_mesh",
            "costume03_mouthopen_C_mesh",
            "costume03_mustache_C_mesh",
            "costume03_smear_C_mesh",
            "default_helmet",
            "attack05_helmet",
            "HappyVariation_Mustache",
            "bandages_helmet"
        };

        public static List<string> Squidward3Costume = new List<string>
{
    "bodyVar_C_mesh",
    "costume04_beret_C_inv_mesh",
    "costume04_beret_C_mesh",
    "costume04_body_C_mesh"
};

        public static Dictionary<string, List<string>> Squidward0Parts = new Dictionary<string, List<string>>
        {
            { "Squidward_Base_Material", Squidward0Base },
            { "Squidward_Costume_03_Mat", Squidward0Costume },
            { "Squidward_Expression_Material", Squidward0Expressions },
            { "Squidward_Props_Material_01", Squidward0Props1 },
            { "Squidward_Props_Material_02", Squidward0Props2 },
            { "Squidward_Props_Material_03", Squidward0Props3 }
        };

        public static Dictionary<string, List<string>> Squidward1Parts = new Dictionary<string, List<string>>
        {
            { "Squidward_Base_Material", Squidward1Base },
            { "Squidward_Costume_03_Mat", Squidward1Body },
            { "Squidward_Costume_02_Mat", Squidward1Costume },
            { "Squidward_Expression_Material", Squidward0Expressions },
            { "Squidward_Props_Material_01", Squidward0Props1 },
            { "Squidward_Props_Material_02", Squidward0Props2 },
            { "Squidward_Props_Material_03", Squidward0Props3 }
        };

        public static Dictionary<string, List<string>> Squidward2Parts = new Dictionary<string, List<string>>
        {
            { "Squidward_Base_Material", Squidward1Base },
            { "Squidward_Costume_03_Mat", Squidward2Costume },
            { "Squidward_Expression_Material", Squidward0Expressions },
            { "Squidward_Props_Material_01", Squidward0Props1 },
            { "Squidward_Props_Material_02", Squidward0Props2 },
            { "Squidward_Props_Material_03", Squidward0Props3 }
        };

        public static Dictionary<string, List<string>> Squidward3Parts = new Dictionary<string, List<string>>
        {
            { "Squidward_Base_Material", Squidward1Base },
            { "Squidward_Costume_03_Mat", Squidward1Body },
            { "Squidward_Costume_04_Mat", Squidward3Costume },
            { "Squidward_Expression_Material", Squidward0Expressions },
            { "Squidward_Props_Material_01", Squidward0Props1 },
            { "Squidward_Props_Material_02", Squidward0Props2 },
            { "Squidward_Props_Material_03", Squidward0Props3 }
        };

        public static Dictionary<int, Dictionary<string, List<string>>> SquidwardAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, Squidward0Parts},
            { 1, Squidward1Parts},
            { 2, Squidward2Parts},
            { 3, Squidward3Parts}
        };

    }
}
