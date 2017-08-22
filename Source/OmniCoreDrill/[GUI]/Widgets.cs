using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DoctorVanGogh.OmniCoreDrill {
    public class ExtraWidgets {

        public static float LogarithmicScaleSlider(Rect rect, float value, float minValue, float maxValue, Func<float, string> valueFormatter, string leftAlignedLabel = null, string rightAlignedLabel = null) {
            return (float)Math.Exp(Verse.Widgets.HorizontalSlider(rect, (float)Math.Log(value), minValue, maxValue, true, valueFormatter(value), leftAlignedLabel, rightAlignedLabel, -1f));
        }
    }
}
