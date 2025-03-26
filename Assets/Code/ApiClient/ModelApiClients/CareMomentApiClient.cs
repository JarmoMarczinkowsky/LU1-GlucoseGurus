using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class CareMomentApiClient : MonoBehaviour
{
    public WebClient webClient;
    public async Awaitable<IWebRequestReponse> ReadCareMoments()
    {
        string route = "/careMoments";
        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseCareMomentListResponse(webRequestResponse);
    }

    private IWebRequestReponse ParseCareMomentResponse(IWebRequestReponse webRequestResponse)
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

    private IWebRequestReponse ParseCareMomentListResponse(IWebRequestReponse webRequestResponse)
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