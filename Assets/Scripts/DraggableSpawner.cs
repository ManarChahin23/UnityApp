using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSpawner : MonoBehaviour, IPointerDownHandler
{
    public GameObject objectToSpawnPrefab;
    public int prefabId; // voeg deze toe zodat je weet welk object dit is

    private ObjectSceneManager sceneManager;

    private void Start()
    {
        sceneManager = FindObjectOfType<ObjectSceneManager>();
        if (sceneManager == null)
        {
            Debug.LogError("ObjectSceneManager niet gevonden in de scene.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject spawned = Instantiate(objectToSpawnPrefab);
        spawned.AddComponent<DraggablePlacedObject>();

        // Registreer het object voor opslaan/undo
        sceneManager.AddPlacedObject(spawned, prefabId);
    }
}
