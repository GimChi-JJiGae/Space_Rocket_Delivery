using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public Animator popAnimator;

    public enum PrintType
    {
        Kit,
        Shotgun,
        Laser,
        Shield,
    }

    public PrintType currentType;

    private GameObject kitObject;
    private GameObject shotgunObject;
    private GameObject shieldObject;
    private GameObject laserObject;

    private GameObject kitModule;
    private GameObject shotgunModule;
    private GameObject laserModule;
    private GameObject shieldModule;

    public GameObject currentModule;

    public int neededOre;
    public int neededFuel;

    public int destroyOre = 0;
    public int destroyFuel = 0;

    // Start is called before the first frame update
    private void Start()
    {
        kitObject = transform.Find("KitBlueprint").gameObject;
        shotgunObject = transform.Find("ShotgunBlueprint").gameObject;
        shieldObject = transform.Find("ShieldBlueprint").gameObject;
        laserObject = transform.Find("LaserBlueprint").gameObject;

        shotgunObject.SetActive(false);
        shieldObject.SetActive(false);
        laserObject.SetActive(false);

        currentType = PrintType.Kit;
        currentModule = null;

        kitModule = Resources.Load<GameObject>("Resources/Kit");
        shotgunModule = Resources.Load<GameObject>("Resources/Shotgun");
        laserModule = Resources.Load<GameObject>("Rosources/Laser");
        shieldModule = Resources.Load<GameObject>("Resources/Shield");

        neededOre = 1;
        neededFuel = 1;
    }

    public void SwitchModule()
    {
        switch (currentType)
        {
            case PrintType.Kit:
                kitObject.SetActive(false);
                currentType = PrintType.Shotgun;
                shotgunObject.SetActive(true);
                neededOre = 1;
                neededFuel = 2;
                break;
            case PrintType.Shotgun:
                shotgunObject.SetActive(false);
                currentType = PrintType.Laser;
                laserObject.SetActive(true);
                neededOre = 2;
                neededFuel = 1;
                break;
            case PrintType.Laser:
                laserObject.SetActive(false);
                currentType = PrintType.Shield;
                shieldObject.SetActive(true);
                neededOre = 0;
                neededFuel = 3;
                break;
            case PrintType.Shield:
                shieldObject.SetActive(false);
                currentType = PrintType.Kit;
                kitObject.SetActive(true);
                neededOre = 1;
                neededFuel = 1;
                break;
        }
    }

    // Update is called once per frame
    public void ProduceModule()
    {
        switch (currentType)
        {
            case PrintType.Kit:
                if (destroyOre >= neededOre && destroyFuel >= neededFuel)
                {
                    currentModule = kitModule;
                    destroyOre -= neededOre;
                    destroyFuel -= neededFuel;
                }
                break;
            case PrintType.Shotgun:
                if (destroyOre >= neededOre && destroyFuel >= neededFuel)
                {
                    currentModule = shotgunModule;
                    destroyOre -= neededOre;
                    destroyFuel -= neededFuel;
                }
                break;
            case PrintType.Laser:
                if (destroyOre >= neededOre && destroyFuel >= neededFuel)
                {
                    currentModule = laserModule;
                    destroyOre -= neededOre;
                    destroyFuel -= neededFuel;
                }
                break;
            case PrintType.Shield:
                if (destroyOre >= neededOre && destroyFuel >= neededFuel)
                {
                    currentModule = shieldModule;
                    destroyOre -= neededOre;
                    destroyFuel -= neededFuel;
                }
                break;
        }

        float positionX = gameObject.transform.position.x;
        float positionZ = gameObject.transform.position.z;
        float positionY = gameObject.transform.position.y;

        Vector3 position = new(positionX, positionY, positionZ - 2);

        if (currentModule != null)
        {
            GameObject newModule = Instantiate(currentModule, position, Quaternion.identity);

            newModule.name = currentType.ToString();
            //popAnimator.Play("FactoryPopAnimation");
        }
        currentModule = null;
    }
}
