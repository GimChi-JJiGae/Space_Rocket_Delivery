using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretSpinController : MonoBehaviour
{
    public float degree = 15f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // 수평 입력 값

        if (horizontal != 0)
        {
            Vector3 rotationDirection = new Vector3(0, horizontal, 0f);
            transform.Rotate(rotationDirection * degree * Time.deltaTime);
        }
    }
}
