using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Shop
{
    public class ShopItemUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemCostText;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private Button selectButton;
        [SerializeField] private GameObject purchasedIndicator;
        [SerializeField] private GameObject selectedIndicator;

        private ShopItem shopItem;

        private void Start()
        {
            if (purchaseButton != null)
                purchaseButton.onClick.AddListener(OnPurchaseClicked);
            
            if (selectButton != null)
                selectButton.onClick.AddListener(OnSelectClicked);

            // Подписываемся на события
            if (PurchaseManager.Instance != null)
            {
                PurchaseManager.Instance.OnItemPurchased += OnItemPurchased;
                PurchaseManager.Instance.OnItemSelected += OnItemSelected;
            }
        }

        private void OnDestroy()
        {
            if (PurchaseManager.Instance != null)
            {
                PurchaseManager.Instance.OnItemPurchased -= OnItemPurchased;
                PurchaseManager.Instance.OnItemSelected -= OnItemSelected;
            }
        }

        public void SetupItem(ShopItem item)
        {
            shopItem = item;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (shopItem == null) return;

            // Устанавливаем основную информацию
            if (itemIcon != null && shopItem.ItemSprite != null)
                itemIcon.sprite = shopItem.ItemSprite;

            if (itemNameText != null)
                itemNameText.text = shopItem.ItemName;

            if (itemCostText != null)
                itemCostText.text = shopItem.Cost.ToString();

            // Проверяем состояние предмета
            bool isPurchased = PurchaseManager.Instance.IsItemPurchased(shopItem.ItemId) || shopItem.IsDefault;
            bool isSelected = PurchaseManager.Instance.GetSelectedItem() == shopItem.ItemId;
            bool canPurchase = ShopManager.Instance.CanPurchaseItem(shopItem);

            // Обновляем UI элементы
            if (purchaseButton != null)
            {
                purchaseButton.gameObject.SetActive(!isPurchased);
                purchaseButton.interactable = canPurchase;
            }

            if (selectButton != null)
            {
                selectButton.gameObject.SetActive(isPurchased);
                selectButton.interactable = !isSelected;
            }

            if (purchasedIndicator != null)
                purchasedIndicator.SetActive(isPurchased);

            if (selectedIndicator != null)
                selectedIndicator.SetActive(isSelected);

            // Если это бесплатный предмет, скрываем цену
            if (shopItem.IsDefault && itemCostText != null)
                itemCostText.text = "БЕСПЛАТНО";
        }

        private void OnPurchaseClicked()
        {
            if (ShopManager.Instance.TryPurchaseItem(shopItem))
            {
                UpdateUI();
            }
        }

        private void OnSelectClicked()
        {
            ShopManager.Instance.SelectItem(shopItem);
        }

        private void OnItemPurchased(string itemId)
        {
            if (shopItem != null && shopItem.ItemId == itemId)
            {
                UpdateUI();
            }
        }

        private void OnItemSelected(string itemId)
        {
            if (shopItem != null)
            {
                UpdateUI();
            }
        }
    }
}
