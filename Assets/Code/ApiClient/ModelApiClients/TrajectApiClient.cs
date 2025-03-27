using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
        string route = "/trajects";
        string data = JsonUtility.ToJson(traject);
        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseTrajectResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> UpdateTraject(Traject traject)
    {
        string route = "/trajects/" + traject.id;
        string data = JsonUtility.ToJson(traject);
        IWebRequestReponse webRequestResponse = await webClient.SendPutRequest(route, data);
        return ParseTrajectResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteTraject(string patientId, string trajectId)
    {
        string route = "/trajects/" + trajectId;
        IWebRequestReponse webRequestResponse = await webClient.SendDeleteRequest(route);
        return ParseTrajectResponse(webRequestResponse);
    }

    private IWebRequestReponse ParseTrajectResponse(IWebRequestReponse webRequestResponse)
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

    private IWebRequestReponse ParseTrajectListResponse(IWebRequestReponse webRequestResponse)
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
