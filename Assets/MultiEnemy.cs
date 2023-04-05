
public class MultiEnemy : MonoBehaviour
{
    Controller controller;
    Multiplayer multiplayer; // ��Ƽ�÷������� Ȯ���ϴ� ����

    public GameObject[] enemyeList = new GameObject[10000];
    Vector3[] targetPosition = new Vector3[10000];
    Quaternion[] targetRotation = new Quaternion[10000];
    public int enemyCount = 0;

    public GameObject[] enemies; // �������� �����ص� ����

    void Start()
    {
        multiplayer = GetComponent<Multiplayer>();
        controller = GetComponent<Controller>();
        StartCoroutine(SendPositionEnemy());
    }

    void FixedUpdate()
    {
        if (multiplayer.isHost == false)
        {
            for (int i = 0; i < enemyeList.Length; i++)
            {
                if (enemyeList[i] != null)
                {
                    Vector3 v = (targetPosition[i] - enemyeList[i].transform.position) * 5.0f * Time.deltaTime;
                    enemyeList[i].transform.position += v;
                    enemyeList[i].transform.rotation = Quaternion.Lerp(targetRotation[i], enemyeList[i].transform.rotation, 0.1f * Time.deltaTime);
                }
            }
        }
    }


    IEnumerator SendPositionEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 0.1�ʸ��� �ݺ�
                                                   // �ݺ��ؼ� ȣ���� �Լ� ȣ��
            try
            {
                if (multiplayer.isMultiplayer && multiplayer.isHost == true)
                {
                    List<object> list = new List<object>();

                    int count = 0;
                    for (int i = 0; i < enemyCount; i++)
                    {
                        if (enemyeList[i] != null)
                        {
                            count++;
                            list.Add((int)i);
                            list.Add((int)0); // Ÿ���� ���� �����ų ����
                            Vector3 a = enemyeList[i].transform.position;
                            list.Add((float)a.x);
                            list.Add((float)a.y);
                            list.Add((float)a.z);
                            Quaternion q = enemyeList[i].transform.rotation;
                            list.Add((float)q.x);
                            list.Add((float)q.y);
                            list.Add((float)q.z);
                            list.Add((float)q.w);
                        }
                    }
                    List<object> sendList = new List<object>();
                    sendList.Add((int)count);
                    sendList.AddRange(list);
                    //controller.ListSend(PacketType.ENEMY_MOVE, sendList);
                }
            }
            catch
            {

            }
            
        }
    }

    public void ReceiveMoveEnemy(DTOenemymove[] DTOenemymove)
    {

        Debug.Log("�޾Ҵ� �̰���");
        try
        {
            for (int i = 0; i < DTOenemymove.Length; i++)
            {
                if (enemyeList[DTOenemymove[i].idxE] == null)   // null ��ü��
                {
                    spawnEnemy(DTOenemymove[i]);
                    Vector3 v = new Vector3(DTOenemymove[i].px, DTOenemymove[i].py, DTOenemymove[i].pz);
                    Quaternion q = new Quaternion(DTOenemymove[i].rx, DTOenemymove[i].ry, DTOenemymove[i].rz, DTOenemymove[i].rw);
                    targetPosition[DTOenemymove[i].idxE] = v;
                    targetRotation[DTOenemymove[i].idxE] = q;
                }
                else                                            // �����ϸ�
                {
                    Vector3 v = new Vector3(DTOenemymove[i].px, DTOenemymove[i].py, DTOenemymove[i].pz);
                    Quaternion q = new Quaternion(DTOenemymove[i].rx, DTOenemymove[i].ry, DTOenemymove[i].rz, DTOenemymove[i].rw);
                    targetPosition[DTOenemymove[i].idxE] = v;
                    targetRotation[DTOenemymove[i].idxE] = q;
                }
            }
        }
        catch
        {
            Debug.Log("�����ΰ�..");
        }
    }

    public void spawnEnemy(DTOenemymove DTOenemymove)
    {
        /*
        GameObject[] currentEnemies;
        int[] currentEnemyHealths;
        
        if (difficultyLevel == 0)
        {
            currentEnemies = enemies;
        }
        else if (difficultyLevel == 1)
        {
            currentEnemies = enemiesTier2;
        }
        else
        {
            currentEnemies = enemiesTier3;
        }

        if (currentEnemies.Length == 0)
        {
            Debug.LogError("currentEnemies �迭�� ����ֽ��ϴ�.");
            return;
        }
        */
        GameObject enemy;
        /* �� ã��
        GameObject closestWall = FindClosestWall();
        if (closestWall == null)
        {
            Debug.LogError("���� ����� ���� ã�� �� �����ϴ�.");
            return;
        }*/

        Vector3 v = new Vector3(DTOenemymove.px, DTOenemymove.py, DTOenemymove.pz);
        Quaternion q = new Quaternion(DTOenemymove.rx, DTOenemymove.ry, DTOenemymove.rz, DTOenemymove.rw);
        enemy = Instantiate(enemies[0], v, q);
        enemy.name = "���̴ٱ���";
        /*
        if (enemy.GetComponent<RangedEnemyController>() != null)
        {
            RangedEnemyController rangedController = enemy.GetComponent<RangedEnemyController>();
            rangedController.target = closestWall;
        }
        else
        {
            controller.spawner = this; // spawner�� �������ּ���.
            controller.target = closestWall;
            controller.enemyDestroyedSound = enemyDestroyedSound;
        }
        */

        BoxCollider collider = enemy.AddComponent<BoxCollider>();
        if (enemy.GetComponent<RangedEnemyController>() != null)
        {
            collider.size = new Vector3(0.7f, 0.7f, 0.7f); // ���Ÿ� ���� ��� ������ ũ��� ����
        }
        else
        {
            collider.size = new Vector3(0.5f, 0.5f, 0.5f); // �ٰŸ� ���� ��� ������ ũ��� ����
        }

        // �߷��� rigidboy�� �޴´�.
        /*
        Rigidbody rb = enemy.AddComponent<Rigidbody>();
        rb.useGravity = false;

        
        Vector3 direction = (closestWall.transform.position - enemy.transform.position).normalized;
        Vector3 velocity = direction * speed;

        rb.velocity = velocity;
        rb.freezeRotation = true;

        enemy.transform.rotation = Quaternion.LookRotation(direction);
        */
        enemyeList[DTOenemymove.idxE] = enemy;
    }
}
