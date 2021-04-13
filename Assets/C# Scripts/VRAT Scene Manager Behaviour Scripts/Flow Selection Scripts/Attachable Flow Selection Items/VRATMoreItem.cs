/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRATMoreItem : MonoBehaviour {

    private EventTrigger moreItemTrigger;
    VRATSceneManager VRATsm;
    VRATNode representedNode;

    // Use this for initialization
    void Start()
    {
        VRATsm = VRATSceneManager.Instance;

        //Add the EventTrigger and the OnClick, OnEnter, OnExit functionality
        moreItemTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
        pointerClickEntry.eventID = EventTriggerType.PointerClick;
        pointerClickEntry.callback.AddListener((eventData) => { OnClickLoadMorePath(); });
        moreItemTrigger.triggers.Add(pointerClickEntry);

        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((eventData) => { OnPointerEnter(); });
        moreItemTrigger.triggers.Add(pointerEnterEntry);

        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((eventData) => { OnPointerExit(); });
        moreItemTrigger.triggers.Add(pointerExitEntry);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        Destroy(moreItemTrigger);
    }

    public void OnClickLoadMorePath()
    {
        VRATsm.VRATtraverseMoreSelection(this.representedNode);
    }

    public void OnPointerEnter()
    {
        Debug.Log("More In");
    }

    public void OnPointerExit()
    {
        Debug.Log("More Out");
    }

    public VRATNode RepresentedNode
    {
        get { return this.representedNode; }
        set { this.representedNode = value; }
    }

}
