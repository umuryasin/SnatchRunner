using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour
{
    [SerializeField] private bool isObjectSnatched = false;
    [SerializeField] private GameObject ColliderObject;
    [SerializeField] private GameObject NormalObject;
    [SerializeField] private GameObject NakedObject;
    [SerializeField] private GameObject SkorObject;
    [SerializeField] private Transform rotationRoot;
    [SerializeField] public float forwardMovementSpeed = 5f;
    [SerializeField] public float VelocityNoise = 2f;
    [SerializeField] private float WaitTime = 1f;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float targetDistanceX = 1;
    [SerializeField] private float targetDistanceZ = 0;
    [SerializeField] private float P = 0.8f;
    [SerializeField] private float I = 0.2f;

    private bool isFollowOrNot = false;
    private bool isFollowWithPIDActive = false;
    private bool isLookAtActive = false;
    private Rigidbody rigidbody;

    private Animator NormalAnimator;
    private Animator NakedAnimator;

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

        NormalAnimator = NormalObject.GetComponent<Animator>();
        NakedAnimator = NakedObject.GetComponent<Animator>();

        ActivateStartAnimation();
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



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Debug.Log("Robber!! trigger");
            Snatch();
        }
        else if (other.transform.root.CompareTag("FinishLine"))
        {
            ColliderObject.GetComponent<Collider>().isTrigger = true;
            StopCharacter();
        }
    }

    private void GameManager_onGameStateChanged(GameStates gameState)
    {
        if (gameState == GameStates.Started)
        {
            //StartGame();
        }
        else if (gameState == GameStates.Finished)
        {
            //FinishGame();
        }
        else if (gameState == GameStates.RestartGame)
        {
            //RestartGame();
        }
        else if(gameState == GameStates.LoseGame || gameState == GameStates.WinGame)
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
            NormalObject.SetActive(false);
            NakedObject.SetActive(true);
            SkorObject.SetActive(true);
            isLookAtActive = true;
            isObjectSnatched = true;

            isFollowOrNot = DecideWhetherFollowOrTerrieldAnimation();

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
        yield return new WaitForSeconds(WaitTime);
        //StartFollow();
        if (isFollowOrNot)
        {
            ActivateRunningAnimation();
            StartFollowWithPID();
        }
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

        velX += (Random.value - 0.5f) * VelocityNoise;
        velZ += (Random.value - 0.5f) * VelocityNoise;

        rigidbody.velocity = new Vector3(velX, 0, velZ);
    }

    private void LookAtTheTarget(Transform target)
    {
        rotationRoot.transform.LookAt(target);
    }

    private void ActivateStartAnimation()
    {
        float randVal = Random.value;

        if (randVal > 0.5)
        {
            NormalAnimator.SetBool("isTalkingOnPhone", true);
        }
        else
        {
            NormalAnimator.SetBool("isWalking", true);
            rigidbody.velocity = new Vector3(0, 0, 1);
        }
    }

    private bool DecideWhetherFollowOrTerrieldAnimation()
    {
        float randVal = Random.value;
        bool isFollowActivate = false;
        rigidbody.velocity = new Vector3(0, 0, 0);

        if (randVal > 0.8)
        {
            NakedAnimator.SetBool("isTerrield", true);
        }
        else
        {
            NakedAnimator.SetBool("isSuprised", true);
            isFollowActivate = true;
        }

        return isFollowActivate;
    }

    private void ActivateRunningAnimation()
    {
        NakedAnimator.SetBool("isRunning", true);
    }

    private void StopCharacter()
    {
        NakedAnimator.SetBool("isGameEnd", true);
        rigidbody.velocity = new Vector3(0, 0, 0);
    }
}
