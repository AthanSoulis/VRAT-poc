/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRATGoOnItem : MonoBehaviour {

    private EventTrigger goOnItemTrigger;
    VRATSceneManager VRATsm;

    // Use this for initialization
    void Start()
    {
        VRATsm = VRATSceneManager.Instance;

        //Add the EventTrigger and the OnClick, OnEnter, OnExit functionality
        goOnItemTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
        pointerClickEntry.eventID = EventTriggerType.PointerClick;
        pointerClickEntry.callback.AddListener((eventData) => { OnClickLoadGoOnPath(); });
        goOnItemTrigger.triggers.Add(pointerClickEntry);

        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((eventData) => { OnPointerEnter(); });
        goOnItemTrigger.triggers.Add(pointerEnterEntry);

        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((eventData) => { OnPointerExit(); });
        goOnItemTrigger.triggers.Add(pointerExitEntry);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        Destroy(goOnItemTrigger);
    }

    public void OnClickLoadGoOnPath()
    {
        VRATsm.VRATtraverseGoOnSelection();
    }

    public void OnPointerEnter()
    {
        Debug.Log("GoOn In");
    }

    public void OnPointerExit()
    {
        Debug.Log("GoOn Out");
    }

}
