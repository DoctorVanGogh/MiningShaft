using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Verse;

namespace DoctorVanGogh.OmniCoreDrill {
    class MaterialParameters : IExposable {
        private Multiplier _work;
        private Multiplier _yield;

        private ThingDef _material;

        public Multiplier Work => _work;

        public Multiplier Yield => _yield;

        public MaterialParameters() {
            _work = new Multiplier();
            _yield = new Multiplier();

            _work.PropertyChanged += Component_PropertyChanged;
            _yield.PropertyChanged += Component_PropertyChanged;
        }

        public MaterialParameters(ThingDef material) : this() {
            _material = material;
        }

        private void Component_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case nameof(Multiplier.Value):
                    ThingDefGenerator.UpdateGeneratedDef(_material);
                    break;
            }
        }

        public void ExposeData() {
            if (Scribe.mode == LoadSaveMode.LoadingVars) {
                _work.PropertyChanged -= Component_PropertyChanged;
                _yield.PropertyChanged -= Component_PropertyChanged;
            }

            Scribe_Deep.Look(ref _work, "work");
            Scribe_Deep.Look(ref _yield, "yield");
            Scribe_Defs.Look(ref _material, "material");

            if (Scribe.mode == LoadSaveMode.LoadingVars) {
                _work.PropertyChanged += Component_PropertyChanged;
                _yield.PropertyChanged += Component_PropertyChanged;
            }
        }
    }
}
