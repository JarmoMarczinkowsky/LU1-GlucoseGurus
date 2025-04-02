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
    public GameObject noteRead;

    public TMP_InputField noteReadDate;
    public TMP_InputField noteReadText;
    public List<Image> lst_NoteMood;


    [Header("Note creator")]
    public GameObject menuNoteOverview;
    public GameObject menuNoteCreator;
    public TMP_Text txbErrorNoteCreator;
    public Button notePrefab;

    [Header("Dependencies")]
    private ApiClientHolder ApiClientHolder;
    private NoteApiClient noteApiClient;
    private UserApiClient userApiClient;

    [Header("Parrot")]
    public GameObject parrot;

    private bool isTabPressed = false;
    private int tabIndex = 0;
    private int moodScale = 0;
    private Note newNote;
    private bool hoverOverCreationMenu = false;
    private List<Note> notes;

    private List<string> motivationTexts = new List<string>() { "Vandaag ben ik sterker geweest dan Superman, omdat...", "Een moment waarop ik sterk was vandaag, was...", "Een kleine overwinning van vandaag was...", "Vandaag voelde ik mij een held, omdat...", "Ik ben trots op mijzelf vandaag, omdat..." };

    //Color palette:
    //https://coolors.co/8bc348-f5c523-fe5377-0b3954-bfd7ea
    void Start()
    {
        ApiClientHolder = ApiClientHolder.instance;
        noteApiClient = ApiClientHolder.noteApiClient;
        userApiClient = ApiClientHolder.userApiClient;

        menuNoteCreator.SetActive(false);
        noteRead.SetActive(false);

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
        string loggedInUserId = ApiClientHolder.Patient.id;

        IWebRequestReponse webRequestResponse = await noteApiClient.ReadNotesByPatient(loggedInUserId);

        switch (webRequestResponse)
        {
            case WebRequestData<List<Note>> dataResponse:
                notes = dataResponse.Data;

                //sort notes by date
                notes.Sort((x, y) => DateTime.Compare(DateTime.Parse(y.date), DateTime.Parse(x.date)));


                if (notes != null)
                {
                    Debug.Log("Aantal notes in lijst: " + notes.Count);
                }

                Debug.Log("List of notes: ");
                notes.ForEach(note => Debug.Log(note.id));
                // TODO: Handle succes scenario.
                foreach (var note in notes)
                {
                    Button retrievedNote = Instantiate(notePrefab, menuNoteOverview.transform); 
                    Debug.Log("Created button for note: " + note.id);

                    retrievedNote.onClick.AddListener(() => ClickNote(note.id));

                    retrievedNote.transform.parent = noteField.transform;

                    string formatDate = DateTime.Parse(note.date).ToString("dd-MM-yyyy HH:mm");

                    //retrievedNote.GetComponent<SingleNoteScript>().SetId(note.id);
                    if (retrievedNote.GetComponentInChildren<TMP_Text>() != null)
                    {
                        retrievedNote.GetComponentInChildren<TMP_Text>().text = formatDate;
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

    public void ClickNote(string noteId )
    {
        Debug.Log("Note clicked: " + noteId);
        Note clickedNote = notes.Find(note => note.id == noteId);
        if (clickedNote != null)
        {
            noteRead.SetActive(true);
            menuNoteOverview.SetActive(false);

            noteReadDate.text = clickedNote.date.ToString();
            noteReadText.text = clickedNote.text;

            for(int i = 0; i < lst_NoteMood.Count; i++)
            {
                if (lst_NoteMood[i] != null)
                {
                    if (i == clickedNote.userMood - 1)
                    {
                        lst_NoteMood[i].color = Color.white;
                    }
                    else
                    {
                        lst_NoteMood[i].color = Color.gray;
                    }
                }
            }

        }
    }

    public void HoverOverNote()
    {
        notePrefab.GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 1);
    }

    public void HoverExitNote()
    {
        notePrefab.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    public void CloseOpenedNote()
    {
        noteRead.SetActive(false);
        menuNoteOverview.SetActive(true);
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

        int random = UnityEngine.Random.Range(0, motivationTexts.Count);

        lst_InputFields[1].placeholder.GetComponent<TMP_Text>().text = motivationTexts[random];
    }

    public void CloseNoteCreator()
    {
        menuNoteCreator.SetActive(false);
        menuNoteOverview.SetActive(true);
    }

    public void CreateNote()
    {
        Guid newGuid = Guid.NewGuid();
        Debug.Log("Clicked create note button");
        string newDate = DateTime.Now.ToString("yyyy-MM-dd") +"T" + DateTime.Now.ToString("HH:mm:ss");

        if (NewNoteValidation())
        {
            newNote = new Note()
            {
                id = newGuid.ToString(),
                text = lst_InputFields[1].text.Trim(),
                userMood = moodScale,
                date = newDate,
                parentGuardianId = ApiClientHolder.ParentGuardianId,
                patientId = ApiClientHolder.Patient.id
            };

            CreateNewNote();
        }
        else
        {
            Debug.Log("Note is not valid");
        }
    }

    public bool NewNoteValidation()
    {

        if(string.IsNullOrWhiteSpace(lst_InputFields[1].text))
        {
            txbErrorNoteCreator.text = "Voer een notitie in";
            Debug.Log("Note is empty");
            return false;
        }
        else if (moodScale == 0)
        {
            txbErrorNoteCreator.text = "Selecteer een stemming";
            Debug.Log("Mood is not selected");
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
                CloseNoteCreator();

                ClearNotes();
                LoadNotes();

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

    public void HoverOverParrot()
    {
        Image parrotImage = parrot.GetComponent<Image>();
        parrotImage.color = new Color(0.75f, 0.75f, 0.75f, 1);
    }

    public void HoverExitParrot()
    {
        Image parrotImage = parrot.GetComponent<Image>();
        parrotImage.color = new Color(1, 1, 1, 1);
    }
}
