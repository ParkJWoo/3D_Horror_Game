using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<ItemData> allItems;
    public Dictionary<int, ItemData> itemDataBase;

    public List<DropItem> filedItem = new List<DropItem>();

    public void Init()
    {
        if (itemDataBase == null)
        {
            itemDataBase = new Dictionary<int, ItemData>();
            foreach (var item in allItems)
            {
                itemDataBase[item.itemCode] = item;
            }
        }
    }

    public void DropItem(ItemInstance item, Vector3 position)
    {
        GameObject drop = Instantiate(item.itemData.dropItemPrefab, position, Quaternion.identity);
        drop.GetComponent<DropItem>().Init(item);
    }
}
