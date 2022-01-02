using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "New Game data")]
public class GameData : ScriptableObject
{
    public GameObject boxPrefab;
    public Vector2Int gridSize;
}
