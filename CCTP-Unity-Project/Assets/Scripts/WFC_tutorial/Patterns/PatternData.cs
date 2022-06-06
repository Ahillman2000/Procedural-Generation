using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class PatternData
    {
        private Pattern pattern;
        private int frequency = 1;
        private float relativeFrequency;
        private float relativeFrequencyLog2;

        public float RelativeFrequency { get => relativeFrequency; }
        public float RelativeFrequencyLog2 { get => relativeFrequencyLog2; }
        public Pattern Pattern { get => pattern; }


        public PatternData(Pattern _pattern)
        {
            pattern = _pattern;
        }

        public void AddToFrequency()
        {
            frequency++;
        }

        public void CalculateRelativeFrequency(int _total)
        {
            relativeFrequency = (float)frequency / _total;
            relativeFrequencyLog2 = Mathf.Log(relativeFrequency, 2);
        }

        public bool CompareGrid(Direction _dir, PatternData _data)
        {
            return pattern.ComparePattern(_dir, _data.Pattern);
        }
    }
}
