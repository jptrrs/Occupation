using HarmonyLib;
using Verse;

namespace Occupation
{
    //In case we ditch HugsLib
    [StaticConstructorOnStartup]
    public class HarmonyPatcher
    {
        static HarmonyPatcher()
        {
            var harmony = new Harmony("Occupation");
            harmony.PatchAll();
        }
    }
}