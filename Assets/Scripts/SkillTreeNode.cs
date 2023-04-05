using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeNode : MonoBehaviour
{
    private GameManager gameManager;
    private int currentModuleCurrentHpLevel = 0;
    private int currentSupplySpeedLevel = 0;
    private float currentRepairSpeedLevel = 0;
    private int currentTurretDamageLevel = 0;
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
            module.hp++; // 체력 증가
        }
        moduleCurrentHpLevelText.text = "" + moduleHpUpgradeLevel;
        Debug.Log(moduleHpUpgradeLevel);
        gameManager.CloseSkillTree();
    }

    public void UpgradeRepairSpeed()
    {
        currentRepairSpeedLevel++;
        repairSpeedLevelText.text = "" + currentRepairSpeedLevel;
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

