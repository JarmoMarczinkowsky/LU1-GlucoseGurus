using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NoteManagerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Input Fields")]
    public List<TMP_InputField> lst_InputFields;
    public Button btnCreate;
    public List<Image> lst_MoodImages;

    [Header("Note overview")]
    public GameObject noteField;

    [Header("Note creator")]
    public GameObject menuNoteOverview;
    public GameObject menuNoteCreator;
    public TMP_Text txbErrorNoteCreator;
    public GameObject notePrefab;

    [Header("Dependencies")]
    public NoteApiClient noteApiClient;
    public UserApiClient userApiClient;


    private bool isTabPressed = false;
    private int tabIndex = 0;
    private int moodScale = 0;
    private Note newNote;
    private bool hoverOverCreationMenu = false;

    //Color palette:
    //https://coolors.co/8bc348-f5c523-fe5377-0b3954-bfd7ea
    void Start()
    {
        menuNoteCreator.SetActive(false);

        ClearNotes();
        LoadNotes();
    }

    // Update is called once per frame
    void Update()
    {
        SelectOtherInputField();

        CheckForClick();
    }

    private void ClearNotes()
    {
        for (int i = noteField.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(noteField.transform.GetChild(i).gameObject);
        }
    }

    private async void LoadNotes()
    {
        IWebRequestReponse webRequestResponse = await noteApiClient.ReadNotesByPatient("123");

        switch (webRequestResponse)
        {
            case WebRequestData<List<Note>> dataResponse:
                List<Note> notes = dataResponse.Data;
                Debug.Log("List of notes: ");
                notes.ForEach(note => Debug.Log(note.id));
                // TODO: Handle succes scenario.
                foreach (var note in notes)
                {
                    GameObject retrievedNote = Instantiate(notePrefab, menuNoteOverview.transform);
                    retrievedNote.transform.parent = noteField.transform;

                    if (retrievedNote.GetComponentInChildren<TMP_Text>() != null)
                    {
                        retrievedNote.GetComponentInChildren<TMP_Text>().text = note.date.ToString();
                    }
                }
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Read notes error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }

    }

    private void SelectOtherInputField()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isTabPressed)
        {
            isTabPressed = true;
            Debug.Log($"TabIndex: {tabIndex}");

            //if tab is pressed, select the next input field, if the last input field is selected, select the button, if the button is selected, select the first input field
            switch (tabIndex)
            {
                //case 0:
                //    lst_InputFields[tabIndex].Select();
                //    break;
                case 1:
                    lst_InputFields[tabIndex].Select();
                    break;
                case 2:
                    btnCreate.Select();
                    break;
                default:
                    tabIndex = 0;
                    lst_InputFields[tabIndex].Select();
                    break;
            };

            tabIndex++;

        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            isTabPressed = false;
        }
    }

    private void CheckForClick()
    {
        if (menuNoteCreator.activeSelf)
        {
            if(Input.GetMouseButtonDown(0) && !hoverOverCreationMenu)
            {
                Debug.Log("Niet gehoverd over menu");
                CloseNoteCreator();
            }
            else if (Input.GetMouseButtonDown(0) && hoverOverCreationMenu)
            {
                Debug.Log("Aan het hoveren over menu");
            }
        }
    }

    public void ShowNoteCreator()
    {
        menuNoteCreator.SetActive(true);
        menuNoteOverview.SetActive(false);
        txbErrorNoteCreator.text = "";
    }

    public void CloseNoteCreator()
    {
        menuNoteCreator.SetActive(false);
        menuNoteOverview.SetActive(true);
    }

    public void CreateNote()
    {
        if (!NewNoteValidation())
        {
            newNote = new Note()
            {
                text = lst_InputFields[1].text,
                userMood = moodScale
            };

            CreateNewNote();
        }
    }

    public bool NewNoteValidation()
    {
        if (string.IsNullOrWhiteSpace(lst_InputFields[0].text))
        {
            txbErrorNoteCreator.text = "Voer een titel in";
            return false;
        }
        else if(string.IsNullOrWhiteSpace(lst_InputFields[1].text))
        {
            txbErrorNoteCreator.text = "Voer een notitie in";
            return false;
        }
        else if (moodScale == 0)
        {
            txbErrorNoteCreator.text = "Selecteer een stemming";
            return false;
        }

        return true;
    }

    public async void CreateNewNote()
    {
        IWebRequestReponse webRequestResponse = await noteApiClient.CreateNote(newNote);

        switch (webRequestResponse)
        {
            case WebRequestData<Note> dataResponse:
                newNote.id = dataResponse.Data.id;
                // TODO: Handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Create note error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    public void ChangeMoodScale(int scale)
    {
        Debug.Log($"Clicked mood scale {scale}");

        moodScale = scale;

        for (int i = 0; i < lst_MoodImages.Count; i++)
        {
            if(lst_MoodImages[i] != null)
            {
                if (i == scale - 1)
                {
                    lst_MoodImages[i].GetComponent<Image>().color = Color.white;
                }
                else
                {
                    lst_MoodImages[i].GetComponent<Image>().color = Color.gray;
                }
            }
        }
    }

    public void HoverOverMoodScale(int scale)
    {
        Debug.Log($"Hover over mood scale {scale}");
        for (int i = 0; i < lst_MoodImages.Count; i++)
        {
            if (lst_MoodImages[i] != null && moodScale == 0)
            {
                if (i == scale - 1)
                {
                    lst_MoodImages[i].GetComponent<Image>().color = new Color((float)0.75, (float)0.75, 1);
                }
                else
                {
                    lst_MoodImages[i].GetComponent<Image>().color = Color.white;
                }
            }
        }
    }

    public void HoverExitMoodScale(int scale)
    {
        Debug.Log($"Hover exited mood scale {scale}");
        for (int i = 0; i < lst_MoodImages.Count; i++)
        {
            if (lst_MoodImages[i] != null && moodScale == 0)
            {
                if (i == scale - 1)
                {
                    lst_MoodImages[i].GetComponent<Image>().color = Color.white;
                }
            }
        }
    }

    public void HoverOverCreationMenu()
    {
        hoverOverCreationMenu = true;
    }

    public void HoverExitCreationMenu()
    {
        hoverOverCreationMenu = false;
    }
}
