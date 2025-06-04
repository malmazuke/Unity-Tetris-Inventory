using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemGrid _selectedItemGrid;
    public event Action<ItemGrid> OnSelectedGridChanged;

    private void Update()
    {
        if (_selectedItemGrid == false)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(_selectedItemGrid.GetTileGridPosition(Input.mousePosition));   
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
