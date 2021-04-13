/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRATBPSelectionsInstantiator : MonoBehaviour {
  
    [SerializeField] private GameObject DefaultGoOnPrefab;
    [SerializeField] private GameObject DefaultProceedPrefab;
    [SerializeField] private GameObject DefaultMoreSelectionPrefab;
    [SerializeField] private GameObject DefaultChoiceSelectionPrefab;

    List<GameObject> BPSelectionGOList;
    GameObject proceedSelectionGO;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        BPSelectionGOList = new List<GameObject>();

        //Keeps VRATBPSelectionsInstantiator alive between scene changes
        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator clearBranchingPointSelectionGameObjects()
    {
        foreach (GameObject go in BPSelectionGOList)
            Destroy(go);

        int count = BPSelectionGOList.Count;
        BPSelectionGOList.RemoveRange(0, count);

        Debug.Log(count+" Branching Point Selection Items Destroyed!");
        yield return null;
    }

    public IEnumerator clearProceedSelectionGameObject()
    {   
        if(proceedSelectionGO == null)
            Debug.Log("Proceed Selection Item null before Destruction call!");
        Destroy(proceedSelectionGO);

        if (proceedSelectionGO != null) { 
            Debug.Log("Proceed Selection Item not null after Destruction call! Scene: "+ proceedSelectionGO.scene);
        }
        Debug.Log("Proceed Selection Item Destroyed!");
        yield return null;
    }

    //TODO Transform should be a field into the FNode

    public GameObject instantiateGoOnBPSelection(Vector3 position)
    {
        return this.instantiateGoOnBPSelection(position, DefaultGoOnPrefab);
    }
    public GameObject instantiateGoOnBPSelection(Vector3 position, GameObject GoOnPrefab)
    {

        GameObject instantiatedGO = Instantiate(GoOnPrefab);
        instantiatedGO.transform.SetParent(null);
        instantiatedGO.transform.position = position;
        BPSelectionGOList.Add(instantiatedGO);

        return instantiatedGO;
    }

    public GameObject instantiateProceedSelection(Vector3 position)
    {
        return this.instantiateProceedSelection(position, DefaultProceedPrefab);
    }

    public GameObject instantiateProceedSelection(Vector3 position, GameObject ProceedPrefab)
    {
        proceedSelectionGO = Instantiate(ProceedPrefab);
        proceedSelectionGO.transform.SetParent(null);
        proceedSelectionGO.transform.position = position;

        return proceedSelectionGO;
    }

    public GameObject instantiateMoreBPSelection(Vector3 position, VRATNode BPSelectionNode)
    {
        return this.instantiateMoreBPSelection(position, BPSelectionNode, DefaultMoreSelectionPrefab);
    }

    public GameObject instantiateMoreBPSelection(Vector3 position, VRATNode BPSelectionNode, GameObject MoreSelectionPrefab)
    {
        GameObject instantiatedGO = Instantiate(MoreSelectionPrefab);
        instantiatedGO.transform.SetParent(null);
        instantiatedGO.transform.position = position;
        BPSelectionGOList.Add(instantiatedGO);

        VRATMoreItem fmi = instantiatedGO.GetComponent<VRATMoreItem>();
        fmi.RepresentedNode = BPSelectionNode;

        return instantiatedGO;
    }

    public GameObject instantiateChoiceBPSelection(Vector3 position, VRATNode BPSelectionNode)
    {
        return this.instantiateChoiceBPSelection(position, BPSelectionNode, DefaultChoiceSelectionPrefab);
    }

    public GameObject instantiateChoiceBPSelection(Vector3 position, VRATNode BPSelectionNode, GameObject ChoiceSelectionPrefab)
    {
        GameObject instantiatedGO = Instantiate(ChoiceSelectionPrefab);
        instantiatedGO.transform.SetParent(null);
        instantiatedGO.transform.position = position;
        BPSelectionGOList.Add(instantiatedGO);

        VRATChoiceItem fci = instantiatedGO.GetComponent<VRATChoiceItem>();
        fci.RepresentedNode = BPSelectionNode;

        return instantiatedGO;
    }


}
