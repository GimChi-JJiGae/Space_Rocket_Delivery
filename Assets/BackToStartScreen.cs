using UnityEngine;
using UnityEngine.UI;

public class BackToStartScreen : MonoBehaviour
{
    public GameObject guide1;
    public GameObject guide2;

    public void OnButtonClick()
    {
        guide1.SetActive(false);
        guide2.SetActive(false);
    }
}
