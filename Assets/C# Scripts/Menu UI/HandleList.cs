using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class HandleList : MonoBehaviour {

    [SerializeField] private GameObject List;
    [SerializeField] private TextMesh textMesh;
    private EventTrigger ItemTrigger;
    

    private bool enabledList;

    // Use this for initialization
    void Start () {

        //Add the EventTrigger and the OnClick, OnEnter, OnExit functionality
        ItemTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
        pointerClickEntry.eventID = EventTriggerType.PointerClick;
        pointerClickEntry.callback.AddListener((eventData) => { OnPointerClick(); });
        ItemTrigger.triggers.Add(pointerClickEntry);

    }
	
    void OnPointerClick()
    {
        List.SetActive(!List.activeInHierarchy);
        if (textMesh.text == "Open Choices")
            textMesh.text = "Close Choices";
        else if(textMesh.text == "Close Choices")
            textMesh.text = "Open Choices";
    }


	// Update is called once per frame
	void Update () {
		
	}
}
