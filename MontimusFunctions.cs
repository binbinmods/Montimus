using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
// using Obeliskial_Content;
// using Obeliskial_Essentials;
using System.IO;
using static UnityEngine.Mathf;
using UnityEngine.TextCore.LowLevel;
using static Montimus.Plugin;
using System.Collections.ObjectModel;
using static Montimus.CustomFunctions;
using UnityEngine;
using System.Reflection;

namespace Montimus
{
    public class MontimusFunctions
    {


        public static void DoCombat(MapManager __instance, CombatData _combatData)
        {


            // PLog("Testing Reflection version before code");
            MethodInfo methodInfo = __instance.GetType().GetMethod("DoCombat", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { _combatData };
            methodInfo.Invoke(__instance, parameters);
        }

        public static string CurrentNode(MapManager __instance)
        {


            // PLog("Testing Reflection version before code");
            MethodInfo methodInfo = __instance.GetType().GetMethod("CurrentNode", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { };
            methodInfo.Invoke(__instance, parameters);
            return "";
        }

    }
}

