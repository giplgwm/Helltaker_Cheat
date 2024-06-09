using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using Helltakercheat;

namespace Helltakercheat
{
    public class HellCheat : MelonMod
    {
        public Rect windowRect = new Rect(20, 20, 200, 255);
        public bool noClipObstacles = false;
        public bool noClipWalls = false;
        public static bool maxWill = false;
        public GameObject border;

        public override void OnGUI()
        {
            windowRect = GUI.Window(420024, windowRect, MakeGuiWork, "Helltaker cheat -");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            checkBoxCollider2Ds();
        }

        public void MakeGuiWork(int windowID)
        {
            if (GUILayout.Button("Kill all enemies"))
            {
                DestroyAllBreakables();
            }
            noClipObstacles = GUILayout.Toggle(noClipObstacles, "No Clip Obstacles");
            noClipWalls = GUILayout.Toggle(noClipWalls, "No Clip Walls");

            if (GUILayout.Button("TP to Goal"))
            {
                TPtoGoal();
            }
            if (GUI.changed)
            {
                checkBoxCollider2Ds();
            }

            maxWill = GUILayout.Toggle(maxWill, "Max Will");

        }

        public void checkBoxCollider2Ds()
        {
            GameObject[] unbreakableObjects = GameObject.FindGameObjectsWithTag("unBreakable");
            GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
            GameObject[] spikes = GameObject.FindGameObjectsWithTag("spikes");
            if (noClipObstacles)
            {
                // Iterate through each found object
                SetActiveBoxCollider2D(unbreakableObjects, false);
                SetActiveBoxCollider2D(walls, false);
                SetActiveBoxCollider2D(spikes, false);
            }
            else
            {
                // Iterate through each found object
                SetActiveBoxCollider2D(unbreakableObjects, true);
                SetActiveBoxCollider2D(walls, true);
                SetActiveBoxCollider2D(spikes, false);
            }

            if (noClipWalls)
            {
                border = GameObject.Find("border");
                border.SetActive(false);
            }
            else
            {
                if (border != null)
                {
                    border.SetActive(true);
                }
            }
        }

        public void SetActiveBoxCollider2D(GameObject[] obj_list, bool active)
        {
            foreach (GameObject obj in obj_list)
            {
                if (obj.GetComponent<BoxCollider2D>().enabled != active)
                {
                    obj.GetComponent<BoxCollider2D>().enabled = active;
                }
                else
                {
                    break;
                }
            }
        }

        public void DestroyAllBreakables()
        {
            GameObject[] breakableObjects = GameObject.FindGameObjectsWithTag("breakable");

            // Iterate through each found object
            foreach (GameObject breakableObject in breakableObjects)
            {
                GameObject.Destroy(breakableObject);
            }
        }

        public void TPtoGoal()
        {
            GameObject player = GameObject.Find("player");
            GameObject goal = GameObject.Find("demon_regular");
            if (player != null && goal != null)
            {
                player.transform.position = goal.transform.position - new Vector3(1, 0, 0);
            }
        }

    }

    [HarmonyPatch(typeof(Player), "MinusWill")]
    static class WillPatch
    {
        private static void Postfix(Player __instance)
        {
            if (HellCheat.maxWill)
            {
                __instance.will = 49;
            }
                
        }
    }


}

