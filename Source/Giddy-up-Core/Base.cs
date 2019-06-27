using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HugsLib;
using HugsLib.Utils;
using GiddyUpCore.Storage;
using GiddyUpCore.Utilities;
using Verse;
using UnityEngine;
using HugsLib.Settings;
using RimWorld;
using GiddyUpCore.Concepts;

namespace GiddyUpCore
{
    public class Base : ModBase
    {
        private ExtendedDataStorage _extendedDataStorage;
        public static Base Instance { get; private set; }
        internal static SettingHandle<float> handlingMovementImpact;
        internal static SettingHandle<int> accuracyPenalty;
        internal static SettingHandle<float> handlingAccuracyImpact;
        public static SettingHandle<DictAnimalRecordHandler> animalSelecter;
        public static SettingHandle<DictAnimalRecordHandler> drawSelecter;
        internal static SettingHandle<String> tabsHandler;
        internal static SettingHandle<float> bodySizeFilter;
        private static Color highlight1 = new Color(0.5f, 0, 0, 0.1f);
        String[] tabNames = { "GUC_tab1".Translate(), "GUC_tab2".Translate()};
        internal static bool GiddyUpMechanoidsLoaded = false;
        internal static List<PawnKindDef> animalsWithBiome = new List<PawnKindDef>();
        internal static List<PawnKindDef> animalsWithoutBiome = new List<PawnKindDef>();
        private int minPercentage = 0;
        private int maxPercentage = 100;
        internal static bool facialStuffLoaded;
        public override string ModIdentifier
        {
            get { return "GiddyUpCore"; }
        }
        public Base()
        {
            Instance = this;
        }
        public override void DefsLoaded()
        {
            base.DefsLoaded();
            if (AssemblyExists("GiddyUpMechanoids"))
            {
                GiddyUpMechanoidsLoaded = true;
            }
            if (AssemblyExists("FacialStuff"))
            {
                facialStuffLoaded = true;
            }

            List<ThingDef> allAnimals = DefUtility.getAnimals();
            allAnimals = allAnimals.OrderBy(o => o.defName).ToList();

            handlingMovementImpact = Settings.GetHandle<float>("handlingMovementImpact", "GUC_HandlingMovementImpact_Title".Translate(), "GUC_HandlingMovementImpact_Description".Translate(), 2.5f, Validators.FloatRangeValidator(0f, 10f));
            accuracyPenalty = Settings.GetHandle<int>("accuracyPenalty", "GUC_AccuracyPenalty_Title".Translate(), "GUC_AccuracyPenalty_Description".Translate(), 10, Validators.IntRangeValidator(minPercentage, maxPercentage));
            handlingAccuracyImpact = Settings.GetHandle<float>("handlingAccuracyImpact", "GUC_HandlingAccuracyImpact_Title".Translate(), "GUC_HandlingAccuracyImpact_Description".Translate(), 0.5f, Validators.FloatRangeValidator(0f, 2f));


            tabsHandler = Settings.GetHandle<String>("tabs", "GUC_Tabs_Title".Translate(), "", "none");
            bodySizeFilter = Settings.GetHandle<float>("bodySizeFilter", "GUC_BodySizeFilter_Title".Translate(), "GUC_BodySizeFilter_Description".Translate(), 0.8f);
            animalSelecter = Settings.GetHandle<DictAnimalRecordHandler>("Animalselecter", "GUC_Animalselection_Title".Translate(), "GUC_Animalselection_Description".Translate(), null);
            drawSelecter = Settings.GetHandle<DictAnimalRecordHandler>("drawSelecter", "GUC_Drawselection_Title".Translate(), "GUC_Drawselection_Description".Translate(), null);


            tabsHandler.CustomDrawer = rect => { return DrawUtility.CustomDrawer_Tabs(rect, tabsHandler, tabNames); };

            bodySizeFilter.CustomDrawer = rect => { return DrawUtility.CustomDrawer_Filter(rect, bodySizeFilter, false, 0, 5, highlight1); };
            animalSelecter.CustomDrawer = rect => { return DrawUtility.CustomDrawer_MatchingAnimals_active(rect, animalSelecter, allAnimals, bodySizeFilter, "GUC_SizeOk".Translate(), "GUC_SizeNotOk".Translate()); };
            bodySizeFilter.VisibilityPredicate = delegate { return tabsHandler.Value == tabNames[0]; };
            animalSelecter.VisibilityPredicate = delegate { return tabsHandler.Value == tabNames[0]; };


            drawSelecter.CustomDrawer = rect => { return DrawUtility.CustomDrawer_MatchingAnimals_active(rect, drawSelecter, allAnimals, null, "GUC_DrawFront".Translate(), "GUC_DrawBack".Translate()); };
            drawSelecter.VisibilityPredicate = delegate { return tabsHandler.Value == tabNames[1]; };


            DrawUtility.filterAnimals(ref animalSelecter, allAnimals, bodySizeFilter);
            DrawUtility.filterAnimals(ref drawSelecter, allAnimals, null);

            foreach (ThingDef td in allAnimals)
            {
                if (td.HasModExtension<DrawingOffsetPatch>())
                {
                    td.GetModExtension<DrawingOffsetPatch>().Init();
                }
            }

            foreach(BiomeDef biomeDef in DefDatabase<BiomeDef>.AllDefs)
            {
                foreach(PawnKindDef animalKind in biomeDef.AllWildAnimals)
                {
                    if (!animalsWithBiome.Contains(animalKind))
                    {
                        animalsWithBiome.Add(animalKind);
                    }
                }
            }
            foreach (PawnKindDef animalWithoutBiome in from d in DefDatabase<PawnKindDef>.AllDefs
                                               where d.RaceProps.Animal &&  !animalsWithBiome.Contains(d)
                                          select d)
            {
                animalsWithoutBiome.Add(animalWithoutBiome);
            }
            /*
            foreach (PawnKindDef pd in DefDatabase<PawnKindDef>.AllDefs)
            {
                if (pd.HasModExtension<CustomMountsPatch>())
                {
                    pd.GetModExtension<CustomMountsPatch>().Init();
                }
            }
            */
        }


        public override void WorldLoaded()
        {
            _extendedDataStorage = UtilityWorldObjectManager.GetUtilityWorldObject<ExtendedDataStorage>();
            _extendedDataStorage.Cleanup();
            base.WorldLoaded();
            LessonAutoActivator.TeachOpportunity(GUC_ConceptDefOf.GUC_Animal_Handling, OpportunityType.GoodToKnow);

        }

        //Mod names sometimes change when Rimworld changes its version. Checking for the assembly name, which probably won't change is therefore a better idea than using ModLister.HasActiveModWithName
        private bool AssemblyExists(string assemblyName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith(assemblyName))
                    return true;
            }
            return false;
        }

        public ExtendedDataStorage GetExtendedDataStorage()
        {
            return _extendedDataStorage;
        }
    }


}
