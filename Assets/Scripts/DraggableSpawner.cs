using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSpawner : MonoBehaviour, IPointerDownHandler
{
    public GameObject objectToSpawnPrefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject spawned = Instantiate(objectToSpawnPrefab);
        spawned.AddComponent<DraggablePlacedObject>();
    }
}
