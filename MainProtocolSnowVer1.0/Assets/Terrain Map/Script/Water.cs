using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField]
    private GameObject Player; //스크립트 가져올 플레이어 객체

    public float waterDrag; // 물 속 저항력
    public float originDrag; // 물 밖 세상의 원래 저항력

    private Color originColor; // 물 밖 세상의 낮의 원래 Fog 색깔
    [SerializeField] private Color originNightColor;  // 물 밖 세상의 밤의 원래 Fog 색깔

    [SerializeField] private Color waterColor; // 낮의 물 속 Fog 색깔
    [SerializeField] private Color waterNightColor; // 밤의 물 속 Fog 색깔

    [SerializeField] private float waterFogDensity; // 낮의 물 속 탁한 정도
    [SerializeField] private float waterNightFogDensity; // 낮의 물 속 탁한 정도

    private float originFogDensity; // 물 밖 세상의 낮의 탁한 정도
    [SerializeField] private float originNightFogDensity; // 물 밖 세상의 밤의 탁한 정도

    [SerializeField] private float breatheTime;//물 안에서 호흡하는 시간.
    private float currentBreatheTime;

    [SerializeField] private GameObject thePlayer;

    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterBreathe;

  
    void Start()
    { 
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity; //안개 저항값

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
        
            GetInWater(other);  // 물에 들어감
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
            GetOutWater(other);  // 물에서 나옴
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

        if (!GameManager.isNight) // 낮일때
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else // 밤일때
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

            if (!GameManager.isNight) // 낮일때
            {
                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else // 밤일때
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
        }
    }





}