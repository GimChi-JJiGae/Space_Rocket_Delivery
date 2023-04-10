using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;



public enum PacketType
{
    NONE,
    HELLO,
    BYE,
    CREATE_ROOM,
    PARTICIPATE_USER,
    OUT_USER,
    PARTICIPATE_ROOM,
    MOVE,
    START_GAME,

    REPLICATION,

    OBJECT_MOVE,
    OBJECT_CONTROL,
    MODULE_CONTROL,
    MODULE_STATUS,
    CURRENT_POSITION,
    ENEMY_MOVE,
    TURRET_STATUS,
    BASIC_TURRET,       // 데이터 roomId, int userId, float rx1, float ry1, float rz1, float rw1, float rx, float ry, float rz, float rw

    MODULE_CREATE,      // 데이터 roomId, int moduleCode(1,2,3), int userId, int x, y     받는거: 그대로 받는다.
    MODULE_INTERACTION, // 데이터 roomId, int userId, int activeNum, 받는 거: 그대로 받는다.
    MODULE_REPAIR,      // 데이터 roomId, int userId, int x, int z,
    MODULE_UPGRADE,     // 데이터 roomId, int userId, int x, int z

    RESOURCE_CREATE,
    RESOURCE_MOVE,

    FACTORY_INPUT, // 데이터 roomId, int userId, int resourceType
    FACTORY_OUTPUT, // 데이터 roomId, int userId, int ore, int fuel,

    //NONE,
    //HELLO,
    //BYE,
    //CREATE_ROOM,
    //PARTICIPATE_USER, // 유저가 방에 입장한다는 것
    //DEPARTURE_USER,
    //PARTICIPATE_ROOM, // 방안에 있는 유저목록을 반환
    //MOVE,
    //MODULE_CONTROL,
    //REPLICATION,

    //OBJECT_MOVE,
    //OBJECT_CONTROL, 

    //MODULE_STATUS,
    //CURRENT_POSITION,
    ////ENEMY_MOVE,
    //TURRET_STATUS,
    //BASIC_TURRET,

    //ENEMY_MOVE = 199, // 적 생성
    //MODULE_CREATE = 200, // 모듈 생성
    //SUPPLIER_CREATE = 210, // SUPPLIER 오브젝트 생성
    //SUPPLIER_CHANGE = 211, // SUPPLIER 오브젝트 변경
    //RESOURCE_MOVE = 212, // 리소스 움직임

    //ENEMY_CREATE = 220, // 적 생성

};


public class DTOcreateRoom
{
    public string nickname;
    public string roomName;
    public bool active = false;

}
public class DTOuser    // 유저 방 Enter 이후
{
    public string roomName;
    public int userId;
    public float x, y, z, r;
    public string userNickName;
}

public class DTOresourcemove    // 자원 움직임
{
    public int idxR;
    public float px, py, pz;
    public float rx, ry, rz, rw;
}

public class DTOenemymove    // 적 움직임
{
    public int idxE;
    public int type;
    public float px, py, pz;
    public float rx, ry, rz, rw;
}

public class DTOgameStart
{
    public string roomCode;
}

public class DTOinteractionModule
{
    public string roomName;
    public int userId;
    public int activeNum;
}

public class DTOmoduleReapair  // 데이터 roomId, int userId, int x, int z,
{
    public string roomName;
    public int userId;
    public int x;
    public int z;
}

public class DTOmoduleUpgrade
{
    public string roomName;
    public int userId;
    public int x;
    public int z;
}

public class DTObasicTurret
{
    public string roomName;
    public int userId;
    public float rx1;
    public float ry1;
    public float rz1;
    public float rw1;

    public float rx2;
    public float ry2;
    public float rz2;
    public float rw2;
}
public class Controller : MonoBehaviour
{

    // 클라이언트의 개인 정보를 여기서 저장해보자
    public string roomCode;
    public int userId;
    public string userNickname;

    CreateRoomController createRoomController;      // 방 생성을 위한 컨트롤러
    EnterRoomController enterRoomController;        // 방 참가를 위한 컨트롤러
    CreateModuleController createModuleController;  // 모듈 추가를 위한 컨트롤러
    CreateResourceController createResourceController; // 자원 추가를 위한 컨트롤러
    MoveResourceController moveResourceController;  // 자원 위치를 위한 컨트롤러
    MoveEnemyController moveEnemyController;
    GameStartController gameStartController;
    // 포지션 변경을 위한 변수
    PlayerPositionController playerPositionController;

