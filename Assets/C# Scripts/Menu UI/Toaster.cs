using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toaster : MonoBehaviour {

    public static Toaster Instance;

    
    public TextMesh textMesh;
    public MeshRenderer backgroundMesh;

    [SerializeField] private float m_FadeDuration = 0.5f;       // How long it takes for the Toast to appear and disappear.
    private float m_CurrentAlpha;                               // The alpha the Toast currently has.
    private float m_TargetAlpha;                                // The alpha the Toast is fading towards.
    private float m_FadeSpeed;                                  // How much the alpha should change per second (calculated from the fade duration).

    // Use this for initialization
    void Start () {
        Instance = this;

        m_FadeSpeed = 1f / m_FadeDuration;

        Color c = textMesh.color;
        c.a = 0;
        textMesh.color = c;

        c = backgroundMesh.material.color;
        c.a = 0;
        backgroundMesh.material.color = c;

    }
	
	// Update is called once per frame
	void Update () {

        m_CurrentAlpha = Mathf.MoveTowards(m_CurrentAlpha, m_TargetAlpha, m_FadeSpeed * Time.deltaTime);

        Color c = textMesh.color;
        c.a = m_CurrentAlpha;
        textMesh.color = c;
        c = backgroundMesh.material.color;
        c.a = m_CurrentAlpha;
        backgroundMesh.material.color = c;

    }

    public void ShowToast()
    {
        m_TargetAlpha = 1f;
    }

    public void HideToast()
    {
        m_TargetAlpha = 0f;
    }
}
