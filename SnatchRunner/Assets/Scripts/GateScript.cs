using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem Confeti1;
    [SerializeField] private ParticleSystem Confeti2;

    // Start is called before the first frame update
    void Start()
    {
        Confeti1.Stop();
        Confeti2.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Confeti1.Play();
            Confeti2.Play();
        }
    }

}
