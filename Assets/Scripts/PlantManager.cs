using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance { get; private set;}
    public GameObject plantPrefab;

    private List<PlantData> plants = new List<PlantData>();
    private int plantIndex = 0;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        FirebaseManager.Instance.onDataLoaded.AddListener(LoadPlantData);
        FirebaseManager.Instance.RetrievePlantDataFromStorage();
    }

    private void LoadPlantData(List<PlantData> plantDatabase)
    {
        plants = plantDatabase;
        Debug.Log("FROM LOCAL: " + OutputPlantNames(plants));
    }

    public bool IsPlantDataEmpty()
    {
        if(plants.Count == 0 || plants.Count == plantIndex)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public PlantData PassPlantData()
    {
        if(!IsPlantDataEmpty())
        {
            PlantData plant = plants[plantIndex];
            plantIndex++;
            return plant;
        }
        else
        {
            Debug.Log("Plants are empty.");
            return null;
        }
    }

    public string OutputPlantNames(List<PlantData> plantList)
    {
        List<string> plantNames = new List<string>();
        foreach (PlantData plant in plantList)
        {
            plantNames.Add(plant.plantName);
        }
        string plantNamesString = string.Join(", ", plantNames);
        return plantNamesString;
    }
}
