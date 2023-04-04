using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas; // 메뉴 캔버스에 대한 참조

    private bool isMenuActive; // 메뉴 캔버스의 활성화 상태를 추적합니다.

    private void Start()
    {
        menuCanvas.SetActive(false); // 시작 시 메뉴 캔버스를 비활성화합니다.
        isMenuActive = false;
    }

    private void Update()
    {
        // ESC 키가 눌릴 때마다 메뉴 캔버스의 활성화 상태를 토글합니다.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    // 메뉴 활성화/비활성화를 토글하는 함수입니다.
    public void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        menuCanvas.SetActive(isMenuActive);
    }

    // 게임 종료 기능을 수행하는 함수입니다.
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
