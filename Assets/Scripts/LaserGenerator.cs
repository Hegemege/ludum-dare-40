using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserGenerator : MonoBehaviour
{
    public GameObject LaserPolePrefab;
    public List<GameObject> LaserMarkers;

    void Awake() 
    {
        foreach (var marker in LaserMarkers)
        {
            var master = Instantiate(LaserPolePrefab);
            master.transform.position = marker.transform.position;
            master.transform.rotation = marker.transform.rotation;

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
                laserPoleScript.MasterScript = master.GetComponent<LaserPoleController>(); // Instead of this, maybe fixes the bug?

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
