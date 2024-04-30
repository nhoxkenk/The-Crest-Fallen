using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterSaveData
{
    [Header("Name")]
    public string characterName = "Character Name";

    [Header("Time Played")]
    public float secondPlayed;

    [Header("World Coordinates")]
    public float characterXPosition;
    public float characterYPosition;
    public float characterZPosition;
}
