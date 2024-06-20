using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roulemapoule : MonoBehaviour
{
    public GameObject Robot;

    public WheelCollider rightMotorWheel;
    public WheelCollider leftMotorWheel;
    public float Torque = 1500f;
    public float brakeTorque = 5f;

    public Transform wheelLeft;
    public Transform wheelRight;

    public Slider ScanVitesse; 
    public Text ScanVitesseValue;

    public Slider ScanAngle;
    public Text ScanAngleValue;

    public GameObject CameraBottom;
    public Slider ScanCameraBottom;
    public Text ScanCameraBottomValue;

    public GameObject CameraTop;
    public Slider ScanCameraTop;
    public Text ScanCameraTopValue;


    private float TICKS_REV = 38400f ;
    private float TICKS_HALF_REV = 0f ;
    private float WHEEL_RADIUS = 0.032f ;
    private float WHEELS_SEP = 0.225f ;
    private float CONTROL_LOOP_PERIOD = 10 ;
    private float PI = 3.141592653589793f;



    void Start()
    {
        TICKS_HALF_REV = TICKS_REV / 2;
        /*
        // Configurer la friction avant pour maximiser l'adh�rence
        WheelFrictionCurve forwardFriction = new WheelFrictionCurve();
        forwardFriction.extremumSlip = 1f; // Tr�s tol�rant au glissement avant de perdre de l'adh�rence
        forwardFriction.extremumValue = 10f; // Friction maximale tr�s �lev�e
        forwardFriction.asymptoteSlip = 2f; // Tol�re encore plus de glissement avant de diminuer
        forwardFriction.asymptoteValue = 10f; // Friction asymptotique tr�s �lev�e pour minimiser la perte d'adh�rence
        forwardFriction.stiffness = 2f; // Rigidit� augment�e pour une adh�rence optimale
        rightMotorWheel.forwardFriction = forwardFriction;
        leftMotorWheel.forwardFriction = forwardFriction;

        // Configurer la friction lat�rale pour maximiser l'adh�rence
        WheelFrictionCurve sidewaysFriction = new WheelFrictionCurve();
        sidewaysFriction.extremumSlip = 1f; // Tr�s tol�rant au glissement lat�ral avant de perdre de l'adh�rence
        sidewaysFriction.extremumValue = 10f; // Friction maximale tr�s �lev�e
        sidewaysFriction.asymptoteSlip = 2f; // Tol�re encore plus de glissement lat�ral avant de diminuer
        sidewaysFriction.asymptoteValue = 10f; // Friction asymptotique tr�s �lev�e pour minimiser la perte d'adh�rence
        sidewaysFriction.stiffness = 2f; // Rigidit� augment�e pour une adh�rence optimale
        rightMotorWheel.sidewaysFriction = sidewaysFriction;
        leftMotorWheel.sidewaysFriction = sidewaysFriction;

        */
    }   

    void Update()
    {
        //pour faire bouger la camera avant 

        ScanCameraBottomValue.text = ScanCameraBottom.value.ToString();
        ScanCameraTopValue.text = ScanCameraTop.value.ToString();
        
        //CameraBottom.transform.rotation = Quaternion.Euler(Robot.transform.rotation.x, ScanCameraBottom.value, Robot.transform.rotation.z);
        //CameraTop.transform.rotation = Quaternion.Euler(ScanCameraTop.value, CameraBottom.transform.rotation.y, CameraBottom.transform.rotation.z);

        //Debug.Log("Robot " + Robot.transform.rotation.x + "  " + Robot.transform.rotation.y + "  " + Robot.transform.rotation.z);
        //Debug.Log("Camera " + CameraBottom.transform.rotation.x + "  " + CameraBottom.transform.rotation.y + "  " + CameraBottom.transform.rotation.z);



        //calcul de la vitesse des pneus en fonction de la rotation et de la vitesse 

        ScanVitesseValue.text = ScanVitesse.value.ToString();
        ScanAngleValue.text = ScanAngle.value.ToString();

        float avgVel = ScanVitesse.value;
        float deltaVel = ScanAngle.value * WHEELS_SEP / 2;
        float dl = avgVel + deltaVel;
        float dr = avgVel - deltaVel;
        float l_ticks = TICKS_REV * dl / (WHEEL_RADIUS * 2 * PI) * CONTROL_LOOP_PERIOD / 1000;
        float r_ticks = TICKS_REV * dr / (WHEEL_RADIUS * 2 * PI) * CONTROL_LOOP_PERIOD / 1000;

        Debug.Log(l_ticks + " et " + r_ticks);

        // Apply motor torque to move the robot
        //leftMotorWheel.motorTorque =  Torque * ScanVitesse.value;
        //rightMotorWheel.motorTorque = Torque * ScanVitesse.value;

        if (l_ticks > 0 || l_ticks < 0)
        {
            leftMotorWheel.motorTorque = (int) l_ticks;
            leftMotorWheel.brakeTorque = 0f;
            //Debug.Log("1");
        }
        else if(l_ticks == 0f) {
            leftMotorWheel.motorTorque = 0f;
            leftMotorWheel.brakeTorque = (int) brakeTorque;
            //Debug.Log("2");
        }
        if (r_ticks > 0 || r_ticks < 0)
        {
            rightMotorWheel.motorTorque = (int) r_ticks;
            rightMotorWheel.brakeTorque = 0f;
            //Debug.Log("3");
        }
        else if(r_ticks == 0f)
        {
            rightMotorWheel.motorTorque = 0f;
            rightMotorWheel.brakeTorque = (int) brakeTorque;
            //Debug.Log("4");
        }

        var pos1 = transform.position;
        var rot1 = transform.rotation;
        leftMotorWheel.GetWorldPose(out pos1, out rot1);
        wheelLeft.position = pos1;
        wheelLeft.rotation = rot1;


        var pos2 = transform.position;
        var rot2 = transform.rotation;
        rightMotorWheel.GetWorldPose(out pos2, out rot2);
        wheelRight.position = pos2;
        wheelRight.rotation = rot2;

    }
}

