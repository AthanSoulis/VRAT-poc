/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FullSerializer;
using DG.Tweening;

public class VRATSceneManager : MonoBehaviour {

    #region Fields

    public static VRATSceneManager Instance;

    public string VRATStoriesFilePath = "VRATStories/";
    public string fileDirectoryPath ;
    public string storyPath;

    [SerializeField] private FadePanel fadePanel;
    [SerializeField] private float fadeTime;

    protected VRATNode story;

    protected VRATImagePlacer VRATimagePlacer;

    protected Stack<Queue<VRATNode>> VRATExecutionStack;
    protected Queue<VRATNode> VRATExecutionQueue;
    protected VRATNode currentNode;

    protected bool moreList;
    protected bool choiceList;

    // We dont need this after we implemented the menu
    //VRATBPSelectionsInstantiator fbpsi;

    [SerializeField] VRATAudioManager VRATAudioManager;

    [SerializeField] GameObject menu;
    VRATProceedItemMenu menuNext;
    GameObject menuOptions;
    GameObject menuOptionsList;

    #endregion

    // This is called once on the first time the VRATSceneManager is created.(Scene0)
    // Initialize the Story Graph
    private void Awake()
    {

        Instance = this;       

        Debug.Log("Story Path : " + storyPath);
        TextAsset storyTextAsset = Resources.Load<TextAsset>(storyPath);
       
        if(storyTextAsset == null)
            Debug.Log("Story is null");

        story = createStory(storyTextAsset.text);

        story.ToString();

        VRATimagePlacer = null;

        //fbpsi = null;
        moreList = false;
        choiceList = false;

        VRATExecutionStack = new Stack<Queue<VRATNode>>();
        Queue<VRATNode> scenesQueue = returnQueueFromNodesArray(story.nodes[0].nodes[0]);
        VRATExecutionStack.Push(scenesQueue);

        //Initialize Tweening Engine
        //DOTween.Init(false, true, LogBehaviour.Verbose);

        SceneManager.sceneLoaded += OnSceneLoaded;

        //Keeps VRAT Scene Manager alive between scene changes
        DontDestroyOnLoad(this.gameObject);
    }

    // This method is called when a scene is loaded. We find the necessary objects that we
    // need to populate with such as images and audio and start traversing through the Story
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded");

        if(VRATAudioManager == null)
            VRATAudioManager = FindObjectOfType<VRATAudioManager>();
        if (fadePanel == null)
            fadePanel = FindObjectOfType<FadePanel>(); //FadePanel.Instance;

        //fbpsi = FindObjectOfType<VRATBPSelectionsInstantiator>();

        //We dont really care if there is a VRATImagePlacer, so if there is not one in the scene we dont create one.
        VRATimagePlacer = FindObjectOfType<VRATImagePlacer>();

        if (this.menu == null)
            this.menu = FindObjectOfType<FixedMenu>().gameObject;

        this.menu = this.menu.gameObject;
        menuNext = this.menu.GetComponent<MenuContents>().menuNext;
        menuOptions = this.menu.GetComponent<MenuContents>().menuOptions;
        menuOptionsList = this.menu.GetComponent<MenuContents>().menuList;
            
