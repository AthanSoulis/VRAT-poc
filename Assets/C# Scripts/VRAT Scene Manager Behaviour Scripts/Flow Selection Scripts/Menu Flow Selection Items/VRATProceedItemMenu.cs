/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRATProceedItemMenu : MonoBehaviour, VRATProceedItem
{

    private EventTrigger proceedItemTrigger;
    private VRATSceneManager fsm;

    // Use this for initialization
    void Start () {

        fsm = VRATSceneManager.Instance;

        //Add the EventTrigger and the OnClick, OnEnter, OnExit functionality
        proceedItemTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
        pointerClickEntry.eventID = EventTriggerType.PointerClick;
        pointerClickEntry.callback.AddListener((eventData) => { OnPointerClick(); });
        proceedItemTrigger.triggers.Add(pointerClickEntry);

        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((eventData) => { OnPointerEnter(); });
        proceedItemTrigger.triggers.Add(pointerEnterEntry);

        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((eventData) => { OnPointerExit(); });
        proceedItemTrigger.triggers.Add(pointerExitEntry);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EnableVRATProceedItem()
    {
        gameObject.SetActive(true);
    }

    public void DisableVRATProceedItem()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Destroy(proceedItemTrigger);
    }

    public void OnPointerClick()
    {
        Debug.Log("VRATProceedItem : OnPointerClick Called");


        if (!fsm)
        { Debug.Log("VRATProceedItem : Cannot find VRATSceneManager"); return; }

        fsm.VRATOnClickProceed();

    }

    public void OnPointerEnter()
    {
        Debug.Log("Proceed In");
    }

    public void OnPointerExit()
    {
        Debug.Log("Proceed Out");
    }
}
