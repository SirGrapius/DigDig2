using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AidsSlider : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Slider mySlider;
    [SerializeField] string sliderText;

    void Start()
    {
        mySlider = GetComponent<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    
    void Update()
    {
        text.text = sliderText + mySlider.value.ToString();
    }

}
