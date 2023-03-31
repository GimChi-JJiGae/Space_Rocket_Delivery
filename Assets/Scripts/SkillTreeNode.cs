using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeNode : MonoBehaviour
{
    private void Start()
    {
        interactionModule = FindObjectOfType<InteractionModule>();
    }

    private int currentModuleCurrentHpLevel = 0;
    private int currentSupplySpeedLevel = 0;
    private float currentRepairSpeedLevel = 0;
    private int currentTurretDamageLevel = 0;
    private int moduleHpUpgradeLevel = 0;
    private InteractionModule interactionModule;

    public TextMeshProUGUI moduleCurrentHpLevelText;
    public TextMeshProUGUI supplySpeedLevelText;
    public TextMeshProUGUI repairSpeedLevelText;
    public TextMeshProUGUI turretDamageLevelText;

    public void UpgradeModuleHp()
    {
        moduleHpUpgradeLevel++; // 스킬 레벨 증가
        Debug.Log(moduleHpUpgradeLevel);
        Debug.Log('1');
        // 맵 내에 있는 모든 모듈의 체력 증가
        Module[] modules = GameObject.FindObjectsOfType<Module>();
        foreach (Module module in modules)
        {
            module.hp++; // 체력 증가
        }
        moduleCurrentHpLevelText.text = "" + moduleHpUpgradeLevel;
    }

    public void UpgradeRepairSpeed()
    {
        Debug.Log('2');
        currentRepairSpeedLevel++;
        repairSpeedLevelText.text = "" + currentRepairSpeedLevel;
    }

    public float GetRepairSpeedLevel()
    {
        return currentRepairSpeedLevel;
    }


    public void UpgradebaseTurret()
    {
        currentTurretDamageLevel++;
        turretDamageLevelText.text = "" + currentTurretDamageLevel;
        Debug.Log('4');
    }

    public void UpgradeSupplySpeed()
    {
        currentSupplySpeedLevel++;
        supplySpeedLevelText.text = "" + currentSupplySpeedLevel;
    }

}
