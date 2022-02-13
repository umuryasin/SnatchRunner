using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BilboardScript : MonoBehaviour
{
    [SerializeField] private Transform lookAtCamera;
    [SerializeField] private bool ActivateFadeOut = false;
    [SerializeField] private float FadeValue = 0.0f;
    [SerializeField] private float FadeTime = 2f;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (ActivateFadeOut)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.DOFade(FadeValue, FadeTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(lookAtCamera);
    }
}
