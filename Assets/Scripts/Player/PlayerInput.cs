using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public string move1AxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string move2AxisName = "Horizontal"; // 좌우 움직임을 위한 입력축 이름
    public string teleportButtonName = "Dash"; // 대쉬 버튼
    public string interactionButtonName = "Interaction"; // Obj를 집어들기 위한 입력 버튼 이름
    public string repairButtonName = "Repair"; // 모듈 수리를 위한 입력 버튼 이름
    public string submitButtonName = "Submit"; // inputText 제출을 위한 엔터 입력

    public float Move_GoBack { get; private set; }
    public float Move_LeftRight { get; private set; }
    public bool Teleport { get; private set; }
    public bool Interact { get; private set; }
    public bool RepairModule { get; private set; }
    public bool Submit { get; private set; }

    private void Update()
    {
        Interact = Input.GetButtonDown(interactionButtonName);
        Teleport = Input.GetButtonDown(teleportButtonName);
    }

    private void FixedUpdate()
    {
        Move_GoBack = Input.GetAxis(move1AxisName);
        Move_LeftRight = Input.GetAxis(move2AxisName);
        RepairModule = Input.GetButton(repairButtonName);
    }
}