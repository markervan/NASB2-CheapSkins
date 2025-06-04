using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class Gerald
    {
        public static List<string> Gerald_Mat = new List<string>
        {
            "costume01_body_C_mesh",
            "costume01_hatFront_C_mesh",
            "costume01_hatSide_C_mesh",
            "costume00_body_C_mesh",
            "default_C_mesh",
            "side_C_mesh",
            "downeyelid_L_mesh",
            "downeyelid_R_mesh",
            "upeyelid_L_mesh",
            "upeyelid_R_mesh",
            "mainHair_C_mesh",
            "sideHair_C_mesh"
        };

        public static List<string> Gerald_Expressions_02_Mat = new List<string>
        {
            "angry_C_mesh",
            "block_C_mesh",
            "bow_C_mesh",
            "determinedRun_C_mesh",
            "down_02_C_mesh",
            "idle_C_mesh",
            "lose02_C_mesh",
            "sleep_C_mesh",
            "win_C_mesh"
        };

        public static List<string> Gerald_Expressions_01_Mat = new List<string>
        {
            "attack_01_C_mesh",
            "down_C_mesh",
            "down_special_C_mesh",
            "effortStrong_C_mesh",
            "hit_C_mesh",
            "lose_C_mesh",
            "oo_C_mesh",
            "sideSmile_C_mesh",
            "smileNeutral_C_mesh",
            "strongUp_C_mesh",
            "taunt_C_mesh",
            "frontHair_C_mesh"
        };

        public static List<string> Gerald_Hair_Mat = new List<string>
        {
            "mainHair_C_mesh"
        };

        public static List<string> Gerald_Props_Bike_Mat = new List<string>
        {
            "bicycle_C_mesh"
        };

        public static List<string> Gerald_Props_Mat = new List<string>
        {
            "card_C1_mesh",
            "card_C2_mesh",
            "sign_C_mesh",
            "skateboard_C_mesh",
            "baseball_C_mesh",
            "baseballBat_C_mesh",
            "basketball_C_mesh",
            "boombox_C_mesh",
            "football_C_mesh",
            "frisbee_C_mesh",
            "racket_C_mesh",
            "watch_C_mesh"
        };
        public static List<string> Gerald_Costumes_Mat = new List<string>
        {
            "costume01_body_C_mesh",
            "costume01_hatFront_C_mesh",
            "costume01_hatMain_C_mesh",
            "costume01_hatSide_C_mesh",
            "costume02_body_C_mesh"
        };
        public static List<string> Gerald_Costume_03_Mat = new List<string>
        {
            "costume03_body_C_mesh",
            "costume03_crownFront_C_mesh",
            "costume03_crownMain_C_mesh",
            "costume03_crownSide_C_mesh",
            "default_crown",
            "down_special_crown"
        };
        public static Dictionary<string, List<string>> Gerald0Parts = new Dictionary<string, List<string>>
        {
            { "Gerald_Mat", Gerald_Mat },
            { "Gerald_Expressions_02_Mat", Gerald_Expressions_02_Mat },
            { "Gerald_Expressions_01_Mat",  Gerald_Expressions_01_Mat},
            { "Gerald_Hair_Mat", Gerald_Hair_Mat},
            { "Gerald_Props_Bike_Mat",  Gerald_Props_Bike_Mat},
            { "Gerald_Props_Mat",  Gerald_Props_Mat}
        };
        public static Dictionary<string, List<string>> Gerald1Parts = new Dictionary<string, List<string>>
        {
            { "Gerald_Mat", Gerald_Mat },
            { "Gerald_Costumes_Mat",  Gerald_Costumes_Mat},
            { "Gerald_Hair_Mat", Gerald_Hair_Mat},
            { "Gerald_Expressions_02_Mat", Gerald_Expressions_02_Mat },
            { "Gerald_Expressions_01_Mat",  Gerald_Expressions_01_Mat},
            { "Gerald_Props_Bike_Mat",  Gerald_Props_Bike_Mat},
            { "Gerald_Props_Mat",  Gerald_Props_Mat}
        };
        public static Dictionary<string, List<string>> Gerald2Parts = new Dictionary<string, List<string>>
        {
            { "Gerald_Costumes_Mat", Gerald_Costumes_Mat },
            { "Gerald_Expressions_02_Mat",  Gerald_Expressions_02_Mat},
            { "Gerald_Expressions_01_Mat", Gerald_Expressions_01_Mat},
            { "Gerald_Mat", Gerald_Mat },
            { "Gerald_Hair_Mat",  Gerald_Hair_Mat},
            { "Gerald_Props_Bike_Mat",  Gerald_Props_Bike_Mat},
            { "Gerald_Props_Mat",  Gerald_Props_Mat}
        };
        public static Dictionary<string, List<string>> Gerald3Parts = new Dictionary<string, List<string>>
        {
            { "Gerald_Costume_03_Mat", Gerald_Costume_03_Mat },
            { "Gerald_Expressions_02_Mat", Gerald_Expressions_02_Mat },
            { "Gerald_Expressions_01_Mat", Gerald_Expressions_01_Mat },
            { "Gerald_Mat", Gerald_Mat },
            { "Gerald_Hair_Mat", Gerald_Hair_Mat },
            { "Gerald_Props_Bike_Mat",  Gerald_Props_Bike_Mat},
            { "Gerald_Props_Mat",  Gerald_Props_Mat}
        };
        public static Dictionary<int, Dictionary<string, List<string>>> GeraldAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, Gerald0Parts},
            { 1, Gerald1Parts},
            { 2, Gerald2Parts},
            { 3, Gerald3Parts},

        };
    }
}
