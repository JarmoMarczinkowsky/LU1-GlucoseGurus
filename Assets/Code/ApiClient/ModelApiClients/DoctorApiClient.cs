using System;
using System.Collections.Generic;
using System.Net;
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

    public async Awaitable<IWebRequestReponse> CreateDoctor(Doctor doctor)
    {
        string route = "/doctors/create";
        string data = JsonUtility.ToJson(doctor);

        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseDoctorResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteDoctor(string doctorId)
    {
        string route = "/doctors/" + doctorId;
        return await webClient.SendDeleteRequest(route);
    }

    public async Awaitable<IWebRequestReponse> UpdateDoctor(Doctor doctor)
    {
        string route = "/doctors/" + doctor.id;
        string data = JsonUtility.ToJson(doctor);

        return await webClient.SendPutRequest(route, data);
    }

    private IWebRequestReponse ParseDoctorResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                Doctor doctor = JsonUtility.FromJson<Doctor>(data.Data);
                WebRequestData<Doctor> parsedWebRequestData = new WebRequestData<Doctor>(doctor);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseDoctorListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                List<Doctor> doctors = JsonHelper.ParseJsonArray<Doctor>(data.Data);
                WebRequestData<List<Doctor>> parsedWebRequestData = new WebRequestData<List<Doctor>>(doctors);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

}

