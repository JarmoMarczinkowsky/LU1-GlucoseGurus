using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class PatientApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> ReadPatientsByParentGuardian(string parentGuardianId)
    {
        string route = "/parentGuardians/" + parentGuardianId + "/patients/";
        string data = JsonUtility.ToJson(parentGuardianId);

        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route, data);
        return ParsePatientListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreatePatient(Patient patient)
    {
        string route = "/parentGuardians/" + patient.parentGuardianId + "/patients/";
        string data = JsonUtility.ToJson(patient);

        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParsePatientResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> UpdatePatient(Patient patient)
    {
        string route = "/parentGuardians/" + patient.parentGuardianId + "/patients/" + patient.id + "/";
        string data = JsonUtility.ToJson(patient);

        IWebRequestReponse webRequestResponse = await webClient.SendPutRequest(route, data);
        return ParsePatientResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeletePatient(string parentGuardianId, string patientId)
    {
        string route = "/parentGuardians/" + parentGuardianId + "/patients/" + patientId + "/";

        IWebRequestReponse webRequestResponse = await webClient.SendDeleteRequest(route);
        return ParsePatientResponse(webRequestResponse);
    }

    private IWebRequestReponse ParsePatientResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Patient patient = JsonUtility.FromJson<Patient>(data.Data);
                WebRequestData<Patient> response = new WebRequestData<Patient>(patient);
                return response;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParsePatientListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                List<Patient> patients = JsonHelper.ParseJsonArray<Patient>(data.Data);
                WebRequestData<List<Patient>> response = new WebRequestData<List<Patient>>(patients);
                return response;
            default:
                return webRequestResponse;
        }

    }
}
