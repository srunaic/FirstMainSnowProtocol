using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField]
    private GameObject Player; //��ũ��Ʈ ������ �÷��̾� ��ü

    public float waterDrag; // �� �� ���׷�
    public float originDrag; // �� �� ������ ���� ���׷�

    private Color originColor; // �� �� ������ ���� ���� Fog ����
    [SerializeField] private Color originNightColor;  // �� �� ������ ���� ���� Fog ����

    [SerializeField] private Color waterColor; // ���� �� �� Fog ����
    [SerializeField] private Color waterNightColor; // ���� �� �� Fog ����

    [SerializeField] private float waterFogDensity; // ���� �� �� Ź�� ����
    [SerializeField] private float waterNightFogDensity; // ���� �� �� Ź�� ����

    private float originFogDensity; // �� �� ������ ���� Ź�� ����
    [SerializeField] private float originNightFogDensity; // �� �� ������ ���� Ź�� ����

    [SerializeField] private float breatheTime;//�� �ȿ��� ȣ���ϴ� �ð�.
    private float currentBreatheTime;

    [SerializeField] private GameObject thePlayer;

    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterBreathe;

  
    void Start()
    { 
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity; //�Ȱ� ���װ�

        originDrag = thePlayer.GetComponent<Rigidbody>().drag;
    }

    void Update()
    {

        if (GameManager.isWater)
        {
            currentBreatheTime += Time.deltaTime;
            if (currentBreatheTime >= breatheTime)
            {
                //SoundManager.instance.PlaySE(sound_WaterBreathe);
                currentBreatheTime = 0;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        
            GetInWater(other);  // ���� ��
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
            GetOutWater(other);  // ������ ����
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
            SmoothColorChange();
    }

    public void SmoothColorChange()
    {
        if (!GameManager.isNight && RenderSettings.fogColor != waterColor)
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, waterColor, 0.1f);

        if (GameManager.isNight && RenderSettings.fogColor != waterNightColor)
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, waterNightColor, 0.1f);
    }

    public void GetInWater(Collider _player)
    {
        //SoundManager.instance.PlaySE(sound_WaterIn);

        GameManager.isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;

        if (!GameManager.isNight) // ���϶�
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else // ���϶�
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
    }

    public void GetOutWater(Collider _player)
    {
        //SoundManager.instance.PlaySE(sound_WaterOut);

        if (GameManager.isWater)
        {
            GameManager.isWater = false;
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;

            if (!GameManager.isNight) // ���϶�
            {
                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else // ���϶�
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
        }
    }





}