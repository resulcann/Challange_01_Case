using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private bool _isX = false;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _xSprite;
    public bool IsX { get => _isX; set => _isX = value; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public void Init(int xPos, int yPos)
    {
        X = xPos;
        Y = yPos;
        _isX = false;
        _spriteRenderer.sprite = null;
    }

    private void OnMouseDown()
    {
        if (_isX) return;
        
        _isX = true;
        _spriteRenderer.sprite = _xSprite;
        GridManager.Instance.CheckAndDestroyMatches();
    }
}