using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public interface IOutputCreator <T>
    {
        T OutputImage { get; }
        void CreateOutput(PatternManager manager, int[][] outputValues, int width, int height);
    }
}
