using Firebase;
using Firebase.Database;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }

    FirebaseStorage storage;
    StorageReference storageRef;
    
    [System.Serializable] public class DataLoadedEvent : UnityEvent<List<PlantData>> { }
    public DataLoadedEvent onDataLoaded;

    DatabaseReference databaseRef;

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
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        InitializeStorage();
        InitializeRealtime();
        Debug.Log("Firebase Initialization Complete.");
    }

    private void InitializeStorage()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl("gs://smallworld-b093d.appspot.com/plant_data/plants.json"); 
        Debug.Log("Firebase Storage Initalized Successfully");
    }

    public void RetrievePlantDataFromStorage()
    {
        storageRef.GetBytesAsync(long.MaxValue).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("ERROR: Firebase Storage Data Retrieval Unsuccessful: " + task.Exception);
                return;
            }
            else
            {
                Debug.Log("Firebase Storage Data Successfully Retrieved");
                byte[] fileContents = task.Result;
                string jsonText = Encoding.UTF8.GetString(fileContents);

                List<PlantData> plantList = JsonUtility.FromJson<PlantDataList>(jsonText).plants;

                Debug.Log("FROM FIREBASE: " + PlantManager.Instance.OutputPlantNames(plantList));

                onDataLoaded?.Invoke(plantList);
            }
        });
    }

    private void InitializeRealtime()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => 
        {
            if(task.IsCanceled)
            {
                Debug.LogError("Firebase Realtime Initialization Canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("ERROR: Firebase Realtime Initialization Faulted: " + task.Exception);
                return;
            }

            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) 
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase Realtime Initialization Successful.");

                
            } 
            else 
            {
                Debug.LogError($"ERROR: Firebas Dependency Issue: {dependencyStatus}");
            }
        });
    }

    public void RetrieveDataFromRealtime()
    {
        if (databaseRef == null)
        {
            Debug.LogError("Firebase Realtime Database Reference is Null. Ensure Firebase Realtime is Initialized.");
            return;
        }
        // Path to the data in the database
        DatabaseReference dataRef = databaseRef.Child("images");

        // Listen for the value once
        dataRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Firebase Realtime Data Retrieval Canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("ERROR: Firebase Realtime Data Retreival Faulted: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("Firebase Realtime Data Retrieved Successfully: " + snapshot.GetRawJsonValue());

                // Parse data 
            }
        });
    }
}
