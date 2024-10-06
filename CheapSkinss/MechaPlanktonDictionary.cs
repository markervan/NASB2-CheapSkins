using System;
using System.Collections.Generic;
using System.Text;

namespace CheapSkinss
{
    internal class MechaPlanktonDictionary
    {
        public static List<string> MechaPlanktonBody = new List<string>
            {
                "antennabase_C_mesh",
                "antennaheli_C_mesh",
                "arms_C_mesh",
                "bodybottom_C_mesh",
                "costume01_legs_C_mesh",
                "costume01_middlebottom_C_mesh",
                "costume01_middletop_C_mesh",
                "costume01_top_C_mesh",
                "angry_C_mesh",
                "default_C_mesh",
                "downeyelid_C_mesh",
                "upeyelid_C_mesh",
                "costume02_legs_C_mesh",
                "costume02_middlebottom_C_mesh",
                "costume02_middletop_C_mesh",
                "costume02_mouth_low_C_mesh",
                "costume02_top_C_mesh",
                "costume03_04_eyeOutline_C_mesh",
                "costume03_04_legs_C_mesh",
                "costume03_04_middlebottom_C_mesh",
                "costume03_04_middletop_C_mesh",
                "costume03_04_top_C_mesh"
            };

        public static List<string> MechaPlanktonProps = new List<string>
            {
                "cannonstand_C_mesh",
                "chain_C_mesh",
                "chainclaw_C_mesh",
                "missile_C_mesh",
                "note_C_mesh"
            };

        public static List<string> MechaPlanktonKaren = new List<string>
            {
                "karenantenna_C_mesh",
                "karenantenna_L_mesh",
                "karenantenna_R_mesh",
                "karenarms_C_mesh",
                "karenbody_C_mesh",
                "karenlaser_C1_mesh",
                "karenlaser_C2_mesh",
                "karenmonitorarm_C_mesh"
            };

        public static List<string> MechaPlanktonPlankton = new List<string>
            {
                "body_C_mesh",
                "angryA_C_mesh",
                "angryB_C_mesh",
                "angryC_C_mesh",
                "default_C_mesh",
                "happy_C_mesh",
                "idle_C_mesh",
                "taunt_1_C_mesh",
                "taunt_3_a_C_mesh",
                "taunt_3_b_C_mesh",
                "downeyelid_C_mesh",
                "upeyelid_C_mesh"
            };

        public static Dictionary<string, List<string>> MechaPlankton0Parts = new Dictionary<string, List<string>>
            {
                { "MechaPlankton_Costume_01_Mat", MechaPlanktonBody },
                { "MechaPlankton_Props_Costume_01_Mat", MechaPlanktonProps },
                { "MechaPlankton_Karen_Mat", MechaPlanktonKaren },
                { "Plankton_Mat", MechaPlanktonPlankton }
            };

        public static Dictionary<string, List<string>> MechaPlankton1Parts = new Dictionary<string, List<string>>
            {
                { "MechaPlankton_Costume_02_Mat", MechaPlanktonBody },
                { "MechaPlankton_Props_Costume_02_Mat", MechaPlanktonProps },
                { "MechaPlankton_Karen_Mat", MechaPlanktonKaren },
                { "Plankton_Mat", MechaPlanktonPlankton }
            };

        public static Dictionary<string, List<string>> MechaPlankton2Parts = new Dictionary<string, List<string>>
            {
                { "MechaPlankton_Costume_03_Mat", MechaPlanktonBody },
                { "MechaPlankton_Props_Costume_03_Mat", MechaPlanktonProps },
                { "MechaPlankton_Karen_Mat", MechaPlanktonKaren },
                { "Plankton_Mat", MechaPlanktonPlankton }
            };

        public static Dictionary<string, List<string>> MechaPlankton3Parts = new Dictionary<string, List<string>>
            {
                { "MechaPlankton_Costume_04_Mat", MechaPlanktonBody },
                { "MechaPlankton_Props_Costume_04_Mat", MechaPlanktonProps },
                { "MechaPlankton_Karen_Mat", MechaPlanktonKaren },
                { "Plankton_Mat", MechaPlanktonPlankton }
            };


        public static Dictionary<int, Dictionary<string, List<string>>> MechaPlanktonAltParts = new Dictionary<int, Dictionary<string, List<string>>>
            {
                { 0, MechaPlankton0Parts},
                { 1, MechaPlankton1Parts},
                { 2, MechaPlankton2Parts},
                { 3, MechaPlankton3Parts}
            };
        public static Dictionary<string, Dictionary<int, Dictionary<string, List<string>>>> characterCodenames = new Dictionary<string, Dictionary<int, Dictionary<string, List<string>>>>
            {
                { "MechaPlankton", MechaPlanktonAltParts }
            };
    }
}