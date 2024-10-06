using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class Norbet
    {
        public static List<string> Norbet_Mat = new List<string>
        {
            "costume00_arms_C_mesh",
            "costume00_arms_mirror_C_mesh",
            "costume00_body_C_mesh",
            "costume00_body_mirror_C_mesh",
            "costume00_feet_C_mesh",
            "charge_C_mesh",
            "specialneutral_C_mesh",
            "attack_mirror_C_mesh",
            "wings_C_mesh"
        };

        public static List<string> Norbet_Mat_Instance = new List<string>
        {
            "costume00_hair_C_mesh",
            "costume00_hair_mirror_C_mesh",
            "attack_C_mesh",
            "default_C_mesh",
            "hurt_C_mesh",
            "idle_C_mesh",
            "default_mirror_C_mesh",
            "hurt_mirror_C_mesh",
            "downeyelid_L_mesh",
            "downeyelid_R_mesh",
            "upeyelid_L_mesh",
            "upeyelid_R_mesh",
            "fist_C_mesh",
            "hands_C_mesh"
        };

        public static List<string> Dagget_Costume_01_Mat_Instance = new List<string>
        {
            "body_C_mesh",
            "body_mirror_C_mesh",
            "costume00_hair_C_mesh",
            "costume00_hair_mirror_C_mesh",
            "dagnadobody_C_mesh",
            "attack_C_mesh",
            "default_C_mesh",
            "happy_C_mesh",
            "hurt_C_mesh",
            "idle_C_mesh",
            "default_mirror_C_mesh",
            "happy_mirror_C_mesh",
            "hurt_mirror_C_mesh",
            "eyebrows_C_mesh",
            "eyebrows_mirror_C_mesh",
            "downeyelid_L_mesh",
            "downeyelid_R_mesh",
            "upeyelid_L_mesh",
            "upeyelid_R_mesh",
            "feet_C_mesh",
            "fist_C_mesh",
            "hands_C_mesh"
        };

        public static List<string> Dagget_Costume_01_Mat = new List<string>
        {
            "dagnado_C_mesh"
        };


        public static List<string> AngryBeavers_Props02_Mat = new List<string>
        {
            "bike_C_mesh",
            "mindulator_C_mesh",
            "mindulatorglass_C_mesh",
            "woodenlog_C_mesh"
        };

        public static List<string> AngryBeavers_Props01_Mat = new List<string>
        {
            "bowlingball_C_mesh",
            "boxingglove_C_mesh",
            "golf_C_mesh",
            "mallet_C_mesh",
            "potatoes_C_mesh",
            "woodsign_C_mesh"
        };

        public static Dictionary<string, List<string>> Norbert0Parts = new Dictionary<string, List<string>>()
        {
            { "Norbet_Mat", Norbet_Mat },
            { "Norbet_Mat_Instance", Norbet_Mat_Instance },
            { "Dagget_Costume_01_Mat_Instance", Dagget_Costume_01_Mat_Instance },
            { "Dagget_Costume_01_Mat", Dagget_Costume_01_Mat },
            { "AngryBeavers_Props02_Mat", AngryBeavers_Props02_Mat },
            { "AngryBeavers_Props01_Mat", AngryBeavers_Props01_Mat }
        };
        public static Dictionary<int, Dictionary<string, List<string>>> NorbertaltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, Norbert0Parts},

        };
    }
}
