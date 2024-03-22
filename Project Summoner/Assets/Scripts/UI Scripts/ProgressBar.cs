using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private Image backgroundImage;

    private float progress;

    public void Start()
    {
        SetProgress(progress);
    }

    public Image GetImage() { return progressImage; }

    public void SetImage(Image image) { progressImage = image; }

    public float GetProgress() { return progress; }

    public void SetProgress(float progress)
    {
        this.progress = Mathf.Clamp(progress, 0f, 1f);
        Vector2 vec2 = new Vector2(backgroundImage.rectTransform.sizeDelta.x * this.progress, backgroundImage.rectTransform.sizeDelta.y);
        progressImage.rectTransform.sizeDelta = vec2;
    }
}
