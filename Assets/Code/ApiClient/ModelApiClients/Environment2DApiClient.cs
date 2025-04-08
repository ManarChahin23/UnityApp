using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Environment2DApiClient : MonoBehaviour
{
    public async Awaitable<IWebRequestResponse> ReadEnvironment2Ds() 
    {
        string route = "/api/environment2d";

        IWebRequestResponse webRequestResponse = await WebClient.Instance.SendGetRequest(route);
        return ParseEnvironment2DListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> CreateEnvironment(Environment2D environment)
    {
        environment.id = Guid.NewGuid().ToString();

        string route = "/api/environment2d";
        string data = JsonUtility.ToJson(environment);

        IWebRequestResponse webRequestResponse = await WebClient.Instance.SendPostRequest(route, data);
        return ParseEnvironment2DResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> DeleteEnvironment(string environmentId)
    {
        string route = "/api/environment2d" + environmentId;
        return await WebClient.Instance.SendDeleteRequest(route);
    }

    private IWebRequestResponse ParseEnvironment2DResponse(IWebRequestResponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                Environment2D environment = JsonUtility.FromJson<Environment2D>(data.Data);
                WebRequestData<Environment2D> parsedWebRequestData = new WebRequestData<Environment2D>(environment);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestResponse ParseEnvironment2DListResponse(IWebRequestResponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                List<Environment2D> environment2Ds = JsonHelper.ParseJsonArray<Environment2D>(data.Data);
                WebRequestData<List<Environment2D>> parsedWebRequestData = new WebRequestData<List<Environment2D>>(environment2Ds);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }
}

