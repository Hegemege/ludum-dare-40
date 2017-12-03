using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserPoleController : DuplicateWorld
{
    public GameObject LaserBeamPrefab;
    public GameObject SelfPrefab;

    public GameObject FrontLaserAnchors;
    public GameObject BackLaserAnchors;

    public bool EnableFrontLasers;
    public bool EnableBackLasers;

    public float FrontLaserLength;
    public float BackLaserLength;

    private List<LineRenderer> FrontLaserLineRenderers;
    private List<LineRenderer> BackLaserLineRenderers;

    private BoxCollider _frontCollider;
    private BoxCollider _backCollider;

    void Awake()
    {
        Init();

        // Create laser beam objects
        FrontLaserLineRenderers = new List<LineRenderer>();
        BackLaserLineRenderers = new List<LineRenderer>();

        foreach (Transform anchor in FrontLaserAnchors.transform)
        {
            var laserBeam = Instantiate(LaserBeamPrefab);
            laserBeam.transform.parent = anchor;
            laserBeam.transform.localPosition = Vector3.zero;
            laserBeam.transform.localRotation = Quaternion.identity;
            var lr = laserBeam.GetComponentInChildren<LineRenderer>();
            FrontLaserLineRenderers.Add(lr);
        }

        foreach (Transform anchor in BackLaserAnchors.transform)
        {
            var laserBeam = Instantiate(LaserBeamPrefab);
            laserBeam.transform.parent = anchor;
            laserBeam.transform.localPosition = Vector3.zero;
            laserBeam.transform.localRotation = Quaternion.identity;
            var lr = laserBeam.GetComponentInChildren<LineRenderer>();
            BackLaserLineRenderers.Add(lr);
        }

        _frontCollider = FrontLaserAnchors.AddComponent<BoxCollider>();
        _backCollider = BackLaserAnchors.AddComponent<BoxCollider>();

        _frontCollider.isTrigger = true;
        _backCollider.isTrigger = true;
    }

    void Update() 
    {

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

        // Update laser existence
        FrontLaserAnchors.SetActive(EnableFrontLasers);
        BackLaserAnchors.SetActive(EnableBackLasers);

        // Update laser lengths
        foreach (var lr in FrontLaserLineRenderers)
        {
            var end = Vector3.forward * FrontLaserLength;
            lr.SetPosition(1, end);
        }

        foreach (var lr in BackLaserLineRenderers)
        {
            var end = Vector3.forward * BackLaserLength;
            lr.SetPosition(1, end);
        }

        // Update laser hitbox
        _frontCollider.center = Vector3.forward * FrontLaserLength / 2f;
        _frontCollider.size = new Vector3(0.05f, 1f, FrontLaserLength);

        _backCollider.center = Vector3.forward * BackLaserLength / 2f;
        _backCollider.size = new Vector3(0.05f, 1f, BackLaserLength);
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

        var localMaster = (LaserPoleController) MasterScript;

        // Copy all values from master to this
        EnableBackLasers = localMaster.EnableBackLasers;
        EnableFrontLasers = localMaster.EnableFrontLasers;
        FrontLaserLength = localMaster.FrontLaserLength;
        BackLaserLength = localMaster.BackLaserLength;

        transform.rotation = localMaster.transform.rotation;
    }
}
