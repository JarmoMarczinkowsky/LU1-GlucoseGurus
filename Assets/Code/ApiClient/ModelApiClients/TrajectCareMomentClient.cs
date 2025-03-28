using System.Collections.Generic;
using UnityEngine;

public class TrajectCareMomentClient : MonoBehaviour
{
    public WebClient webClient;
    public async Awaitable<IWebRequestReponse> ReadTrajectCareMoments()
    {
        string route = "/trajectCareMoments";
        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseTrajectCareMomentListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateTrajectCareMoment(TrajectCareMoment trajectCareMoment)
    {
        string route = "/trajectCareMoments";
        string data = JsonUtility.ToJson(trajectCareMoment);
        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseTrajectCareMomentResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> UpdateTrajectCareMoment(TrajectCareMoment trajectCareMoment)
    {
        string route = "/trajectCareMoments/" + trajectCareMoment.trajectId + "/" + trajectCareMoment.CareMomentId;
        string data = JsonUtility.ToJson(trajectCareMoment);
        IWebRequestReponse webRequestResponse = await webClient.SendPutRequest(route, data);
        return ParseTrajectCareMomentResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteTrajectCareMoment(string trajectId, string careMomentId)
    {
        string route = "/trajectCareMoments/" + trajectId + "/" + careMomentId;
        IWebRequestReponse webRequestResponse = await webClient.SendDeleteRequest(route);
        return ParseTrajectCareMomentResponse(webRequestResponse);
    }

    private IWebRequestReponse ParseTrajectCareMomentResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                TrajectCareMoment trajectCareMoment = JsonUtility.FromJson<TrajectCareMoment>(data.Data);
                WebRequestData<TrajectCareMoment> response = new WebRequestData<TrajectCareMoment>(trajectCareMoment);
                return response;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseTrajectCareMomentListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                List<TrajectCareMoment>
                trajectCareMoments = JsonHelper.ParseJsonArray<TrajectCareMoment>(data.Data);
                WebRequestData<List<TrajectCareMoment>> response = new WebRequestData<List<TrajectCareMoment>>(trajectCareMoments);
                return response;
            default:
                return webRequestResponse;
        }
    }

}
