/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerToPosition : MonoBehaviour {

    GameObject player;
    [SerializeField] float colliderHeightFromGround;
    [SerializeField] float playerHeight;

    // Use this for initialization
    void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickMoveCameraToPosition()
    {
        Vector3 movePosition = this.transform.position;
        movePosition.y = movePosition.y - colliderHeightFromGround + playerHeight;
        

        this.player.GetComponent<Transform>().position = movePosition;

    }
}
