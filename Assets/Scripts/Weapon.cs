using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float _attackDuration = 0.2f;
    [SerializeField] protected float _timeBtweenAttacks = 1f;
    protected float _tempTimeBetweenAttacks;

    private void Start()
    {
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        SpecificWeaponInitializations();
    }

    private void Update()
    {
        _tempTimeBetweenAttacks -= Time.deltaTime;
    }

    public virtual void Attack()
    {
        if (_tempTimeBetweenAttacks < 0)
        {
            Debug.Log("I'm attacking now");
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
            StartCoroutine(AttackTime());
            _tempTimeBetweenAttacks = _timeBtweenAttacks;
        }
    }

    private IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
    }

    public void DestroyWeapon()
    {
        //TODO: dropping weapon animation
        
    }

    public virtual bool IsTimeToDropWeapon()
    {
        return false;
    }

    public virtual string GetWeaponName()
    {
        return "";
    }

    protected virtual void SpecificWeaponInitializations()
    {
    }
}