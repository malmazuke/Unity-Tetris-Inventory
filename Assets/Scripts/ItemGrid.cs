using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    private const float TileSizeWidth = 32f;
    private const float TileSizeHeight = 32f;

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
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
