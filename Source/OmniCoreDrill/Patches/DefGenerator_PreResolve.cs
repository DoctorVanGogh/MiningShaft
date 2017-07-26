using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Harmony;
using RimWorld;
using Verse;

namespace DoctorVanGogh.OmniCoreDrill.Patches {

    [HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PreResolve))]
    public class DefGenerator_PreResolve {

        [HarmonyPrefix]
        public static void Prefix() {
            /* HACK: 
             * 
             * patching RimWorld.RecipeDefGenerator.ImpliedRecipeDefs _simply_ _does_ _not_ _work_
             * 
             * no idea why & dont care... injected method never get's called...
             * 
             * This patch works...
             */
            foreach (RecipeDef current3 in ThingDefGenerator.MineDeepResourceDefs()) {
                current3.PostLoad();
                DefDatabase<RecipeDef>.Add(current3);
            }
        }
    }
}
