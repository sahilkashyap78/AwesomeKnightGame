using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffects : MonoBehaviour
{
    public GameObject groundImpact_Spawn, kickFX_Spawn, fireTornado_Spawn, fireShield_Spawn;
    public GameObject groundImpact_Prefab, kickFX_Prefab, fireTornado_Prefab, fireShield_Prefab,
        healFX_Prefab, thundeFX_Prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void GroundImpact ()
    {
        Instantiate (groundImpact_Prefab, groundImpact_Spawn.transform.position, Quaternion.identity);
    }

    void Kick ()
    {
        Instantiate(kickFX_Prefab, kickFX_Spawn.transform.position, Quaternion.identity);
    }

    void FireTornado ()
    {
        Instantiate(fireTornado_Prefab, fireTornado_Spawn.transform.position, Quaternion.identity);
    }

    void FireShield ()
    {
      GameObject fireObj =  Instantiate (fireShield_Prefab, fireShield_Spawn.transform.position, Quaternion.identity) as GameObject;
        fireObj.transform.SetParent (transform); // it will apply the exact position as parent
    }

    void Heal ()
    {
        Vector3 temp = transform.position;
        temp.y += 2f;
        GameObject healObj = Instantiate (healFX_Prefab, temp, Quaternion.identity) as GameObject;
        healObj.transform.SetParent (transform);
    }

    void ThunderAttack ()
    {
        for (int index = 0; index < 8; index++)
        {
            Vector3 pos = Vector3.zero;

            if (index == 0)
            {
                pos = new Vector3(transform.position.x - 4f, transform.position.y + 2f, transform.position.z);
            }

            else if (index == 1)
            {
                pos = new Vector3(transform.position.x + 4f, transform.position.y + 2f, transform.position.z);
            }

            else if (index == 2)
            {
                pos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z - 4f);
            }

            else if (index == 3)
            {
                pos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z + 4f);
            }

            else if (index == 4)
            {
                pos = new Vector3(transform.position.x + 2.5f, transform.position.y + 2f, transform.position.z + 2.5f);
            }

            else if (index == 5)
            {
                pos = new Vector3(transform.position.x - 2.5f, transform.position.y + 2f, transform.position.z + 2.5f);
            }

            else if (index == 6)
            {
                pos = new Vector3(transform.position.x - 2.5f, transform.position.y + 2f, transform.position.z - 2.5f);
            }

            else if (index == 7)
            {
                pos = new Vector3(transform.position.x + 2.5f, transform.position.y + 2f, transform.position.z +2.5f);
            }

            Instantiate(thundeFX_Prefab, pos, Quaternion.identity);
        }

    }
}
