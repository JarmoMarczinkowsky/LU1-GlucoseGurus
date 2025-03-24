using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class ParentGuardianApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestResponse> ReadParentGuardians()
    {
        string route = "/parentGuardians";

        IWebRequestResponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseParentGuardianListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> ReadParentGuardiansByUser(string UserId)
    {
        string route = "/parentGuardians";

        IWebRequestResponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseParentGuardianListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> CreateParentGuardian(ParentGuardian parentGuardian)
    {
        string route = "/parentGuardians";
        string data = JsonUtility.ToJson(parentGuardian);

        IWebRequestResponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseParentGuardianResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> UpdateParentGuardian(ParentGuardian parentGuardian)
    {
        string route = "/parentGuardians/" + parentGuardian.id;
        string data = JsonUtility.ToJson(parentGuardian);

        IWebRequestResponse webRequestResponse = await webClient.SendPutRequest(route, data);
        return ParseParentGuardianResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> DeleteParentGuardian(string parentGuardianId)
    {
        string route = "/parentGuardians/" + parentGuardianId;

        IWebRequestResponse webRequestResponse = await webClient.SendDeleteRequest(route);
        return ParseParentGuardianResponse(webRequestResponse);
    }

    private IWebRequestResponse ParseParentGuardianResponse(IWebRequestResponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                ParentGuardian parentGuardian = JsonUtility.FromJson<ParentGuardian>(data.Data);
                WebRequestData<ParentGuardian> response = new WebRequestData<ParentGuardian>(parentGuardian);
                return response;
            default:
                return webRequestResponse; 
        }
    }

    private IWebRequestResponse ParseParentGuardianListResponse(IWebRequestResponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                List<ParentGuardian> parentGuardians = JsonHelper.ParseJsonArray<ParentGuardian>(data.Data);
                WebRequestData<List<ParentGuardian>> response = new WebRequestData<List<ParentGuardian>>(parentGuardians);
                return response;
            default:
                return webRequestResponse;
        }

    }
}
