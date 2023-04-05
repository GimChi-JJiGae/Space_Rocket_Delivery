using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeNode : MonoBehaviour
{
    private GameManager gameManager;
    public int currentModuleCurrentHpLevel = 0;
    public int currentSupplySpeedLevel = 0;
    public float currentRepairSpeedLevel = 0;
    public int currentTurretDamageLevel = 0;
    public int moduleHpUpgradeLevel = 0;

    public TextMeshProUGUI moduleCurrentHpLevelText;
    public TextMeshProUGUI supplySpeedLevelText;
    public TextMeshProUGUI repairSpeedLevelText;
    public TextMeshProUGUI turretDamageLevelText;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        moduleCurrentHpLevelText.text = "" + moduleHpUpgradeLevel;
        supplySpeedLevelText.text = "" + currentSupplySpeedLevel;
        repairSpeedLevelText.text = "" + currentRepairSpeedLevel;
        turretDamageLevelText.text = "" + currentTurretDamageLevel;
    }

    public void UpgradeModuleHp()
    {
        moduleHpUpgradeLevel++;
        // 맵 내에 있는 모든 모듈의 체력 증가
        Module[] modules = FindObjectsOfType<Module>();
        foreach (Module module in modules)
        {
            module.maxHp += 1.0f; // 체력 증가
            module.hp += 1.0f; // 체력 증가
        }
        moduleCurrentHpLevelText.text = "" + moduleHpUpgradeLevel;
        Debug.Log(moduleHpUpgradeLevel);
        gameManager.CloseSkillTree();
    }

    public void UpgradeRepairSpeed()
    {
        currentRepairSpeedLevel++;
        repairSpeedLevelText.text = "" + currentRepairSpeedLevel;
        InteractionModule interactionModule = FindAnyObjectByType<InteractionModule>();
        interactionModule.repairSpeed += 0.1f;
        gameManager.CloseSkillTree();
        Debug.Log(currentRepairSpeedLevel);
    }

    public float GetRepairSpeedLevel()
    {
        return currentRepairSpeedLevel;
    }

    public void UpgradebaseTurret()
    {
        currentTurretDamageLevel++;
        turretDamageLevelText.text = "" + currentTurretDamageLevel;

        // 모든 BasicTurretDamage 오브젝트의 데미지를 업데이트
        BasicTurretDamage[] basicTurrets = FindObjectsOfType<BasicTurretDamage>();
        foreach (BasicTurretDamage turret in basicTurrets)
        {
            turret.damage = 1 + currentTurretDamageLevel;
        }
        Debug.Log(currentTurretDamageLevel);
        gameManager.CloseSkillTree();
    }

    public int GetCurrentTurretDamageLevel()
    {
        return currentTurretDamageLevel;
    }

    public void UpgradeSupplySpeed()
    {
        currentSupplySpeedLevel++;
        supplySpeedLevelText.text = "" + currentSupplySpeedLevel;

        // 보급기의 생성 속도를 줄입니다.
        Supplier[] suppliers = FindObjectsOfType<Supplier>();
        foreach (Supplier supplier in suppliers)
        {
            float newRespawnTime = 10 - currentSupplySpeedLevel;
            supplier.SetRespawnTime(newRespawnTime);
        }
        Debug.Log(currentSupplySpeedLevel);
        gameManager.CloseSkillTree();
    }
}
