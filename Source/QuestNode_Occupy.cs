using System.Collections.Generic;
using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Occupation
{
    public class QuestNode_Occupy : QuestNode
    {
        [NoTranslate] public SlateRef<string> inSignal;
        public SlateRef<IEnumerable<Pawn>> pawns;
        public SlateRef<Faction> faction;

        public override void RunInt()
        {
            Slate slate = QuestGen.slate;
            //string test = pawns.GetValue(slate).EnumerableNullOrEmpty() ? "empty" : pawns.GetValue(slate).EnumerableCount().ToString();
            //Log.Message($"Running! pawns on slate: {test}");
            if (pawns.GetValue(slate) == null) return;
            string signal = QuestGenUtility.HardcodedSignalWithQuestID(inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal");
            QuestGen.quest.AddPart(new QuestPart_Occupy
            {
                inSignal = signal,
                faction = faction.GetValue(slate),
                pawns = pawns.GetValue(slate)
            }); ;
        }

        public override bool TestRunInt(Slate slate)
        {
            return true;
        }
    }

    public class QuestPart_Occupy : QuestPart
    {
        public string inSignal;
        public IEnumerable<Pawn> pawns;
        public Faction faction;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref pawns, "pawns");
        }

        public override void Notify_QuestSignalReceived(Signal signal)
        {
            base.Notify_QuestSignalReceived(signal);
            if (signal.tag != inSignal || pawns.EnumerableNullOrEmpty()) return;
            ConvertToGuests();
            //Find.LetterStack.ReceiveLetter("ImplantsRecieved".Translate(),
            //    "ImplantsRecievedText".Translate(Faction.Empire.Name), LetterDefOf.PositiveEvent);
            //Find.World.gameConditionManager.GetActiveCondition<GameCondition_Corruption>()?.CheckPawns();
            //Faction.Empire.allowRoyalFavorRewards = true;
        }

        private void ConvertToGuests()
        {
            Faction.OfPlayer.SetRelationDirect(faction, FactionRelationKind.Neutral);
            foreach (var pawn in pawns)
            {
                pawn.ClearMind();
                pawn.guest.SetGuestStatus(Faction.OfPlayer);
            }
        }

        //public override void AssignDebugData()
        //{
        //    base.AssignDebugData();
        //    this.inSignal = "DebugSignal" + Rand.Int;
        //    if (Find.AnyPlayerHomeMap == null)
        //        return;
        //    Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
        //    IntVec3 center = DropCellFinder.RandomDropSpot(randomPlayerHomeMap);
        //    this.shuttle = ThingMaker.MakeThing(ThingDefOf.Shuttle);
        //    GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, this.shuttle), center,
        //        randomPlayerHomeMap, ThingPlaceMode.Near);
        //}

        //public override void ReplacePawnReferences(Pawn replace, Pawn with)
        //{
        //    shuttle?.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
        //}
    }
}