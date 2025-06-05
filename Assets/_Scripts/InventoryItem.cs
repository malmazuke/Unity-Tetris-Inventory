using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class InventoryItem : MonoBehaviour
{
    // ReSharper disable Unity.PerformanceAnalysis
    public void Init(InventoryItemSO itemSO, Vector2 gridTileSize)
    {
        ItemSO = itemSO;
        GetComponent<Image>().sprite = itemSO.Icon;
        GetComponent<RectTransform>().sizeDelta = new Vector2
        {
            x = gridTileSize.x * itemSO.Size.x,
            y = gridTileSize.y * itemSO.Size.y
        };
    }
    
    public Vector2Int Size => ItemSO.Size;

    private InventoryItemSO ItemSO { get; set; }
}
