using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitStatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Slider healthSlider;
    public Slider damageSlider;
    public Slider dpsSlider;
    public Slider costSlider;
    public Slider armorSlider;
    public Slider speedSlider;
    public Image spriteImage;

    public PolygonCollider2D polygon;
    public float startOffset;
    public float maxPolygonDistance;

    public Camera cam;
    public RawImage image;

    public void DisplayUnit(Unit unit) {
        nameText.text = unit.unitName;

        healthSlider.value = unit.HP;
        damageSlider.value = unit.Damage;
        dpsSlider.value = unit.Damage/unit.attackAnimation.length;
        costSlider.value = unit.Cost;
        armorSlider.value = unit.Armor;
        speedSlider.value = unit.baseSpeed;

        if (unit.identifierSprite != null)
        {
            spriteImage.enabled = true;
            spriteImage.sprite = unit.identifierSprite;
        }

        if (polygon != null) {
            SetStatsWheel();
            SetScreenShot();
        }
    }

    private void SetStatsWheel() {
        Vector2[] points = new Vector2[6];  //Hard setting 6 for now

        points[0] = GetPointPosition(healthSlider);
        points[1] = GetPointPosition(damageSlider);
        points[2] = GetPointPosition(dpsSlider);
        points[3] = GetPointPosition(costSlider);
        points[4] = GetPointPosition(armorSlider);
        points[5] = GetPointPosition(speedSlider);

        polygon.points = points;
        PolygonMesh2D polygonScript = polygon.GetComponent<PolygonMesh2D>();
        polygonScript.OnColliderUpdate();
    }

    private Vector2 GetPointPosition(Slider slider)
    {
        float distanceMultiplier = (slider.value - slider.minValue) / (slider.maxValue - slider.minValue);
        Debug.Log("Distance Multiplier = " + distanceMultiplier);

        Debug.Log("Slider angle = " + slider.transform.localRotation.eulerAngles.z);
        float angle = slider.transform.localRotation.eulerAngles.z * Mathf.Deg2Rad;

        float x = (distanceMultiplier * maxPolygonDistance + startOffset) * Mathf.Cos(angle);
        float y = (distanceMultiplier * maxPolygonDistance + startOffset) * Mathf.Sin(angle);

        return new Vector2(x, y);
    }

    private void SetScreenShot() {
        int width = 1080;
        int height = 1080;

        cam.gameObject.SetActive(true);
        // Create a RenderTexture to capture the camera view
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        cam.targetTexture = renderTexture;

        // Render the camera view
        cam.Render();

        // Set the RenderTexture as active and read pixels from it
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(width, height);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        // Display the captured image in the UI RawImage
        image.texture = screenshot;

        // Clean up
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        cam.gameObject.SetActive(false);
        image.gameObject.SetActive(true);
    }
}
