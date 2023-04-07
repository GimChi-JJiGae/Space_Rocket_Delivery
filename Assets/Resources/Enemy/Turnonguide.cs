using UnityEngine;
using UnityEngine.UI;

public class Turnonguide : MonoBehaviour
{
    public GameObject guide1;
    public void OnGuideClick()
    {
        guide1.SetActive(true);
        Debug.Log('a');
    }
}