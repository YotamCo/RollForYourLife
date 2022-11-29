using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed = 0.15f; // move every seconds
    private float _tempMoveSpeed;

    private Vector3 _bulletDireccion;
    private Vector3 _bulletEulerAngles;

    GameObject gameManager;
    MapManager mapManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        _tempMoveSpeed = 0;

        gameManager = GameObject.Find("GameManager");
        mapManagerScript = gameManager.GetComponent<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.eulerAngles = _bulletEulerAngles;
        _tempMoveSpeed -= Time.deltaTime;
        if (_tempMoveSpeed < 0)
        {
            transform.position += _bulletDireccion;
            _tempMoveSpeed = _bulletSpeed;
        }
        if(mapManagerScript.IsInMapBounds(gameObject.transform.position))
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
        
        if (muzzleDirection.z == 0)
        {
            _bulletDireccion = new Vector3(1, 0, 0);
        }
        else if(muzzleDirection.z == 90)
        {
            _bulletDireccion = new Vector3(0, 1, 0);
        }
        else if(muzzleDirection.z == 180)
        {
            _bulletDireccion = new Vector3(-1, 0, 0);
        }
        else if(muzzleDirection.z == 270)
        {
            _bulletDireccion = new Vector3(0, -1, 0);
        }
    }
}
