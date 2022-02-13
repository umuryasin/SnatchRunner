using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyItemScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float MoveYValue = 0.5f;
    [SerializeField] private float FadeValue = 0.5f;
    [SerializeField] private float AnimationTime = 1f;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            transform.DOLocalMoveY(MoveYValue, AnimationTime).OnComplete(DestroyObject);
            SkorManager.Instance.ScoreUp();
        }
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
