using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class Lucy
    {
        public static List<string> Lucy0Body = new List<string>
{
    "armDetails_C_mesh",
    "ears_C_mesh",
    "costume01Dress_C_mesh",
    "default_mesh",
    "hair_C_mesh",
    "hair_inv_C_mesh",
    "hands_C_mesh",
    "legs_C_mesh",
    "neck_C_mesh",
    "shoes_C_mesh"
};

        public static List<string> Lucy0Expressions = new List<string>
{
    "attack_inv_mesh",
    "attack_mesh",
    "bite_mesh",
    "biteClosed_mesh",
    "hurt_inv_mesh",
    "hurt_mesh",
    "idle_mesh",
    "smile_mesh",
    "taunt2_inv_mesh",
    "taunt2_mesh",
    "vamp_mesh"
};

        public static List<string> Lucy0Hair = new List<string>
{
    "hairBlockTeeter_C_mesh",
    "hairBlockTeeter_inv_C_mesh",
    "hairDown_C_mesh",
    "hairDown_inv_C_mesh",
    "hairEdgeGrab_C_mesh",
    "hairEdgeGrab_inv_C_mesh"
};

        public static List<string> Lucy0Props1 = new List<string>
{
    "bat01Body_C_mesh",
    "bat02Body_C_mesh",
    "batEars_C_mesh",
    "batFeets_C_mesh",
    "batWings_C_mesh",
    "hairStrand_C_mesh",
    "cape_mesh",
    "hood_mesh",
    "shoedetail_mesh",
    "spadesymbol_mesh",
    "capeWings_C_mesh",
    "coffinBase_C_mesh",
    "door_C_mesh",
    "handles_C_mesh",
    "hingeBase_C_mesh",
    "middlePiece_C_mesh",
    "umbrella_C_mesh",
    "umbrellaClosed_C_mesh",
    "notebook_C_mesh",
    "pencil_C_mesh",
    "crystalBall_C_mesh",
    "edwin_C_mesh",
    "eigthOfSpadeShovel_C_mesh",
    "morticianClubGavel_C_mesh",
    "poetryBook_C_mesh",
    "poetryBook_open_C_mesh",
    "scythe_C_mesh",
    "threeOfSwords_C_mesh"
};

        public static List<string> Lucy1Costume = new List<string>
{
    "glasses_mesh",
    "baseDetails_mesh",
    "gem_base_mesh",
    "gem_mesh",
    "hat_inv_mesh",
    "hat_mesh",
    "hatWrap_mesh"
};

        public static List<string> Lucille = new List<string>
{
    "lucille_C_mesh"
};

        public static List<string> Lucy1Hair = new List<string>
{
    "hairBlockTeeter_C_mesh",
    "hairBlockTeeter_inv_C_mesh",
    "hairDown_C_mesh",
    "hairDown_inv_C_mesh"
};

        public static List<string> Lucy1Hair2 = new List<string>
{
    "hairEdgeGrab_C_mesh",
    "hairEdgeGrab_inv_C_mesh"
};

        public static List<string> Lucy2Body = new List<string>
{
    "armDetails_C_mesh",
    "costume01Dress_C_mesh",
    "hair_C_mesh",
    "hands_C_mesh",
    "legs_C_mesh",
    "shoes_C_mesh"
};

        public static List<string> Lucy2Body2 = new List<string>
{
    "ears_C_mesh",
    "default_mesh",
    "hair_inv_C_mesh",
    "neck_C_mesh"
};

        public static List<string> Lucy3Body = new List<string>
{
    "armDetails_C_mesh",
    "ears_C_mesh",
    "default_mesh",
    "hair_C_mesh",
    "hair_inv_C_mesh",
    "hands_C_mesh",
    "legs_C_mesh",
    "neck_C_mesh",
    "shoes_C_mesh"
};

        public static List<string> Lucy3Dress = new List<string>
{
    "costume01Dress_C_mesh"
};

        public static Dictionary<string, List<string>> Lucy0Parts = new Dictionary<string, List<string>>
{
    { "Lucy_Preview_02_Mat", Lucy0Body },
    { "Lucy_Expressions_Material", Lucy0Expressions },
    { "Lucy_HairProp_Mat", Lucy0Hair },
    { "Lucy_Preview_02_Props_Mat", Lucy0Props1 },
    { "Lucy_Preview_Costumes_01_Mat", Lucy1Costume },
    { "Lucy_Lucille_Occulsion_Mat", Lucille}
};

        public static Dictionary<string, List<string>> Lucy1Parts = new Dictionary<string, List<string>>
{
    { "Lucy_Preview_02_Mat", Lucy0Body },
    { "Lucy_Expressions_Material", Lucy0Expressions },
    { "Lucy_HairProp_Mat", Lucy1Hair },
    { "Lucy_Preview_02_Props_Mat", Lucy0Props1 },
    { "Lucy_Preview_Costumes_01_Mat", Lucy1Costume },
    { "Lucy_Lucille_Occulsion_Mat", Lucille},
    { "Lucy_HairMask_Mat", Lucy1Hair2 }
};

        public static Dictionary<string, List<string>> Lucy2Parts = new Dictionary<string, List<string>>
{
    { "Lucy_Costume_EightOfSpades_Mat", Lucy2Body },
    { "Lucy_Expressions_Material", Lucy0Expressions },
    { "Lucy_HairProp_Mat", Lucy1Hair },
    { "Lucy_Preview_02_Props_Mat", Lucy0Props1 },
    { "Lucy_Preview_Costumes_01_Mat", Lucy1Costume },
    { "Lucy_Lucille_Occulsion_Mat", Lucille},
    { "Lucy_HairMask_Mat", Lucy1Hair2 },
    { "Lucy_Preview_02_Mat", Lucy2Body2 }
};

        public static Dictionary<string, List<string>> Lucy3Parts = new Dictionary<string, List<string>>
{
    { "Lucy_Preview_02_Mat", Lucy3Body },
    { "Lucy_Expressions_Material", Lucy0Expressions },
    { "Lucy_HairProp_Mat", Lucy1Hair },
    { "Lucy_Preview_02_Props_Mat", Lucy0Props1 },
    { "Lucy_Preview_Costumes_01_Mat", Lucy1Costume },
    { "Lucy_Lucille_Occulsion_Mat", Lucille},
    { "Lucy_HairMask_Mat", Lucy1Hair2 },
    { "Lucy_Base_Material", Lucy3Dress }
};

        public static Dictionary<int, Dictionary<string, List<string>>> LucyAltParts = new Dictionary<int, Dictionary<string, List<string>>>
{
    { 0, Lucy0Parts },
    { 1, Lucy1Parts },
    { 2, Lucy2Parts },
    { 3, Lucy3Parts }
};
    }
}
