using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
// using static Obeliskial_Essentials.Essentials;
using System;
using static Montimus.Plugin;
using static Montimus.CustomFunctions;
using static Montimus.MontimusFunctions;
using System.Collections.Generic;
using static Functions;
using UnityEngine;
// using Photon.Pun;
using TMPro;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Reflection;
using UnityEngine.UIElements;
// using Unity.TextMeshPro;

// Make sure your namespace is the same everywhere
namespace Montimus
{

    [HarmonyPatch] // DO NOT REMOVE/CHANGE - This tells your plugin that this is part of the mod

    public class MontimusPatches
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapManager), "SetPositionInCurrentNode")]
        public static void SetPositionInCurrentNodePrefix(MapManager __instance)
        {
            string str = AtOManager.Instance.currentMapNode;  //CurrentNode(__instance);
            if (str == "voidhigh_13" && AtOManager.Instance.bossesKilledName != null && AtOManager.Instance.bossesKilledName.Any<string>((Func<string, bool>)(s => s.StartsWith("lordmontimus", StringComparison.OrdinalIgnoreCase))))
            {
                if (!AtOManager.Instance.bossesKilledName.Any<string>((Func<string, bool>)(s => s.StartsWith("archonmont", StringComparison.OrdinalIgnoreCase))))
                {
                    AtOManager.Instance.SetCombatData(Globals.Instance.GetCombatData("evoidhigh_13archonmont"));
                    DoCombat(__instance, AtOManager.Instance.GetCurrentCombatData());
                    return;
                }
                AtOManager.Instance.FinishGame();
                return;
            }
        }


    }
}