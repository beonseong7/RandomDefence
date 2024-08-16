using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecipeButton : MonoBehaviour
{
    public void OnMouseEnter()
    {
        InGameManager.instance.SystemMs(this.gameObject.name);
    }
    public void OnMouseExit()
    {
        InGameManager.instance.ClearSystemMs();
    }
    // Update is called once per frame
}
