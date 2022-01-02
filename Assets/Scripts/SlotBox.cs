using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotBox : MonoBehaviour
{
    public Vector2Int id;
    public Strip slotStrip;

    public string currentlyShownItem = "";

    private void Start()
    {
        Strip.SpinComplete += () => { SetCurrentItem(); };

        // Check if proper strips are assigned to slots
        if(id != slotStrip.id)
        {
            Debug.LogError("Mismatched strip to slot added");
            return;
        }
        else
        {
            // print("Associated strip found!");
        }
    }

    /// <summary>
    /// Set the current item on slot based on Strip's position
    /// </summary>
    public void SetCurrentItem()
    {
        currentlyShownItem = slotStrip.GetStoppedOnItem();
    }
}
