using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserPoleController : DuplicateWorld
{
    public LayerMask LaserCollisionMask;

    public GameObject LaserBeamPrefab;
    public GameObject SelfPrefab;

    public GameObject FrontLaserAnchors;
    public GameObject BackLaserAnchors;

    public bool FrontLaserEnabled;
    public bool BackLaserEnabled;

    public float FrontLaserLength;
    public float BackLaserLength;

    protected float _effectiveFrontLaserLength;
    protected float _effectiveBackLaserLength;

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

        // Raycast laser length, cannot check raycasts on mirrors and it would be identical anyways
        var frontSource = new Ray(transform.position + Vector3.up * 0.2f + FrontLaserAnchors.transform.forward * 0.2f, FrontLaserAnchors.transform.forward);
        var backSource = new Ray(transform.position + Vector3.up * 0.2f + BackLaserAnchors.transform.forward * 0.2f, BackLaserAnchors.transform.forward);
        RaycastHit frontHit;
        RaycastHit backHit;

        _effectiveFrontLaserLength = FrontLaserLength;
        _effectiveBackLaserLength = BackLaserLength;

        if (Physics.Raycast(frontSource.origin, frontSource.direction, out frontHit, FrontLaserLength, LaserCollisionMask))
        {
            _effectiveFrontLaserLength = Vector3.Distance(frontHit.point, transform.position + Vector3.up * 0.2f);
        }

        if (Physics.Raycast(backSource.origin, backSource.direction, out backHit, FrontLaserLength, LaserCollisionMask))
        {
            _effectiveBackLaserLength = Vector3.Distance(backHit.point, transform.position + Vector3.up * 0.2f);
        }
    }

    void CommonFixedUpdate()
    {
        // Both master and mirror

        // Update laser existence
        FrontLaserAnchors.SetActive(FrontLaserEnabled);
        BackLaserAnchors.SetActive(BackLaserEnabled);

        // Update laser lengths
        foreach (var lr in FrontLaserLineRenderers)
        {
            lr.SetPosition(1, Vector3.forward * _effectiveFrontLaserLength);
        }

        foreach (var lr in BackLaserLineRenderers)
        {
            lr.SetPosition(1, Vector3.forward * _effectiveBackLaserLength);
        }

        // Update laser hitbox
        _frontCollider.center = Vector3.forward * _effectiveFrontLaserLength / 2f;
        _frontCollider.size = new Vector3(0.05f, 1f, _effectiveFrontLaserLength);

        _backCollider.center = Vector3.forward * _effectiveBackLaserLength / 2f;
        _backCollider.size = new Vector3(0.05f, 1f, _effectiveBackLaserLength);
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
        FrontLaserEnabled = localMaster.FrontLaserEnabled;
        BackLaserEnabled = localMaster.BackLaserEnabled;
        FrontLaserLength = localMaster.FrontLaserLength;
        BackLaserLength = localMaster.BackLaserLength;

        _effectiveFrontLaserLength = localMaster._effectiveFrontLaserLength;
        _effectiveBackLaserLength = localMaster._effectiveBackLaserLength;

        transform.rotation = localMaster.transform.rotation;
    }
}
