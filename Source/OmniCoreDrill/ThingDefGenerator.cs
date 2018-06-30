using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace DoctorVanGogh.OmniCoreDrill {
    public static class ThingDefGenerator {
        private static IDictionary<ThingDef, RecipeDef> _miningRecipes;

        private static IDictionary<ThingDef, DrillingProperties> _drillProperties;

        private static IDictionary<ThingDef, string> _sourceLabels;

        public static ICollection<RecipeDef> GetCoreMiningDefs() {

            if (_miningRecipes == null) {

                _miningRecipes = new Dictionary<ThingDef, RecipeDef>();
                _drillProperties = new Dictionary<ThingDef, DrillingProperties>();
                _sourceLabels = new Dictionary<ThingDef, string>();

                foreach (ThingDef source in DefDatabase<ThingDef>.AllDefs.Where(td => td.mineable && td.building?.mineableThing != null)) {
                    BuildingProperties b;

                    DrillingProperties props = CalculateDrillProperties(source, out b);
                    if (props == null) {
                        Log.Warning($"Cannot determine hitpoints for {source.defName} - skipping deep resource extraction recipe.");
                        continue;
                    }

                    ThingDef t = b.mineableThing;

                    var recipe = new RecipeDef {
                                     efficiencyStat = StatDefOf.MiningYield,
                                     workSpeedStat = StatDefOf.MiningSpeed,
                                     effectWorking = EffecterDefOf.Drill,
                                     workSkillLearnFactor = 0.2f,
                                     workSkill = SkillDefOf.Mining,
                                     defName = $"OCD_MineDeep{t.defName}",
                                     label = LanguageKeys.keyed.ocd_label.Translate(t.LabelCap),
                                     jobString = LanguageKeys.keyed.ocd_jobString.Translate(t.LabelCap),
                                     products = new List<ThingDefCountClass> {
                                                    new ThingDefCountClass {
                                                        thingDef = t,
                                                        count = 0
                                                    }
                                                },
                                     recipeUsers = new List<ThingDef> {
                                                       DefReferences.Thing_CoreDrill
                                                   },
                                     unfinishedThingDef = DefReferences.Thing_UnfinishedDrillingPlan
                                 };

                    UpdateGeneratedDef(t, recipe, props, source.LabelCap);

                    _miningRecipes[t] = recipe;
                    _drillProperties[t] = props;
                    _sourceLabels[t] = source.LabelCap;
                }
            }
            return _miningRecipes.Values;
        }

        private static DrillingProperties CalculateDrillProperties(ThingDef source, out BuildingProperties b) {
            b = source.building;

            float? hitpointsPerLump = source.statBases.FirstOrDefault(sm => sm.stat == StatDefOf.MaxHitPoints)?.value;

            if (hitpointsPerLump == null) {
                return null;
            }

            return new DrillingProperties(hitpointsPerLump.Value, b);
        }

        public static void UpdateGeneratedDef(ThingDef material) {
            UpdateGeneratedDef(material, _miningRecipes[material], _drillProperties[material], _sourceLabels[material]);
        }

        private static void UpdateGeneratedDef(ThingDef material, RecipeDef recipe, DrillingProperties props, string sourceLabel) {

            float yield = props.Yield;
            float work = props.Work;

            MaterialParameters mp = Current.Game?.GetComponent<GameMaterialParameters>()[material];
            if (mp != null) {
                yield *= mp.Yield.Value;
                work *= mp.Work.Value;
            }

            // scale up work if extremely low yield (example 'components')
            if (yield < 1f) {
                work = work/yield;
                yield = 1f;
            }

            int iYield = (int) Math.Round(yield);

            recipe.workAmount = work;
            recipe.description = LanguageKeys.keyed.ocd_description.Translate(sourceLabel, iYield, material.LabelCap);
            recipe.products.First().count = iYield;
        }

        public static void UpdateAllGeneratedDefs() {
            foreach (var miningRecipe in _miningRecipes) {
                UpdateGeneratedDef(miningRecipe.Key, miningRecipe.Value, _drillProperties[miningRecipe.Key], _sourceLabels[miningRecipe.Key]);
            }
        }

        public static IEnumerable<KeyValuePair<ThingDef, RecipeDef>> AllDrillRecipes => _miningRecipes;
    }
}