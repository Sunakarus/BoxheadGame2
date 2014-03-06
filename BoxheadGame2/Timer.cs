using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxheadGame2
{
    class Timer
    {
         public int value {get; set;}
         public int maxValue { get; set; }
         bool loop = false;

        public Timer(int value)
        {
            this.value = value;
            this.maxValue = value;
        }

        public Timer(int value, int maxValue)
        {
            this.value = value;
            this.maxValue = maxValue;
        }

        public Timer(int value, int maxValue, bool loop)
        {
            this.value = value;
            this.maxValue = maxValue;
            this.loop = loop;
        }

        public void Update()
        {
            if (value <= 0 && loop)
            {
                Reset();
            }
            if (value>0)
            {
                value--;
            }

        }

        public bool IsActive()
        {
            if (value>0)
            {
                return true;
            }
            return false;
        }

        public void Reset()
        {
            value = maxValue;
        }

        public override string ToString()
        {
            return value + "/" + maxValue;
        }
    }
}
