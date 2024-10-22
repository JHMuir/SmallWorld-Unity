using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant Data", menuName = "PlantSOs/Plants", order = 1)]
public class PlantSO : ScriptableObject
{
    public string commonName;
    public string speciesName;
    public string description;
}
