using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class ElTigre
    {
        public static List<string> ElTigre0Body = new List<string>
{
    "body_C_mesh",
    "boots_C_mesh",
    "bronzeboots_C_mesh",
    "gloves_C_mesh",
    "rag_C_mesh",
    "armbandage_C_mesh",
    "cape_C_mesh",
    "claws_C_mesh",
    "goldensombrero_C_mesh",
    "headbandage_C_mesh",
    "mustache_C_mesh",
    "sawhands_C_mesh",
    "sawlikearms_C_mesh",
    "wristchainhand_C_mesh"
};

        public static List<string> ElTigre0Expressions = new List<string>
{
    "angry_C_mesh",
    "closedmouth_C_mesh",
    "default_C_mesh",
    "evilsmile_C_mesh",
    "fall_C_mesh",
    "frontsmile_C_mesh",
    "idlev2_C_mesh",
    "idlev3_C_mesh",
    "laugh_C_mesh",
    "lose_C_mesh",
    "sidesmile_C_mesh",
    "sleep_C_mesh",
    "ultimate_C_mesh",
    "downeyelid_L_mesh",
    "downeyelid_R_mesh",
    "upeyelid_L_mesh",
    "upeyelid_R_mesh"
};

        public static List<string> ElTigre3Body = new List<string>
{
    "body_C_mesh",
    "boots_C_mesh"
};


        public static List<string> ElTigre3Props = new List<string>
{
    "bronzeboots_C_mesh",
    "gloves_C_mesh",
    "rag_C_mesh",
    "armbandage_C_mesh",
    "cape_C_mesh",
    "claws_C_mesh",
    "goldensombrero_C_mesh",
    "headbandage_C_mesh",
    "mustache_C_mesh",
    "sawhands_C_mesh",
    "sawlikearms_C_mesh",
    "wristchainhand_C_mesh"
};

        public static Dictionary<string, List<string>> ElTigre0Parts = new Dictionary<string, List<string>>
{
    { "ElTigre_Body_Mat", ElTigre0Body },
    { "ElTigre_Expressions_Mat", ElTigre0Expressions }
};

        public static Dictionary<string, List<string>> ElTigre1Parts = new Dictionary<string, List<string>>
{
    { "ElTigre_Body_Costume_02_Mat", ElTigre0Body },
    { "ElTigre_Expressions_Mat", ElTigre0Expressions }
};

        public static Dictionary<string, List<string>> ElTigre2Parts = new Dictionary<string, List<string>>
{
    { "ElTigre_Body_Costume_02_Mat", ElTigre0Body },
    { "ElTigre_Expressions_Mat", ElTigre0Expressions }
};

        public static Dictionary<string, List<string>> ElTigre3Parts = new Dictionary<string, List<string>>
{
    { "ElTigre_Body_Costume_02_Mat", ElTigre3Body },
    { "ElTigre_Expressions_Mat", ElTigre0Expressions },
    { "ElTigre_Body_Mat", ElTigre3Props }
};

        public static Dictionary<int, Dictionary<string, List<string>>> ElTigreAltParts = new Dictionary<int, Dictionary<string, List<string>>>
{
    { 0, ElTigre0Parts },
    { 1, ElTigre1Parts },
    { 2, ElTigre2Parts },
    { 3, ElTigre3Parts }
};
        public static Dictionary<string, Dictionary<int, Dictionary<string, List<string>>>> characterCodenames = new Dictionary<string, Dictionary<int, Dictionary<string, List<string>>>>
{
    { "ElTigre", ElTigreAltParts }
};
    }
}
