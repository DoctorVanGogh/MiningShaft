using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace DoctorVanGogh.OmniCoreDrill {
    public class Multiplier : IExposable, INotifyPropertyChanged {

        private float _value = 1f;

        public void ExposeData() {
            Scribe_Values.Look(ref _value, "value", 1f);
        }

        public float Value {
            get { return _value; }
            set {
                if (value.Equals(_value)) return;
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void DoWindowContents(Rect target, float minValue, float maxValue, string minLabel, string maxLabel) {
            Value = ExtraWidgets.LogarithmicScaleSlider(target,
                                                        _value, minValue, maxValue,
                                                        v => v.ToStringPercent(), minLabel, maxLabel);
        }
    }
}
