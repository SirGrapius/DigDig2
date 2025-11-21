using UnityEngine;

public class Wiggle : MonoBehaviour
{
    Vector2 origin;
    [SerializeField] Vector2 WigglePos;
    [SerializeField] AnimationClip wiggleTime;
    [SerializeField] CottonScript top;
    float time;

    public void OriginPoint()
    {
        origin = transform.position;
    }
    void Update()
    {
        if (top.baseAnimator.GetBool("Adult"))
        {

            time += Time.deltaTime;
            if (time >= wiggleTime.length / 2)
            {
                transform.position = WigglePos + origin;
            }
            if (time >= wiggleTime.length)
            {
                transform.position = origin;
                time -= wiggleTime.length;
            }
        }
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
