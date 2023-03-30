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

    private GameObject kitModule;
    private GameObject shotgunModule;
    private GameObject laserModule;
    private GameObject shieldModule;

    public GameObject currentModule;

    private int neededOre;
    private int neededFuel;

    // Start is called before the first frame update
    private void Start()
    {
        currentType = PrintType.Kit;
        currentModule = null;

        kitModule = Resources.Load<GameObject>();
        shotgunModule = Resources.Load<GameObject>();
        laserModule = Resources.Load<GameObject>();
        shieldModule = Resources.Load<GameObject>();

        neededOre = 0;
        neededFuel = 0;
    }

    public void SwitchModule()
    {
        switch (currentType)
        {
            case PrintType.Kit:
                currentType = PrintType.Shotgun;
                neededOre = 1;
                neededFuel = 2;
                break;
            case PrintType.Shotgun:
                currentType = PrintType.Laser;
                neededOre = 2;
                neededFuel = 1;
                break;
            case PrintType.Laser:
                currentType = PrintType.Shield;
                neededOre = 0;
                neededFuel = 3;
                break;
            case PrintType.Shield:
                currentType = PrintType.Kit;
                neededOre = 1;
                neededFuel = 1;
                break;
        }
    }

    // Update is called once per frame
    private void ProduceModule()
    {
        Transform factoryInput = transform.Find("Input");

        Component[] childComponents = factoryInput.GetComponentsInChildren<Component>();

        int ore = childComponents.Count(child => child.name == "Ore");
        int fuel = childComponents.Count(child => child.name == "Fuel");

        switch (currentType)
        {
            case PrintType.Kit:
                if (ore >= neededOre && fuel >= neededFuel)
                {
                    ore -= neededOre;
                    fuel -= neededFuel;
                    currentModule = kitModule;
                }
                break;
            case PrintType.Shotgun:
                if (ore >= neededOre && fuel >= neededFuel)
                {
                    ore -= neededOre;
                    fuel -= neededFuel;
                    currentModule = shotgunModule;
                }
                break;
            case PrintType.Laser:
                if (ore >= neededOre && fuel >= neededFuel)
                {
                    ore -= neededOre;
                    fuel -= neededFuel;
                    currentModule = laserModule;
                }
                break;
            case PrintType.Shield:
                if (ore >= neededOre && fuel >= neededFuel)
                {
                    ore -= neededOre;
                    fuel -= neededFuel;
                    currentModule = shieldModule;
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

            newModule.name = currentModule.ToString();
            popAnimator.Play("FactoryPopAnimation");
        }

        currentModule = null;
    }

}
