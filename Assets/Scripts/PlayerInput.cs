using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public string horizontalInput = "Horizontal"; // 좌우 움직임을 위한 입력축 이름
    public string verticalInput = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string interactionButtonName = "Interaction"; // Obj를 집어들기 위한 입력 버튼 이름
    public string repairButtonName = "Repair";
    public string destroyButtonName = "Destroy";

    public float Move { get; private set; } // 감지된 움직임 입력값
    public bool Interact { get; private set; }
    public bool RepairModule { get; private set; }
    public bool DestroyModule { get; private set; }

    private void Update() {
        // if (GameManager.instance != null && GameManager.instance.isGameover)
        // {
        //     move = 0;
        //     pick = false;
        //     drop = false;
        //     return;
        // }

        // move = Input.GetAxis(horizontalInput);
        // vertical = Input.GetAxis(verticalInput);
        Interact = Input.GetButton(interactionButtonName);
        RepairModule = Input.GetButton(repairButtonName);
        DestroyModule = Input.GetButton(destroyButtonName);
    }
}