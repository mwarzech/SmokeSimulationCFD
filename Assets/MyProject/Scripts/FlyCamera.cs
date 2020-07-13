using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    [Header("Camera settings")]
    [Tooltip("Multiplier for camera movement upwards")]
    [Range(0f, 10f)]
    public float climbSpeed = 4f;
    [Tooltip("Multiplier for normal camera movement")]
    [Range(0f, 20f)]
    public float normalMoveSpeed = 10f;
    [Tooltip("Multiplier for slower camera movement")]
    [Range(0f, 5f)]
    public float slowMoveSpeed = 0.25f;
    [Tooltip("Multiplier for faster camera movement")]
    [Range(0f, 40f)]
    public float fastMoveSpeed = 3f;
    [Tooltip("Rotation limits for the X-axis in degrees")]
    public Vector2 rotationLimitsX;
    [Tooltip("Rotation limits for the X-axis in degrees")]
    public Vector2 rotationLimitsY;
    [Tooltip("Whether the rotation on the X-axis should be limited")]
    public bool limitXRotation;
    [Tooltip("Whether the rotation on the Y-axis should be limited")]
    public bool limitYRotation;
    [Header("Keyboard settings")]
    [Tooltip("Key for moving the camera upwards")]
    public KeyCode moveUp;
    [Tooltip("Key for moving the camera downwards")]
    public KeyCode moveDown;
    [Tooltip("Key for faster camera movement")]
    public KeyCode moveFast;
    [Tooltip("Key for slower camera movement")]
    public KeyCode moveSlow;
    [Header("Mouse settings")]
    [Tooltip("Multiplier for camera sensitivity")]
    [Range(0f, 200f)]
    public float cameraSensitivity = 90f;
    [Tooltip("Whether the cursor should be hidden in playmode")]
    public bool hideCursor = false;
    [Tooltip("Whether the cursor should be locked in playmode")]
    public bool lockCursor = false;

    private Vector2 _rotation;

    // Use this for initialization
    void Start()
    {
        if (hideCursor)
        {
            Cursor.visible = false;
        }
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _rotation.x += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        _rotation.y += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;

        if (limitXRotation)
        {
            _rotation.x = Mathf.Clamp(_rotation.x, rotationLimitsX.x, rotationLimitsX.y);
        }
        if (limitYRotation)
        {
            _rotation.y = Mathf.Clamp(_rotation.y, rotationLimitsY.x, rotationLimitsY.y);
        }

        transform.localRotation = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(_rotation.y, Vector3.left);

        if (Input.GetKey(moveFast))
        {
            transform.position += transform.forward * (normalMoveSpeed * fastMoveSpeed) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * fastMoveSpeed) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else if (Input.GetKey(moveSlow))
        {
            transform.position += transform.forward * (normalMoveSpeed * slowMoveSpeed) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * slowMoveSpeed) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        }

        if (Input.GetKey(moveUp))
        {
            transform.position += transform.up * climbSpeed * Time.deltaTime;
        }

        if (Input.GetKey(moveDown))
        {
            transform.position -= transform.up * climbSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
