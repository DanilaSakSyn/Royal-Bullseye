using UnityEngine;
using Game.Wallet;

namespace Game.Shop
{
    public class ShopManager : MonoBehaviour
    {
        [Header("Shop Items")]
        [SerializeField] private ShopItem[] shopItems;
        
        [Header("UI References")]
        [SerializeField] private Transform shopItemsParent;
        [SerializeField] private GameObject shopItemUIPrefab;
        
        public static ShopManager Instance { get; private set; }
        
        public event System.Action OnShopUpdated;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            InitializeShop();
            if (PurchaseManager.Instance != null)
            {
                PurchaseManager.Instance.OnItemPurchased += OnItemPurchased;
            }
        }

        private void OnDestroy()
        {
            if (PurchaseManager.Instance != null)
            {
                PurchaseManager.Instance.OnItemPurchased -= OnItemPurchased;
            }
        }

        public bool TryPurchaseItem(ShopItem item)
        {
            if (item == null)
            {
                Debug.LogError("Попытка купить null предмет");
                return false;
            }

            if (PurchaseManager.Instance.IsItemPurchased(item.ItemId))
            {
                Debug.Log($"Предмет {item.ItemName} уже куплен");
                return false;
            }

            if (Wallet.Wallet.Instance.Coins < item.Cost)
            {
                Debug.Log($"Недостаточно монет для покупки {item.ItemName}. Нужно: {item.Cost}, есть: {Wallet.Wallet.Instance.Coins}");
                return false;
            }

            // Списываем деньги
            Wallet.Wallet.Instance.AddCoins(-item.Cost);
            
            // Добавляем предмет в купленные
            PurchaseManager.Instance.PurchaseItem(item.ItemId);
            
            Debug.Log($"Успешно куплен предмет: {item.ItemName} за {item.Cost} монет");
            return true;
        }

        public bool CanPurchaseItem(ShopItem item)
        {
            if (item == null) return false;
            if (PurchaseManager.Instance.IsItemPurchased(item.ItemId)) return false;
            return Wallet.Wallet.Instance.Coins >= item.Cost;
        }

        public ShopItem[] GetShopItems()
        {
            return shopItems;
        }

        public ShopItem GetItemById(string itemId)
        {
            foreach (var item in shopItems)
            {
                if (item.ItemId == itemId)
                    return item;
            }
            return null;
        }

        private void InitializeShop()
        {
            CreateShopItems();
            OnShopUpdated?.Invoke();
        }

        private void CreateShopItems()
        {
            if (shopItemsParent == null || shopItemUIPrefab == null)
            {
                Debug.LogError("ShopItemsParent или ShopItemUIPrefab не установлены в ShopManager");
                return;
            }

            if (shopItems == null || shopItems.Length == 0)
            {
                Debug.LogWarning("Нет товаров для отображения в магазине");
                return;
            }

            // Удаляем существующие UI элементы
            foreach (Transform child in shopItemsParent)
            {
                Destroy(child.gameObject);
            }

            // Создаем UI элементы для каждого товара
            foreach (var shopItem in shopItems)
            {
                if (shopItem == null) continue;

                GameObject itemUI = Instantiate(shopItemUIPrefab, shopItemsParent);
                ShopItemUI shopItemUI = itemUI.GetComponent<ShopItemUI>();
                
                if (shopItemUI != null)
                {
                    shopItemUI.SetupItem(shopItem);
                }
                else
                {
                    Debug.LogError($"На префабе {shopItemUIPrefab.name} нет компонента ShopItemUI");
                }
            }
        }

        private void OnItemPurchased(string itemId)
        {
            OnShopUpdated?.Invoke();
            Debug.Log($"Предмет {itemId} был куплен");
        }

        public void SelectItem(ShopItem item)
        {
            if (item == null) return;
            
            if (PurchaseManager.Instance.IsItemPurchased(item.ItemId) || item.IsDefault)
            {
                PurchaseManager.Instance.SelectItem(item.ItemId);
                Debug.Log($"Выбран предмет: {item.ItemName}");
            }
            else
            {
                Debug.Log($"Предмет {item.ItemName} не куплен");
            }
        }

        public void RefreshShop()
        {
            CreateShopItems();
        }
    }
}
