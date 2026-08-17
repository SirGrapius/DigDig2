using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AidsSlider : MonoBehaviour
{
    [SerializeField] GameStateManager gsManager;
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
        gsManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameStateManager>();

        if (sfxSlider)
        {
            mySlider.value = gsManager.sfxVolume * 100;
        }
        if (musicSlider)
        {
            mySlider.value = gsManager.musicVolume * 100;
        }
    }

    
    void Update()
    {
        text.text = sliderText + mySlider.value.ToString();
        if (sfxSlider)
        {
            gsManager.sfxVolume = mySlider.value / 100;
        }
        if (musicSlider)
        {
            gsManager.musicVolume = mySlider.value / 100;
        }
    }
}