using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILogic : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private Solver solver;
    [SerializeField] private WFCAlgorithm wFCAlgorithm;

    [SerializeField] private Slider gridDimensionSlider;
    [SerializeField] private TMP_Text gridDimensionText;

    private void Start()
    {
        gridDimensionSlider.value = gridGenerator.gridDimension;
        gridDimensionText.text    = gridGenerator.gridDimension.ToString();
    }

    public void OnGenerateGridPressed()
    {
        gridGenerator.GenerateGrid();
    }

    public void OnSingleSolverIterationPressed()
    {
        solver.Iterate();
    }

    public void OnSolvePressed()
    {
        wFCAlgorithm.Execute();
    }

    public void OnGridDimenionChanged(float value)
    {
        gridGenerator.gridDimension = (int)value;
        gridDimensionText.text = gridGenerator.gridDimension.ToString();
    }
}
