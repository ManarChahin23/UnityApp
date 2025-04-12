using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateWorldPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_InputField worldNameInput;
    public Button continueButton;
    public Button closeButton;
    public Environment2DApiClient apiClient;

    private void Start()
    {
        popupPanel.SetActive(false);
        continueButton.onClick.AddListener(CreateWorld);
        closeButton.onClick.AddListener(() => popupPanel.SetActive(false));
    }

    public void OpenPopup()
    {
        popupPanel.SetActive(true);
    }

    private async void CreateWorld()
    {
        string name = worldNameInput.text.Trim();

        if (string.IsNullOrWhiteSpace(name) || name.Length > 25)
        {
            Debug.LogWarning("Voer een geldige naam in (1-25 karakters).");
            return;
        }

        string userId = PlayerPrefs.GetString("userId");

        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("Geen geldige userId gevonden! Zorg dat de gebruiker is ingelogd.");
            return;
        }

        var environment = new Environment2D
        {
            name = name,
            maxHeight = 50,
            maxWidth = 100,
            userId = userId
        };

        var result = await apiClient.CreateEnvironment(environment);

        if (result is WebRequestData<Environment2D> data)
        {
            PlayerPrefs.SetString("selectedEnvironmentId", data.Data.id);
            SceneManager.LoadScene("CreateEnvironmentScene");
        }
        else if (result is WebRequestError error)
        {
            Debug.LogError("Fout bij aanmaken: " + error.ErrorMessage);
        }
    }
}
