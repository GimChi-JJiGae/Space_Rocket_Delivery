using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraText : MonoBehaviour
{
    public GameObject Cam;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TextMesh>().text = "뭐임마";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Cam.transform.rotation;
    }
}
