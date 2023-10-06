using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform cueBall;
    [SerializeField] float rotationSpeed;
    [SerializeField] Vector3 offset;
    [SerializeField] float downAngle;
    [SerializeField] GameObject cueStick;
    private float horizontalInput;
    private bool isTakingShot = false;

    [SerializeField] float power;
    [SerializeField] float maxDrawDistance;

    private float SavedmousePosition;

    GameManager gameManager;
    [SerializeField] TextMeshProUGUI powerText;



    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        foreach (GameObject Ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            if (Ball.GetComponent<Ball>().IsCueBall())
            {
                cueBall = Ball.transform;
                break;

            }
        }
        ResetCamera();

    }

    // Update is called once per frame
    void Update()
    {
        if (cueBall != null)
        {
            horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            transform.RotateAround(cueBall.position, Vector3.up, horizontalInput);
        }



        Shoot();

    }
    public void ResetCamera()
    {
        cueStick.SetActive(true);
        transform.position = cueBall.position + offset;
        transform.LookAt(cueBall.position);
        transform.localEulerAngles = new Vector3(downAngle, transform.localEulerAngles.y, 0);
        
    }
    void Shoot()
    {
        if (gameObject.GetComponent<Camera>().enabled)
        {
            if(Input.GetButtonDown("Fire1") && !isTakingShot)
            {
                isTakingShot = true;
                SavedmousePosition = 0f;

            }
            else if(isTakingShot){
                if(SavedmousePosition + Input.GetAxis("Mouse Y")<= 0)
                {
                    SavedmousePosition += Input.GetAxis("Mouse Y");
                    if(SavedmousePosition <= maxDrawDistance)
                    {
                        SavedmousePosition = maxDrawDistance;
                    }
                    float powerValueNumber = ((SavedmousePosition - 0) / (maxDrawDistance - 0)) * (100 - 0) + 0;
                    int powerValueInt = Mathf.RoundToInt(powerValueNumber);
                    powerText.text = "Power " +  powerValueInt.ToString();
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    Vector3 hitDirection = transform.forward;
                    hitDirection = new Vector3(hitDirection.x, 0, hitDirection.z).normalized;

                    cueBall.gameObject.GetComponent<Rigidbody>().AddForce(hitDirection * power * Mathf.Abs(SavedmousePosition), ForceMode.Impulse);
                    cueStick.SetActive(false);
                    gameManager.SwitchCameras();
                    isTakingShot=false;

                }
            }
        }
    }
}
