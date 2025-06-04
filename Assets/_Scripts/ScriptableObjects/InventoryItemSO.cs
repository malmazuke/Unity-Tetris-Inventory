using UnityEngine;

[CreateAssetMenu]
public class InventoryItemSO : ScriptableObject
{
    public Vector2Int Size = new Vector2Int(1, 1);
    public Sprite Icon;
    public GameObject Prefab;
}
