using System;
using UnityEngine;

public class DrillShoot : MonoBehaviour
{
    [SerializeField] GameObject _drillPrefab;
    [SerializeField] Transform _drillParent;
    
    [SerializeField] float _fireRate = 0.5f;

    float nextFireTime = 0f;

    public void Shoot()
    {
        if(!_drillPrefab)
            return;
        if (Time.time >= _fireRate)Instantiate(_drillPrefab, _drillParent.transform.position, _drillParent.transform.rotation);
        nextFireTime = Time.time + _fireRate;
    }
}
