using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : ItemBase
{

    public override void Use()
    {
        Debug.Log("Item::体力を回復するアイテムを使った");
        base.Use();
    }
}
