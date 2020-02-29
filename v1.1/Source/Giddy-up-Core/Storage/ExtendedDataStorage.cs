using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HugsLib.Utils;
using RimWorld.Planet;
using Verse;

namespace GiddyUpCore.Storage
{


    public class ExtendedDataStorage : WorldComponent, IExposable
    {
        private Dictionary<int, ExtendedPawnData> _store =
            new Dictionary<int, ExtendedPawnData>();

        private List<int> _idWorkingList;

        private List<ExtendedPawnData> _extendedPawnDataWorkingList;

        public ExtendedDataStorage(World world) : base(world)
        {
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(
                ref _store, "store",
                LookMode.Value, LookMode.Deep,
                ref _idWorkingList, ref _extendedPawnDataWorkingList);
        }

        // Return the associate extended data for a given Pawn, creating a new association
        // if required.
        public ExtendedPawnData GetExtendedDataFor(Pawn pawn)
        {

            var id = pawn.thingIDNumber;
            if (_store.TryGetValue(id, out ExtendedPawnData data))
            {
                return data;
            }

            var newExtendedData = new ExtendedPawnData();

            _store[id] = newExtendedData;
            return newExtendedData;
        }

        public void DeleteExtendedDataFor(Pawn pawn)
        {
            _store.Remove(pawn.thingIDNumber);
        }

        public void Cleanup()
        {
            List<int> shouldRemove = new List<int>();
            foreach (KeyValuePair<int, ExtendedPawnData> kv in _store)
            {
                if (kv.Value == null || kv.Value.ShouldClean())
                {
                    shouldRemove.Add(kv.Key);
                }
            }
            foreach (int key in shouldRemove)
            {
                _store.Remove(key);
            }
            Log.Message("Cleaned up " + shouldRemove.Count + " depricated records from Giddy-up!");
        }

    }
}
