using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set;}
    public GameObject popupPrefab; 
    private GameObject currentPopup;

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
    public void ShowPopup(PlantData data)
    {
        if(currentPopup != null)
        {
            Destroy(currentPopup);
        }
        currentPopup = Instantiate(popupPrefab, transform);
        currentPopup.transform.SetParent(GameObject.Find("Canvas").transform, false);
        TextMeshProUGUI[] plantTexts = currentPopup.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI plantText in plantTexts)
        {
            if(plantText.gameObject.name == "PlantName")
            {
                plantText.text = data.plantName;
            }
            else if(plantText.gameObject.name == "Body")
            {
                plantText.text = $"Native Habitat: {data.nativeHabitat}\nSpecies: {data.species}\nDescription:{data.description}";
            }
        }
    }
}
