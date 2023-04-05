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

        public ResourceType resourceType;

        // Start is called before the first frame update
        private void Start()
        {
            //multiSpaceship = GameObject.Find("Server").GetComponent<MultiSpaceship>();

            fuelObject = transform.Find("FuelBlueprint").gameObject;
            oreObject = transform.Find("OreBlueprint").gameObject;

            oreObject.SetActive(false);

            resourceType = ResourceType.Fuel;
        }

        public void SwitchResource()
        {
            switch (resourceType)
            {
                case ResourceType.Fuel:
                    fuelObject.SetActive(false);
                    resourceType = ResourceType.Ore;
                    oreObject.SetActive(true);
                    break;
                case ResourceType.Ore:
                    oreObject.SetActive(false);
                    resourceType = ResourceType.Fuel;
                    fuelObject.SetActive(true);
                    break;
            }
        }
    }
}