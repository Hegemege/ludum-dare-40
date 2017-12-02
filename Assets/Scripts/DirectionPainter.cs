using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirectionPainter : MonoBehaviour
{
    public GameObject ArrowPrefab;
    public float Interval;

    public float MaximumStorage;

    private float _storage;

    private List<GameObject> _paintedArrows;
    private GameObject _lastPaintedArrow;
    private GameObject _arrowContainer;

    void Awake()
    {
        _storage = MaximumStorage;
        _arrowContainer = new GameObject();
        _arrowContainer.name = "PaintedArrowContainer";
        _paintedArrows = new List<GameObject>();

    }

    void Start() 
    {
        
    }
    
    void FixedUpdate() 
    {
        if (Input.GetMouseButtonUp(1))
        {
            Reset();
        }
    }

    public void Reset()
    {
        _storage = MaximumStorage;
        foreach (var arrow in _paintedArrows)
        {
            Destroy(arrow);
        }
        _paintedArrows.Clear();
    }

    public void SendClickDrag(Vector3 position)
    {
        if (_storage <= 0) return;

        if (_lastPaintedArrow != null && Vector3.Distance(_lastPaintedArrow.transform.position, position) < Interval)
        {
            var towards = position - _lastPaintedArrow.transform.position;
            towards.y = 0f;
            towards.Normalize();

            _lastPaintedArrow.transform.rotation = Quaternion.LookRotation(towards, Vector3.up);
        }

        foreach (var arrow in _paintedArrows)
        {
            if (Vector3.Distance(arrow.transform.position, position) <= Interval)
            {
                return;
            }
        }

        // If we have dragged too far, manual release
        if (_lastPaintedArrow != null)
        {
            if (Vector3.Distance(position, _lastPaintedArrow.transform.position) > 2f * Interval)
            {
                SendRelease(position);
            }
        }

        // Make new arrow stick to old one
        var wantedPosition = position;
        if (_lastPaintedArrow != null)
        {
            // Make wantedPosition very close to the last painted arrow
            var forward = position - _lastPaintedArrow.transform.localPosition;
            forward.y = 0f;
            forward.Normalize();

            wantedPosition = _lastPaintedArrow.transform.localPosition + Interval * forward;
        }

        // We can place a new arrow
        var newArrow = Instantiate(ArrowPrefab);
        newArrow.transform.localPosition = wantedPosition;
        newArrow.transform.parent = _arrowContainer.transform;

        _paintedArrows.Add(newArrow);

        Vector3 direction;
        if (_paintedArrows.Count > 1 && _lastPaintedArrow != null)
        {
            direction = newArrow.transform.position - _paintedArrows[_paintedArrows.Count - 2].transform.position;
            direction.y = 0f;
            direction.Normalize();
            _paintedArrows[_paintedArrows.Count - 2].transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            direction = Vector3.forward;
        }

        newArrow.transform.localRotation = Quaternion.LookRotation(direction, Vector3.up);

        _lastPaintedArrow = newArrow;
    }

    public void SendRelease(Vector3 position)
    {
        _lastPaintedArrow = null;
    }
}
