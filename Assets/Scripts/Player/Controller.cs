using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject groundObject;
    [SerializeField] ParticleSystem pointParticle;
    [SerializeField] Transform cameraAim;
    [SerializeField] Transform targetPos;
    float groundMult ;
    float swipeMult ;
    float angle ;
    Vector2 mouseButtonDownPos;
    Vector2 mouseButtonUpPos;
    Vector2 mouseUpAndDownDifference;
    Rigidbody ballRb;

    public BallMovementCreate ballMovement;

    void Start()
    {
        SetValues();
    }

    void SetValues()
    {
        ballRb = GetComponent<Rigidbody>();

        //alttaki deðerler test edilirken update methodu icinde kullanýldý. Ayarlandýktan sonra startta bir kez calýstýrýldý.
        groundMult = ballMovement.GroundMult;
        swipeMult = ballMovement.SwipeMult;
        angle = ballMovement.Angle;

    }

    void Update()
    {
        MovementFunc();
        CameraMovementSetter();
        
    }

    void MovementFunc()
    {
        if (LevelManager.gameState == GameState.Normal || LevelManager.gameState==GameState.Basketable)
        {
            if (transform.position.y < groundObject.transform.position.y)
            {
                if (LevelManager.gameState == GameState.Normal)
                {
                    LevelManager.gameState = GameState.Failed;
                    FailedPanel.instance.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                mouseButtonDownPos = Input.mousePosition;
                if (groundObject.gameObject.GetComponent<Collider>().bounds.size.z / 2 < Mathf.Abs(transform.position.z))
                {
                    LevelManager.gameState = GameState.Basketable;
                }
                else
                {
                    LevelManager.gameState = GameState.Normal;
                }
            }
            if (Input.GetMouseButton(0))
            {
                mouseButtonUpPos = Input.mousePosition;
                mouseUpAndDownDifference = mouseButtonUpPos - mouseButtonDownPos;

                if (mouseUpAndDownDifference.x < -(transform.position.x)) //Left
                {
                    BallMovementDirection(-transform.right, swipeMult);

                }
                if (mouseUpAndDownDifference.x > (transform.position.x)) //Right
                {
                    BallMovementDirection(transform.right, swipeMult);
                }
                if (mouseUpAndDownDifference.y > (transform.position.y))  //forward
                {
                    BallMovementDirection(cameraAim.forward, swipeMult / 2);
                }
                if (mouseUpAndDownDifference.y < -(transform.position.y)) //back
                {
                    BallMovementDirection(-cameraAim.forward, swipeMult);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (LevelManager.gameState == GameState.Basketable && mouseUpAndDownDifference.y > Screen.height / 3)
                {
                    FunctionThatMakesYouStephCurry();
                }
                if (LevelManager.gameState == GameState.Normal && mouseUpAndDownDifference.y > Screen.height / 3)
                {
                    transform.rotation = Quaternion.AngleAxis(75, Vector3.left);
                    ballRb.AddForce(transform.forward * 10, ForceMode.Impulse);
                }
            }
        }
        
    }

    void CameraMovementSetter() // we need this because if camera had followed the basketball, it's y value would have changed as the ball was.
    {
        cameraAim.position = new Vector3(transform.position.x, cameraAim.position.y, transform.position.z - 1.5f);
        transform.LookAt(targetPos.position);
        cameraAim.LookAt(targetPos.position);
    }

    void BallMovementDirection(Vector3 direction, float multiplier)
    {
        ballRb.AddForce(direction * Time.deltaTime, ForceMode.Impulse);
        transform.Translate(direction * multiplier * Time.deltaTime, Space.World);
    }
    private void OnCollisionEnter(Collision other)
    {
        ObjectTypeDetector objectType = other.gameObject.GetComponent<ObjectTypeDetector>();
        if (objectType!=null)
        {
            if (objectType.type == ObjectType.Ground)
            {
                ballRb.AddForce(Vector3.up * groundMult, ForceMode.VelocityChange);
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        ObjectTypeDetector objectType = other.GetComponent<ObjectTypeDetector>();
        if (objectType!=null)
        {
            if (objectType.type == ObjectType.BasketPoint)
            {
                LevelManager.gameState = GameState.Finish;
                pointParticle.gameObject.SetActive(true);
                pointParticle.Play();
                FinishPanel.instance.transform.GetChild(0).gameObject.SetActive(true);
            }

        }
        
    }
    void FunctionThatMakesYouStephCurry()
    {
        ////prediction Trajectory SetUp (from related tutorial videos)
        float radian = angle * Mathf.Deg2Rad;
        Vector3 planarTarget = new Vector3(targetPos.position.x, 0, targetPos.position.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);
        float distance = Vector3.Distance(planarTarget, planarPostion);
        float yOffset = transform.position.y - targetPos.position.y;
        float initialVelocity = (1 / Mathf.Cos(radian)) * Mathf.Sqrt((0.5f * Physics.gravity.magnitude * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(radian) + yOffset));
        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(radian), initialVelocity * Mathf.Cos(radian));
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (targetPos.position.x > transform.position.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
        ballRb.velocity = finalVelocity;
    }
}

