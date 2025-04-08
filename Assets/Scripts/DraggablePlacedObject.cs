using UnityEngine;

public class DraggablePlacedObject : MonoBehaviour
{
    public bool isDragging = false;

    void Start()
    {
        isDragging = true; // Begin met slepen zodra object gespawned wordt
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }

    void OnMouseDown()
    {
        // Elke klik wisselt tussen slepen aan/uit
        isDragging = !isDragging;
    }
}


