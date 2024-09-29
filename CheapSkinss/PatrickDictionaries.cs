using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class PatrickDictionaries
    {
        public static List<string> Patrick0Body = new List<string>
{
    "body_C_mesh",
    "legs_C_mesh",
    "pants_C_mesh",
    "steelbunsnormal_C_mesh",
    "buffed_C_mesh"
};

        public static List<string> Patrick0Expressions = new List<string>
{
    "attack_C_mesh",
    "cute_C_mesh",
    "default_C_mesh",
    "entrance_L_mesh",
    "entrance_R_mesh",
    "fightme_C_mesh",
    "frontalmouthbeam_C_mesh",
    "frontalmouthopen_C_mesh",
    "hit_C_mesh",
    "mouthopenwide_C_mesh",
    "munch_C_mesh",
    "opensuperwide_C_mesh",
    "sadsmile_C_mesh",
    "sidemouthcurve_C_mesh",
    "sidemouthopen_C_mesh",
    "sidesmile_C_mesh",
    "slightopenedmouth_C_mesh",
    "smileteethfrontal_C_mesh",
    "taunt01_C_mesh",
    "taunt02_C_mesh",
    "downeyelid_L_mesh",
    "downeyelid_R_mesh",
    "upeyelid_L_mesh",
    "upeyelid_R_mesh"
};

        public static List<string> Patrick0ConeBody = new List<string>
{
    "bodycone_C_mesh"
};

        public static List<string> Patrick0Props = new List<string>
{
    "net_mesh",
    "phone_mesh"
};

        public static List<string> Patrick1Body = new List<string>
{
    "body_C_mesh"
};

        public static List<string> Patrick1BodyAlt = new List<string>
{
    "legs_C_mesh",
    "pants_C_mesh",
    "steelbunsnormal_C_mesh",
    "buffed_C_mesh"
};

        public static List<string> Patrick2Body = new List<string>
{
    "body_C_mesh",
    "legs_C_mesh"
};

        public static List<string> Patrick2BodyAlt = new List<string>
{
    "cone_C_mesh",
    "cone_opensuperwide_C_mesh",
    "glove_L_mesh",
    "glove_R_mesh",
    "p_C_mesh",
    "p_inv_C_mesh",
    "pantscostume02_C_mesh",
    "steelbunscostum02_C_mesh"
};

        public static List<string> Patrick2Buff = new List<string>
{
    "buffed_C_mesh"
};

        public static List<string> Patrick3Body = new List<string>
{
    "body_C_mesh"
};

        public static List<string> Patrick3Legs = new List<string>
{
    "legs_C_mesh"
};

        public static List<string> Patrick3BodyAlt = new List<string>
{
    "constume01_C_mesh",
    "costume01fist_C_mesh",
    "costume01gloves_C_mesh",
    "hatdefault_C_mesh",
    "hattaunt01_C_mesh",
    "hattaunt02_C_mesh",
    "hatwide_C_mesh"
};

        public static List<string> Patrick4Body = new List<string>
{
    "elastic_C_mesh",
    "elastic_googles_C_mesh",
    "elastic_googles_opensuperwide_C_mesh",
    "elastic_googles_taunt01_C_mesh",
    "elastic_googles_taunt02_C_mesh",
};

        public static List<string> Patrick4Buns = new List<string>
{
    "steelbunscostume03_C_mesh"
};

        public static Dictionary<string, List<string>> Patrick0Parts = new Dictionary<string, List<string>>
{
    { "Patrick_Costume_01_Mat", Patrick0Body },
    { "Patrick_Expressions_Mat", Patrick0Expressions },
    { "Patrick_Conebody", Patrick0ConeBody },
    { "Patrick_Props_01_Mat", Patrick0Props }
};

        public static Dictionary<string, List<string>> Patrick1Parts = new Dictionary<string, List<string>>
{
    { "Patrick_Costume_01_Mat", Patrick1Body },
    { "Patrick_Costume_04_Mat", Patrick1BodyAlt },
    { "Patrick_Expressions_Mat", Patrick0Expressions },
    { "Patrick_Conebody", Patrick0ConeBody },
    { "Patrick_Props_01_Mat", Patrick0Props }
};

        public static Dictionary<string, List<string>> Patrick2Parts = new Dictionary<string, List<string>>
{
    { "Patrick_Costume_01_Mat", Patrick2Body },
    { "Patrick_Costume_03_Mat", Patrick2BodyAlt },
    { "Patrick_Expressions_Mat", Patrick0Expressions },
    { "Patrick_Conebody", Patrick0ConeBody },
    { "Patrick_Props_01_Mat", Patrick0Props },
    { "Patrick_BuffedArmsMask_Mat", Patrick2Buff }
};

        public static Dictionary<string, List<string>> Patrick3Parts = new Dictionary<string, List<string>>
{
    { "Patrick_Costume_06_BarnacleBoy_Mat", Patrick3Body },
    { "Patrick_Costume_01_Mat", Patrick3Legs },
    { "Patrick_Costume_05_Mat", Patrick3BodyAlt },
    { "Patrick_Expressions_Mat", Patrick0Expressions },
    { "Patrick_Conebody", Patrick0ConeBody },
    { "Patrick_Props_01_Mat", Patrick0Props },
    { "Patrick_BuffedArmsLegsMask_Mat", Patrick2Buff }
};

        public static Dictionary<string, List<string>> Patrick4Parts = new Dictionary<string, List<string>>
{
    { "Patrick_Costume_ElasticWaistband_Mat", Patrick4Body },
    { "Patrick_Elastic_SteelBuns_Mat", Patrick4Buns },
    { "Patrick_Expressions_Mat", Patrick0Expressions },
    { "Patrick_Conebody", Patrick0ConeBody },
    { "Patrick_Props_01_Mat", Patrick0Props },
    { "Patrick_Buffed_Elastic_Mat", Patrick2Buff }
};

        public static Dictionary<int, Dictionary<string, List<string>>> PatrickAltParts = new Dictionary<int, Dictionary<string, List<string>>>
{
    { 0, Patrick0Parts},
    { 1, Patrick1Parts},
    { 2, Patrick2Parts},
    { 3, Patrick3Parts},
    { 4, Patrick4Parts}
};
    }
}
