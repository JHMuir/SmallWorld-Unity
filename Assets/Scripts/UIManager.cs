using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("HELLLOOOO PLEASE WORK");
        if(currentPopup != null)
        {
            Destroy(currentPopup);
        }

        currentPopup = Instantiate(popupPrefab, transform);//.position, Quaternion.identity);
        currentPopup.transform.SetParent(GameObject.Find("Canvas").transform, false);
        // Text plantNameText = currentPopup.transform.Find("PlantNameText").GetComponent<Text>();
        // Text habitatText = currentPopup.transform.Find("HabitatText").GetComponent<Text>();
        // Text speciesText = currentPopup.transform.Find("SpeciesText").GetComponent<Text>();
        // Text descriptionText = currentPopup.transform.Find("DescriptionText").GetComponent<Text>();

        // plantNameText.text = data.plantName;
        // habitatText.text = data.nativeHabitat;
        // speciesText.text = data.species;
        // descriptionText.text = data.description;

    }
}
