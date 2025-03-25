using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class TrajectApiClient : MonoBehaviour
{

    public WebClient webClient;

    public async Awaitable<IWebRequestResponse> ReadTrajects()
    {
        string route = "/trajects";
        IWebRequestResponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseTrajectListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> CreateTraject(Traject traject)
    {
        string route = "/trajects";
        string data = JsonUtility.ToJson(traject);
        IWebRequestResponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseTrajectResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> UpdateTraject(Traject traject)
    {
        string route = "/trajects/" + traject.id;
        string data = JsonUtility.ToJson(traject);
        IWebRequestResponse webRequestResponse = await webClient.SendPutRequest(route, data);
        return ParseTrajectResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> DeleteTraject(string patientId, string trajectId)
    {
        string route = "/trajects/" + trajectId;
        IWebRequestResponse webRequestResponse = await webClient.SendDeleteRequest(route);
        return ParseTrajectResponse(webRequestResponse);
    }

    private IWebRequestResponse ParseTrajectResponse(IWebRequestResponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Traject traject = JsonUtility.FromJson<Traject>(data.Data);
                WebRequestData<Traject> response = new WebRequestData<Traject>(traject);
                return response;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestResponse ParseTrajectListResponse(IWebRequestResponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                List<Traject>
                trajects = JsonHelper.ParseJsonArray<Traject>(data.Data);
            WebRequestData<List<Traject>> response = new WebRequestData<List<Traject>>(trajects);
            return response;
        default:
            return webRequestResponse;
        }
    }
}