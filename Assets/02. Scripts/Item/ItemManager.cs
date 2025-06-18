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

        if (!GameManager.Instance.isNewGame)
        {
            foreach (var item in filedItem)
            {
                Destroy(item.gameObject);
            }

            filedItem.Clear();

            for (int i = 0; i < SaveManager.Instance.saveData.filedItemData.Count; i++)
            {

            }
        }
    }

    public ItemData FindSOData(int ItemNum)
    {
        if(itemDataBase.TryGetValue(ItemNum, out ItemData value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public void DropItem(ItemInstance item, Vector3 position)
    {
        GameObject drop = Instantiate(item.itemData.dropItemPrefab, position, Quaternion.identity, transform);
        DropItem dropItem = drop.GetComponent<DropItem>();
        dropItem.Init(item);
        dropItem.OnDestoryItem += RemoveDropItem;
        filedItem.Add(dropItem);
    }

    public void RemoveDropItem(DropItem removeItem)
    {
        filedItem.Remove(removeItem);
    }

    public void Save()
    {
        List<SaveFieldItemData> currentItemData = new List<SaveFieldItemData>();

        for (int i = 0; i < filedItem.Count; i++)
        {
            currentItemData.Add(new SaveFieldItemData(filedItem[i]));
        }

        SaveManager.Instance.saveData.filedItemData = currentItemData;
    }
}
