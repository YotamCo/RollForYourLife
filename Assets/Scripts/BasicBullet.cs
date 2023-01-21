using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    enum MuzzleDirection{
        RIGHT = 0,
        UP = 90,
        LEFT = 180,
        DOWN = 270
    }

    [SerializeField] private float _bulletSpeed = 0.15f; // move every seconds
    private float _tempMoveSpeed;

    private Vector3 _bulletDireccion;
    private Vector3 _bulletEulerAngles;

    ValidPositionChecker validPositionChecker;

    void Start()
    {
        _tempMoveSpeed = 0;
        validPositionChecker = new ValidPositionChecker();
    }

    void Update()
    {
        gameObject.transform.eulerAngles = _bulletEulerAngles;
        //TODO - Refactoring
        // Might need to change it so different fps will run the same
        _tempMoveSpeed -= Time.deltaTime;
        if (_tempMoveSpeed < 0)
        {
            transform.position += _bulletDireccion;
            _tempMoveSpeed = _bulletSpeed;
        }
        if(validPositionChecker.IsInMapBoundaries(gameObject.transform.position))
        {           
            Destroy(gameObject, 2); //Destroying when the bullet will already be out of bounds
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

    public void SetBulletDirection(Vector3 muzzleDirection)
    {
        _bulletEulerAngles = muzzleDirection;
        
        if (muzzleDirection.z == (float)MuzzleDirection.RIGHT)
        {
            _bulletDireccion = new Vector3(1, 0, 0);
        }
        else if(muzzleDirection.z == (float)MuzzleDirection.UP)
        {
            _bulletDireccion = new Vector3(0, 1, 0);
        }
        else if(muzzleDirection.z == (float)MuzzleDirection.LEFT)
        {
            _bulletDireccion = new Vector3(-1, 0, 0);
        }
        else if(muzzleDirection.z == (float)MuzzleDirection.DOWN)
        {
            _bulletDireccion = new Vector3(0, -1, 0);
        }
    }
}
