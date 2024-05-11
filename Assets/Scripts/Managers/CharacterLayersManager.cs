using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLayersManager : Singleton<CharacterLayersManager>
{
    [Header("Layers")]
    [SerializeField] private LayerMask characterLayerMask;
    [SerializeField] private LayerMask environmentLayerMask;

    public LayerMask CharacterLayerMask {  get { return characterLayerMask; } set {  characterLayerMask = value; } }
    public LayerMask EnvironmentLayerMask { get { return environmentLayerMask; } set => environmentLayerMask = value; }
}
