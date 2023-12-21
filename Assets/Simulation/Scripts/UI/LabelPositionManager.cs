using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public struct UiItems
{
    public GameObject Item;
    public BoolReference IsActive;
}
public class LabelPositionManager : MonoBehaviour
{
    [SerializeField] private HorizontalDrawer drawer;
    [SerializeField] private List<UiItems> uiItems;
    [SerializeField] private float spacingTop;
    [SerializeField] private float startY;
    private int nbOfItemsActive;

    public void Init()
    {
        float showingY = 0;
        nbOfItemsActive = 0;
        for (int i = 0; i < uiItems.Count; i++)
        {
            UiItems UIitem = uiItems[i];
            if (UIitem.IsActive.Value) {
                UIitem.Item.SetActive(true);

                float newY = startY + spacingTop*nbOfItemsActive;
                UIitem.Item.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, newY, 0);
                nbOfItemsActive++;
            }
            else
            {
                UIitem.Item.SetActive(false);
            }
        }

        if (nbOfItemsActive==0)
        {
            showingY = 50;
        }
        else
        {
            showingY += -(startY+spacingTop*nbOfItemsActive);
        }

        drawer.SetShowingY(showingY);
    }
}
