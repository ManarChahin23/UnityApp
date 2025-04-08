using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;


public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TMP_Text feedbackText;
    public UserApiClient userApiClient;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
    }

    private async void OnLoginClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Vul zowel e-mail als wachtwoord in.";
            return;
        }

        var loginData = new User
        {
            email = email,
            password = password
        };

        var response = await userApiClient.Login(loginData);

        if (response is WebRequestData<string> success)
        {
            //Niet alles ingevuld stuur naar Gegevens invulscherm
            SceneManager.LoadScene("EnvironmentsScene");
        }
        else if (response is WebRequestError error)
        {
            Debug.LogError("Login fout: " + error.ErrorMessage);
            feedbackText.text = "Fout bij inloggen.";
        }
    }

    public void ToRegister()
    {
        SceneManager.LoadScene("RegisterScene");
    }
}
