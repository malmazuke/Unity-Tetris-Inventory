using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class InventoryItem : MonoBehaviour
{
    // ReSharper disable Unity.PerformanceAnalysis
    public void Init(InventoryItemSO itemSO)
    {
        ItemSO = itemSO;
        GetComponent<Image>().sprite = itemSO.Icon;
    }

    public InventoryItemSO ItemSO { get; private set; }
}
