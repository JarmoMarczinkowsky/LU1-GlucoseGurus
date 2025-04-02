using UnityEngine;

public class SingleNoteScript : MonoBehaviour
{
    private string _guid;

    public void SetId(string guid)
    {
        _guid = guid;
    }

    public string GetId()
    {
        return _guid;
    }

    public void OnMouseUpAsButton()
    {
        Debug.Log("Clicked on note with id: " + _guid);
    }
}

