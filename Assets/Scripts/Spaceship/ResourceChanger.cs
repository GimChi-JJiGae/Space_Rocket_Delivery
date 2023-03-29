using UnityEngine;

namespace ResourceNamespace
{
    public enum ResourceType
    {
        Fuel,
        Ore,
    }

    public class ResourceChanger : MonoBehaviour
    {
        private GameObject fuelObject;
        private GameObject oreObject;

        private GameObject fuelPrefab;
        private GameObject orePrefab;

        public GameObject currentPrefab;

        ResourceType resourceType = ResourceType.Fuel;

        // Start is called before the first frame update
        private void Start()
        {
            fuelObject = transform.Find("Resource").Find("FuelBlueprint").gameObject;
            oreObject = transform.Find("Resource").Find("OreBlueprint").gameObject;

            fuelPrefab = Resources.Load<GameObject>("Resources/Fuel");
            orePrefab = Resources.Load<GameObject>("Resources/Ore");

            currentPrefab = fuelPrefab;

            oreObject.SetActive(false);
        }

        public void SwitchResource()
        {
            switch (resourceType)
            {
                case ResourceType.Fuel:
                    fuelObject.SetActive(false);
                    resourceType = ResourceType.Ore;
                    currentPrefab = orePrefab;
                    oreObject.SetActive(true);
                    break;
                case ResourceType.Ore:
                    oreObject.SetActive(false);
                    resourceType = ResourceType.Fuel;
                    currentPrefab = fuelPrefab;
                    fuelObject.SetActive(true);
                    break;
            }
        }
    }
}