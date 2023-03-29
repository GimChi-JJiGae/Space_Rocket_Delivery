using UnityEngine;
using UnityEngine.UI;

public class SkillTreeNode : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(UpgradeModuleHp); ;    // 방 생성
    }
    public enum SkillType
    {
        ModuleMaxHp,
        ModuleCurrentHp,
        SupplySpeed,
        RepairSpeed,
        TurretDamage,
        TurretAttackSpeed
    }

    public SkillType skillType;
    public int skillLevel;
    public int maxSkillLevel;

    public float moduleMaxHpIncrease;
    public float moduleCurrentHpIncrease;
    public float supplySpeedIncrease;
    public float repairSpeedIncrease;
    public float turretDamageIncrease;
    public float turretAttackSpeedIncrease;

    private float currentModuleMaxHpIncrease;
    private float currentModuleCurrentHpIncrease;
    private float currentSupplySpeedIncrease;
    private float currentRepairSpeedIncrease;
    private float currentTurretDamageIncrease;
    private float currentTurretAttackSpeedIncrease;
    private int moduleHpUpgradeLevel = 0;

    public void SelectSkill()
    {
        // 선택한 스킬에 따라서 적용할 변수를 변경한다.
        switch (skillType)
        {
            case SkillType.ModuleMaxHp:
                currentModuleMaxHpIncrease += moduleMaxHpIncrease;
                break;
            case SkillType.ModuleCurrentHp:
                currentModuleCurrentHpIncrease += moduleCurrentHpIncrease;
                break;
            case SkillType.SupplySpeed:
                currentSupplySpeedIncrease += supplySpeedIncrease;
                break;
            case SkillType.RepairSpeed:
                currentRepairSpeedIncrease += repairSpeedIncrease;
                break;
            case SkillType.TurretDamage:
                currentTurretDamageIncrease += turretDamageIncrease;
                break;
            case SkillType.TurretAttackSpeed:
                currentTurretAttackSpeedIncrease += turretAttackSpeedIncrease;
                break;
            default:
                break;
        }
    }

    public float GetModuleMaxHpIncrease()
    {
        return currentModuleMaxHpIncrease;
    }

    public void UpgradeModuleHp()
    {
        moduleHpUpgradeLevel++; // 스킬 레벨 증가
        Debug.Log('ㅁ');
        // 맵 내에 있는 모든 모듈의 체력 증가
        Module[] modules = GameObject.FindObjectsOfType<Module>();
        foreach (Module module in modules)
        {
            module.hp++; // 체력 증가
        }
    }

    public void UpgradeFixSpeed()
    {
        Debug.Log('ㅁ');
    }

    public void UpgradeSupplier()
    {
        Debug.Log('ㅁ');
    }

    public void UpgradebaseTurret()
    {
        Debug.Log('ㅁ');
    }
    public float GetModuleCurrentHpIncrease()
    {
        return currentModuleCurrentHpIncrease;
    }

    public float GetSupplySpeedIncrease()
    {
        return currentSupplySpeedIncrease;
    }

    public float GetRepairSpeedIncrease()
    {
        return currentRepairSpeedIncrease;
    }

    public float GetTurretDamageIncrease()
    {
        return currentTurretDamageIncrease;
    }

    public float GetTurretAttackSpeedIncrease()
    {
        return currentTurretAttackSpeedIncrease;
    }
}
