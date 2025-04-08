using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField repeatPasswordInput;
    public Button registerButton;
    public UserApiClient userApiClient;

    private void Start()
    {
        registerButton.onClick.AddListener(OnRegisterClicked);
    }


    public async void OnRegisterClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string repeatPassword = repeatPasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repeatPassword))
        {
            Debug.LogWarning("Vul alle velden in.");
            return;
        }

        if (password != repeatPassword)
        {
            Debug.LogWarning("Wachtwoorden komen niet overeen.");
            return;
        }

        var registerData = new User
        {
            email = email,
            password = password

        };

        var result = await userApiClient.Register(registerData);

        if (result is WebRequestData<string> success)
        {

            await userApiClient.Login(registerData);
            //Niet alles ingevuld : stuur naar Gegevens invulscherm
            SceneManager.LoadScene("LoginScene");
        }
        else if (result is WebRequestError error)
        {
            Debug.LogError("Login fout: " + error.ErrorMessage);
        }
    }

    public void ToLogin()
    {
        SceneManager.LoadScene("LoginScene");
    }

}
