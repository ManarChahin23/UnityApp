using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

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

    public void DeleteEnvironment(string id)
    {
        StartCoroutine(DeleteEnvironmentCoroutine(id));
    }

    private IEnumerator DeleteEnvironmentCoroutine(string id)
    {
        string url = $"https://avansict2191579.azurewebsites.net/api/environment2d/{id}";
        UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Delete(url);
        request.SetRequestHeader("Authorization", $"Bearer {PlayerPrefs.GetString("accessToken")}");

        yield return request.SendWebRequest();

        if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            Debug.Log("Wereld verwijderd!");
            RefreshList();
        }
        else
        {
            Debug.LogError("Fout bij verwijderen: " + request.error);
        }
    }


    public void RefreshList()
    {
        foreach (Transform child in listContainer)
        {
            if (child.GetComponent<WereldItem>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        environmentCount = 0;
        Start(); // herlaad alles
    }



}
