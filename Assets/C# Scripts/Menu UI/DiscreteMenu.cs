using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteMenu : MonoBehaviour {
    
    [SerializeField] private float m_FadeDuration = 0.5f;       // How long it takes for the menu to appear and disappear.
    [SerializeField] private float m_ShowAngle = 15f;           // How close from the desired facing direction the player must be facing for the menu to appear.
    [SerializeField] private float alertInterval = 10f;         // How long it takes for the look down alert to appear.
    [SerializeField] private Transform menuTransform;           // Indicates the direction of the menu
    [SerializeField] private Transform cameraTransform;         // Reference to the camera Transform to determine which way the player is facing.
    [SerializeField] MenuItem[] menuItemList;                   // The Menu Items the menu is populated with.
    

    private float m_CurrentAlpha;                               // The alpha the menu currently has.
    private float m_TargetAlpha;                                // The alpha the menu is fading towards.
    private float m_FadeSpeed;                                  // How much the alpha should change per second (calculated from the fade duration).
    private Vector3 flatCamDown;

    public bool lookedDown;
    public float alertTimePassed;

    // Use this for initialization
    void Start () {

        m_FadeSpeed = 1f / m_FadeDuration;

        flatCamDown = Vector3.ProjectOnPlane(-cameraTransform.up, Vector3.right).normalized;
    }

    private void OnEnable()
    {
        resetLookDownTimer();
    }

    public void resetLookDownTimer()
    {
        Debug.Log("Timer reset");

        lookedDown = false;
        alertTimePassed = alertInterval;
    }

    // Update is called once per frame
    void Update () {

        if (menuTransform)
        {
            alertTimePassed -= Time.deltaTime;

            // The vector in which the player should be facing is the direction of the menu transform.
            Vector3 directionVector = menuTransform.position - cameraTransform.position;

            Vector3 desiredForward = Vector3.ProjectOnPlane(directionVector, Vector3.right).normalized;
            //Debug.DrawRay(cameraTransform.position, desiredForward, Color.green);

            // The forward vector of the camera as it would be on a flat plane.
            Vector3 flatCamForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.right).normalized;
            //Debug.DrawRay(cameraTransform.position, flatCamForward, Color.red);

            //Debug.DrawRay(cameraTransform.position, flatCamDown, Color.yellow);

            Vector3 flatCamForwardUP = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            //Debug.DrawRay(cameraTransform.position, flatCamForwardUP, Color.red);

            Vector3 menuForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            //Debug.DrawRay(cameraTransform.position, menuForward, Color.blue);

            // The difference angle between the desired facing and the current facing of the player.
            float desiredAngleDelta = Vector3.Angle(desiredForward, flatCamForward);

            float desiredAngleDeltaDown = Vector3.Angle(flatCamForward, flatCamDown);

            // If the difference is smaller than the angle at which the menu is shown, target alpha is one otherwise it is zero.
            if (desiredAngleDelta < m_ShowAngle || desiredAngleDeltaDown < m_ShowAngle) { 
                m_TargetAlpha =  1f;
                lookedDown = true;
            }
            else
                m_TargetAlpha = 0f;
            //Debug.Log("AngleDelta: "+desiredAngleDelta + " AngleDeltaDown: "+desiredAngleDeltaDown);

                // Increment the current alpha value towards the now chosen target alpha and the calculated speed.
            m_CurrentAlpha = Mathf.MoveTowards(m_CurrentAlpha, m_TargetAlpha, m_FadeSpeed * Time.deltaTime);

            for(int i=0; i < menuItemList.Length; i++)
            {
                Color textColor = menuItemList[i].textMesh.color;
                textColor.a = m_CurrentAlpha;
                menuItemList[i].textMesh.color = textColor;

                Color planeColor = menuItemList[i].planeMesh.material.GetColor("_TintColor");
                planeColor.a = m_CurrentAlpha;
                menuItemList[i].planeMesh.material.SetColor("_TintColor", planeColor);

            }

        }

	}

    private void LateUpdate()
    {

        if (alertTimePassed < 0 && !lookedDown)
        {   
            //Debug.Log("Look Down !");
            //Issue a lookDown alert
            Toaster.Instance.ShowToast();
        }
        if ( lookedDown)
            Toaster.Instance.HideToast();

    }

    [System.Serializable]
    public class MenuItem
    {
        public Transform textTransform;
        public TextMesh textMesh;
        public Transform planeTransform;
        public MeshRenderer planeMesh;

    }

}