    // 기본 포탑 각도 변경
    BasicTurretControll basicTurretControll;


    /// =========================================
    InteractionModuleController interactionModuleController;

    RepairController repairController;

    ModuleUpgradeController moduleUpgradeController;

    FactoryOutputController factoryOutputController;
    /// </summary>

    // Player관련 함수
    Multiplayer multiplayer;
    MultiSpaceship multiSpaceship;
    MultiEnemy multiEnemy;
    GameObject server;

    SocketClient socketClient;
    MutiplayWaitingRoom waitingRoom;


    BasicTurretController basicTurretController;
    BasicTurretSpinController basicTurretSpinController;

    GameObject BasicTurret;
    GameObject BasicTurretHead;


    private void Awake()
    {
        // DontDestroyOnLoad 함수를 사용하여 해당 게임 오브젝트를 삭제하지 않도록 설정
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        GameObject socketObj = GameObject.Find("SocketClient");
        socketClient = socketObj.GetComponent<SocketClient>();
        try
        {
            MutiplayWaitingRoom waitingRoom = GetComponent<MutiplayWaitingRoom>();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("불러오기 실패");
        }
        // 필요한 컨트롤러 인스턴스 생성.
        createRoomController = new CreateRoomController();
        enterRoomController = new EnterRoomController();
        playerPositionController = new PlayerPositionController();
        createModuleController = new CreateModuleController();
        createResourceController = new CreateResourceController();
        moveResourceController = new MoveResourceController();
        moveEnemyController = new MoveEnemyController();
        gameStartController = new GameStartController();
        basicTurretControll = new BasicTurretControll();

        factoryOutputController = new FactoryOutputController();

        //===============================================================
        interactionModuleController = new InteractionModuleController();
        repairController = new RepairController();
        moduleUpgradeController = new ModuleUpgradeController();

        // 멀티플레이 관련 로직 
        //multiplayer = GetComponent<Multiplayer>();
        //multiSpaceship = GetComponent<MultiSpaceship>();
        //multiEnemy = GetComponent<MultiEnemy>();
    }
    private void Update()
    {
        if (server == null)
        {
            try
            {
                server = GameObject.Find("Server");
                multiplayer = server.GetComponent<Multiplayer>();
                multiSpaceship = server.GetComponent<MultiSpaceship>();
                multiEnemy = server.GetComponent<MultiEnemy>();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }


        if (multiplayer == null)
        {
            try
            {
                multiplayer = GameObject.Find ("Server").GetComponent<Multiplayer>();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        if (multiSpaceship == null)
        {
            try
            {
                multiSpaceship = GameObject.Find("Server").GetComponent<MultiSpaceship>();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        if (multiEnemy == null)
        {
            try
            {
                multiEnemy = GameObject.Find("Server").GetComponent<MultiEnemy>();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        BasicTurret = GameObject.Find("TurretHead");
        BasicTurretHead = GameObject.Find("TurretShooting");

        //basicTurretSpinController = BasicTurret.GetComponent<BasicTurretSpinController>();
        //basicTurretController = BasicTurretHead.GetComponent<BasicTurretController>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        createRoomController.Service();
        enterRoomController.Service();
        playerPositionController.Service(multiplayer);
        
        //createResourceController.Service(multiSpaceship);
        //moveResourceController.Service();
        //moveEnemyController.Service();

        if (multiSpaceship != null)
        {
            createModuleController.Service(multiSpaceship);
            interactionModuleController.Service(multiSpaceship);
            repairController.Service(multiSpaceship);
            moduleUpgradeController.Service(multiSpaceship);
        }

    }

    public void Receive(PacketType header, byte[] data)
    {
        
        try
        {
            Debug.Log("받았당" + (PacketType)header);
            switch (header)
            {
                
                case PacketType.CREATE_ROOM:
                    //createRoomController.ReceiveDTO(data);
                    //createRoomController.SetAct(true);
                    byte[] isCreateSucess = SplitArray(data, 0, 1);
                    int createRoomHead = 0;
                    DTOcreateRoom createRoom = new();
                    createRoomController.newReceiveDTO(data, createRoom, ref createRoomHead);
                    createRoom.active = true;
                    roomCode = createRoom.roomName;
                    userId = 0;

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {

                        PlayerPrefs.SetString("roomCode", createRoom.roomName);
                        Debug.Log(PlayerPrefs.GetString("roomCode"));

                        Debug.Log("컨트롤러에서 정보확인:" + roomCode);
                        SceneManager.LoadScene("WaitingRoom");
                    });

                    Debug.Log("방생성 수신");
                    
                    break;
                case PacketType.PARTICIPATE_ROOM:
                    byte[] isSucess = SplitArray(data, 0, 1);
                    byte[] userCount = SplitArray(data, 1, 4);
                    int head = 5;
                    //waitingRoom.userStringList = new List<string>();
                    for (int i = 0; i < BitConverter.ToInt32(userCount, 0); i++)
                    {
                        
                        DTOuser user = new();
                        enterRoomController.newReceiveDTO(data, user, ref head);
                        //waitingRoom.userStringList.Add(user.userNickName);
                        if (user.userNickName.Equals(userNickname))
                        {
                            userId = user.userId;
                            Debug.Log("반복문 돌면서 확인 몇번이나 찍히나");
                            Debug.Log(userId);
                        }
                        //Debug.Log(user.userNickName);
                        //Debug.Log(user.userId);
                        //Debug.Log(user.roomName);
                        //Debug.Log("--------");
                    }
                    Debug.Log("나자신의 정보");
                    Debug.Log(userNickname);
                    Debug.Log(userId);
                    Debug.Log(roomCode);
                    //enterRoomController.ReceiveDTO(data);
                    enterRoomController.SetAct(true);
                    break;
                case PacketType.START_GAME:
                    Debug.Log("게임시작");
                    DTOgameStart gameStart = new();
                    int GameHead = 0;
                    gameStartController.newReceiveDTO(data, gameStart, ref GameHead);
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        Debug.Log("======시작하는 방 확인, 내방, 시작 방");
                        Debug.Log(roomCode);
                        //Debug.Log(PlayerPrefs.GetString("roomCode"));
                        Debug.Log(gameStart.roomCode);
                        Debug.Log("======");
                        
                        SceneManager.LoadScene("New_Multiplay");
                        
                        
                    });
                    break;

                case PacketType.OUT_USER:
                    Debug.Log("방 나가기");
                    break;

                case PacketType.MOVE:
                    playerPositionController.ReceiveDTO(data);
                    playerPositionController.SetAct(true);
                    break;
                case PacketType.MODULE_CREATE:
                    createModuleController.ReceiveDTO(data);
                    createModuleController.SetAct(true);
                    break;
                case PacketType.RESOURCE_CREATE:
                    createResourceController.ReceiveDTO(data);
                    createResourceController.SetAct(true);
                    break;
                case PacketType.RESOURCE_MOVE:
                    try
                    {
                        byte[] resourceCount = SplitArray(data, 0, 4);
                        DTOresourcemove[] resourceList = new DTOresourcemove[BitConverter.ToInt32(resourceCount, 0)];
                        int head2 = 4;
                        for (int i = 0; i < BitConverter.ToInt32(resourceCount, 0); i++)
                        {
                            DTOresourcemove resource = new();
                            moveResourceController.newReceiveDTO(data, resource, ref head2);
                            resourceList[i] = resource;
                        }
                        multiSpaceship.ReceiveMoveResource(resourceList);
                        moveResourceController.SetAct(true);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    break;
                case PacketType.ENEMY_MOVE:
                    Debug.Log("ENEMY_MOVE");
                    if (true)//multiplayer.isHost == false
                    {
                        try
                        {
                            byte[] enemyCount = SplitArray(data, 0, 4);
                            DTOenemymove[] enemyList = new DTOenemymove[BitConverter.ToInt32(enemyCount, 0)];
                            int head3 = 4;
                            Debug.Log(BitConverter.ToInt32(enemyCount, 0));
                            for (int i = 0; i < BitConverter.ToInt32(enemyCount, 0); i++)
                            {
                                DTOenemymove resource = new();
                                moveEnemyController.newReceiveDTO(data, resource, ref head3);
                                enemyList[i] = resource;
                            }
                            multiEnemy.ReceiveMoveEnemy(enemyList);
                            moveEnemyController.SetAct(true);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                    break;


                case PacketType.MODULE_INTERACTION:

                    Debug.Log("모듈 인터랙션 수신");
                    DTOinteractionModule interactionModuleDto = new();
                    int interactionHead = 0;
                    interactionModuleController.newReceiveDTO(data, interactionModuleDto, ref interactionHead);
                    
                    interactionModuleController.roomId = interactionModuleDto.roomName;
                    interactionModuleController.activeNum = interactionModuleDto.activeNum;

                    interactionModuleController.SetAct(true);

                    //interactionModuleController.ReceiveDTO(data);
                    //interactionModuleController.SetAct(true);
                    break;
                case PacketType.MODULE_REPAIR:

                    Debug.Log("모듈 수리 수신");
                    DTOmoduleReapair moduleRepair = new DTOmoduleReapair();
                    int moduleRepairHead = 0;
                    repairController.newReceiveDTO(data, moduleRepair, ref moduleRepairHead);

                    repairController.roomId = moduleRepair.roomName;
                    repairController.id = moduleRepair.userId;
                    repairController.xIdx = moduleRepair.x;
                    repairController.zIdx = moduleRepair.z;

                    //repairController.ReceiveDTO(data);
                    //repairController.SetAct(true);
                    break;


                case PacketType.MODULE_UPGRADE:
                    Debug.Log("모듈 업그레이드 수신");

                    DTOmoduleUpgrade moduleUpgradeDto = new DTOmoduleUpgrade();
                    int moduleUpgradeHead = 0;
                    moduleUpgradeController.newReceiveDTO(data, moduleUpgradeDto, ref moduleUpgradeHead);

                    moduleUpgradeController.roomId = moduleUpgradeDto.roomName;
                    moduleUpgradeController.id = moduleUpgradeDto.userId;
                    moduleUpgradeController.x = moduleUpgradeDto.x;
                    moduleUpgradeController.z = moduleUpgradeDto.z;
                    //moduleUpgradeController.ReceiveDTO(data);
                    //moduleUpgradeController.SetAct(true);
                    break;

                case PacketType.BASIC_TURRET:
                    Debug.Log("베이스 터렛 수신");
                    DTObasicTurret basicTurretDto = new DTObasicTurret();
                    int basicTurretHead = 0;
                    Debug.Log("--------");
                    Debug.Log(basicTurretDto.roomName);
                    Debug.Log(basicTurretDto.userId);
                    Debug.Log(basicTurretDto.rx1);
                    Debug.Log(basicTurretDto.ry1);
                    Debug.Log(basicTurretDto.rz1);
                    Debug.Log(basicTurretDto.rw1);
                    Debug.Log(basicTurretDto.rx2);
                    Debug.Log(basicTurretDto.ry2);
                    Debug.Log(basicTurretDto.rz2);
                    Debug.Log(basicTurretDto.rw2);
                    basicTurretControll.newReceiveDTO(data, basicTurretDto, ref basicTurretHead);
                    if(userId != basicTurretDto.userId)
                    {
                        Debug.Log("베이스터렛 이동란에 들어옵니까?");

                        Quaternion target1 = new Quaternion(basicTurretDto.rx1, basicTurretDto.ry1, basicTurretDto.rz1, basicTurretDto.rw1);
                        Quaternion target2 = new Quaternion(basicTurretDto.rx2, basicTurretDto.ry2, basicTurretDto.rz2, basicTurretDto.rw2);
                        BasicTurret.transform.rotation = Quaternion.Lerp(BasicTurret.transform.rotation, target1,100.0f * Time.deltaTime);   // 수평이동
                        BasicTurretHead.transform.rotation = Quaternion.Lerp(BasicTurretHead.transform.rotation, target2 , 100.0f * Time.deltaTime);   // 수직이동
                    }
                    break;
            }

        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void Send(PacketType header, params object[] args)      // 인자를 object배열로 받아옴
    {
        List<byte> byteList = new()
        {
            // header 세팅. header를 해석하면 뒷단 정보 구조를 제공받을 수 있음
            (byte)header // BitConverter.GetBytes()
        };             // List를 byte로 받아옴

        // params 직렬화
        for (int i = 0; i < args.Length; i++)
        {
            object arg = args[i];
            Type type = arg.GetType();

            if (type.Equals(typeof(int)))
            {
                byteList.AddRange(BitConverter.GetBytes((int)arg));
            }
            else if (type.Equals(typeof(float)))
            {
                byteList.AddRange(BitConverter.GetBytes((float)arg));
            }
            else if (type.Equals(typeof(double)))
            {
                byteList.AddRange(BitConverter.GetBytes((double)arg));
            }
            else if (type.Equals(typeof(string))) 
            {
                byte[] stringBytes = Encoding.UTF8.GetBytes((string)arg); // 문자열을 바이트 배열로 변환
                byteList.AddRange(BitConverter.GetBytes(stringBytes.Length)); // 문자열 바이트 길이를 먼저 추가
                byteList.AddRange(stringBytes); // 문자열 바이트 배열 추가
            }
        }
        byte[] byteArray = byteList.ToArray();
        // 전송 시작
        Debug.Log("보낸당~");
        socketClient.Send(byteArray);
    }

    public void ListSend(PacketType header, List<object> args)      // 인자를 object배열로 받아옴
    {
        List<byte> byteList = new()
        {
            // header 세팅. header를 해석하면 뒷단 정보 구조를 제공받을 수 있음
            (byte)header // BitConverter.GetBytes()
        };             // List를 byte로 받아옴

        // params 직렬화
        for (int i = 0; i < args.Count; i++)
        {
            object arg = args[i];
            Type type = arg.GetType();

            if (type.Equals(typeof(int)))
            {
                byteList.AddRange(BitConverter.GetBytes((int)arg));
            }
            else if (type.Equals(typeof(float)))
            {
                byteList.AddRange(BitConverter.GetBytes((float)arg));
            }
            else if (type.Equals(typeof(double)))
            {
                byteList.AddRange(BitConverter.GetBytes((double)arg));
            }
            else if (type.Equals(typeof(string)))
            {
                byte[] stringBytes = Encoding.UTF8.GetBytes((string)arg); // 문자열을 바이트 배열로 변환
                byteList.AddRange(BitConverter.GetBytes(stringBytes.Length)); // 문자열 바이트 길이를 먼저 추가
                byteList.AddRange(stringBytes); // 문자열 바이트 배열 추가
            }
        }
        byte[] byteArray = byteList.ToArray();
        // 전송 시작
        //Debug.Log("보낸다: " + header);
        socketClient.Send(byteArray);
    }

    public byte[] SplitArray(byte[] array, int startIndex, int length)
    {
        byte[] result = new byte[length];
        Array.Copy(array, startIndex, result, 0, length);
        return result;
    }
}

// 유저 움직임
public class PlayerPositionController : ReceiveController
{
    public string roomCode;
    public int userId;
    public float px;
    public float py;
    public float pz;
    public float rx;
    public float ry;
    public float rz;
    public float rw;

    public void Service(Multiplayer multiplayer) // isAct가 활성화 되었을 때 실행할 로직
    {
        if (this.GetAct())
        {
            multiplayer.MoveOtherPlayer(roomCode, userId, px, py, pz, rx, ry,rz, rw);
            this.SetAct(false);
        }
    }
}

// CreateRoomController
public class CreateRoomController : ReceiveController
{
    public string text;      // 텍스트
    public string text2;      // 텍스트

    public new void Service() // isAct가 활성화 되었을 때 실행할 로직
    {
        if (this.GetAct())
        {
            // 여기에서 방이름을 로그로만 띄운 후
            Debug.Log(text);
            Debug.Log(text2);

            this.SetAct(false);
        }
    }
}

// EnterRoomController
public class EnterRoomController : ReceiveController
{
    public new void Service() // isAct가 활성화 되었을 때 실행할 로직
    {
        if (this.GetAct())
        {
            Debug.Log("여기 도착?");
            
            this.SetAct(false);
        }
    }
}

// ----------------------------------------------------------------------

// 모듈 컨트롤러
public class CreateModuleController : ReceiveController
{
    public int id;
    public int xIdx;           // 위치
    public int zIdx;

    public int moduleType;     // 모듈 타입

    public void Service(MultiSpaceship multiSpaceship) // isAct가 활성화 되었을 때 실행할 로직
    {
        if (this.GetAct())
        {
            Debug.Log("CreateModuleController : " + xIdx +", "+ zIdx + ", " + moduleType);
            multiSpaceship.CreateModule_RECEIVE(id, xIdx, zIdx, moduleType);
            this.SetAct(false);
        }
    }
}

public class InteractionModuleController : ReceiveController
{
    public string roomId;
    public int id;
    public int activeNum;

    public void Service(MultiSpaceship multiSpaceship) // isAct가 활성화 되었을 때 실행할 로직
    {
        Debug.Log("모듈 인터랙션 클래스 진입");
        if (GetAct())
        {
            Debug.Log("InteractionModule : 상호작용 함수 진입"); 
            Debug.Log(roomId);
            Debug.Log(id);
            Debug.Log(activeNum);

            switch (activeNum)
            {
                case 0:
                    multiSpaceship.ChangeResource_RECEIVE(id);
                    break;
                case 1:
                    multiSpaceship.ChangeModule_RECEIVE(id);
                    break;
                case 2:
                    //multiSpaceship.ProduceModule_RECEIVE(id);
                    break;
                case 3:
                    multiSpaceship.IncreaseOxygen_RECEIVE(id);
                    break;
                case 4:
                    multiSpaceship.Respawn_RECEIVE(id);
                    break;
            }

            SetAct(false);
        }
    }
}

public class RepairController : ReceiveController
{
    public string roomId;
    public int id;
    public int xIdx;
    public int zIdx;

    public void Service(MultiSpaceship multiSpaceship) // isAct가 활성화 되었을 때 실행할 로직
    {
        Debug.Log("리페어 컨트롤러");
        if (GetAct())
        {
            Debug.Log("Repair : 수리");

            multiSpaceship.Repair_RECEIVE(id, xIdx, zIdx);

            SetAct(false);
        }
    }
}

public class ModuleUpgradeController : ReceiveController
{
    public string roomId;
    public int id;
    public int x;
    public int z;

    public void Service(MultiSpaceship multiSpaceship) // isAct가 활성화 되었을 때 실행할 로직
    {
        Debug.Log("업그레이드 컨트롤러");
        if (GetAct())
        {
            Debug.Log("ModuleUpgrade : 업그레이드");

            multiSpaceship.ModuleUpgrade_RECEIVE(id, x, z);

            SetAct(false);
        }
    }
}

public class GameStartController : ReceiveController
{
    
}

// 자원 생성
public class CreateResourceController : ReceiveController
{
    public int rIdx;
    public void Service(MultiSpaceship multiSpaceship) // isAct가 활성화 되었을 때 실행할 로직
    {
        if (this.GetAct())
        {
            Debug.Log("CreateResourceController : 자원 생성");
            //multiSpaceship.ReceiveCreateResource(rIdx);
            this.SetAct(false);
        }
    }
}

// 자원 움직임
public class MoveResourceController : ReceiveController
{
    public void Service(MultiSpaceship multiSpaceship) // isAct가 활성화 되었을 때 실행할 로직
    {
        if (this.GetAct())
        {
            Debug.Log("MoveResourceController : 자원 위치 변경");
            //multiSpaceship.ReceiveChangeResource();
            this.SetAct(false);
        }
    }
}

// 적 움직임
public class MoveEnemyController : ReceiveController
{
    public new void Service() // isAct가 활성화 되었을 때 실행할 로직
    {
        if (this.GetAct())
        {
            //multiSpaceship.ReceiveChangeResource();
            this.SetAct(false);
        }
    }
}

public class FactoryOutputController : ReceiveController
{
    public int ore;
    public int fuel;
    public bool isMade;
    public int type = 4;

    public void Service(MultiSpaceship multiSpaceship)
    {
        if (GetAct())
        {
            multiSpaceship.FactoryInput_RECEIVE(ore, fuel, isMade, type);
            SetAct(false);
        }
    }
}

public class BasicTurretControll : ReceiveController
{

}

// 컨트롤러 정의
public class ReceiveController
{

    static public byte[] SplitArray(byte[] array, int startIndex, int length)
    {
        byte[] result = new byte[length];
        Array.Copy(array, startIndex, result, 0, length);
        return result;
    }

    private bool isAct = false;     // 활성화 되어있으면 실행시킨다.

    public void ReceiveDTO(byte[] data) // 데이터를 받으면 역직렬화 후 Class에 맞는 데이터로 변형시킨다.
    {
        Type typeClass = this.GetType();
        FieldInfo[] fields = typeClass.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); // 이 클래스를 참조하여 필요한 필드를 찾는다.

        int n = 0;
        foreach (FieldInfo field in fields) // 필드별로 돌며 역직렬화 한다.
        {

            Type type = field.FieldType;

            if (type.Equals(typeof(int)))
            {

                byte[] result = new byte[sizeof(int)];
                Array.Copy(data, n, result, 0, sizeof(int));
                field.SetValue(this, BitConverter.ToInt32(result));
                n += sizeof(int);
            }
            else if (type.Equals(typeof(float)))
            {
                byte[] result = new byte[sizeof(float)];
                Array.Copy(data, n, result, 0, sizeof(float));
                field.SetValue(this, BitConverter.ToSingle(result));
                n += sizeof(float);
            }
            else if (type.Equals(typeof(double)))
            {
                byte[] result = new byte[sizeof(double)];
                Array.Copy(data, n, result, 0, sizeof(double));
                field.SetValue(this, BitConverter.ToDouble(result));
                n += sizeof(double);
            }
            else if (type.Equals(typeof(string))) // 추가된 부분
            {
                
                byte[] stringLengthBytes = new byte[sizeof(int)];
                Array.Copy(data, n, stringLengthBytes, 0, sizeof(int));
                int stringLength = BitConverter.ToInt32(stringLengthBytes);
                n += sizeof(int);
                
                Debug.Log("스트링 렝스" + stringLength);
                byte[] stringBytes = new byte[stringLength];
                Array.Copy(data, n, stringBytes, 0, stringLength);
                
                n += stringLength;
                
                string stringValue = Encoding.UTF8.GetString(stringBytes);
                field.SetValue(this, stringValue);

      
                

            }
        }
    }

    public void newReceiveDTO<T>(byte[] data, T c, ref int mHead)
    {
        Type type = typeof(T);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            Type t = field.FieldType;
            if (t.Equals(typeof(int)))
            {
                //Console.WriteLine("인트형");
                
                //배열 헤드부터 4바이트 만큼 자름
                byte[] intByte = SplitArray(data, mHead, 4);
                field.SetValue(c, BitConverter.ToInt32(intByte));
                Debug.Log("인트");
                Debug.Log(BitConverter.ToInt32(intByte));

                //헤드 올림
                mHead += sizeof(int);
                //field.SetValue(c, 100);
            }
            else if (t.Equals(typeof(float)))
            {
                //Console.WriteLine("실수형");

                //배열 헤드부터 4바이트 만큼 자름
                byte[] floatByte = SplitArray(data, mHead, 4);
                field.SetValue(c, BitConverter.ToSingle(floatByte));
                Debug.Log("플로트");
                Debug.Log(BitConverter.ToInt32(floatByte));
                //헤드 올림
                mHead += sizeof(float);
                //field.SetValue(c, 100);
            }
            else if (t.Equals(typeof(string)))
            {
                //Console.WriteLine("문자열");
                //4바이트만큼 자름
                byte[] intByte = SplitArray(data, mHead, 4);
                int size = BitConverter.ToInt32(intByte);
                mHead += sizeof(int);


                //앞서 얻은 크기만큼 배열을 자른다.

                byte[] stringByte = SplitArray(data, mHead, size);
                string stringValue = Encoding.UTF8.GetString(stringByte);
                field.SetValue(c, stringValue);
                Debug.Log("스트링");
                Debug.Log(Encoding.UTF8.GetString(stringByte));

                mHead += size;
            }
        }

    }
 
    public bool GetAct()
    {
        return isAct;
    }

    public void SetAct(bool b)
    {
        isAct = b;
    }

    public void Service() { }
}