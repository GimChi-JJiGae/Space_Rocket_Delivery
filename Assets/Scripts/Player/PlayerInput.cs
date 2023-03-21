using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public string move1AxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string move2AxisName = "Horizontal"; // 좌우 움직임을 위한 입력축 이름
    public string interactionButtonName = "Interaction"; // Obj를 집어들기 위한 입력 버튼 이름
    public string repairButtonName = "Repair"; // 모듈 수리를 위한 입력 버튼 이름
    public string destroyButtonName = "Destroy"; // 모듈 파괴를 위한 입력 버튼 이름

    public float Move_GoBack { get; private set; } // 감지된 움직임 입력값
    public float Move_LeftRight { get; private set; }
    public bool Interact { get; private set; }
    public bool RepairModule { get; private set; }
    public bool DestroyModule { get; private set; }

    private bool canInteract = true;
    private bool interacting = false;

    private void FixedUpdate()
    {
        // if (GameManager.instance != null && GameManager.instance.isGameover)
        // {
        //     Move_GoBack = 0;
        //     Move_LeftRight = 0;
        //     Interact = false;
        //     RepairModule = false;
        //     DestroyModule = false;
        //     return;
        // }

        Move_GoBack = Input.GetAxis(move1AxisName);
        Move_LeftRight = Input.GetAxis(move2AxisName);
        Interact = Input.GetButton(interactionButtonName);
        RepairModule = Input.GetButton(repairButtonName);
        DestroyModule = Input.GetButton(destroyButtonName);
    }

    public IEnumerator DisableInteractionForSeconds(float seconds)
    {
        canInteract = false;
        yield return new WaitForSeconds(seconds);
        canInteract = true;
    }

    private IEnumerator InteractCoroutine()
    {
        interacting = true;
        while (Interact)
        {
            yield return null;
        }
        interacting = false;
    }

    public bool CanInteract()
    {
        return canInteract;
    }

    public bool InteractPressed()
    {
        if (Interact && !interacting)
        {
            StartCoroutine(InteractCoroutine());
            return true;
        }
        return false;
    }
}