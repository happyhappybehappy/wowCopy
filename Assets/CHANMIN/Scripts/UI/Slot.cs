using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public int itemCount;
    public Image itemImage;

    [SerializeField] private Text textCount;
    [SerializeField] private GameObject go_CountImage;

    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }


    public void AddItem(Item item, int count = 1)
    {
        this.item = item;
        itemCount = count;
        itemImage.sprite = this.item.itemImage;

        if (this.item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            textCount.text = itemCount.ToString();
        }

        else
        {
            textCount.text = "0";
            go_CountImage.SetActive(false);
        }


        SetColor(1);
    }

    public void SetSlotCount(int count)
    {
        itemCount += count;
        textCount.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        go_CountImage.SetActive(false);
        textCount.text = "0";
    }

}
