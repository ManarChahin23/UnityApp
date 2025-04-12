using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectSceneManager : MonoBehaviour
{
    public Object2DApiClient objectApiClient;
    public List<GameObject> availablePrefabs;

    private string environmentId;
    private List<(GameObject, int)> placedObjects = new();
    private Stack<(GameObject, int)> undoStack = new();

    public Button backButton;

    private async void Start()
    {
        backButton.onClick.AddListener(GoBackToEnvironments);

        environmentId = PlayerPrefs.GetString("selectedEnvironmentId");
        if (string.IsNullOrEmpty(environmentId))
        {
            Debug.LogError("Geen geselecteerde wereld gevonden!");
            return;
        }

        await LoadObjects();

    }

    private void GoBackToEnvironments()
    {
        SceneManager.LoadScene("EnvironmentsScene");
    }



    private async Task LoadObjects()
    {
        var result = await objectApiClient.ReadObject2Ds(environmentId);

        if (result is WebRequestData<List<Object2D>> data)
        {
            foreach (var obj in data.Data)
            {
                int index = int.Parse(obj.prefabId);
                GameObject prefab = availablePrefabs[index];
                GameObject spawned = Instantiate(prefab, new Vector3(obj.positionX, obj.positionY, 0), Quaternion.identity);

                spawned.transform.localScale = new Vector3(obj.scaleX, obj.scaleY, 1);
                spawned.transform.rotation = Quaternion.Euler(0, 0, obj.rotationZ);

                placedObjects.Add((spawned, index));
            }
        }
        else
        {
            Debug.LogWarning("Geen objecten geladen.");
        }
    }

    public async 
    Task
SaveObject(GameObject placedObject, int prefabId)
    {
        Object2D newObj = new Object2D
        {
            id = System.Guid.NewGuid().ToString(),
            prefabId = prefabId.ToString(),
            environmentId = environmentId,
            positionX = placedObject.transform.position.x,
            positionY = placedObject.transform.position.y,
            scaleX = placedObject.transform.localScale.x,
            scaleY = placedObject.transform.localScale.y,
            rotationZ = placedObject.transform.eulerAngles.z,
            sortingLayer = 0
        };

        await objectApiClient.CreateObject2D(newObj);
    }

    public async void SaveAllObjects()
    {
        await objectApiClient.DeleteAllObjects2D(environmentId);

        foreach (var (obj, prefabId) in placedObjects)
        {
            await SaveObject(obj, prefabId);
        }

        Debug.Log("Alle objecten zijn opgeslagen.");
    }

    public void ResetScene()
    {
        foreach (var (obj, _) in placedObjects)
        {
            Destroy(obj);
        }
        placedObjects.Clear();
        undoStack.Clear();
    }

    public void AddPlacedObject(GameObject obj, int prefabId)
    {
        placedObjects.Add((obj, prefabId));
        undoStack.Push((obj, prefabId));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            UndoLastObject();
        }
    }

    private void UndoLastObject()
    {
        if (undoStack.Count > 0)
        {
            var (obj, prefabId) = undoStack.Pop();
            placedObjects.Remove((obj, prefabId));
            Destroy(obj);
            Debug.Log("Laatste object verwijderd.");
        }
    }

}
