using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkorScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float MoveYValue = 0.5f;
    [SerializeField] private float AnimationTime = 1f;


    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveY(MoveYValue, AnimationTime).OnComplete(DestroyObject);
        SkorManager.Instance.ScoreUp();
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
