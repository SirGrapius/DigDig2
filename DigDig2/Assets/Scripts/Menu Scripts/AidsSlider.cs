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

    void Start()
    {
        mySlider = GetComponent<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        roundManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<RoundManager>();
    }

    
    void Update()
    {
        text.text = sliderText + mySlider.value.ToString();

        if ( sfxSlider )
        {
            roundManager.sfxVolume = mySlider.value;
        }
        if ( musicSlider )
        {
            roundManager.musicVolume = mySlider.value;
        }
    }

}
