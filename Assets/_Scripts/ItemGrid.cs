using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    #region Types
    
    public struct ItemSlot
    {
        public InventoryItem Item;
        public bool IsOrigin;
        
        // ---------- Convenience ----------

        /// <summary>An empty slot constant – useful when initialising arrays.</summary>
        public static readonly ItemSlot Empty = new ItemSlot { Item = null, IsOrigin = false };

        /// <summary>Returns true if no item occupies this slot.</summary>
        public bool IsEmpty => Item == null;

        /// <summary>Clears the slot in one call.</summary>
        public void Clear()
        {
            Item     = null;
            IsOrigin = false;
        }
    }
    
    #endregion
    
    #region Serialized Fields
    
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(10, 10);
    [SerializeField] private GameObject _itemPrefab;
    
    #endregion
    
    #region Public Fields
    
    public static Vector2 GridTileSize => new(TileSizeWidth, TileSizeHeight);
    
    #endregion

    #region Private Fields
    
    private const float TileSizeWidth = 32f;
    private const float TileSizeHeight = 32f;

    private RectTransform _rectTransform;
    
    private ItemSlot[] _itemSlots;

    // private InventoryItem[,] _inventoryItemSlot;
    
    #endregion
    
    #region Convenience Helpers

    private int Index(int x, int y) => y * _gridSize.x + x;
    
    private bool InBounds(int x, int y) =>
        (uint)x < (uint)_gridSize.x && (uint)y < (uint)_gridSize.y;
    
    #endregion
    
    #region Unity Methods

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        InitialiseGrid(_gridSize.x, _gridSize.y);
    }
    
    #endregion

    #region Public Methods

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

    /// <summary>
    /// Returns true if the item can be placed at the given grid coordinates
    /// without overlapping existing items and while staying inside bounds.
    /// </summary>
    public bool CanPlaceItem(InventoryItem item, int x, int y)
    {
        // Bounds check first.
        if (!InBounds(x, y) ||
            !InBounds(x + item.Size.x - 1, y + item.Size.y - 1))
            return false;

        // Overlap check.
        for (int dy = 0; dy < item.Size.y; dy++)
        {
            for (int dx = 0; dx < item.Size.x; dx++)
            {
                if (_itemSlots[Index(x + dx, y + dy)].IsEmpty == false)
                {
                    return false;
                } 
                // if (!this[x + dx, y + dy].IsEmpty)
                //     return false;
            }
        }

        return true;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public bool PlaceItem(InventoryItem item, int x, int y)
    {
        if (!CanPlaceItem(item, x, y))
        {
            // Placement failed because the space is occupied or out of bounds.
            return false;
        }

        var rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(_rectTransform);

        for (var dy = 0; dy < item.Size.y; dy++)
        {
            for (var dx = 0; dx < item.Size.x; dx++)
            {
                // ref var slot = ref this[x + dx, y + dy];
                var slot = _itemSlots[Index(x + dx, y + dy)];
                slot.Item     = item;
                slot.IsOrigin = (dx == 0 && dy == 0); // true only for the top-left tile
                _itemSlots[Index(x + dx, y + dy)] = slot;
            }
        }
        
        var position = new Vector2
        {
            x = x * TileSizeWidth + TileSizeWidth * item.Size.x / 2f,
            y = -(y * TileSizeHeight + TileSizeHeight * item.Size.y / 2f)
        };
        rectTransform.localPosition = position;
        return true;
    }

    /// <summary>
    /// Picks up (removes) the item occupying the grid coordinate (x, y) and
    /// returns it. If the square is empty or out of bounds, returns null.
    /// </summary>
    public InventoryItem PickUpItem(int x, int y)
    {
        // ----- Validate coordinates -----
        if (!InBounds(x, y))
        {
            return null;
        }

        var slot = _itemSlots[Index(x, y)];
        if (slot.IsEmpty)
        {
            return null; // nothing here
        }

        var item = slot.Item;

        // ----- Clear all slots occupied by this item -----
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (_itemSlots[i].Item == item)
                _itemSlots[i] = ItemSlot.Empty;
        }

        // ----- Detach the item's rect transform from the grid -----
        item.transform.SetParent(null);
        
        return item;
    }
    
    #endregion
    
    #region Private Methods
    
    private void InitialiseGrid(int width, int height)
    {
        _itemSlots = new ItemSlot[width * height];
        var size = new Vector2(width * TileSizeWidth, height * TileSizeHeight);
        _rectTransform.sizeDelta = size;
    }
    
    #endregion
}
