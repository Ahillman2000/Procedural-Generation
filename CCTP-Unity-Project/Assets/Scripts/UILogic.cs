using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

public class UILogic : MonoBehaviour
{
    [SerializeField] private Slider gridDimensionSlider;
    [SerializeField] private TMP_Text gridDimensionText;

    [SerializeField] private Slider solverDelaySilder;

    private void Start()
    {
        /*var gridDimensionrange = typeof(GridGenerator).GetField(nameof(GridGenerator.gridDimension)).GetCustomAttribute<RangeAttribute>();
        gridDimensionSlider.minValue = gridDimensionrange.min;
        gridDimensionSlider.maxValue = gridDimensionrange.max;*/

        gridDimensionSlider.value = GridGenerator.Instance.gridDimension;
        gridDimensionText.text    = GridGenerator.Instance.gridDimension.ToString();

        /*var solverDelayrange = typeof(Solver).GetField(nameof(Solver.delay)).GetCustomAttribute<RangeAttribute>();
        gridDimensionSlider.minValue = solverDelayrange.min;
        gridDimensionSlider.maxValue = solverDelayrange.max;*/

        solverDelaySilder.value = Solver.Instance.delay;
    }

    public void OnGenerateGridPressed()
    {
        GridGenerator.Instance.GenerateGrid();
    }

    public void OnSingleSolverIterationPressed()
    {
        Solver.Instance.Iterate();
    }

    public void OnSolvePressed()
    {
        StartCoroutine(Solver.Instance.Solve());
    }

    public void FullWFC()
    {
        WFCAlgorithm.Instance.Execute();
    }

    public void OnGridDimenionChanged(float value)
    {
        GridGenerator.Instance.gridDimension = (int)value;
        gridDimensionText.text = GridGenerator.Instance.gridDimension.ToString();
    }

    public void OnSolverDelayChanged(float value)
    {
        Solver.Instance.delay = value;
    }
}
