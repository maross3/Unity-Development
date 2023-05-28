using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform anchorPoint;
    public float rotationSpeed = 5f;
    public float zoomSpeed = 2f;
    public float panSpeed = 0.1f;
    public float minZoomDistance = 1f;
    public float maxZoomDistance = 10f;

    private bool _isRotating;
    private bool _isZooming;
    private bool _isPanning;
    private Vector3 _lastMousePosition;
    private float _currentZoomDistance;

    private void Start()
    {
        _currentZoomDistance = Vector3.Distance(transform.position, anchorPoint.position);
    }
    
    private void Update()
    {
        // Check for middle mouse button input
        if (Input.GetMouseButtonDown(2))
        {
            _isRotating = true;
            _lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            _isRotating = false;
        }

        // Perform camera rotation when middle mouse button is held down
        if (_isRotating)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - _lastMousePosition;

            // Rotate the camera around the anchor point
            transform.RotateAround(anchorPoint.position, Vector3.up, mouseDelta.x * rotationSpeed);
            transform.RotateAround(anchorPoint.position, transform.right, -mouseDelta.y * rotationSpeed);

            _lastMousePosition = currentMousePosition;
        }
        
        // Check for middle mouse button input

        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        // Perform camera zoom based on the scroll wheel input
        if (scrollDelta != 0f)
        {
            _isZooming = true;

            // Adjust the zoom distance based on the scroll wheel input
            _currentZoomDistance -= scrollDelta * zoomSpeed;
            _currentZoomDistance = Mathf.Clamp(_currentZoomDistance, minZoomDistance, maxZoomDistance);

            // Update the camera position based on the zoom distance
            Vector3 cameraDirection = (transform.position - anchorPoint.position).normalized;
            transform.position = anchorPoint.position + cameraDirection * _currentZoomDistance;
        }
        
        if (Input.GetKeyUp(KeyCode.Escape)) 
            Application.Quit();


        // Check for left mouse button input
        if (Input.GetMouseButtonDown(0))
        {
            _isPanning = true;
            _lastMousePosition = Input.mousePosition;

            // Hide the mouse cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isPanning = false;

            // Show the mouse cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        // Perform camera panning when left mouse button is held down
        if (_isPanning)
        {
            var currentMousePosition = Input.mousePosition;
            var mouseDelta = currentMousePosition - _lastMousePosition;

            // Calculate the panning direction relative to the camera's orientation
            var right = transform.right;
            var up = Vector3.Cross(transform.forward, right);
            var panDirection = (mouseDelta.x * -right + mouseDelta.y * -up) * panSpeed;

            // Pan the camera
            transform.position += panDirection;

            _lastMousePosition = currentMousePosition;
        }
    }
}
