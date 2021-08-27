using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public int allowedBlockID;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            bool isBlockAllowed = false;
            if(eventData.pointerDrag.GetComponent<DragAndDrop>().blockID == allowedBlockID)
            {
                // Add one score if block is set in correct lane.
                Score.AddScore(1);
                isBlockAllowed = true;
            }

            if(isBlockAllowed)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                eventData.pointerDrag.GetComponent<DragAndDrop>().droppedOnSlot = true;

                foreach (Transform child in transform)
                {
                    //child is your child transform
                    if (child.gameObject.tag == "Dino")
                    {
                        Debug.Log("Dino hidastuu");
                        child.gameObject.GetComponent<DinoMoving>().DinoSlowed();
                    }
                }
            }
        }
    }
}

