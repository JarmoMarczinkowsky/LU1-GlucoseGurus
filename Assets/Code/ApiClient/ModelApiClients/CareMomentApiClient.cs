using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CareMomentApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> ReadCareMoments() 
    {
        string route = "/caremoments";

        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseCareMomentListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateCareMoment(CareMoment caremoment)
    {
        string route = "/caremoments/create";
        string data = JsonUtility.ToJson(caremoment);

        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseCareMomentResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteCareMoment(string caremomentId)
    {
        string route = "/caremoments/" + caremomentId;
        return await webClient.SendDeleteRequest(route);
    }

    public async Awaitable<IWebRequestReponse> UpdateCareMoment(CareMoment caremoment)
    {
        string route = "/caremoments/" + caremoment.id;
        string data = JsonUtility.ToJson(caremoment);

        return await webClient.SendPutRequest(route, data);
    }

    private IWebRequestReponse ParseCareMomentResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                CareMoment caremoment = JsonUtility.FromJson<CareMoment>(data.Data);
                WebRequestData<CareMoment> parsedWebRequestData = new WebRequestData<CareMoment>(caremoment);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseCareMomentListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                List<CareMoment> caremoments = JsonHelper.ParseJsonArray<CareMoment>(data.Data);
                WebRequestData<List<CareMoment>> parsedWebRequestData = new WebRequestData<List<CareMoment>>(caremoments);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

}

