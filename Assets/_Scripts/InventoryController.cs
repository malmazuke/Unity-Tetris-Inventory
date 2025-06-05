using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Canvas _canvas; 
    [SerializeField] private List<InventoryItemSO> _inventoryItemSOs;
    private ItemGrid _selectedItemGrid;
    public event Action<ItemGrid> OnSelectedGridChanged;
    
    private InventoryItem _selectedItem;
    private RectTransform _selectedItemTransform;
    
    

    private void Update()
    {
        UpdateDraggingItem();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }
        
        if (_selectedItemGrid == false)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            HandleItemSelection();
        }
    }

    private void CreateRandomItem()
    {
        var itemSO = _inventoryItemSOs[Random.Range(0, _inventoryItemSOs.Count)];
        var newItem = Instantiate(itemSO.Prefab).GetComponent<InventoryItem>();
        newItem.Init(itemSO, _selectedItemGrid.GridTileSize);
        if (_selectedItem)
        {
            Destroy(_selectedItem);
            _selectedItemTransform = null;
        }
        
        _selectedItem = newItem;
        var rectTransform = newItem.GetComponent<RectTransform>();
        rectTransform.SetParent(_canvas.transform);
        _selectedItemTransform = rectTransform;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void HandleItemSelection()
    {
        var positionOnGrid = _selectedItemGrid.GetTileGridPosition(Input.mousePosition);

        if (_selectedItem == false)
        {
            PickUpItem(positionOnGrid);
        }
        else
        {
            PlaceItem(positionOnGrid);
        }
    }

    private void PlaceItem(Vector2Int positionOnGrid)
    {
        _selectedItemGrid.PlaceItem(_selectedItem, positionOnGrid.x, positionOnGrid.y);
        _selectedItem = null;
    }

    private void PickUpItem(Vector2Int positionOnGrid)
    {
        _selectedItem = _selectedItemGrid.PickUpItem(positionOnGrid.x, positionOnGrid.y);;
        if (_selectedItem)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            _selectedItemTransform = _selectedItem.GetComponent<RectTransform>();
        }
    }

    private void UpdateDraggingItem()
    {
        if (_selectedItem)
        {
            _selectedItemTransform.position = Input.mousePosition;
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
