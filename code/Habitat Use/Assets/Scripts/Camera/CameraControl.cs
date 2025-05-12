using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveTimeFactor = 0.2f;
    private float moveTime;
    public float scrollTime = 0.5f;
    public float rotTime = 3f;

    private Quaternion _startRotation;
    private Collider CameraBox;

    private void Start()
    {
        if (CameraBox == null)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("CameraBox");
            if (gos.Length < 1)
                throw new Exception("No Object with Tag CameraBox");
            CameraBox = gos[0].GetComponent<BoxCollider>();
        }
        if (!isPositionValid(transform.position))
        {
            throw new Exception("startposition of camera not in CameraBox");
        }
        _startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        moveTime = Time.deltaTime*1000*moveTimeFactor;

        //SCROLL
        var scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            var forward = transform.forward;
            var oldPos = transform.position;
            var newPos = oldPos + scroll * forward * scrollTime;
            if (isPositionValid(newPos))
                transform.position = Vector3.Lerp(oldPos, newPos, moveTime);
        }

        //MOVEMENT
        Vector3 changePosition = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            var up = transform.up;
            changePosition += new Vector3(up.x, 0, up.z)*moveTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            var left = -transform.right;
            changePosition += new Vector3(left.x, 0, left.z) * moveTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            var down = -transform.up;
            changePosition += new Vector3(down.x, 0, down.z) * moveTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            var right = transform.right;
            changePosition += new Vector3(right.x, 0, right.z) * moveTime;
        }

        Vector3 oldPosition = transform.position;
        Vector3 newPosition = transform.position + changePosition;

        if (isPositionValid(newPosition))
            transform.position = newPosition;


        //ROTATION
        if (Input.GetKey(KeyCode.Q))
        {
            Quaternion oldRotation = transform.rotation;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y -= rotTime * moveTime;
            Quaternion QuatRotation = Quaternion.Euler(rotation);
            transform.rotation =QuatRotation;
        }
        if (Input.GetKey(KeyCode.E))
        {
            Quaternion oldRotation = transform.rotation;
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y += rotTime * moveTime;
            Quaternion QuatRotation = Quaternion.Euler(rotation);
            transform.rotation = QuatRotation;
        }

        //REST ROTATION
        if (Input.GetKey(KeyCode.Space))
        {
            transform.rotation = _startRotation;
        }
    }
    public bool isPositionValid(Vector3 pos)
    {
        return CameraBox.bounds.Contains(pos);
    }
}
