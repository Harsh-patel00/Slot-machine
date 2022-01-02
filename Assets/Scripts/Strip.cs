using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class Strip : MonoBehaviour
{
    // Initial vars
    public Vector2Int id;
    public float startingYPos   = 1.75f;
    public float endingYPos     = -1.75f;

    // Collections
    public Dictionary<float, string> stripItemsDict;
    public Dictionary<string, float> invStripItemsDict;

    // Gameplay vars
    public string stoppedOnItem = ""; // Stores on which item the spinning stopped
    public bool isRotating = false;
    public float timeInterval; // this determines the spinning speed

    // Actions
    public static Action SpinComplete;
    public static Action CheckResults;

    private void Start()
    {
        GameManager.SpinClickedIndex += StartSpinning;
        GameManager.SpinClickedNames += StartSpinning;

        stripItemsDict = new Dictionary<float, string>();
        invStripItemsDict = new Dictionary<string, float>();
        isRotating = false;

        InitStrip(stripItemsDict, invStripItemsDict);
    }

    private void InitStrip(Dictionary<float, string> stripItemsDict, 
                           Dictionary<string, float> invStripItemsDict)
    {
        // We already know that in the strip image, 1st image is at 1.75 on Y-axis
        // We also know that each item is at an interval of 0.5 on Y-axis
        // So, by doing ' startingYPos - (0.5f * stripItemsDict.Count) ',
        //  we are saying that,
        //  1. "Get the starting position (in our case it is 1.75f),
        //  2. then based on the number of item in the list (stripItemsDict.Count), subtract 0.5,
        //  because each item is at 0.5f interval on Y-axis"

        stripItemsDict.Add(startingYPos, "Diamond");
        stripItemsDict.Add(startingYPos - (0.5f * stripItemsDict.Count), "Lemon");
        stripItemsDict.Add(startingYPos - (0.5f * stripItemsDict.Count), "Cherry");
        stripItemsDict.Add(startingYPos - (0.5f * stripItemsDict.Count), "Seven");
        stripItemsDict.Add(startingYPos - (0.5f * stripItemsDict.Count), "Bar");
        stripItemsDict.Add(startingYPos - (0.5f * stripItemsDict.Count), "Watermelon");
        stripItemsDict.Add(startingYPos - (0.5f * stripItemsDict.Count), "Crown");


        // Store inverted dictionary to easily find the keys based on value!
        // We can do this because we have 1-to-1 relationship b/w keys and values

        invStripItemsDict.Add("Diamond",   startingYPos);
        invStripItemsDict.Add("Lemon",     startingYPos - (0.5f * invStripItemsDict.Count));
        invStripItemsDict.Add("Cherry",    startingYPos - (0.5f * invStripItemsDict.Count));
        invStripItemsDict.Add("Seven",     startingYPos - (0.5f * invStripItemsDict.Count));
        invStripItemsDict.Add("Bar",       startingYPos - (0.5f * invStripItemsDict.Count));
        invStripItemsDict.Add("Watermelon",startingYPos - (0.5f * invStripItemsDict.Count));
        invStripItemsDict.Add("Crown",     startingYPos - (0.5f * invStripItemsDict.Count));
    }

    /// <summary>
    /// Get the local y-position of the strip
    /// </summary>
    /// <returns> Strip's local Y-postion </returns>
    public float GetYPosition()
    {
        return transform.localPosition.y;
    }

    /// <summary>
    /// Set the Y-pos of the strip based on the item name
    /// </summary>
    /// <param name="itemName"> Name of the item on strip </param>
    public void SetYPosition(string itemName)
    {
        float yPos = 0.0f;
        if(invStripItemsDict.TryGetValue(itemName, out yPos))
        {
            transform.localPosition = new Vector3(transform.localPosition.x, yPos);
        }
        else
        {
            Debug.LogError("Please enter correct item name.\n" +
                "These are valid names : \n" +
                "1. Diamond\n" +
                "2. Crown\n" +
                "3. Watermelon\n" +
                "4. Bar\n" +
                "5. Seven\n" +
                "6. Cherry\n" +
                "7. Lemon\n");
        }
    }

    /// <summary>
    /// Set the Y-pos of the strip based on the float value
    /// </summary>
    /// <param name="newPos"> Y-pos of the strip </param>
    public void SetYPosition(float newPos)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, newPos);
    }

    /// <summary>
    /// Get the item name on strip based on it's position
    /// </summary>
    /// <param name="itemPosOnstrip"> Position of item on strip </param>
    /// <returns> Item name on strip </returns>
    public string GetItemName(float itemPosOnstrip)
    {
        return stripItemsDict[itemPosOnstrip];
    }

    public void StartSpinning(int[,] landingPoints)
    {
        //print("start spinning... : " + id +
        //    ", Landing pts : " + landingPoints[id.x, id.y]);

        if (!isRotating)
            StartCoroutine(Spin(landingPoints[id.x, id.y]));
        else
            print("Wait till all spins are completed.");
    }
    
    public void StartSpinning(string[,] landingNames)
    {
        //print("start spinning... : " + id +
        //    ", Landing pts : " + landingNames[id.x, id.y]);

        if (!isRotating)
            StartCoroutine(Spin(landingNames[id.x, id.y]));
        else
            print("Wait till all spins are completed.");
    }

    /// <summary>
    /// Core loop to spin the strips
    /// </summary>
    /// <param name="landingIndex"> A random index to choose from dictionary </param>
    /// <returns> WaitForSeconds </returns>
    IEnumerator Spin(int landingIndex)
    {
        isRotating = true;

        // Get position on strip from dict. using landing index
        float landPos = stripItemsDict.Keys.ElementAt(landingIndex);
   
        // Spin 30 times 
        for(int i = 0; i < 30; i++)
        {
            // Reset postion of strip, to give it a loop
            if (GetYPosition() <= -1.75f)
                ResetStripPosition();

            // Spin the strip
            transform.localPosition = new Vector2(transform.localPosition.x,
                                                  GetYPosition() - 0.5f);

            // Wait for 'timeInterval' seconds before shifting the strip down
            yield return new WaitForSeconds(timeInterval);
        }

        SetYPosition(landPos);

        stoppedOnItem = stripItemsDict[landPos];
        isRotating = false;

        SpinComplete?.Invoke();
        CheckResults?.Invoke();
    }

    /// <summary>
    /// Core loop to spin the strips
    /// </summary>
    /// <param name="landingName"> Item name to display </param>
    /// <returns> WaitForSeconds </returns>
    IEnumerator Spin(string landingName)
    {
        isRotating = true;

        // Get position on strip from dict. using landing index
        float landPos = invStripItemsDict[landingName];
   
        // Spin 30 times 
        for(int i = 0; i < 30; i++)
        {
            // Reset postion of strip, to give it a loop
            if (GetYPosition() <= -1.75f)
                ResetStripPosition();

            // Spin the strip
            transform.localPosition = new Vector2(transform.localPosition.x,
                                                  GetYPosition() - 0.5f);

            // Wait for 'timeInterval' seconds before shifting the strip down
            yield return new WaitForSeconds(timeInterval);
        }

        SetYPosition(landPos);

        stoppedOnItem = landingName;
        isRotating = false;

        SpinComplete?.Invoke();
        CheckResults?.Invoke();
    }

    /// <summary>
    /// Resets the strip position to start
    /// </summary>
    private void ResetStripPosition()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, startingYPos);
    }

    public string GetStoppedOnItem()
    {
        return stoppedOnItem;
    }
}
