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

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "GlobalAuraCurseModificationByTraitsAndItems")]
        public static void GlobalAuraCurseModificationByTraitsAndItemsPostfix(ref AtOManager __instance, ref AuraCurseData __result, string _type, string _acId, Character _characterCaster, Character _characterTarget)
        {
            LogInfo($"GACM MoreMadness");
            Character characterOfInterest = _type == "set" ? _characterTarget : _characterCaster;
            // bool gainsPerksNPC = IsLivingNPC(characterOfInterest) && difficultyLevelInt >= (int)DifficultyLevelEnum.Hard && HasCorruptor(Corruptors.Decadence);
            // bool gainsPerksHero = IsLivingHero(characterOfInterest) && difficultyLevelInt >= (int)DifficultyLevelEnum.Hard && HasCorruptor(Corruptors.Decadence);
            string enchantId;
            if (!IsLivingNPC(characterOfInterest))
            {
                return;
            }
            switch (_acId)
            {
                case "evasion":
                    enchantId = "montproliferate";
                    if (NpcHaveEnchant(characterOfInterest, enchantId))
                    {
                        __result.GainCharges = true;
                    }
                    enchantId = "montdistraction";
                    if (NpcTeamHaveEnchant(enchantId))
                    {
                        __result.GainCharges = true;
                    }
                    break;
                case "fast":
                    enchantId = "montproliferate";
                    if (NpcHaveEnchant(characterOfInterest, enchantId))
                    {
                        __result.GainCharges = true;
                    }
                    enchantId = "montnimblehops";
                    if (NpcHaveEnchant(characterOfInterest, enchantId))
                    {
                        __result.GainCharges = true;
                    }
                    break;
                case "buffer":
                    enchantId = "montproliferate";
                    if (NpcHaveEnchant(characterOfInterest, enchantId))
                    {
                        __result.GainCharges = true;
                    }
                    enchantId = "montluxuriouscoat";
                    if (NpcHaveEnchant(characterOfInterest, enchantId))
                    {
                        __result.GainCharges = true;
                    }
                    break;
                case "zeal":
                    enchantId = "montluxuriouscoat";
                    if (NpcHaveEnchant(characterOfInterest, enchantId))
                    {
                        __result.GainCharges = true;
                    }
                    break;
                case "sharp":
                    enchantId = "montluxuriouscoat";
                    if (NpcHaveEnchant(characterOfInterest, enchantId))
                    {
                        __result.AuraDamageType3 = Enums.DamageType.Mind;
                        __result.AuraDamageIncreasedPerStack3 = 1;
                    }
                    break;
            }
        }


    }
}