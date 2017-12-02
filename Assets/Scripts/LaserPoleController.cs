using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserPoleController : DuplicateWorld
{
    public GameObject FrontLaserAnchors;
    public GameObject BackLaserAnchors;

    public bool EnableFrontLasers;
    public bool EnableBackLasers;

    public float FrontLaserLength;
    public float BackLaserLength;

    protected bool _spawned;

    void Awake()
    {
        base.Awake();
    }

    protected override void InitialSpawn()
    {
        // If the object does not have a master yet
        if (Master != null) return;

        // Create a mirror container;
        var mirrorContainer = new GameObject();
        mirrorContainer.name = "MirrorContainer";

        // Create the mirrors
        var topMirror = Instantiate(gameObject);
        var bottomMirror = Instantiate(gameObject);
        var leftMirror = Instantiate(gameObject);
        var rightMirror = Instantiate(gameObject);
        var topLeftMirror = Instantiate(gameObject);
        var topRightMirror = Instantiate(gameObject);
        var bottomLeftMirror = Instantiate(gameObject);
        var bottomRightMirror = Instantiate(gameObject);

        Mirrors.Add(topMirror);
        Mirrors.Add(bottomMirror);
        Mirrors.Add(leftMirror);
        Mirrors.Add(rightMirror);
        Mirrors.Add(topLeftMirror);
        Mirrors.Add(topRightMirror);
        Mirrors.Add(bottomLeftMirror);
        Mirrors.Add(bottomRightMirror);

        foreach (var mirror in Mirrors)
        {
            var laserPoleScript = mirror.GetComponent<LaserPoleController>();
            laserPoleScript.Master = this;
            laserPoleScript.gameObject.transform.parent = mirrorContainer.transform;
        }

        topMirror.transform.localPosition = transform.localPosition - 9f * Vector3.right;
        bottomMirror.transform.localPosition = transform.localPosition + 9f * Vector3.right;
        leftMirror.transform.localPosition = transform.localPosition - 16f * Vector3.forward;
        rightMirror.transform.localPosition = transform.localPosition + 16f * Vector3.forward;

        topLeftMirror.transform.localPosition = transform.localPosition - 9f * Vector3.right - 16f * Vector3.forward;
        topRightMirror.transform.localPosition = transform.localPosition - 9f * Vector3.right + 16f * Vector3.forward;
        bottomLeftMirror.transform.localPosition = transform.localPosition + 9f * Vector3.right - 16f * Vector3.forward;
        bottomRightMirror.transform.localPosition = transform.localPosition + 9f * Vector3.right + 16f * Vector3.forward;
    }

    void Update() 
    {
        if (!_spawned)
        {
            _spawned = true;
            InitialSpawn();
        }
    }
    
    void FixedUpdate()
    {
        CommonFixedUpdate();
        if (Master != null)
        {
            MirrorFixedUpdate();
            UpdateValues();
            return;
        }

        // Master only 
    }

    void CommonFixedUpdate()
    {
        // Both master and mirror
    }

    void MirrorFixedUpdate()
    {
        // Mirror only
    }

    /// <summary>
    /// If a master is assigned, copy all values
    /// </summary>
    protected override void UpdateValues()
    {
        if (Master == null) return;

        var localMaster = (LaserPoleController) Master;

        // Copy all values from master to this
        EnableBackLasers = localMaster.EnableBackLasers;
        EnableFrontLasers = localMaster.EnableFrontLasers;
        FrontLaserLength = localMaster.FrontLaserLength;
        BackLaserLength = localMaster.BackLaserLength;

        transform.rotation = localMaster.transform.rotation;
    }
}
