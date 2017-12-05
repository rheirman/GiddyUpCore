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
using RunAndGun.Utilities;

namespace GiddyUpCore
{
    public class Base : ModBase
    {
        private ExtendedDataStorage _extendedDataStorage;
        public static Base Instance { get; private set; }
        public static SettingHandle<DictAnimalRecordHandler> animalSelecter;
        public static SettingHandle<DictAnimalRecordHandler> drawSelecter;
        internal static SettingHandle<String> tabsHandler;
        internal static SettingHandle<float> bodySizeFilter;
        private static Color highlight1 = new Color(0.5f, 0, 0, 0.1f);
        String[] tabNames = { "GUC_tab1".Translate(), "GUC_tab2".Translate()};

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
      
            List<ThingDef> allAnimals = DefUtility.getAnimals();
            allAnimals = allAnimals.OrderBy(o => o.defName).ToList();

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

            if(animalSelecter.Value == null)
            {
                animalSelecter.Value = getDefaultForAnimalSelecter(allAnimals);
            }
            if(drawSelecter.Value == null)
            {
                drawSelecter.Value = getDefaultForDrawSelecter(allAnimals);
            }

        }

        private DictAnimalRecordHandler getDefaultForAnimalSelecter(List<ThingDef> allAnimals)
        {
            DictAnimalRecordHandler dict = new DictAnimalRecordHandler();
            Dictionary<String, AnimalRecord> result = new Dictionary<string, AnimalRecord>();
            foreach(ThingDef animal in allAnimals)
            {
                CompProperties_Mount prop = animal.GetCompProperties<CompProperties_Mount>();

                float mass = animal.race.baseBodySize;
                if (prop != null && prop.isException)
                {
                    result.Add(animal.defName, new AnimalRecord(false, true, animal.label));   
                }
                else
                {
                    bool shouldSelect = mass >= bodySizeFilter.Value;
                    result.Add(animal.defName, new AnimalRecord(shouldSelect, false, animal.label));
                }
            }
            //result.Add("", new AnimalRecord(shouldSelect, false));
            dict.InnerList = result;
            return dict;
        }
        private DictAnimalRecordHandler getDefaultForDrawSelecter(List<ThingDef> allAnimals)
        {
            DictAnimalRecordHandler dict = new DictAnimalRecordHandler();
            Dictionary<String, AnimalRecord> result = new Dictionary<string, AnimalRecord>();
            foreach (ThingDef animal in allAnimals)
            {
                CompProperties_Mount prop = animal.GetCompProperties<CompProperties_Mount>();

                float mass = animal.race.baseBodySize;
                if (prop != null && prop.drawFront)
                {
                    result.Add(animal.defName, new AnimalRecord(true, true, animal.label));
                }
                else
                {
                    result.Add(animal.defName, new AnimalRecord(false, false, animal.label));
                }
            }
            //result.Add("", new AnimalRecord(shouldSelect, false));
            dict.InnerList = result;
            return dict;
        }

        public override void WorldLoaded()
        {
            _extendedDataStorage = UtilityWorldObjectManager.GetUtilityWorldObject<ExtendedDataStorage>();
            base.WorldLoaded();
        }

        public ExtendedDataStorage GetExtendedDataStorage()
        {
            return _extendedDataStorage;
        }
    }


}