        this.menuNext.DisableVRATProceedItem();
        this.menuOptions.SetActive(false);
        this.menuOptionsList.SetActive(false);
        this.menu.SetActive(false);

        
        fadePanel.PanelFade(0f, fadeTime).OnStart<Tweener>(() => { Debug.Log("Fade In"); VRATtraverseStory(); });

    }

    void VRATtraverseStory()
    {
        Debug.Log("VRATTraverseStory()");

        if (VRATExecutionStack.Count != 0)
        {
            Debug.Log("VRATExecutionStack : " + VRATExecutionStack.Count);
            VRATExecutionQueue = VRATExecutionStack.Peek();

            if (VRATExecutionQueue.Count != 0)
            {
                Debug.Log("VRATExecutionQueue : " + VRATExecutionQueue.Count);
                currentNode = VRATExecutionQueue.Peek();

                VRATtraverseNode(currentNode);
            }
            else
            {
                Debug.Log("VRATExecutionStack : Poping Stack");
                VRATExecutionStack.Pop();

                Debug.Log("VRATExecutionQueue : Dequeing ");
                if(VRATExecutionStack.Count == 0)
                {
                    Debug.Log("Application Quit");
                    Application.Quit();
                }
                VRATExecutionQueue = VRATExecutionStack.Peek();                

                //This is an exception on the dequeing of the execution queue as 
                //we want to revisit the execution queue in the More Branching Point
                if (VRATExecutionQueue.Peek().type != "GoOn" && VRATExecutionQueue.Peek().type != "Branch")
                    VRATExecutionQueue.Dequeue();


                Debug.Log("VRATExecutionStack : " + VRATExecutionStack.Count + "\nVRATExecutionQueue : " + VRATExecutionStack.Peek().Count);
                VRATtraverseStory();
            }
        }        
    }

    void VRATtraverseNode(VRATNode node)
    {
        if (node.type == "Scene")
            VRATtraverseScene(node);
        else if (node.type == "More")
            VRATtraverseMore(node);
        else if (node.type == "Choice")
            VRATtraverseChoice(node);
        else if (node.type == "Page")
            VRATtraversePage(node);
        //This will only happen with a More Branching Point since with
        //a Choice BP the branches are not being pushed on the stack
        else if (node.type == "GoOn" || node.type == "Branch")
            showMoreBPChoices(node);
    }

    void VRATtraverseMore(VRATNode node)
    {
        Debug.Log("Traversing " + node.type + " | Id: " + node.id);

        Queue<VRATNode> moreBPselectionsQueue = returnQueueFromNodesArray(node);
        VRATExecutionStack.Push(moreBPselectionsQueue);

        if (VRATimagePlacer)
            VRATimagePlacer.placeImage(node.data.image, fileDirectoryPath);

        VRATAudioManager.playSingle(node.data.audio, fileDirectoryPath, () => { showMoreBPChoices(node); });

    }


    //Renders the Branching Point Selections for the More Node
    void showMoreBPChoices(VRATNode node)
    {
        moreList = true;
        choiceList = false;
        Queue<VRATNode> moreBPselectionsQueue = VRATExecutionStack.Peek();

        ListBank.Instance.contents = moreBPselectionsQueue.ToArray();

        this.menu.SetActive(true);
        this.menuOptions.SetActive(true);
        //this.menu.GetComponent<DiscreteMenu>().resetLookDownTimer();

        TextMesh textMesh = this.menuOptions.GetComponentInChildren<TextMesh>();
        if(textMesh)
            textMesh.text = node.data.showTitle && node.data.prompt != null ? node.data.prompt : "Open Choices";

    }

    public void VRATtraverseBranchSelection(VRATNode nodeBranch)
    {
        if (moreList)
            VRATtraverseMoreSelection(nodeBranch);
        if (choiceList)
            VRATtraverseChoiceSelection(nodeBranch);
    }

    public void VRATtraverseMoreSelection(VRATNode nodeBranch)
    {
        Debug.Log("Traversing "+ nodeBranch.type+" | Id: " + nodeBranch.id);

        //Clearing the BranchingPoint node selections
        //StartCoroutine(fbpsi.clearBranchingPointSelectionGameObjects());

        //(!!! Not a good practice. Suggestions?) 
        //Removing the Branch we are about to traverse from the stack

        VRATExecutionQueue = VRATExecutionStack.Peek();
        Queue<VRATNode> retQueue = new Queue<VRATNode>();

        foreach (VRATNode node in VRATExecutionQueue)
        {
            if (node.id != nodeBranch.id)
                retQueue.Enqueue(node);
        }

        Debug.Log("VRATExecutionQueue : " + VRATExecutionStack.Peek().Count);

        VRATExecutionStack.Pop();
        VRATExecutionStack.Push(retQueue);
        Debug.Log("Removed Selected Branch! \nVRATExecutionQueue : " + VRATExecutionStack.Peek().Count);

        //Push the nodes of the branch in the VRATExecutionStack
        Queue<VRATNode> branchNodeQueue = returnQueueFromNodesArray(nodeBranch);
        VRATExecutionStack.Push(branchNodeQueue);

        menuOptions.SetActive(false);
        menuOptionsList.SetActive(false);

        VRATtraverseStory();
    }

    public void VRATtraverseGoOnSelection()
    {
        Debug.Log("Traversing GoOn");
        //Clearing the More node selections
        //StartCoroutine(fbpsi.clearBranchingPointSelectionGameObjects());

        //Remove the branches of the More node from the VRATExecutionStack
        VRATExecutionStack.Pop();
        //Remove the More node from the VRATExecutionQueue
        VRATExecutionQueue = VRATExecutionStack.Peek();
        VRATExecutionQueue.Dequeue();

        Debug.Log("VRATExecutionStack : " + VRATExecutionStack.Count + "\nVRATExecutionQueue : " + VRATExecutionStack.Peek().Count);

        menuOptions.SetActive(false);
        menuOptionsList.SetActive(false);

        VRATtraverseStory();
    }

    void VRATtraverseChoice(VRATNode node)
    {
        Debug.Log("Traversing " + node.type + " | Id: " + node.id);   

        if (VRATimagePlacer)
            VRATimagePlacer.placeImage(node.data.image, fileDirectoryPath);

        VRATAudioManager.playSingle(node.data.audio, fileDirectoryPath, () => { showChoiceBPChoices(node); });
    }

    public void VRATtraverseChoiceSelection(VRATNode node)
    {
        Debug.Log("Traversing " + node.type + " | Id: " + node.id);

        //Clearing the BranchingPoint node selections
        //StartCoroutine(fbpsi.clearBranchingPointSelectionGameObjects());

        Queue<VRATNode> choiceBPselectionsQueue = returnQueueFromNodesArray(node);
        VRATExecutionStack.Push(choiceBPselectionsQueue);
                
        menuOptions.SetActive(false);
        menuOptionsList.SetActive(false);

        VRATtraverseStory();
    }

    
    //Renders the Branching Point Selections for the Choice Node
    void showChoiceBPChoices(VRATNode node)
    {
        moreList = false;
        choiceList = true;
        Queue<VRATNode> choiceBPselectionsQueue = returnQueueFromNodesArray(node);
        ListBank.Instance.contents = choiceBPselectionsQueue.ToArray();


        this.menu.SetActive(true);
        this.menuOptions.SetActive(true);
        //this.menu.GetComponent<DiscreteMenu>().resetLookDownTimer();

        TextMesh textMesh = this.menuOptions.GetComponentInChildren<TextMesh>();
        if (textMesh)
            textMesh.text = node.data.showTitle && node.data.prompt!= null ? node.data.prompt : "Open Choices";
    }

    void VRATtraverseScene(VRATNode node)
    {
        Debug.Log("Traversing " + node.type + " | Id: " + node.id);

        Queue<VRATNode> pageQueue = returnQueueFromNodesArray(node.nodes[0]);
        VRATExecutionStack.Push(pageQueue);

        Debug.Log("Loading Scene: " + node.data.unityScene);

        if (SceneManager.GetActiveScene().name != node.data.unityScene)
            fadePanel.PanelFade(1f, fadeTime).OnComplete<Tweener>(() => { Debug.Log("Fade Out"); SceneManager.LoadScene(node.data.unityScene);});
        else
            VRATtraverseStory();

    }

    void VRATtraversePage(VRATNode node)
    {
        Debug.Log("Traversing " + node.type + " | Id: " + node.id);

        //Getting Page template
        string pageTemplate = node.data.template;

        if (pageTemplate == "simple")
            VRATtraversePageSimple(node);
        else if (pageTemplate == "conversation")
            VRATtraversePageConversation(node);
    }

    void VRATtraversePageSimple(VRATNode node)
    {
        Debug.Log("Page Template: " + node.data.template);

        if(VRATimagePlacer)
            VRATimagePlacer.placeImage(node.data.image, fileDirectoryPath);

        VRATAudioManager.playSingle(node.data.audio, fileDirectoryPath, 
                                    ()=> { if (!node.autoProceed)
                                            VRATenableProceedTarget(node);
                                        else
                                            this.VRATOnClickProceed();
                                    });
    }

