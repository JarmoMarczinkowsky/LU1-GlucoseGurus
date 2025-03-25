using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class PatientApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestResponse> ReadPatientsByParentGuardian(string parentGuardianId)
    {
        string route = "/parentGuardians/" + parentGuardianId + "/patients";

        IWebRequestResponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParsePatientListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> CreatePatient(Patient patient)
    {
        string route = "/parentGuardians" + patient.parentGuardianId + "/patients";
        string data = JsonUtility.ToJson(patient);

        IWebRequestResponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParsePatientResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> UpdatePatient(Patient patient)
    {
        string route = "/parentGuardians/" + patient.parentGuardianId + "/patients/" + patient.id;
        string data = JsonUtility.ToJson(patient);

        IWebRequestResponse webRequestResponse = await webClient.SendPutRequest(route, data);
        return ParsePatientResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestResponse> DeletePatient(string parentGuardianId, string patientId)
    {
        string route = "/parentGuardians/" + parentGuardianId + "/patients/" + patientId;

        IWebRequestResponse webRequestResponse = await webClient.SendDeleteRequest(route);
        return ParsePatientResponse(webRequestResponse);
    }

    private IWebRequestResponse ParsePatientResponse(IWebRequestResponse webRequestResponse)
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

    private IWebRequestResponse ParsePatientListResponse(IWebRequestResponse webRequestResponse)
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
