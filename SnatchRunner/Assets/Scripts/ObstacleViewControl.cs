using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleViewControl : MonoBehaviour
{

    [SerializeField] private GameObject gameObject;

    private CapsuleCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log(" view trigger");
            collider.isTrigger = false;
        }
    }

}
