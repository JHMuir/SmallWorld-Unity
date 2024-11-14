using System.Collections.Generic;

[System.Serializable]
public class PlantData
{
    public string plantName;
    public string nativeHabitat;
    public string species;
    public string description;
}

[System.Serializable]
public class PlantDataList
{
    public List<PlantData> plants;
}
