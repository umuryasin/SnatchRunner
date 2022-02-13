using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControlScripts : MonoBehaviour
{
    [SerializeField] private bool isObjectSnatched = false;
    [SerializeField] private GameObject clothedObject;
    [SerializeField] private GameObject nakedObject;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform sideMovementRoot;
    [SerializeField] private Transform rotationRoot;
    [SerializeField] private float forwardMovementSpeed = 1f;
    [SerializeField] private float VelocityNoise = 2f;
    [SerializeField] private float WaitTime = 1f;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float targetDistanceX = 3;
    [SerializeField] private float targetDistanceZ = 0;
    [SerializeField] private float P = 0.5f;
    [SerializeField] private float I = 0.01f;
    [SerializeField] private bool ActivateCatch = false;
    [SerializeField] private Animator AnimationControl;

    private bool isFollowActive = false;
    private bool isFollowWithPIDActive = false;
    private bool isLookAtActive = false;
    private Rigidbody rigidbody;

    private float sumErrX = 0;
    private float sumErrZ = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (isFollowActive)
        //{
        //    Follow(targetTransform);
        //}
        
        if (isFollowWithPIDActive)
        {
            FollowWithPID(targetTransform);
        }

        if (isLookAtActive)
        {
            LookAtTheTarget(targetTransform);
        }

        if (ActivateCatch)
        {
            float distance = Vector3.Distance(this.transform.position, targetTransform.position);

            if (distance < targetDistanceZ * 2)
            {
                Snatch();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("Robber!! trigger");
            Snatch();
        }
    }

    private void Snatch()
    {
        if (!isObjectSnatched)
        {
            isObjectSnatched = true;
            clothedObject.SetActive(false);
            nakedObject.SetActive(true);
            isLookAtActive = true;

            FollowSnatcher();
        }
    }

    private void FollowSnatcherParent()
    {
        // wait snatcher get lost (use coroutunie)
        // after a certain distance 
        transform.SetParent(parent);
        //transform.localPosition = Vector3.zero;
        float randomLocalPosX = Random.value * 2;
        float randomLocalPosZ = Random.value - 2f;
        transform.localPosition = new Vector3(randomLocalPosX, 0, randomLocalPosZ);
    }

    private void FollowSnatcher()
    {
        // wait snatcher get lost (use coroutunie)
        // after a certain distance 
        StartCoroutine(WaitFollow());
    }

    private IEnumerator WaitFollow()
    {
        // Start Corotune
        ActivateYellingAnimation();
        yield return new WaitForSeconds(WaitTime);
        //StartFollow();
        ActivateCatchingAnimation();
        StartFollowWithPID();
        yield return null;
    }

    private void StartFollow()
    {
        isFollowActive = true;
        float localPosX = Random.value * 2 - 1f;
        float localPosY = transform.position.y;
        float localPosZ = transform.position.z + -2f * Random.value;
        transform.position = new Vector3(localPosX, localPosY, localPosZ);
        //rigidbody.isKinematic = false;
    }

    private void Follow(Transform target)
    {
        transform.Translate(Vector3.forward * forwardMovementSpeed * Time.deltaTime);
        rotationRoot.transform.LookAt(target);

        float currentPos = sideMovementRoot.localPosition.x;
        float targetPos = target.localPosition.x;
        float localPos = Mathf.Lerp(currentPos, targetPos, 0.1f);
        sideMovementRoot.localPosition = Vector3.right * localPos;
    }

    private void StartFollowWithPID()
    {
        rigidbody.isKinematic = false;
        nakedObject.GetComponent<Collider>().isTrigger = false;
        isFollowWithPIDActive = true;
        isLookAtActive = true;

    }

    private void FollowWithPID(Transform target)
    {
        float distX = transform.position.x - target.position.x;
        float errX = targetDistanceX - distX;
        sumErrX += errX;
        float velX = P * errX + I * sumErrX;

        float distZ = transform.position.z - target.position.z;
        float errZ = targetDistanceZ - distZ;
        sumErrZ += errZ;
        float velZ = P * errZ + I * sumErrZ;

        float MaxVelX = forwardMovementSpeed;
        float MaxVelZ = forwardMovementSpeed;

        velX = Mathf.Clamp(velX, -MaxVelX, MaxVelX);
        velZ = Mathf.Clamp(velZ, -MaxVelZ, MaxVelZ);
        sumErrX = Mathf.Clamp(sumErrX, -MaxVelX, MaxVelX);
        sumErrZ = Mathf.Clamp(sumErrZ, -MaxVelZ, MaxVelZ);

        //if (velX >= MaxVelX)
        //    velX = MaxVelX;

        //if (velX <= -MaxVelX)
        //    velX = -MaxVelX;

        //if (velZ >= MaxVelZ)
        //    velZ = MaxVelZ;

        //if (sumErrX >= MaxVelX)
        //    sumErrX = MaxVelX;

        //if (sumErrX <= -MaxVelX)
        //    sumErrX = -MaxVelX;

        //if (sumErrZ >= MaxVelZ)
        //    sumErrZ = MaxVelZ;

        //if (sumErrZ <= -MaxVelZ)
        //    sumErrZ = -MaxVelZ;

        velX += (Random.value - 0.5f) * VelocityNoise;
        velZ += (Random.value - 0.5f) * VelocityNoise;

        rigidbody.velocity = new Vector3(velX, 0, velZ);

        // Debug.Log(" velX = " + velX + " velZ = " + velZ);
    }

    private void LookAtTheTarget(Transform target)
    {
        rotationRoot.transform.LookAt(target);
    }

    private void ActivateCatchingAnimation()
    {
        AnimationControl.SetBool("isCatchingActive", true);
    }

    private void ActivateYellingAnimation()
    {
        AnimationControl.SetBool("isYellingActive", true);
    }

}
