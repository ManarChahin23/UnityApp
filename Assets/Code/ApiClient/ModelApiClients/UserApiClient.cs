using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class UserApiClient : MonoBehaviour
{
    public async Awaitable<IWebRequestResponse> Register(User user)
    {
        string route = "/auth/register";
        string data = JsonUtility.ToJson(user);

        return await WebClient.Instance.SendPostRequest(route, data);
    }

    public async Awaitable<IWebRequestResponse> Login(User user)
    {
        string route = "/auth/login";
        string data = JsonUtility.ToJson(user);

        IWebRequestResponse response = await WebClient.Instance.SendPostRequest(route, data);
        return ProcessLoginResponse(response);
    }

    private IWebRequestResponse ProcessLoginResponse(IWebRequestResponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                string token = JsonHelper.ExtractToken(data.Data);
                WebClient.Instance.SetToken(token);
                return new WebRequestData<string>("Succes");
            default:
                return webRequestResponse;
        }
    }
}

