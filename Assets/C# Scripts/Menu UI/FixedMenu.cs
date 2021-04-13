using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMenu : MonoBehaviour {

    private Transform menuTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float angleSlackLimit;

    float previousY;

    private void Awake()
    {

        previousY = cameraTransform.eulerAngles.y;
    }
    private void Start()
    {
        menuTransform.RotateAround(playerTransform.transform.position, Vector3.up, cameraTransform.eulerAngles.y - previousY);
        previousY = cameraTransform.eulerAngles.y;
    }

    
    private void OnEnable()
    {
        menuTransform = GetComponent<Transform>();

        menuTransform.RotateAround(playerTransform.transform.position, Vector3.up, cameraTransform.eulerAngles.y - previousY);
        previousY = cameraTransform.eulerAngles.y;
    }
    

    private void OnDisable()
    {
        previousY = cameraTransform.eulerAngles.y;
    }

    void Update(){
        Vector3 cameraEulerAngles = cameraTransform.eulerAngles;
        //float cameraYaw = Quaternion.AngleAxis(cameraEulerAngles.y, Vector3.up).eulerAngles.y; ;

        float cameraY = cameraEulerAngles.y;

        if (Mathf.Abs(cameraY - menuTransform.eulerAngles.y) > angleSlackLimit)
            menuTransform.RotateAround(playerTransform.transform.position, Vector3.up, cameraY - previousY);
        previousY = cameraY;

    }
}
