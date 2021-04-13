/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableParticles : MonoBehaviour {

    ParticleSystem positionParticleSystem;
    Collider positionCollider;

    GameObject player;
    
    // Use this for initialization
    void Start () {

        
        positionCollider = GetComponent<Collider>();
        positionParticleSystem = GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player");

    }
	
	// Update is called once per frame
	void Update () {

        var emmissionModule = this.positionParticleSystem.emission;

        if (this.transform.position.x == player.transform.position.x &&
            this.transform.position.z == player.transform.position.z)
        {
            positionCollider.enabled = false;

            emmissionModule.enabled = false;
        }
        else
        {
            positionCollider.enabled = true;

            emmissionModule.enabled = true;
        }

    }

}
