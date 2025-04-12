using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Object2DApiClient : MonoBehaviour
{
    public async Awaitable<IWebRequestResponse> ReadObject2Ds(string environmentId)
    {
        string route = "/api/environment2d/" + environmentId + "/object2d";

        IWebRequestResponse webRequestResponse = await WebClient.Instance.SendGetRequest(route);
        return ParseObject2DListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> DeleteAllObjects2D(string environmentId)
    {
        string route = "/api/environment2d/" + environmentId + "/object2d";

        IWebRequestResponse webRequestResponse = await WebClient.Instance.SendDeleteRequest(route);
        return webRequestResponse;
    }

    public async Awaitable<IWebRequestResponse> CreateObject2D(Object2D object2D)
    {
        object2D.id = Guid.NewGuid().ToString();
        string route = "/api/environment2d/" + object2D.environmentId + "/object2d";
        string data = JsonUtility.ToJson(object2D);

        IWebRequestResponse webRequestResponse = await WebClient.Instance.SendPostRequest(route, data);
        return ParseObject2DResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> UpdateObject2D(Object2D object2D)
    {
        string route = "/api/environment2d/" + object2D.environmentId + "/object2d/" + object2D.id;
        string data = JsonUtility.ToJson(object2D);

        return await WebClient.Instance.SendGetRequest(route);
    }

    private IWebRequestResponse ParseObject2DResponse(IWebRequestResponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                Object2D object2D = JsonUtility.FromJson<Object2D>(data.Data);
                WebRequestData<Object2D> parsedWebRequestData = new WebRequestData<Object2D>(object2D);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestResponse ParseObject2DListResponse(IWebRequestResponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                List<Object2D> environments = JsonHelper.ParseJsonArray<Object2D>(data.Data);
                WebRequestData<List<Object2D>> parsedData = new WebRequestData<List<Object2D>>(environments);
                return parsedData;
            default:
                return webRequestResponse;
        }
    }
}