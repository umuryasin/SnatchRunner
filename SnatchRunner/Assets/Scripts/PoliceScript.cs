using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceScript : MonoBehaviour
{
    [SerializeField] private bool isObjectSnatched = false;
    [SerializeField] private GameObject ColliderObject;
    [SerializeField] private GameObject SkorObject;
    [SerializeField] private Transform rotationRoot;
    [SerializeField] private float forwardMovementSpeed = 1f;
    [SerializeField] private float VelocityNoise = 2f;
    [SerializeField] private float WaitTime = 1f;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float targetDistanceX = 3;
    [SerializeField] private float targetDistanceZ = 0;
    [SerializeField] private float P = 0.5f;
    [SerializeField] private float I = 0.01f;
    [SerializeField] private Animator AnimationControl;

    private bool ActivateCatch = true;
    private bool isFollowWithPIDActive = false;
    private bool isLookAtActive = false;
    private Rigidbody rigidbody;

    private float sumErrX = 0;
    private float sumErrZ = 0;

    void OnEnable()
    {
        // Subscribe to the event
        GameManager.onGameStateChanged += GameManager_onGameStateChanged;
    }

    void OnDisable()
    {
        // Unsubscribe to the event
        GameManager.onGameStateChanged -= GameManager_onGameStateChanged;
    }

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
            ActivateCatchingAnimation();
            isFollowWithPIDActive = false;
            ColliderObject.GetComponent<Collider>().isTrigger = true;
            StopCharacter();
        }
        else if (other.transform.root.CompareTag("FinishLine"))
        {
            ColliderObject.GetComponent<Collider>().isTrigger = true;
            StopCharacter();
        }
    }

    private void GameManager_onGameStateChanged(GameStates GameState)
    {
        if (GameState == GameStates.Started)
        {
            //StartGame();
        }
        else if (GameState == GameStates.Finished)
        {
            //FinishGame();
        }
        else if (GameState == GameStates.RestartGame)
        {
            //RestartGame();
        }
        else if (GameState == GameStates.LoseGame || GameState == GameStates.WinGame)
        {
            isFollowWithPIDActive = false;
            ColliderObject.GetComponent<Collider>().isTrigger = true;
            StopCharacter();
        }
    }

    private void Snatch()
    {
        if (!isObjectSnatched)
        {
            //Object.SetActive(true);
            SkorObject.SetActive(true);
            isObjectSnatched = true;
            isLookAtActive = true;

            FollowSnatcher();
        }
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
        ActivateFollowingAnimation();
        StartFollowWithPID();
        yield return null;
    }
    private void StartFollowWithPID()
    {
        rigidbody.isKinematic = false;
        ColliderObject.GetComponent<Collider>().isTrigger = false;
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

    private void StopCharacter()
    {
        rigidbody.velocity = new Vector3(0, 0, 0);
    }

    private void LookAtTheTarget(Transform target)
    {
        rotationRoot.transform.LookAt(target);
    }

    private void ActivateFollowingAnimation()
    {
        AnimationControl.SetBool("isFollowActive", true);
    }

    private void ActivateYellingAnimation()
    {
        AnimationControl.SetBool("isYellingActive", true);
    }

    private void ActivateCatchingAnimation()
    {
        AnimationControl.SetBool("isCatchingActive", true);
    }
}
