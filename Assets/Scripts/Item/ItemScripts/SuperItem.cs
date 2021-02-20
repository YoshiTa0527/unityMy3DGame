using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperItem : ItemBase
{
    public override void Use()
    {
        Debug.Log("item::スーパーアイテムを使った");
        base.Use();
    }
}
