/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadePanel : MonoBehaviour {

    [SerializeField] private Image fadePanelImage;

    public static FadePanel Instance;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        //Initialize Tweening Engine
        DOTween.Init(false, true, LogBehaviour.Verbose);
    }

    public Tweener PanelFade(float alpha, float fadeTime)
    {
        Tweener fadeTween = fadePanelImage.DOFade(alpha, fadeTime);
        return fadeTween;
    }


}
