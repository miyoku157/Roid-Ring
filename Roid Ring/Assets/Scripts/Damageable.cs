﻿using System.Collections; using System.Collections.Generic; using UnityEngine;  public class Damageable : MonoBehaviour {     public int maxHull, maxShield;     public float shieldRechargeRate = 1;     public bool isPlayer;      private int hull, shield;     private int temp;      void Start()     {         shield = maxShield;         hull = maxHull;          if(maxShield > 0)         {             StartCoroutine("ShieldRecharge");         }     }      public void TakeDamage(int amount)     {         temp = 0;         shield -= amount;                      if(shield < 0)         {             shield = 0;             temp = -shield;             hull -= temp;         }          if(hull < 0)         {             Destroy(this.gameObject);         }     }      void OnCollisionEnter(Collision coll)     {         if(coll.gameObject.tag == "Shot")         {             TakeDamage(10);             Destroy(coll.gameObject);         }         else if(coll.gameObject.tag == "Player")         {             Vector3 forceVec = -coll.gameObject.GetComponent<Rigidbody>().velocity * 10;             coll.gameObject.GetComponent<Rigidbody>().AddForce(forceVec, ForceMode.VelocityChange);         }     }      IEnumerator ShieldRecharge()     {         while (true)         {             if (shield < maxShield)             {                 shield += 1;             }              yield return new WaitForSeconds(1 / shieldRechargeRate / 6.66f);         }     }      public int getHull()     {         return hull;     }      public int getShield()     {         return shield;     } }