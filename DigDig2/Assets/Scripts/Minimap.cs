using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] RectTransform mapRect;
    [SerializeField] GameObject iconPrefab;
    [SerializeField] Camera minimapCamera;

    [Header("Settings")]
    [SerializeField] float mapRadiusMultiplier = 1f;

    [Header("Sprites")]
    [SerializeField] Sprite enemyCenterSprite;
    [SerializeField] float enemyCenterSize = 10f;
    [SerializeField] Color enemyCenterColor;

    [Space(10)]
    [SerializeField] Sprite enemyEdgeSprite;
    [SerializeField] float enemyEdgeSize = 10f;
    [SerializeField] Color enemyEdgeColor;

    [Space(10)]
    [SerializeField] Sprite playerIconSprite;
    [SerializeField] float playerIconSize = 10f;
    [SerializeField] Color playerIconColor;

    private RectTransform playerIcon;
    private List<Transform> enemies = new List<Transform>();
    private Dictionary<Transform, RectTransform> icons = new Dictionary<Transform, RectTransform>();

    float MapRadius => minimapCamera != null
        ? minimapCamera.orthographicSize * mapRadiusMultiplier
        : 50f;

    void Start()
    {
        GameObject obj = Instantiate(iconPrefab, mapRect);
        playerIcon = obj.GetComponent<RectTransform>();

        playerIcon.anchoredPosition = Vector2.zero;

        Image img = obj.GetComponent<Image>();
        if (img != null)
        {
            img.sprite = playerIconSprite;
            img.color = playerIconColor;
        }

        playerIcon.sizeDelta = new Vector2(playerIconSize, playerIconSize);
    }

    void Update()
    {
        UpdateIcons();
    }

    void LateUpdate()
    {
        if (player == null || minimapCamera == null) return;

        Vector3 pos = player.position;

        minimapCamera.transform.position = new Vector3(
            pos.x,
            pos.y,
            -50f 
        );
    }

    public void RegisterEnemy(Transform enemy)
    {
        if (icons.ContainsKey(enemy)) return;

        enemies.Add(enemy);

        GameObject iconObject = Instantiate(iconPrefab, mapRect);
        RectTransform iconRect = iconObject.GetComponent<RectTransform>();
        iconRect.sizeDelta = new Vector2(enemyCenterSize, enemyCenterSize);

        icons.Add(enemy, iconRect);
    }

    void UpdateIcons()
    {
        float mapRadius = MapRadius;

        if (playerIcon != null)
        {
            playerIcon.sizeDelta = new Vector2(playerIconSize, playerIconSize);
            playerIcon.anchoredPosition = Vector2.zero;
        }

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            Transform enemy = enemies[i];

            if (enemy == null)
            {
                if (icons.ContainsKey(enemy))
                {
                    Destroy(icons[enemy].gameObject);
                    icons.Remove(enemy);
                }

                enemies.RemoveAt(i);
                continue;
            }

            RectTransform icon = icons[enemy];
            Image iconImage = icon.GetComponent<Image>();

            Vector3 offset = enemy.position - player.position;
            float distance = offset.magnitude;

            Vector2 dir = new Vector2(offset.x, offset.y);

            Vector2 mapPos = (dir / mapRadius) * (mapRect.sizeDelta / 2f);

            if (distance <= mapRadius)
            {
                iconImage.sprite = enemyCenterSprite;
                iconImage.color = enemyCenterColor;

                icon.anchoredPosition = mapPos;
                icon.rotation = Quaternion.identity;

                icon.sizeDelta = new Vector2(enemyCenterSize, enemyCenterSize);
            }
            else
            {
                iconImage.sprite = enemyEdgeSprite;
                iconImage.color = enemyEdgeColor;

                Vector2 edgeDir = dir.normalized;
                Vector2 edgePos = edgeDir * (mapRect.sizeDelta.x / 2f);

                icon.anchoredPosition = edgePos;

                float angle = Mathf.Atan2(edgeDir.y, edgeDir.x) * Mathf.Rad2Deg;
                icon.rotation = Quaternion.Euler(0, 0, angle - 90f);

                icon.sizeDelta = new Vector2(enemyEdgeSize, enemyEdgeSize);
            }
        }
    }
}