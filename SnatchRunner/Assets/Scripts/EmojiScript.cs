using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EmojiScript : MonoBehaviour
{
    [SerializeField] private Transform lookAtCamera;
    [SerializeField] private float MoveYValue = -1f;
    [SerializeField] private float FadeValue = 0.0f;
    [SerializeField] private float AnimationTime = 1f;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.DOFade(FadeValue, AnimationTime);
        transform.DOLocalMoveY(MoveYValue, AnimationTime).OnComplete(DestroyObject);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(lookAtCamera);

    }
    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
