using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(10, 10);

    private const float TileSizeWidth = 32f;
    private const float TileSizeHeight = 32f;

    private RectTransform _rectTransform;

    private InventoryItem[,] _inventoryItemSlot;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        InitialiseGrid(_gridSize.x, _gridSize.y);
    }

    private void InitialiseGrid(int width, int height)
    {
        _inventoryItemSlot = new InventoryItem[width, height];
        var size = new Vector2(width * TileSizeWidth, height * TileSizeHeight);
        _rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        var positionOnTheGrid = new Vector2
        {
            x = mousePosition.x - _rectTransform.position.x,
            y = _rectTransform.position.y - mousePosition.y
        };

        var tileGridPosition = new Vector2Int
        {
            x = (int)(positionOnTheGrid.x / TileSizeWidth),
            y = (int)(positionOnTheGrid.y / TileSizeHeight)
        };

        return tileGridPosition;
    }
}
