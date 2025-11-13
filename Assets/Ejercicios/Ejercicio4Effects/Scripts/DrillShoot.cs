using System;
using System.Collections.Generic;
using UnityEngine;

public class DrillShoot : MonoBehaviour
{
    [SerializeField] GameObject _drillPrefab;
    [SerializeField] Transform _drillParent;
    
    [SerializeField] float _fireRate = 0.5f;
    public List<GameObject> _drills = new List<GameObject>();
    float nextFireTime = 0f;

    public void Shoot()
    {
        if(!_drillPrefab)
            return;
        if (Time.time >= _fireRate)
        {
            _drills.Add(Instantiate(_drillPrefab, _drillParent.transform.position, _drillParent.transform.rotation)); 
            nextFireTime = Time.time + _fireRate;
        }
    }

    public void Stop()
    {
        foreach (GameObject drills in _drills)
        {
            Destroy(drills);
            
        }
        _drills.Clear();
    }
}
