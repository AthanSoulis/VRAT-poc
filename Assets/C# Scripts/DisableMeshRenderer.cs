/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMeshRenderer : MonoBehaviour {

    MeshRenderer meshRenderer;
    Collider positionCollider;

    GameObject player;

	// Use this for initialization
	void Start () {
        
        meshRenderer = GetComponent<MeshRenderer>();
        positionCollider = GetComponent<Collider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (this.transform.position.x == player.transform.position.x &&
            this.transform.position.z == player.transform.position.z)
        { 
            meshRenderer.enabled = false;
            positionCollider.enabled = false;
        }
        else
        {
            meshRenderer.enabled = true;
            positionCollider.enabled = true;
        }
            
    }


}