/*
    void VRATsimplePageCallback()
    {
        Debug.Log("Simple Page Callback");
        //This enables a click target that lets the user
        // go to the next part of the executionQueue
        bool ret = VRATenableProceedTarget();

        Debug.Log("Proceed Enabled: " + ret);
    }
*/

    void VRATtraversePageConversation(VRATNode node)
    {
        Debug.Log("Page Template: " + node.data.template);
        if (VRATimagePlacer)
            VRATimagePlacer.placeImage(node.data.image, fileDirectoryPath);

        VRATAudioManager.playConversation(node.data.characters, node.data.quotes, fileDirectoryPath,
                                                        () => {
                                                            if (!node.autoProceed)
                                                                VRATenableProceedTarget(node);
                                                            else
                                                                this.VRATOnClickProceed();
                                                        });
    }

    //Reveals the Item that lets the user proceed with the story when he clicks on it
    public void VRATenableProceedTarget(VRATNode node)
    {

        this.menu.SetActive(true);
        this.menuNext.EnableVRATProceedItem();
        //this.menu.GetComponent<DiscreteMenu>().resetLookDownTimer();

        Debug.Log("Proceed Enabled ");
    }

    //Callback method called by the VRATProceedItem
    public void VRATOnClickProceed()
    {
        //Low-prio TODO : Clean the AudioPlayer of any clips

        // Destroy the ProceedSelection GO
        //StartCoroutine(fbpsi.clearProceedSelectionGameObject());

        this.menuNext.DisableVRATProceedItem();
        this.menu.SetActive(false);

        VRATExecutionQueue = VRATExecutionStack.Peek();        
        VRATExecutionQueue.Dequeue();

        Debug.Log("VRATExecutionStack : " + VRATExecutionStack.Count +"\nVRATExecutionQueue : " + VRATExecutionStack.Peek().Count);

        VRATtraverseStory();
    }

    Queue<VRATNode> returnQueueFromNodesArray(VRATNode node)
    {
        Queue<VRATNode> ret = new Queue<VRATNode>();
        foreach(VRATNode childNode in node.nodes)
        {
            if (childNode.type == "Scene" 
                || childNode.type == "More"
                || childNode.type == "GoOn"
                || childNode.type == "Branch"
                || childNode.type == "Choice" 
                || childNode.type == "Page")
                ret.Enqueue(childNode);
        }

        return (ret.Count!= 0 ? ret : null);
    }

    // Creates an VRATNode from a given story.json 
    VRATNode createStory(string jsonString)
    {
        fsSerializer _serializer = new fsSerializer();

        // parse the JSON data
        fsData data = fsJsonParser.Parse(jsonString);
        VRATNode story = null;

        _serializer.TryDeserialize<VRATNode>(data, ref story).AssertSuccessWithoutWarnings();

        if (story != null)
            return story;

        Debug.Log("Creating Story Failed");
        return null;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}

