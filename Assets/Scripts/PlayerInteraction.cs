using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    // 들 수 있는 모든 obj를 담을 liftableObjects를 리스트로 선언
    public List<GameObject> LiftableObjects = new();
    // character의 왼손, 오른손, 상호작용 할 수 있는 물체와의 최대 거리 선언
    public Transform leftHand;
    public Transform rightHand;
    public float maxDistance = 1f;

    [SerializeField] private GameObject fuelPrefab;
    [SerializeField] private GameObject orePrefab;
    //[SerializeField] private GameObject upgradePrefab;
    //[SerializeField] private GameObject laserPrefab;


    //public Text LiftPrompText;

    // currentObject(현재 물체)를 null로 선언
    private GameObject currentObject = null;

    private PlayerInput playerInput;
    private Animator playerAnimator;

    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        Mesh fuelMesh = fuelPrefab.GetComponent<MeshFilter>().sharedMesh;
        Mesh gasMesh = orePrefab.GetComponent<MeshFilter>().sharedMesh;
        //Mesh upgradeMesh = upgradePrefab.GetComponent<MeshFilter>().sharedMesh;
        //Mesh laserMesh = laserPrefab.GetComponent<MeshFilter>().sharedMesh;

        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            MeshFilter objMeshFilter = obj.GetComponent<MeshFilter>();

            if (objMeshFilter != null && (objMeshFilter.sharedMesh == fuelMesh ||
                objMeshFilter.sharedMesh == gasMesh))
            {
                LiftableObjects.Add(obj);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LiftableObjects.Contains(collision.gameObject))
        {
            currentObject = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        currentObject = null;
    }

    // Update is called once per frame
    private void Update()
    {
        // 상호작용 키를 눌렀을 때,
        if (playerInput.Interact)
        {
            // currentObject
            if (currentObject != null)
            {
                if (playerAnimator.GetBool("Lift") == false)
                {
                    playerAnimator.SetBool("Lift", true);
                    
                    currentObject.transform.parent = transform;
                    currentObject.transform.SetLocalPositionAndRotation(leftHand.localPosition, leftHand.localRotation);

                }
                else
                {
                    playerAnimator.SetBool("Lift", false);

                    currentObject.transform.parent = null;
                }
            }
        }
    }
}
