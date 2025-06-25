using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGL.API.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SliderLimitsAttribute : Attribute
    {
        public float Min { get; }
        public float Max { get; }
        public SliderLimitsAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }

}
