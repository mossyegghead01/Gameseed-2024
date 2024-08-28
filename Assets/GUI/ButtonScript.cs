using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{

    public SlotState slotState;
    public GameObject gridManagerObj;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        gridManagerObj.GetComponent<GridManager>().GetGrid().GetBuildInventory().AddSlot(slotState);

    }

}
