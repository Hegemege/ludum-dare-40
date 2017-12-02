using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject MainQuad;

    public GameObject AuxVerticalQuad;
    public GameObject AuxVerticalCamera;

    public GameObject AuxHorizontalQuad;
    public GameObject AuxHorizontalCamera;

    public GameObject AuxDiagonalQuad;
    public GameObject AuxDiagonalCamera;

    private Camera _verticalCamera;
    private Camera _horizontalCamera;
    private Camera _diagonalCamera;

    void Awake()
    {
        _verticalCamera = AuxVerticalCamera.GetComponent<Camera>();
        _horizontalCamera = AuxHorizontalCamera.GetComponent<Camera>();
        _diagonalCamera = AuxDiagonalCamera.GetComponent<Camera>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // Move main camera

        var realPosition = MainCamera.transform.localPosition;
        realPosition.y -= 0.01f;

        var realQuadPosition = realPosition;
        realQuadPosition.y = -0.001f;

        // Update auxiliary camera positions
        var verticalSign = realPosition.x > 0f ? -1f : 1f;
        AuxVerticalCamera.transform.localPosition = realPosition + verticalSign * Vector3.right * 8.999f;
        AuxVerticalQuad.transform.localPosition = realQuadPosition;

        var horizontalSign = realPosition.z > 0f ? -1f : 1f;
        AuxHorizontalCamera.transform.localPosition = realPosition + horizontalSign * Vector3.forward * 15.999f;
        AuxHorizontalQuad.transform.localPosition = realQuadPosition;

        var diagonalXSign = realPosition.x > 0f ? -1f : 1f;
        var diagonalZSign = realPosition.z > 0f ? -1f : 1f;
        AuxDiagonalCamera.transform.localPosition = realPosition + diagonalXSign * Vector3.right * 8.999f + diagonalZSign * Vector3.forward * 15.999f;
        AuxDiagonalQuad.transform.localPosition = realQuadPosition;

        // Update auxiliary camera rendering order
        // The quad closest to the main camera gets highest priority, -2, then -3 and -4
        // There are only 3 quads so lets just hardcode them
        var verticalDistance = Vector3.Distance(MainCamera.transform.localPosition, AuxVerticalQuad.transform.localPosition);
        var horizontalDistance = Vector3.Distance(MainCamera.transform.localPosition, AuxHorizontalQuad.transform.localPosition);
        var diagonalDistance = Vector3.Distance(MainCamera.transform.localPosition, AuxDiagonalQuad.transform.localPosition);

        if (verticalDistance < horizontalDistance && horizontalDistance < diagonalDistance)
        {
            _verticalCamera.depth = -2;
            _horizontalCamera.depth = -3;
            _diagonalCamera.depth = -4;
        }

        if (verticalDistance < diagonalDistance && diagonalDistance < horizontalDistance)
        {
            _verticalCamera.depth = -2;
            _horizontalCamera.depth = -4;
            _diagonalCamera.depth = -3;
        }

        if (diagonalDistance < verticalDistance && verticalDistance < horizontalDistance)
        {
            _verticalCamera.depth = -3;
            _horizontalCamera.depth = -4;
            _diagonalCamera.depth = -2;
        }

        if (diagonalDistance < horizontalDistance && horizontalDistance < verticalDistance)
        {
            _verticalCamera.depth = -4;
            _horizontalCamera.depth = -3;
            _diagonalCamera.depth = -2;
        }

        if (horizontalDistance < verticalDistance && verticalDistance < diagonalDistance)
        {
            _verticalCamera.depth = -3;
            _horizontalCamera.depth = -2;
            _diagonalCamera.depth = -4;
        }

        if (horizontalDistance < diagonalDistance && diagonalDistance < verticalDistance)
        {
            _verticalCamera.depth = -4;
            _horizontalCamera.depth = -2;
            _diagonalCamera.depth = -2;
        }

    }
}
