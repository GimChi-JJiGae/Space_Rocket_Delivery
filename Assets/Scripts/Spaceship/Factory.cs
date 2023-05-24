using Unity.VisualScripting;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private MultiSpaceship multiSpaceship;

    private Controller controller;
    private GameObject socketObj;

    public enum PrintType
    {
        Shotgun,
        Laser,
        Shield,
        Kit,
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

    public TextMesh oreText;
    public TextMesh fuelText;

    // Start is called before the first frame update
    private void Start()
    {
        socketObj = GameObject.Find("SocketClient");
        controller = socketObj.GetComponent<Controller>();

        shotgunObject = transform.Find("ShotgunBlueprint").gameObject;
        shieldObject = transform.Find("ShieldBlueprint").gameObject;
        laserObject = transform.Find("LaserBlueprint").gameObject;
        kitObject = transform.Find("KitBlueprint").gameObject;

        kitObject.SetActive(false);
        //shotgunObject.SetActive(false);
        shieldObject.SetActive(false);
        laserObject.SetActive(false);
        kitObject.SetActive(false);

        currentType = PrintType.Shotgun;
        currentModule = null;

        shotgunModule = Resources.Load<GameObject>("Resources/Shotgun");
        laserModule = Resources.Load<GameObject>("Resources/Laser");
        shieldModule = Resources.Load<GameObject>("Resources/Shield");
        kitModule = Resources.Load<GameObject>("Resources/Kit");

        neededOre = 1;
        neededFuel = 2;

        multiSpaceship = GameObject.Find("Server").GetComponent<MultiSpaceship>();
    }
    // Update is called once per frame
    private void Update()
    {
        oreText.text = "광석: " + destroyOre + " / " + neededOre;
        fuelText.text = "연료: " + destroyFuel + " / " + neededFuel;
    }

    public void SwitchModule(int id)
    {
        switch (currentType)
        {
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
            case PrintType.Kit:
                kitObject.SetActive(false);
                currentType = PrintType.Shotgun;
                shotgunObject.SetActive(true);
                neededOre = 1;
                neededFuel = 2;
                break;
        }

        if (controller.userId == id)
        {
            multiSpaceship.ChangeModule_SEND(id);
        }
    }

    // Update is called once per frame
    public void ProduceModule(int type)
    {
        switch (type)
        {
            case (int)PrintType.Kit:
                currentModule = kitModule;
                break;
            case (int)PrintType.Shotgun:
                currentModule = shotgunModule;
                break;
            case (int)PrintType.Laser:
                currentModule = laserModule;
                break;
            case (int)PrintType.Shield:
                currentModule = laserModule;
                break;
        }

        float positionX = gameObject.transform.position.x;
        float positionY = gameObject.transform.position.y;
        float positionZ = gameObject.transform.position.z;

        Vector3 position = new(positionX, positionY, positionZ - 2);

        if (currentModule != null)
        {
            GameObject newModule = Instantiate(currentModule, position, Quaternion.identity);

            newModule.name = currentType.ToString();

            currentModule = null;
        }
    }
}
