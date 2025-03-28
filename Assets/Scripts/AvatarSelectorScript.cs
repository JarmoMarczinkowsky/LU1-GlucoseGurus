using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class AvatarSelectorScript : MonoBehaviour
{
    public List<UnityEngine.UI.Image> ListAvatars;
    public GameObject previewImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //previewImage.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickImage(int selectedImage)
    {
        Debug.Log($"Clicked image: {selectedImage}");

        previewImage.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);

        if (selectedImage <= ListAvatars.Count - 1)
        {
            previewImage.GetComponent<UnityEngine.UI.Image>().sprite = ListAvatars[selectedImage].GetComponent<UnityEngine.UI.Image>().sprite;   
        }
    }
}
