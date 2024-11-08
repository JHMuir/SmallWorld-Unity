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
        FirebaseManager.Instance.RetrievePlantData();
    }

    private void LoadPlantData(List<PlantData> plantDatabase)
    {
        foreach(PlantData plantData in plantDatabase)
        {
            Debug.Log(plantData.plantName);
            Debug.Log(plantData.nativeHabitat);
            Debug.Log(plantData.species);
            Debug.Log(plantData.description);

        }
        plants = plantDatabase;
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
}
