using UnityEngine;

namespace Game.Shop
{
    [CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop/Shop Item")]
    public class ShopItem : ScriptableObject
    {
        [SerializeField] private string itemId;
        [SerializeField] private string itemName;
        [SerializeField] private int cost;
        [SerializeField] private Sprite itemSprite;
        [SerializeField] private bool isDefault = false;

        public string ItemId => itemId;
        public string ItemName => itemName;
        public int Cost => cost;
        public Sprite ItemSprite => itemSprite;
        public bool IsDefault => isDefault;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(itemId))
            {
                itemId = name.Replace(" ", "_").ToLower();
            }
        }
    }
}
