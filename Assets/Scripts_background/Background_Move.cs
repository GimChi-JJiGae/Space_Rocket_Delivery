using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Move : MonoBehaviour
{
    public float speed = 30f;

    public Transform SpaceShipObj;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.forward * Time.deltaTime * speed, SpaceShipObj);
        /*
        if (transform.position.z > -30)
        {
            transform.Translate(-Vector3.forward * Time.deltaTime * speed, SpaceShipObj);

            if (transform.position.z < -20)
            {
                
                transform.Rotate(Vector3.up * Time.deltaTime * 10f);
            }
            Vector3 temp = Vector3.Normalize(transform.position);

            
        }

    
        else
        {


            transform.Translate(-Vector3.forward * speed * Time.deltaTime, SpaceShipObj);
            //transform.Translate(Vector3.forward * Time.deltaTime * -speed);
            //Debug.Log(Vector3.forward);
            //Debug.Log(transform.position);
            //Vector3 d = new Vector3(0, 0, transform.eulerAngles.z);

            //transform.Translate(new_direction * Time.deltaTime * speed);
        }
    
    }
        */


    }
}
