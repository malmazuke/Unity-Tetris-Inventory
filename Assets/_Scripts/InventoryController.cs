using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemGrid selectedItemGrid;

    private void Update()
    {
        if (selectedItemGrid == false)
            return;
        
        Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
    }
}
