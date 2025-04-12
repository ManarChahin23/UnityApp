using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EnvironmentsListManager : MonoBehaviour
{
    public static EnvironmentsListManager Instance;

    public Transform listContainer;
    public WereldItem environmentItemPrefab;
    public Environment2DApiClient apiClient; 
    private int environmentCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        string userId = PlayerPrefs.GetString("userId");

        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("Geen geldige userId gevonden!");
            return;
        }

        var result = await apiClient.ReadEnvironment2Ds();

        if (result is WebRequestData<List<Environment2D>> data)
        {
            foreach (var env in data.Data)
            {
                AddEnvironmentToList(env);
            }
        }
        else if (result is WebRequestError error)
        {
            Debug.LogError("Fout bij ophalen omgevingen: " + error.ErrorMessage);
        }
    }

    public void AddEnvironmentToList(Environment2D env)
    {
        WereldItem item = Instantiate(environmentItemPrefab, listContainer);
        item.transform.position += new Vector3(0f, -150f * environmentCount, 0f);

        item.wereldNaamText.text = env.name;

        item.environmentId = env.id;

        environmentCount++;


    }

}
