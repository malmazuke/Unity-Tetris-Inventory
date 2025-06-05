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

    // ReSharper disable Unity.PerformanceAnalysis
    private void CreateRandomItem()
    {
        var itemSO = _inventoryItemSOs[Random.Range(0, _inventoryItemSOs.Count)];
        var newItem = Instantiate(itemSO.Prefab).GetComponent<InventoryItem>();
        
        newItem.Init(itemSO, ItemGrid.GridTileSize);
        if (_selectedItem)
        {
            DestroySelectedItem();
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
        if (_selectedItemGrid.PlaceItem(_selectedItem, positionOnGrid.x, positionOnGrid.y))
        {
            _selectedItem = null;
            _selectedItemTransform = null;
        }
    }

    private void PickUpItem(Vector2Int positionOnGrid)
    {
        var pickedUpItem = _selectedItemGrid.PickUpItem(positionOnGrid.x, positionOnGrid.y);
        if (!pickedUpItem) return;
        
        _selectedItem = pickedUpItem;
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        _selectedItemTransform = _selectedItem.GetComponent<RectTransform>();
        _selectedItemTransform.SetParent(_canvas.transform);
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

    private void DestroySelectedItem()
    {
        Destroy(_selectedItem.gameObject);
        _selectedItem = null;
        _selectedItemTransform = null;
    }
    
}
