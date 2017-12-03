using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserGenerator : MonoBehaviour
{
    public GameObject LaserPolePrefab;

    void Awake()
    {
        var allMarkers = GameObject.FindGameObjectsWithTag("LaserPoleMarker");

        foreach (var marker in allMarkers)
        {
            var master = Instantiate(LaserPolePrefab);
            master.transform.position = marker.transform.position;
            master.transform.rotation = marker.transform.rotation;

            // Pass parameters from primer to master
            var masterScript = master.GetComponent<LaserPoleController>();
            var primerScript = marker.GetComponent<LaserPolePrimer>();

            masterScript.FrontLaserLength = primerScript.FrontLaserLength;
            masterScript.BackLaserLength = primerScript.BackLaserLength;
            masterScript.FrontLaserEnabled = primerScript.FrontLaserEnabled;
            masterScript.BackLaserEnabled = primerScript.BackLaserEnabled;

            // Pass the rotation component parameters if set

            var axisRotator = master.GetComponent<RotateAroundAxis>();
            if (primerScript.Rotating)
            {
                axisRotator.enabled = true;
                axisRotator.RotationSpeed = primerScript.RotationSpeed;
            }

            Destroy(marker);

            // Create a mirror container;
            var mirrorContainer = new GameObject();
            mirrorContainer.name = "MirrorContainer";

            // Create the mirrors
            var topMirror = Instantiate(LaserPolePrefab);
            var bottomMirror = Instantiate(LaserPolePrefab);
            var leftMirror = Instantiate(LaserPolePrefab);
            var rightMirror = Instantiate(LaserPolePrefab);
            var topLeftMirror = Instantiate(LaserPolePrefab);
            var topRightMirror = Instantiate(LaserPolePrefab);
            var bottomLeftMirror = Instantiate(LaserPolePrefab);
            var bottomRightMirror = Instantiate(LaserPolePrefab);

            var mirrors = new List<GameObject>();

            mirrors.Add(topMirror);
            mirrors.Add(bottomMirror);
            mirrors.Add(leftMirror);
            mirrors.Add(rightMirror);
            mirrors.Add(topLeftMirror);
            mirrors.Add(topRightMirror);
            mirrors.Add(bottomLeftMirror);
            mirrors.Add(bottomRightMirror);

            foreach (var mirror in mirrors)
            {
                var laserPoleScript = mirror.GetComponent<LaserPoleController>();
                laserPoleScript.Master = master;
                laserPoleScript.MasterScript = masterScript;

                laserPoleScript.gameObject.transform.parent = mirrorContainer.transform;
            }

            topMirror.transform.localPosition = master.transform.localPosition - 9f * Vector3.right;
            bottomMirror.transform.localPosition = master.transform.localPosition + 9f * Vector3.right;
            leftMirror.transform.localPosition = master.transform.localPosition - 16f * Vector3.forward;
            rightMirror.transform.localPosition = master.transform.localPosition + 16f * Vector3.forward;

            topLeftMirror.transform.localPosition = master.transform.localPosition - 9f * Vector3.right - 16f * Vector3.forward;
            topRightMirror.transform.localPosition = master.transform.localPosition - 9f * Vector3.right + 16f * Vector3.forward;
            bottomLeftMirror.transform.localPosition = master.transform.localPosition + 9f * Vector3.right - 16f * Vector3.forward;
            bottomRightMirror.transform.localPosition = master.transform.localPosition + 9f * Vector3.right + 16f * Vector3.forward;
        }
    }
}
