using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretEnter : MonoBehaviour
{

    Vector3 SittingPosition = new Vector3(-1.23f, 2.2f, 4.31f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("여기충돌했어!");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                other.transform.position = SittingPosition;
            }
        }
    }
}
