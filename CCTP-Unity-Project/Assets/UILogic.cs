using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILogic : MonoBehaviour
{
    [SerializeField] private Slider gridDimensionSlider;
    [SerializeField] private TMP_Text gridDimensionText;

    private void Start()
    {
        gridDimensionSlider.value = GridGenerator.Instance.gridDimension;
        gridDimensionText.text = GridGenerator.Instance.gridDimension.ToString();
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
        WFCAlgorithm.Instance.Execute();
    }

    public void OnGridDimenionChanged(float value)
    {
        GridGenerator.Instance.gridDimension = (int)value;
        gridDimensionText.text = GridGenerator.Instance.gridDimension.ToString();
    }
}
