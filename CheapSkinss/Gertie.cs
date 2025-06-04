using Quantum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class Gertie
    {
        public static List<string> Gertie_Costume_01_Mat = new List<string>
        {
            "costume00_apron_C_mesh",
            "costume00_body_C_mesh",
            "costume00_dress_C_mesh",
            "costume00_legs_C_mesh",
            "default_C_mesh"
        };

        public static List<string> Gertie_Expressions_01_Mat = new List<string>
        {
            "attack1_C_mesh",
            "attack2_C_mesh",
            "down_C_mesh",
            "fall_C_mesh",
            "idle_C_mesh",
            "mad_C_mesh",
            "costume01_attack1_C_mesh",
            "costume01_attack2_C_mesh",
            "costume01_down_C_mesh",
            "costume01_fall_C_mesh",
            "costume01_idle_C_mesh",
            "costume01_mad_C_mesh"
        };

        public static List<string> Gertie_Props_02_Mat = new List<string>
        {
            "electricwires_C_mesh",
            "fireworks_C_mesh",
            "hook_C_mesh",
            "purse_C_mesh",
            "sweater_C_mesh",
            "airplane_C_mesh",
            "alarmclock_C_mesh",
            "ballofyarn_C_mesh",
            "balloons_C_mesh",
            "beehive_C_mesh",
            "bowlingball_C_mesh",
            "broom__C_mesh",
            "cookingpot_C_mesh",
            "flyswatter_C_mesh",
            "fryingpan_C_mesh",
            "halberd_C_mesh",
            "knittingneedles_C_mesh",
            "megaphone_C_mesh",
            "pancakes_C_mesh",
            "tacks_C_mesh",
            "wreckingball_C_mesh",
            "HookMesh"
        };
        public static List<string> Gertie_Costume_02_Dress_Mat = new List<string>
        {
            "costume02_apron_C_mesh",
            "costume02_body_C_mesh",
            "costume02_cookinghat_default_C_mesh",
            "costume02_legs_C_mesh"
        };

        public static List<string> Gertie_Costume_03_Mat = new List<string>
        {
            "costume02_cookinghat_attack2_C_mesh",
            "costume02_cookinghat_down_C_mesh",
            "costume02_dress_C_mesh",
            "default_C_mesh"
        };

        public static List<string> Gertie_Props_01_Mat = new List<string>
        {
            "train_C_mesh"
        };

        public static List<string> Gertie_Props_Piano_Mat = new List<string>
        {
            "piano_C_mesh"
        };
        public static List<string> Gertie_Costume_02_Mat = new List<string>
        {
            "costume01_body_C_mesh",
            "costume01_legs_C_mesh",
            "costume01_sweater_C_mesh",
            "costume01_attack1_C_mesh",
            "costume01_attack2_C_mesh",
            "costume01_default_C_mesh",
            "costume01_down_C_mesh",
            "costume01_fall_C_mesh",
            "costume01_idle_C_mesh",
            "costume01_mad_C_mesh"
        };
        public static List<string> Gertie_Costume_04_Mat = new List<string>
        {
            "costume03_body_C_mesh",
            "costume03_jacket_C_mesh",
            "costume03_legs_C_mesh",
            "costume03_attack1_C_mesh",
            "costume03_attack2_C_mesh",
            "costume03_default_C_mesh",
            "costume03_down_C_mesh",
            "costume03_fall_C_mesh",
            "costume03_idle_C_mesh",
            "costume03_mad_C_mesh"
        };

        public static Dictionary<string, List<string>> GrandmaGertie0Parts = new Dictionary<string, List<string>>
        {
            {"Gertie_Costume_01_Mat", Gertie_Costume_01_Mat },
            {"Gertie_Expressions_01_Mat", Gertie_Expressions_01_Mat },
            {"Gertie_Props_02_Mat", Gertie_Props_02_Mat },
            {"Gertie_Props_01_Mat", Gertie_Props_01_Mat  },
            {"Gertie_Props_Piano_Mat",  Gertie_Props_Piano_Mat}
        };
        public static Dictionary<string, List<string>> GrandmaGertie1Parts = new Dictionary<string, List<string>>
        {
            {"Gertie_Costume_02_Mat ", Gertie_Costume_02_Mat },
            {"Gertie_Expressions_01_Mat", Gertie_Expressions_01_Mat },
            {"Gertie_Props_02_Mat", Gertie_Props_02_Mat },
            {"Gertie_Props_01_Mat", Gertie_Props_01_Mat  },
            {"Gertie_Props_Piano_Mat",  Gertie_Props_Piano_Mat}
        };
        public static Dictionary<string, List<string>> GrandmaGertie2Parts = new Dictionary<string, List<string>>
        {
            {"Gertie_Costume_02_Dress_Mat ", Gertie_Costume_02_Dress_Mat },
            {"Gertie_Costume_03_Mat", Gertie_Costume_03_Mat },
            {"Gertie_Expressions_01_Mat", Gertie_Expressions_01_Mat },
            {"Gertie_Props_02_Mat", Gertie_Props_02_Mat },
            {"Gertie_Props_01_Mat", Gertie_Props_01_Mat  },
            {"Gertie_Props_Piano_Mat",  Gertie_Props_Piano_Mat}
        };
        public static Dictionary<string, List<string>> GrandmaGertie3Parts = new Dictionary<string, List<string>>
        {
            {"Gertie_Costume_04_Mat ", Gertie_Costume_04_Mat },
            {"Gertie_Expressions_01_Mat", Gertie_Expressions_01_Mat },
            {"Gertie_Props_02_Mat", Gertie_Props_02_Mat },
            {"Gertie_Props_01_Mat", Gertie_Props_01_Mat  },
            {"Gertie_Props_Piano_Mat",  Gertie_Props_Piano_Mat}
        };
        public static Dictionary<int, Dictionary<string, List<string>>> GrandmaGertieAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, GrandmaGertie0Parts},
            { 1, GrandmaGertie1Parts},
            { 2, GrandmaGertie2Parts},
            { 3, GrandmaGertie3Parts}

        };
    }
}
