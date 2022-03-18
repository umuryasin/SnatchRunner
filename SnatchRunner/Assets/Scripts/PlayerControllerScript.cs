using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerControllerScript : MonoBehaviour
{

    [SerializeField] public Transform sideMovementRoot;
    [SerializeField] public Transform leftLimit, RightLimit;
    [SerializeField] public GameObject PlayerObject;
    [SerializeField] public float forwardMovementSpeed = 1f;   
    [SerializeField] public float sideMovementSensivity = 0.03f;
    [SerializeField] public float LookAtMovementSensivity = 10f;
    [SerializeField] public float LookAtMovementRotationLimit = 60;
    [SerializeField] public float LookAtMovementRotationSpeed = 0.85f;

    private Vector2 inputDrag;
    private Vector2 inputpreviousMousePosition;

    private float WaitTime = 0.75f;

    private bool isMovement = false;
    private bool isSideMovement = true;
    private float AbsoluteForwardMovementSpeed;
    private Animator GameObjectAnimator;

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
        GameObjectAnimator =  PlayerObject.GetComponent<Animator>();
        AbsoluteForwardMovementSpeed = forwardMovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovement)
        {
            HandleForwardMovement();

            if (isSideMovement)
            {
                HandleInput();
                HandleSideMovement();
                HandleLookAtMovement();
            }
         
        }

    }

    private void HandleForwardMovement()
    {
        transform.Translate(Vector3.forward * forwardMovementSpeed * Time.deltaTime);
    }

    private void HandleSideMovement()
    {
        Vector3 localPos = sideMovementRoot.localPosition;
        localPos += Vector3.right * inputDrag.x * sideMovementSensivity;
        localPos.x = Mathf.Clamp(localPos.x, leftLimit.position.x, RightLimit.localPosition.x);
        sideMovementRoot.localPosition = localPos;
    }

    private void HandleLookAtMovement2()
    {
        float previousAngle = 0;
        float targetAngle = inputDrag.x * LookAtMovementSensivity + sideMovementRoot.rotation.eulerAngles.y;
        float RotateAngle = Mathf.LerpAngle(previousAngle, targetAngle, LookAtMovementRotationSpeed);
        RotateAngle = Mathf.Clamp(RotateAngle, -LookAtMovementRotationLimit, LookAtMovementRotationLimit);

        Vector3 RotateVector = RotateAngle * Vector3.up;
        Quaternion RotateQuaternion = Quaternion.Euler(RotateVector);
        sideMovementRoot.rotation = RotateQuaternion;
    }

    private void HandleLookAtMovement()
    {
        float previousAngle = sideMovementRoot.rotation.eulerAngles.y;
        if (previousAngle >= 180)
            previousAngle -= 360;

        if (previousAngle <= -180)
            previousAngle += 360;

        float targetAngle = inputDrag.x * LookAtMovementSensivity;
        float RotateAngle = targetAngle * LookAtMovementRotationSpeed + previousAngle * (1 - LookAtMovementRotationSpeed);
        RotateAngle = Mathf.Clamp(RotateAngle, -LookAtMovementRotationLimit, LookAtMovementRotationLimit);

        Vector3 RotateVector = RotateAngle * Vector3.up;
        Quaternion RotateQuaternion = Quaternion.Euler(RotateVector);
        sideMovementRoot.rotation = RotateQuaternion;
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inputpreviousMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 deltaMouse = (Vector2)Input.mousePosition - inputpreviousMousePosition;
            inputDrag = deltaMouse;
            inputpreviousMousePosition = Input.mousePosition;
        }
        else
        {
            inputDrag = Vector2.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Obstacle"))
        {
            Debug.Log("Ouch!! I fall down");
            FallDown();
        }
        else if (other.transform.root.CompareTag("Npc"))
        {
            Debug.Log("He he, Snatch!!");
        }
        else if (other.transform.root.CompareTag("Police"))
        {
            Debug.Log("Oh Noo!! I busted!");
            Busted();
        }
        else if (other.transform.root.CompareTag("FinishLine"))
        {
            Debug.Log("Yeahh!! I win!");
            Win();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.root.CompareTag("Obstacle"))
    //    {
    //        Debug.Log("Ouch!! I fall down");
    //        FallDown();
    //    }
    //}


    private void GameManager_onGameStateChanged(GameStates GameState)
    {
        if (GameState == GameStates.Started)
        {
            StartGame();
        }
        else if (GameState == GameStates.Finished)
        {
            //FinishGame();
        }
        else if (GameState == GameStates.RestartGame)
        {
            //RestartGame();
        }
    }

    private void FallDown()
    {
        FallingAnimatorControl();
    }

    private void Busted()
    {
        isMovement = false;
        BustedAnimatorControl();
        GameManager.Instance.LoseGame();
    }

    private void Win()
    {
        isMovement = false;
        int score = SkorManager.Instance.GetScore();

        float TranslatePos = transform.position.z + score * 1f;
        float endGameTime = 5f;

        sideMovementRoot.DOLocalMoveX(0, endGameTime);
        transform.DOLocalMoveZ(TranslatePos, endGameTime).OnComplete(WinAnimatorControl);

        GameManager.Instance.WinGame();
    }

    private void StartGame()
    {
        GameObjectAnimator.SetBool("isStarted", true);
        isMovement = true;
    }

    private void FallingAnimatorControl()
    {
        // wait snatcher get lost (use coroutunie)
        // after a certain distance 
        StartCoroutine(WaitFallingAndGettingUp());
    }

    private void BustedAnimatorControl()
    {
        GameObjectAnimator.SetBool("isBusted", true);
    }

    private void WinAnimatorControl()
    {
        sideMovementRoot.Rotate(new Vector3(0, -90, 0));
        GameObjectAnimator.SetBool("isDancing", true);
    }

    private IEnumerator WaitFallingAndGettingUp()
    {
        isSideMovement = false;
        forwardMovementSpeed = forwardMovementSpeed / 2;
        GameObjectAnimator.SetBool("isFalling", true);
        yield return new WaitForSeconds(WaitTime);

        forwardMovementSpeed = forwardMovementSpeed * 2;
        GameObjectAnimator.SetBool("isFalling", false);
        inputDrag = Vector2.zero;
        inputpreviousMousePosition = Vector2.zero;
        isSideMovement = true;
        yield return null;
    }

    private IEnumerator WaitGameEnd()
    {
        yield return new WaitForSeconds(WaitTime);


        yield return null;
    }


}
