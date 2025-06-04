using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemGrid _selectedItemGrid;
    public event Action<ItemGrid> OnSelectedGridChanged;
    
    private InventoryItem _selectedItem;
    private RectTransform _selectedItemTransform;

    private void Update()
    {
        if (_selectedItem)
        {
            _selectedItemTransform.position = Input.mousePosition;
        }

        if (_selectedItemGrid == false)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            var positionOnGrid = _selectedItemGrid.GetTileGridPosition(Input.mousePosition);

            if (_selectedItem == false)
            {
                _selectedItem = _selectedItemGrid.PickUpItem(positionOnGrid.x, positionOnGrid.y);;
                if (_selectedItem)
                {
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    _selectedItemTransform = _selectedItem.GetComponent<RectTransform>();
                }
            }
            else
            {
                _selectedItemGrid.PlaceItem(_selectedItem, positionOnGrid.x, positionOnGrid.y);
                _selectedItem = null;
            }
        }
    }
    
    public ItemGrid SelectedItemGrid
    {
        get => _selectedItemGrid;
        set
        {
            if (_selectedItemGrid == value)
                return;
            
            _selectedItemGrid = value;
            OnSelectedGridChanged?.Invoke(value);
        }
    }
}
