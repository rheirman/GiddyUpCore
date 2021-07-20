using GiddyUpCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GiddyUpCore.Utilities
{
    public static class PawnDataUtility
    {
        public static ExtendedPawnData GetExtendedPawnData(this Pawn pawn)
        {
            if (Base.Instance.GetExtendedDataStorage() is ExtendedDataStorage store && store.GetExtendedDataFor(pawn) is ExtendedPawnData pawnData)
            {
                return pawnData;
            }
            return null;
        }
    }
}
