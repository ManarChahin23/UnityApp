using NUnit;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WereldItem : MonoBehaviour
{
    public TMP_Text wereldNaamText;
    public Button openBtn;
    public string environmentId;


    private void Start()
    {
        openBtn.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("selectedEnvironmentId", environmentId);
            SceneManager.LoadScene("CreateEnvironmentScene");
        });
    }

}

