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
        [HarmonyPatch(typeof(MapManager), "DoCombat")]
        public static void DoCombatPrefix(MapManager __instance, ref CombatData _combatData)
        {

            LogDebug($"DoCombatPrefix: {string.Join(", ", AtOManager.Instance.bossesKilledName ?? new List<string>())}");
            bool killedLordMont = AtOManager.Instance.bossesKilledName != null && AtOManager.Instance.bossesKilledName.Any<string>((Func<string, bool>)(s => s.StartsWith("lordmontimus", StringComparison.OrdinalIgnoreCase)));
            if (_combatData.CombatId == "evoidhigh_13b" && killedLordMont)
            {
                LogDebug("DoCombatPrefix - Getting Combat Data for Archon Mont");
                try
                {
                    _combatData = Globals.Instance.GetCombatData("evoidhigh_13archonmont");
                }
                catch (Exception e)
                {
                    LogError($"DoCombatPrefix - Error getting combat data: {e.Message}");
                    return; // Prevent further execution if combat data cannot be retrieved
                }
            }
            // else if (AtOManager.Instance.bossesKilledName != null && AtOManager.Instance.bossesKilledName.Any<string>((Func<string, bool>)(s => s.StartsWith("lordmontimus", StringComparison.OrdinalIgnoreCase))))
            // {
            //     LogDebug("DoCombatPrefix - Killed Lord Montshek");
            //     if (!AtOManager.Instance.bossesKilledName.Any<string>((Func<string, bool>)(s => s.StartsWith("archonmont", StringComparison.OrdinalIgnoreCase))))
            //     {
            //         LogDebug("DoCombatPrefix - Archon Mont Combat starting");
            //         AtOManager.Instance.SetCombatData(Globals.Instance.GetCombatData("evoidhigh_13archonmont"));
            //         DoCombat(__instance, AtOManager.Instance.GetCurrentCombatData());
            //         return false;
            //     }
            //     AtOManager.Instance.FinishGame();
            //     return false;
            // }
            // return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapManager), "SetPositionInCurrentNode")]
        public static bool SetPositionInCurrentNodePrefix(MapManager __instance)
        {

            string str = AtOManager.Instance.currentMapNode;  //CurrentNode(__instance);
            LogDebug($"SetPositionInCurrentNodePrefix + {str}: {string.Join(", ", AtOManager.Instance.bossesKilledName ?? new List<string>())}");
            if (str == "voidhigh_13" && AtOManager.Instance.bossesKilledName != null && AtOManager.Instance.bossesKilledName.Any<string>((Func<string, bool>)(s => s.StartsWith("lordmontimus", StringComparison.OrdinalIgnoreCase))))
            {
                LogDebug("SetPositionInCurrentNodePrefix - Killed Lord Montshek");
                if (!AtOManager.Instance.bossesKilledName.Any<string>((Func<string, bool>)(s => s.StartsWith("archonmont", StringComparison.OrdinalIgnoreCase))))
                {
                    LogDebug("SetPositionInCurrentNodePrefix - Archon Mont Combat starting");
                    AtOManager.Instance.SetCombatData(Globals.Instance.GetCombatData("evoidhigh_13archonmont"));
                    DoCombat(__instance, AtOManager.Instance.GetCurrentCombatData());
                    return false;
                }
                AtOManager.Instance.FinishGame();
                return false;
            }
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), "DoItem")]

        public static void DoItemPostfix(
            ref Character __instance,
            Enums.EventActivation theEvent,
            CardData cardData,
            string item,
            Character target,
            int auxInt,
            string auxString,
            int order,
            CardData _castedCard)
        {
            if (item.StartsWith("montproliferate"))
            {
                LogDebug("Removing Proliferate");
                if (__instance.Enchantment.StartsWith("montproliferate"))
                {
                    __instance.Enchantment = "";
                }
                if (__instance.Enchantment2.StartsWith("montproliferate"))
                {
                    __instance.Enchantment2 = "";
                }
                if (__instance.Enchantment3.StartsWith("montproliferate"))
                {
                    __instance.Enchantment3 = "";
                }

                __instance.ReorganizeEnchantments();
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