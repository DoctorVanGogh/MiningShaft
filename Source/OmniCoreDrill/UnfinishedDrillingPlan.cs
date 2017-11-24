using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace DoctorVanGogh.OmniCoreDrill {
    public class UnfinishedDrillingPlan : UnfinishedThing {
        public override string LabelNoCount => LanguageKeys.keyed.ocd_unfinishedPlan.Translate(Recipe.products[0].thingDef.label);
    }
}
