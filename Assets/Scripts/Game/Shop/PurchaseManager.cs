using System.Collections.Generic;
using UnityEngine;

namespace Game.Shop
{
    public class PurchaseManager : MonoBehaviour
    {
        public static PurchaseManager Instance { get; private set; }
        private const string PurchasedItemsKey = "PurchasedItems";
        private const string SelectedItemKey = "SelectedItem";
        
        private HashSet<string> purchasedItems = new HashSet<string>();
        private string selectedItemId;

        public event System.Action<string> OnItemPurchased;
        public event System.Action<string> OnItemSelected;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPurchasedItems();
            LoadSelectedItem();
        }

        public bool IsItemPurchased(string itemId)
        {
            return purchasedItems.Contains(itemId);
        }

        public void PurchaseItem(string itemId)
        {
            if (!purchasedItems.Contains(itemId))
            {
                purchasedItems.Add(itemId);
                SavePurchasedItems();
                OnItemPurchased?.Invoke(itemId);
            }
        }

        public string GetSelectedItem()
        {
            return selectedItemId;
        }

        public void SelectItem(string itemId)
        {
            if (IsItemPurchased(itemId) || IsDefaultItem(itemId))
            {
                selectedItemId = itemId;
                SaveSelectedItem();
                OnItemSelected?.Invoke(itemId);
            }
        }

        private bool IsDefaultItem(string itemId)
        {
            // Проверяем является ли предмет стандартным (бесплатным)
            return itemId == "default_skin";
        }

        private void LoadPurchasedItems()
        {
            string purchasedData = PlayerPrefs.GetString(PurchasedItemsKey, "");
            if (!string.IsNullOrEmpty(purchasedData))
            {
                string[] items = purchasedData.Split(',');
                foreach (string item in items)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        purchasedItems.Add(item);
                    }
                }
            }
        }

        private void SavePurchasedItems()
        {
            string purchasedData = string.Join(",", purchasedItems);
            PlayerPrefs.SetString(PurchasedItemsKey, purchasedData);
            PlayerPrefs.Save();
        }

        private void LoadSelectedItem()
        {
            selectedItemId = PlayerPrefs.GetString(SelectedItemKey, "default_skin");
        }

        private void SaveSelectedItem()
        {
            PlayerPrefs.SetString(SelectedItemKey, selectedItemId);
            PlayerPrefs.Save();
        }
    }
}
