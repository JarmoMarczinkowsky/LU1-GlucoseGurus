using UnityEngine;
using UnityEngine.EventSystems;

public class SingleNoteScript : MonoBehaviour
{
    private string _guid;

    public void SetId(string guid)
    {
        _guid = guid;
        Debug.Log("Set id to: " + _guid);
    }

    public string GetId()
    {
        return _guid;
    }

    public void OnClick()
    {
        Debug.Log("Clicked on note with ID: " + _guid);
    }
}

