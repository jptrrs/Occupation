using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using System.Collections.Generic;
using Verse;

namespace Occupation
{
    //Fires the Occupation quest with every enemy raid.
    [HarmonyPatch(typeof(IncidentWorker_RaidEnemy), nameof(IncidentWorker_RaidEnemy.GenerateRaidLoot))]
    public static class IncidentWorker_RaidEnemy_GenerateRaidLoot
    {
        public static void Postfix(IncidentParms parms, List<Pawn> pawns)
        {
            Slate slate = new Slate();
            Log.Message($"DEBUG IncidentWorker_RaidEnemy_GenerateRaidLoot: {pawns.Count} pawns");
            slate.Set<IEnumerable<Pawn>>("occupiers", pawns);
            slate.Set<Faction>("enemyFaction", parms.faction);
            //slate.Set<Pawn>("asker", parms.faction.leader);
            slate.Set<bool>("hostile", parms.faction.def.permanentEnemy);
            QuestUtility.SendLetterQuestAvailable(QuestUtility.GenerateQuestAndMakeAvailable(DefDatabase<QuestScriptDef>.GetNamed("Occupation"), slate));
        }
    }
}