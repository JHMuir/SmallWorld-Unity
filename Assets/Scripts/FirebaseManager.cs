using Firebase;
using Firebase.Database;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }
    DatabaseReference database;

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
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance.RootReference;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                "ERROR: Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        Debug.Log("Firebase Initialization Complete.");
    }

    private void RetrieveDataFromFirebase()
    {
        // Path to the data in the database
        DatabaseReference dataRef = database.Child("path/to/data");

        // Listen for the value once
        dataRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle any errors here
                Debug.LogError("Failed to retrieve data.");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Access the data in the snapshot
                Debug.Log("Data retrieved: " + snapshot.GetRawJsonValue());
            }
        });
    }

}
