/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRATAudioManager : MonoBehaviour {


    [SerializeField] private GameObject VRATAudioPlayerPrefab;
    [SerializeField] private Vector3 VRATAudioManagerPositionVector;

    List<GameObject> VRATAudioPlayerList;
    Queue<VRATQuote> quoteQueue;

    string storyDirectoryPath;
    AudioCallback VRATsmCallback;

    public delegate void AudioCallback();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        VRATAudioPlayerList = new List<GameObject>();
        quoteQueue = new Queue<VRATQuote>();
       

        //Keeps VRATAudioManager alive between scene changes
        DontDestroyOnLoad(this.gameObject);
    }

    public void playSingle(VRATAudio audio, string storyDirectoryPath, AudioCallback callback)
    {
        this.storyDirectoryPath = storyDirectoryPath;
        this.VRATsmCallback = callback;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        GameObject instantiatedGO = Instantiate(VRATAudioPlayerPrefab, player.transform);
        instantiatedGO.transform.localPosition = VRATAudioManagerPositionVector;

        if( !instantiatedGO.GetComponent<VRATAudioPlayer>().playAudio(audio, storyDirectoryPath,
            () => {Destroy(instantiatedGO); callback(); })  )
        {
            Destroy(instantiatedGO);
            callback();
        }

    }

    public void playConversation(VRATCharacter[] characters, VRATQuote[] quotes, string storyDirectoryPath , AudioCallback callback)
    {
        this.storyDirectoryPath = storyDirectoryPath;
        this.VRATsmCallback = callback;

        if (VRATAudioPlayerList.Count != 0)
            StartCoroutine(deleteAudioPlayers());

        foreach(VRATCharacter character in characters)
        {
            GameObject instantiatedGO;

            //instantiatedGO.transform.SetParent(null);
            //Get the transform from the json
            Vector3 positionVector, rotationVector, scaleVector;
            
            //If there is no transform put the audio source item as a child of the player GO
            if (character.transform == null) {

                GameObject player = GameObject.FindGameObjectWithTag("Player");

                instantiatedGO = Instantiate(VRATAudioPlayerPrefab, player.transform);
                positionVector = VRATAudioManagerPositionVector;

            }
            else {

                instantiatedGO = Instantiate(VRATAudioPlayerPrefab);

                positionVector = new Vector3(character.transform.position[0],
                                                    character.transform.position[1],
                                                    character.transform.position[2]);
                rotationVector = new Vector3(character.transform.rotation[0],
                                                    character.transform.rotation[1],
                                                    character.transform.rotation[2]);
                scaleVector = new Vector3(character.transform.scale[0],
                                                    character.transform.scale[1],
                                                    character.transform.scale[2]);

                instantiatedGO.transform.localScale = scaleVector;
                instantiatedGO.transform.localRotation = Quaternion.Euler(rotationVector);
            }
            //Apply the transform to the object in this order Scale > Rotation > Position
            instantiatedGO.transform.localPosition = positionVector;

            VRATAudioPlayerList.Add(instantiatedGO);
        }

        foreach (VRATQuote quote in quotes)
            quoteQueue.Enqueue(quote);

        VRATQuote startQuote = quoteQueue.Dequeue();
        VRATAudioPlayer characterAudioPlayer = VRATAudioPlayerList[startQuote.character].GetComponent<VRATAudioPlayer>();

        characterAudioPlayer.playAudio(startQuote.audio, storyDirectoryPath, conversationCallback);

    }

    void conversationCallback()
    {
        Debug.Log("Conversation Callback");

        if (quoteQueue.Count != 0)
        {
            VRATQuote nextQuote = quoteQueue.Dequeue();
            VRATAudioPlayer characterAudioPlayer = VRATAudioPlayerList[nextQuote.character].GetComponent<VRATAudioPlayer>();

            if (!characterAudioPlayer.playAudio(nextQuote.audio, storyDirectoryPath, conversationCallback))
                conversationCallback();

            return;
        }

        //Destroy AudioPlayers
        StartCoroutine(deleteAudioPlayers());

        //Call the callback from the VRATSM
        VRATsmCallback();
               
    }

    IEnumerator deleteAudioPlayers()
    {
        
        foreach (GameObject go in VRATAudioPlayerList)
            Destroy(go);

        int count = VRATAudioPlayerList.Count;
        VRATAudioPlayerList.RemoveRange(0, VRATAudioPlayerList.Count);

        Debug.Log(count + " VRAT Audio Players Destroyed!");
        yield return null;
    }
}
