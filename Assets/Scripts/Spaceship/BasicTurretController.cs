using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurretController : MonoBehaviour
{
    public float degree = 15f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        float vertical = Input.GetAxis("Vertical"); // 수직 입력 값

        //if (horizontal != 0 || vertical != 0)
        //{
        //    Debug.Log(horizontal);
        //    Debug.Log(vertical);
        //    Vector3 rotationDirection = new Vector3(-vertical, horizontal, 0f);

        //    // 회전 방향으로 오브젝트 회전
        //    transform.Rotate(rotationDirection * degree * Time.deltaTime);
        //}
        
        if (vertical != 0)
        {
            Vector3 rotationDirection = new Vector3(-vertical, 0, 0f);
            transform.Rotate(rotationDirection * degree * Time.deltaTime);
        }
    }
}
