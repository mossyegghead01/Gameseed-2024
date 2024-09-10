using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testbutton : MonoBehaviour
{
    public GameObject obelisk;
    // Start is called before the first frame update
    void Start()
    {
        // Get the Button component
        UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();

        // Add a listener for the click event
        button.onClick.AddListener(OnButtonClick);
    }

    // Method to be called when the button is clicked
    void OnButtonClick()
    {
        obelisk.GetComponent<obeliskScript>().SetStage(obelisk.GetComponent<obeliskScript>().GetStage() + 1);
    }

}
