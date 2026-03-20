using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AidsSlider : MonoBehaviour
{
    [SerializeField] RoundManager roundManager;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Slider mySlider;
    [SerializeField] string sliderText;
    [SerializeField] bool sfxSlider;
    [SerializeField] bool musicSlider;

    private void Awake()
    {
        
    }

    void Start()
    {
        mySlider = GetComponent<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        roundManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<RoundManager>();

        if (sfxSlider)
        {
            mySlider.value = roundManager.sfxVolume * 10;
        }
        if (musicSlider)
        {
            mySlider.value = roundManager.musicVolume * 10;
        }
    }

    
    void Update()
    {
        text.text = sliderText + mySlider.value.ToString();
    }

    public void Save(sfxData sfxData, musicData musicData)
    {
        if (sfxSlider)
        {
            sfxData.sfxVolD = mySlider.value / 10;
        }
        if (musicSlider)
        {
            musicData.musicVolD = mySlider.value / 10;
        }
    }
}

[System.Serializable]

public struct sfxData
{
    public float sfxVolD;
}

public struct musicData
{
    public float musicVolD;
}