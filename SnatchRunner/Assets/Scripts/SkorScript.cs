using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkorScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float MoveYValue = 0.5f;
    [SerializeField] private float AnimationTime = 1f;
    [SerializeField] private int AmountOfMoney = 5; /// amount of score 

    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveY(MoveYValue, AnimationTime).OnComplete(DestroyObject);
        SkorManager.Instance.ScoreUp(AmountOfMoney);
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
