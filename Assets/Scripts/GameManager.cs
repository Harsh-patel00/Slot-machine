using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // Reference of all slots in the game
    public List<SlotBox> Slots;

    // Stores random index for slot boxes
    private int[,] _randMat;

    // Vars related to displaying selected names
    public bool displayDefinedItems;
    public string[,] _namesMat;

    private bool _resultsChecked = false;

    // Action to subscribe, accepts a parameter of 2D int array
    public static Action<int[,]> SpinClickedIndex;
    public static Action<string[,]> SpinClickedNames;

    // Gameobjects
    public Button spinBtn;

    private void Start()
    {
        // Event Subscriptions
        Strip.CheckResults += CheckResults;

        // Initialization
        _randMat = new int[3, 5];
        _namesMat = new string[3, 5];
    }

    /// <summary>
    /// Generate random index matrix
    /// </summary>
    public void GenerateRandomIndexMat()
    {
        // Foreach slot boxes, store a random index in the matrix.
        // The item on this index will be the item that will be landing on that slot.
        
        foreach (var slot in Slots)
        { 
            _randMat[slot.id.x, slot.id.y] = Random.Range(0, slot.slotStrip.stripItemsDict.Count);
        }
    }

    /// <summary>
    /// Associated with Spin button
    /// </summary>
    public void Spin()
    {
        // Notify all strips that spin is clicked.
        // Pass a random matrix with index to each strip
        // print("Spin clicked!");

        // Disable Spin button once clicked
        spinBtn.interactable = false;

        GenerateRandomIndexMat();

        _resultsChecked = false;

        if(displayDefinedItems)
        {
            // Set the names of items to display here
            SetNames();

            SpinClickedNames.Invoke(_namesMat);
        }
        else
        {
            SpinClickedIndex.Invoke(_randMat);
        }
    }

    public void CheckResults()
    {
        if (_resultsChecked) return;

        // If 1st row have same values
        if (!displayDefinedItems && ((_randMat[0, 0] == _randMat[0, 1]) &&
           (_randMat[0, 0] == _randMat[0, 2]) &&
           (_randMat[0, 0] == _randMat[0, 3]) &&
           (_randMat[0, 0] == _randMat[0, 4])))
        {
            print("Win!");
            _resultsChecked = true;
        }
        
        if(displayDefinedItems &&
            ((_namesMat[0, 0] == _namesMat[0, 1]) &&
            (_namesMat[0, 0] == _namesMat[0, 2]) &&
            (_namesMat[0, 0] == _namesMat[0, 3]) &&
            (_namesMat[0, 0] == _namesMat[0, 4])))
        {
            print("Win!");
            _resultsChecked = true;
        }

        spinBtn.interactable = true;
    }

    public void SetNames()
    {
        // Set the names of items to display here
        // -------------- Row 1 ---------------
        _namesMat.SetValue("Lemon", 0, 0); // Col1
        _namesMat.SetValue("Lemon", 0, 1); // Col2
        _namesMat.SetValue("Lemon", 0, 2); // Col3
        _namesMat.SetValue("Lemon", 0, 3); // Col4
        _namesMat.SetValue("Lemon", 0, 4); // Col5
        // -------------- Row 2 ---------------
        _namesMat.SetValue("Crown", 1, 0); // Col1
        _namesMat.SetValue("Crown", 1, 1); // Col2
        _namesMat.SetValue("Crown", 1, 2); // Col3
        _namesMat.SetValue("Crown", 1, 3); // Col4
        _namesMat.SetValue("Crown", 1, 4); // Col5
        // -------------- Row 3 ---------------
        _namesMat.SetValue("Seven", 2, 0); // Col1
        _namesMat.SetValue("Seven", 2, 1); // Col2
        _namesMat.SetValue("Seven", 2, 2); // Col3
        _namesMat.SetValue("Seven", 2, 3); // Col4
        _namesMat.SetValue("Seven", 2, 4); // Col5
    }
}
