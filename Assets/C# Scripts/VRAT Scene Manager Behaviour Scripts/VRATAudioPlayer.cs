/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VRATAudioPlayer : MonoBehaviour {

    #region Fields

    AudioClip VRATAudioClip = null;
    GvrAudioSource audioSource;

    Queue<VRATAudio> audioQueue;

    #endregion
  

    private IEnumerator AudioEndsCallback(GvrAudioSource audioSource ,VRATAudioManager.AudioCallback callback)
    {
        float time = audioSource.clip.length;
        yield return new WaitForSeconds(time);
        Debug.Log("Stopped Playing!");
        callback();
    }

    public bool playAudio(VRATAudio audio, string fileDirectoryPath) { return this.playAudio(audio, fileDirectoryPath, null); }
    public bool playAudio(VRATAudio audio, string fileDirectoryPath, VRATAudioManager.AudioCallback callback)
    {
        if (audio == null || audio.src == null)
        {
            Debug.Log("Audio NOT Found: " + (audio == null ? "Audio is null" : "No Audio src"));

            return false;
        }

        //Trim the picture suffix from the audioSrc [.mp3]
        string suffix = audio.src.Split('.').Last();
        string audioSrcPath = fileDirectoryPath + audio.src.Substring(0, audio.src.Length - (suffix.Length + 1));

        Debug.Log("Path: " + audioSrcPath);

        //Load audio clip
        VRATAudioClip = Resources.Load<AudioClip>(audioSrcPath);
        if(!VRATAudioClip)
        {
            Debug.Log("Could not load Audio in :"+audioSrcPath);

            return false;
        }

        if (audioSource == null)
            audioSource = GetComponent<GvrAudioSource>();

        if (audioSource.isPlaying)
            this.stopAudio();

        audioSource.clip = VRATAudioClip;

        if (audioSource.clip.loadState == AudioDataLoadState.Loaded)
        {
            audioSource.Play();
            Debug.Log("Playing!");

            if (callback != null)
                StartCoroutine(AudioEndsCallback(audioSource, callback));
        }
        return true;
    }

    public bool isPlaying()
    {
        bool ret = audioSource.isPlaying;
        if (!ret)
            Debug.Log("Stopped Playing!");

        return ret;
    }

    public void pauseAudio()
    {
        audioSource.Pause();
        Debug.Log("Paused :" + audioSource.clip.name);
    }

    public void unpauseAudio()
    {
        audioSource.UnPause();
        Debug.Log("UnPaused :" + audioSource.clip.name);
    }

    public void stopAudio()
    {        
        audioSource.Stop();
        Debug.Log("Stopped :" + audioSource.clip.name);
    }

    public Queue<VRATAudio> AudioQueue
    {
        get { return this.audioQueue; }
        set { this.audioQueue = value ; }
    }

    // Use this for initialization
    void Start()
    {
        audioQueue = new Queue<VRATAudio>();
        audioSource = GetComponent<GvrAudioSource>();
    }


    // Update is called once per frame
    void Update () {
		
	}
}
