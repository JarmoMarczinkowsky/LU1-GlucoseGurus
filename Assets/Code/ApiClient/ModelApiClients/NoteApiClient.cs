using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NoteApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> ReadNotes() 
    {
        string route = "/notes";

        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseNoteListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateNote(Note note)
    {
        string route = "/notes/create";
        string data = JsonUtility.ToJson(note);

        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseNoteResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteNote(string noteId)
    {
        string route = "/notes/" + noteId;
        return await webClient.SendDeleteRequest(route);
    }

    public async Awaitable<IWebRequestReponse> UpdateNote(Note note)
    {
        string route = "/notes/" + note.id;
        string data = JsonUtility.ToJson(note);

        return await webClient.SendPutRequest(route, data);
    }

    private IWebRequestReponse ParseNoteResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                Note note = JsonUtility.FromJson<Note>(data.Data);
                WebRequestData<Note> parsedWebRequestData = new WebRequestData<Note>(note);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseNoteListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                List<Note> notes = JsonHelper.ParseJsonArray<Note>(data.Data);
                WebRequestData<List<Note>> parsedWebRequestData = new WebRequestData<List<Note>>(notes);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

}

