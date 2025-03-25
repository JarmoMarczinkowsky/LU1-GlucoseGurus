using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class CareMomentApiClient : MonoBehaviour
{
    public WebClient webClient;
    public async Awaitable<IWebRequestResponse> ReadCareMoments()
    {
        string route = "/careMoments";
        IWebRequestResponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseCareMomentListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> CreateCareMoment(CareMoment careMoment)
    {
        string route = "/careMoments";
        string data = JsonUtility.ToJson(careMoment);
        IWebRequestResponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseCareMomentResponse(webRequestResponse);
    }
    public async Awaitable<IWebRequestResponse> UpdateCareMoment(CareMoment careMoment)
    {
        string route = "/careMoments/" + careMoment.id;
        string data = JsonUtility.ToJson(careMoment);
        IWebRequestResponse webRequestResponse = await webClient.SendPutRequest(route, data);
        return ParseCareMomentResponse(webRequestResponse);
    }
    public async Awaitable<IWebRequestResponse> DeleteCareMoment(string careMomentId)
    {
        string route = "/careMoments/" + careMomentId;
        IWebRequestResponse webRequestResponse = await webClient.SendDeleteRequest(route);
        return ParseCareMomentResponse(webRequestResponse);
    }
    private IWebRequestResponse ParseCareMomentResponse(IWebRequestResponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                CareMoment careMoment = JsonUtility.FromJson<CareMoment>(data.Data);
                WebRequestData<CareMoment> response = new WebRequestData<CareMoment>(careMoment);
                return response;
            default:
                return webRequestResponse;
        }
    }
    private IWebRequestResponse ParseCareMomentListResponse(IWebRequestResponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                List<CareMoment> careMoments = JsonHelper.ParseJsonArray<CareMoment>(data.Data);
                WebRequestData<List<CareMoment>> response = new WebRequestData<List<CareMoment>>(careMoments);
                return response;
            default:
                return webRequestResponse;
        }
    }
}