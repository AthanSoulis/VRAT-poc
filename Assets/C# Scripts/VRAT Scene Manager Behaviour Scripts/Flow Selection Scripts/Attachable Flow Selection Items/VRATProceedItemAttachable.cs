/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRATProceedItemAttachable : MonoBehaviour, VRATProceedItem
{

    private EventTrigger proceedItemTrigger;
    [SerializeField] private Material outlineMaterial;
    private VRATSceneManager VRATsm;
    private MeshRenderer goMeshRenderer;

    // Use this for initialization
    void Start()
    {

        if (outlineMaterial == null)
            outlineMaterial = Resources.Load("Materials/Outline") as Material;

        VRATsm = VRATSceneManager.Instance;

        //Add the EventTrigger for the OnClick, OnEnter, OnExit functionality
        proceedItemTrigger = gameObject.AddComponent<EventTrigger>();

        goMeshRenderer = gameObject.GetComponent<MeshRenderer>();

    }

    public void EnableFernwehProceedItem()
    {
        if (outlineMaterial == null)
            return;

        Material[] meshRendMaterials = goMeshRenderer.materials;
        Material[] newMeshRendMaterials = new Material[meshRendMaterials.LongLength + 1];

        for (int i = 0; i < meshRendMaterials.LongLength; i++)
            newMeshRendMaterials[i] = meshRendMaterials[i];
        newMeshRendMaterials[meshRendMaterials.LongLength] = outlineMaterial;

        goMeshRenderer.materials = newMeshRendMaterials;
        Debug.Log("Added Outline");

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

    public void DisableFernwehProceedItem()
    {
        if (outlineMaterial == null)
            return;

        Material[] meshRendMaterials = goMeshRenderer.materials;
        Material[] newMeshRendMaterials = new Material[meshRendMaterials.LongLength - 1];

        for (int i = 0; i < newMeshRendMaterials.LongLength; i++)
            newMeshRendMaterials[i] = meshRendMaterials[i];

        goMeshRenderer.materials = newMeshRendMaterials;
        Debug.Log("Removed Outline");

        Destroy(proceedItemTrigger);
    }

    public void OnPointerClick()
    {
        Debug.Log("FernwehProceedItem : OnPointerClick Called");

        if (!VRATsm)
        { Debug.Log("FernwehProceedItem : Cannot find FernwehSceneManager"); return; }

        VRATsm.VRATOnClickProceed();

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
