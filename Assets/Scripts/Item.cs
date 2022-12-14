using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item {

    public enum ItemType {
        Sword,
        HealthPotion,
        ManaPotion,
        Coin,
        Medkit,
		flame_2,
    }

    public ItemType itemType;
    public int amount;


    public Sprite GetSprite() {
        switch (itemType) {
        default:
        case ItemType.flame_2:		return ItemAssets.Instance.flame_2Sprite;
        }
    }

}
