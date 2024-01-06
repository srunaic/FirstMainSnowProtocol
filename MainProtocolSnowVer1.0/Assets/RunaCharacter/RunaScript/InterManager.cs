using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public enum KindBuilding
{
  NoneBuild,
  GameBuild
}
public enum GameJoy 
{
  None,
  DollGame,
  SongGame,
  ShootingGame
}
public class InterManager : MonoBehaviour
{
    [Header("���ӱ� ����")]
    public KindBuilding _ContactBuild;
    public GameJoy _JoyGame;

    public FbsControler player;
    public Seat nearSeat;//�����̿� �ִ� ���ڸ� ����.
    public LiftMoving DollGameObj;//�������ӱ⿡ ������ �ߴٸ�,
    public BuildingInterActive _building;

    [Header("���̵� �� �ƿ�")]
    [SerializeField]
    private FaidInOut FadeInOut;
   
    [Header("�����̱� UI �ð��� ����")]
    public TextMeshProUGUI _TimeTxt;
    [SerializeField]
    private float TimeGames;

    void Start()
    {
          TimeGames = 0;
        _TimeTxt.text = "0:20";//�ð� ��

        _ContactBuild = KindBuilding.NoneBuild;
        _JoyGame = GameJoy.None;//���� ���ӱ� ���� ����

        player = FindObjectOfType<FbsControler>();
        FadeInOut = FindObjectOfType<FaidInOut>();
        _building = FindObjectOfType<BuildingInterActive>();
    }

    void FixedUpdate()
    {   //�����̱� ��� ĵ��
        if (_JoyGame != GameJoy.None && Input.GetKeyDown(KeyCode.C))
        {
            _JoyGame = GameJoy.None;
            player.onMoveable = true;
            DollGameObj.DollGameCam.SetActive(false);//�����̱� ķ ��Ȱ��ȭ.
        }

        DollGameTimes();

    }
    void Update()
    {
        if (nearSeat != null && Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(SetSit());
        }
        else if (player.OnSit && Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(OffSit());
        }
        else if (_JoyGame == GameJoy.None && DollGameObj !=null
            && Input.GetKeyDown(KeyCode.Z))
        {
            _JoyGame = GameJoy.DollGame;//������� ���ӱ� ��ȯ.
           StartCoroutine(DollGame());
        }
    }

    IEnumerator SetSit()//�ɱ�
    {
        player.onMoveable = false;

        transform.rotation = nearSeat.sitPos.rotation;
        player.RunaAnim.SetTrigger("Sit");

        yield return new WaitForSeconds(0.5f);
        player.OnSit = true;

    }
    IEnumerator OffSit()//�ɴ�Ű ����
    {
        player.RunaAnim.ResetTrigger("Sit");
        player.RunaAnim.SetTrigger("Stend");

        yield return new WaitForSeconds(1f);
        player.onMoveable = true;
        player.OnSit = false;
        //�ִϸ��̼� �߰�.
    }
    //���� ����
    IEnumerator DollGame()
    {
        player.onMoveable = false;
        transform.position = DollGameObj.LiftPos.position;

        if(_JoyGame == GameJoy.DollGame)//�����̱� ��� Ȱ��ȭ.
        {
            yield return new WaitForSeconds(1f);
            FadeInOut.Fade();
            DollGameObj.DollGameCam.SetActive(true);//�����̱� ķ Ȱ��ȭ.
        }
    }
    //���� �ð� ����
    void DollGameTimes()
    {
        if (_JoyGame == GameJoy.DollGame)//�����̱� ��� Ȱ��ȭ.
        {
            float timeIncrement = 1.0f; //�ð��� ����
            TimeGames += timeIncrement * Time.deltaTime;

            int roundedTime = Mathf.RoundToInt(TimeGames);//1�� ���� 1 ��ŭ �ð� �帧.
            _TimeTxt.text = roundedTime + ":20";
        }
        if (TimeGames >= 20)
        {
            TimeGames = 0;
            StartCoroutine(NoneGame(2f));
            DollGameObj.DollGameCam.SetActive(false);
            player.onMoveable = true;
        }
 
    }

    //��ȣ�ۿ� ��ü ����.
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Seat>(out Seat findSeat))
        {
            nearSeat = findSeat;
        }
        else if (other.TryGetComponent<LiftMoving>(out LiftMoving findDollGame))
        {
            DollGameObj = findDollGame;
            Debug.Log("���ӱ�� ����" + findDollGame);
        }
        else if (other.TryGetComponent<BuildingInterActive>(out BuildingInterActive _buildActive))
        {
            _building = _buildActive;
            Debug.Log("�����̶� ����" + _buildActive);

            _ContactBuild = KindBuilding.GameBuild;
            if(_ContactBuild == KindBuilding.GameBuild) 
            {
                _building.BuildAnim.SetTrigger("RightDoor");
                _building.BuildAnim.SetTrigger("LeftDoor");
                _building.col.isTrigger = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Seat>(out Seat findSeat))
        {
            if (nearSeat == findSeat)
            {
               nearSeat = null;
            }
        }
        else if (other.TryGetComponent<LiftMoving>(out LiftMoving findDollGame))
        {
            if (DollGameObj = findDollGame)
            {
                DollGameObj = null;
            }
        }
        //�����̶� ��ȣ�ۿ�.
        else if (other.TryGetComponent<BuildingInterActive>(out BuildingInterActive _buildActive))
        {
            StartCoroutine(DoorAnim(2f)); //DistanceDoor ������ Ÿ�ְ̹� �ӵ���.
            _ContactBuild = KindBuilding.NoneBuild;
        }
    }
    IEnumerator NoneGame(float RateGameTime)//�����ʱ�ȭ.
    {
        _JoyGame = GameJoy.None;
        yield return new WaitForSeconds(RateGameTime);
    
    }
    //���� ������ Ÿ�ְ̹� �浹ü�� Ÿ�̹� ����Ʈ
    IEnumerator DoorAnim(float DisatanceDoor)
    {
         if (_ContactBuild == KindBuilding.NoneBuild)
         {
                yield return new WaitForSeconds(DisatanceDoor);
                _building.BuildAnim.SetTrigger("CloseRightDoor");
                _building.BuildAnim.SetTrigger("CloseLeftDoor");
                _building.col.isTrigger = false;

         }
        
    }
}
