using Ali.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : LocalSingleton<GameManager>
{
    [SerializeField] private TextMeshProUGUI _matchCountText;
    [SerializeField] private TMP_InputField  _sizeInputField;
    [SerializeField] private Button _rebuildButton;
    private int _matchCount = 0;
    private void Start()
    {
        GridManager.Instance.Init();
        _rebuildButton.onClick.AddListener(OnRebuildButtonClicked);
        ResetMatchCount();
        _sizeInputField.text = GridManager.Instance.GetGridSize().ToString();
    }

    private void OnRebuildButtonClicked()
    {
        if (!int.TryParse(_sizeInputField.text, out var newSize))
        {
            return;
        }

        ResetMatchCount();
        GridManager.Instance.SetGridSize(newSize);
        GridManager.Instance.GenerateGrid();
        GridManager.Instance.AdjustCamera();
    }

    public void IncreaseMatchCount(int increaseAmount)
    {
        _matchCount += increaseAmount;
        UpdateMatchCountText();
    }

    private void UpdateMatchCountText()
    {
        _matchCountText.text = $"Match Count: {_matchCount}";
    }

    private void ResetMatchCount()
    {
        _matchCount = 0;
        UpdateMatchCountText();
    }
}