using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class DoctorApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> ReadDoctors()
    {
        string route = "/doctors";

        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseDoctorListResponse(webRequestResponse);
    }


    private IWebRequestReponse ParseDoctorResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Doctor doctor = JsonUtility.FromJson<Doctor>(data.Data);
                WebRequestData<Doctor> response = new WebRequestData<Doctor>(doctor);
                return response;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseDoctorListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                List<Doctor> doctors = JsonHelper.ParseJsonArray<Doctor>(data.Data);
                WebRequestData<List<Doctor>> response = new WebRequestData<List<Doctor>>(doctors);
                return response;
            default:
                return webRequestResponse;
        }

    }
}
