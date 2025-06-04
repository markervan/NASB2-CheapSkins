using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CheapSkinss
{
    internal class AprilDictionaries
    {

        public static List<string> April_Costume_01_Mat = new List<string>
        {
            "accesory_C_mesh",
            "accesory_inv_C_mesh",
            "body_C_mesh",
            "downeyelid_L_mesh",
            "downeyelid_R_mesh",
            "eyelashes_L_mesh",
            "eyelashes_R_mesh",
            "upeyelid_L_mesh",
            "upeyelid_R_mesh",
            "hair_C_mesh",
            "hair_inv_C_mesh",
            "head_C_mesh",
            "watch_C_mesh",
            "watch_inv_C_mesh"
        };
        public static List<string> April_Props_Materia = new List<string>
        {
            "blimp_C_mesh",
            "headphones_C_mesh",
            "mic_C_mesh",
            "mic_inv_C_mesh",
            "micboom_C_mesh",
            "cableposes_C_mesh",
            "cablestraight_C_mesh",
            "camera_C_mesh",
            "handheldcamera_C_mesh",
            "notebook_C_mesh",
            "pen_C_mesh",
            "studiocamera_C_mesh",
            "studiocamera_inv_C_mesh_Int"
        };

        public static Dictionary<string, List<string>> April0Parts = new Dictionary<string, List<string>>
        {
            { "April_Costume_01_Mat", April_Costume_01_Mat },
            { "April Props Material", April_Props_Materia },
        };
        public static Dictionary<int, Dictionary<string, List<string>>> AprilAltParts = new Dictionary<int, Dictionary<string, List<string>>>
        {
            { 0, April0Parts},

        };
    }
}
