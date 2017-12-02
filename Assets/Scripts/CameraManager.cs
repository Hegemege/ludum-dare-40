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

    void Awake()
    {
        
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
    }
}
