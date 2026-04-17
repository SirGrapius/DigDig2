using TMPro;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] RoundManager roundManager;
    [SerializeField] TextMeshPro dialogbox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (false)
        {
            roundManager.gameObject.SetActive(false);
        }
        else 
        { 
            dialogbox.gameObject.SetActive(false); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
