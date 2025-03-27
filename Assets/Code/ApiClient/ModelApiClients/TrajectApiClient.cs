using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TrajectApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> ReadTrajects() 
    {
        string route = "/trajects";

        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseTrajectListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateTraject(Traject traject)
    {
        string route = "/trajects/create";
        string data = JsonUtility.ToJson(traject);

        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseTrajectResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteTraject(string trajectId)
    {
        string route = "/trajects/" + trajectId;
        return await webClient.SendDeleteRequest(route);
    }

    public async Awaitable<IWebRequestReponse> UpdateTraject(Traject traject)
    {
        string route = "/trajects/" + traject.id;
        string data = JsonUtility.ToJson(traject);

        return await webClient.SendPutRequest(route, data);
    }

    private IWebRequestReponse ParseTrajectResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                Traject traject = JsonUtility.FromJson<Traject>(data.Data);
                WebRequestData<Traject> parsedWebRequestData = new WebRequestData<Traject>(traject);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseTrajectListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                List<Traject> trajects = JsonHelper.ParseJsonArray<Traject>(data.Data);
                WebRequestData<List<Traject>> parsedWebRequestData = new WebRequestData<List<Traject>>(trajects);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

}

