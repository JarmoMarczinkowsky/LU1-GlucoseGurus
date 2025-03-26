using System.Collections.Generic;
using UnityEngine;

public class NoteApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> ReadNotesByPatient(string patientId)
    {
        string route = "/notes" + "/patient/" + patientId;
        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseNoteListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> ReadNotedByParentGuardian(string parentGuardianId)
    {
        string route = "/notes" + "/parentGuardian/" + parentGuardianId;
        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseNoteListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateNote(Note note)
    {
        string route = "/notes";
        string data = JsonUtility.ToJson(note);
        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseNoteResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> UpdateNote(Note note)
    {
        string route = "/notes/" + note.id;
        string data = JsonUtility.ToJson(note);
        IWebRequestReponse webRequestResponse = await webClient.SendPutRequest(route, data);
        return ParseNoteResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteNote(string noteId)
    {
        string route = "/notes/" + noteId;
        IWebRequestReponse webRequestResponse = await webClient.SendDeleteRequest(route);
        return ParseNoteResponse(webRequestResponse);
    }

    private IWebRequestReponse ParseNoteResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Note note = JsonUtility.FromJson<Note>(data.Data);
                WebRequestData<Note> response = new WebRequestData<Note>(note);
                return response;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseNoteListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                List<Note> notes = JsonHelper.ParseJsonArray<Note>(data.Data);
                WebRequestData<List<Note>> response = new WebRequestData<List<Note>>(notes);
                return response;
            default:
                return webRequestResponse;
        }
    }
}
