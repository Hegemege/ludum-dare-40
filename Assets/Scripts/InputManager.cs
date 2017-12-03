using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Camera CameraRef;

    public GameObject DirectionPainterObject;

    private DirectionPainter _directionPainter;

    void Awake()
    {
        _directionPainter = DirectionPainterObject.GetComponent<DirectionPainter>();
    }

    void Update() 
    {
        CheckAllInput();
    }

    private void CheckAllInput()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // Handle native touch events
        for (var i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            HandleTouch(touch.fingerId, CameraRef.ScreenPointToRay(touch.position), touch.phase);
        }

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraRef.transform.position.y);

        // Simulate touch events from mouse events
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleTouch(10, CameraRef.ScreenPointToRay(mousePosition), TouchPhase.Began);
            }
            if (Input.GetMouseButton(0))
            {
                HandleTouch(10, CameraRef.ScreenPointToRay(mousePosition), TouchPhase.Moved);
            }
            if (Input.GetMouseButtonUp(0))
            {
                HandleTouch(10, CameraRef.ScreenPointToRay(mousePosition), TouchPhase.Ended);
            }
        }
    }

    private void HandleTouch(int touchFingerId, Ray touchPosition, TouchPhase touchPhase)
    {
        RaycastHit hit;
        if (Physics.Raycast(CameraRef.transform.position, touchPosition.direction*10f, out hit))
        {
            var normalizedPoint = hit.point;

            if (normalizedPoint.x > 4.5f)
            {
                normalizedPoint.x -= 9f;
            }
            if (normalizedPoint.x < -4.5f)
            {
                normalizedPoint.x += 9f;
            }
            if (normalizedPoint.z > 8f)
            {
                normalizedPoint.z -= 16f;
            }
            if (normalizedPoint.z < -8f)
            {
                normalizedPoint.z += 16f;
            }

            // Inform any manager of input
            var colliders = Physics.OverlapSphere(normalizedPoint, 0.01f);
            
            // TODO: do overlap sphere for UI elements too

            foreach (var overlapCollider in colliders)
            {
                // Depending on the tag of the object, send information
                if (overlapCollider.gameObject.CompareTag("Paintable"))
                {
                    if (touchPhase == TouchPhase.Began || touchPhase == TouchPhase.Moved)
                    {
                        _directionPainter.SendClickDrag(normalizedPoint);
                    }

                    if (touchPhase == TouchPhase.Ended)
                    {
                        _directionPainter.SendRelease(normalizedPoint);
                    }

                    break;
                }
            }

        }
    }
}
