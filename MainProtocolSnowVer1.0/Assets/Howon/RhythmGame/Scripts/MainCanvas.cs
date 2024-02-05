using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    private Slider _sliderHpBar;
    private Text _txtScore;

    private void Awake()
    {
        _sliderHpBar = transform.Find("HpBar").GetComponent<Slider>();
        _txtScore = transform.Find("Score").GetComponent<Text>();
    }

    private void OnEnable()
    {
        _sliderHpBar.value = 1000;
    }
}
