using Ali.Helper;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : LocalSingleton<GridManager>
{
    [SerializeField] private int _gridSize = 4;
    [SerializeField] private float _gap = 0.1f;
    private Cell[,] _gridCells;
    private float _cellScale = 1f;

    public void Init()
    {
        GenerateGrid();
        AdjustCamera();
    }
    public void GenerateGrid()
    {
        // Mevcut grid varsa temizle
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        _gridCells = new Cell[_gridSize, _gridSize];

        // Ekran genişliğine göre cell boyutu ve _gap hesapla
        var screenWidth = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        // Cell boyutu hesaplaması gap dahil edilerek yeniden yapılıyor
        _cellScale = (screenWidth - (_gridSize - 1) * _gap) / _gridSize;

        // Ekranın genişliğini hesapla ve gridi ona göre ekranın köşelerine otracak şekilde pozisyonlandır.
        var startX = -screenWidth / 2 + _cellScale / 2;
        var startY = Camera.main.orthographicSize * 2 - _cellScale / 2;

        // Gridi oluştur.
        for (var x = 0; x < _gridSize; x++)
        {
            for (var y = 0; y < _gridSize; y++)
            {
                var cell = PoolManager.Instance.SpawnCell().GetComponent<Cell>();
                if (cell)
                {
                    cell.transform.SetParent(transform);
                    float xPos = startX + x * (_cellScale + _gap);
                    float yPos = startY - y * (_cellScale + _gap);
                    cell.transform.position = new Vector3(xPos, yPos, 0);
                    cell.transform.rotation = Quaternion.identity;
                    cell.transform.localScale = Vector3.one * _cellScale;
                }

                cell.Init(x, y);
                _gridCells[x, y] = cell;
            }
        }
    }

    public void AdjustCamera()
    {
        var cam = Camera.main!;
        cam.orthographic = true;
        // Kameranın yatay olarak ekranı tam dolduracak şekilde ayarlanması
        var screenWidth = Screen.width / (float)Screen.height;
        var requiredWidth = _gridSize * _cellScale + (_gridSize - 1) * _gap;
        cam.orthographicSize = requiredWidth / (2 * screenWidth);
        cam.transform.position = new Vector3(0, Camera.main.orthographicSize, -1);
    }

    public void CheckAndDestroyMatches()
    {
        var cellsToDestroy = new List<Cell>();

        for (var x = 0; x < _gridSize; x++)
        {
            for (var y = 0; y < _gridSize; y++)
            {
                if (_gridCells[x, y] != null && _gridCells[x, y].IsX)
                {
                    var matchCells = CheckForMatch(x, y);
                    if (matchCells.Count >= 3)
                    {
                        cellsToDestroy.AddRange(matchCells);
                    }
                }
            }
        }
        cellsToDestroy = new List<Cell>(new HashSet<Cell>(cellsToDestroy));

        if (cellsToDestroy.Count > 0)
        {
            GameManager.Instance.IncreaseMatchCount(1);
            
            foreach (var cell in cellsToDestroy)
            {
                //PoolManager.Instance.DeSpawnCell(cell.gameObject);
                cell.ResetCell();
                //_gridCells[cell.X, cell.Y] = null;
            }
        }
        
    }

    private List<Cell> CheckForMatch(int x, int y)
    {
        var matchCells = new List<Cell> { _gridCells[x, y] };

        // Sağ ve sol kontrol
        if (x > 0 && _gridCells[x - 1, y] != null && _gridCells[x - 1, y].IsX) matchCells.Add(_gridCells[x - 1, y]);
        if (x < _gridSize - 1 && _gridCells[x + 1, y] != null && _gridCells[x + 1, y].IsX) matchCells.Add(_gridCells[x + 1, y]);

        // Üst ve alt kontrol
        if (y > 0 && _gridCells[x, y - 1] != null && _gridCells[x, y - 1].IsX) matchCells.Add(_gridCells[x, y - 1]);
        if (y < _gridSize - 1 && _gridCells[x, y + 1] != null && _gridCells[x, y + 1].IsX) matchCells.Add(_gridCells[x, y + 1]);

        return matchCells;
    }
    public void SetGridSize(int newSize) => _gridSize = newSize;
    public int GetGridSize() => _gridSize;
}
