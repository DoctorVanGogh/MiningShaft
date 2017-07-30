using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace DoctorVanGogh.OmniCoreDrill {
    public static class ThingDefGenerator {

        private const float workPerHitpoint = 2f;
        private const float densityFactor = 40f;
        private const float commnalityFactor = 2000;

        private const float minScatterCommonality = 0.001f;

        public static IEnumerable<RecipeDef> MineDeepResourceDefs() {

            foreach (var source in DefDatabase<ThingDef>.AllDefs.Where(td => td.mineable && td.building?.mineableThing != null)) {
                var b = source.building;
                var t = b.mineableThing;

                var hitpointsPerLump = source.statBases.FirstOrDefault(sm => sm.stat == DefReferences.Stat_MaxHitPoints)?.value;

                if (hitpointsPerLump == null) {
                    Log.Warning($"Cannot determine hitpoints for {source.defName} - skipping deep resource extraction recipe.");
                    continue;
                }

                var work = hitpointsPerLump.Value*workPerHitpoint
                           + (b.isResourceRock
                               ? (float)(commnalityFactor/Math.Sqrt(Math.Max(b.mineableScatterCommonality, minScatterCommonality)))
                               : 0);

                var yield = b.isResourceRock
                    ? (b.mineableScatterLumpSizeRange.Average*b.mineableYield) / densityFactor
                    : 1f;

                // scale up work if extremely low yield (example 'components')
                if (yield < 1f) {
                    work = work/yield;
                    yield = 1f;
                }

                int iYield = (int) Math.Round(yield);

                //Log.Message($"{t.defName}: Work={work:n0}, Yield={iYield}");


                yield return new RecipeDef {
                                 workAmount = work,
                                 efficiencyStat = DefReferences.Stat_MiningYield,
                                 workSpeedStat = DefReferences.Stat_MiningSpeed,
                                 effectWorking = DefReferences.Effecter_Drill,
                                 workSkillLearnFactor = 0.2f,
                                 workSkill = DefReferences.Skill_Mining,
                                 defName = $"OCD_MineDeep{t.defName}",
                                 label = LanguageKeys.keyed.ocd_label.Translate(t.LabelCap),
                                 description = LanguageKeys.keyed.ocd_description.Translate(source.LabelCap, iYield, t.LabelCap),
                                 jobString = LanguageKeys.keyed.ocd_jobString.Translate(t.LabelCap),
                                 products = new List<ThingCountClass> {
                                                new ThingCountClass {
                                                    thingDef = t,
                                                    count = iYield
                                                }
                                            },
                                 recipeUsers = new List<ThingDef> {
                                                   DefReferences.Thing_CoreDrill
                                               },
                                 

                             };
            }
        }
    }
}