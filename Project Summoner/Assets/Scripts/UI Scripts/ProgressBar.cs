using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private Image backgroundImage;
    [Range(0f, 1f)]
    [SerializeField] private float progress;


    void Start()
    {
        SetProgress(progress);
    }

    public Image GetImage() { return progressImage; }

    public void SetImage(Image image) {  progressImage = image; }

    public float GetProgress() { return progress; }

    public void SetProgress(float progress)
    {
        this.progress = Mathf.Clamp(progress, 0f, 1f);
        progressImage.rectTransform.sizeDelta = new Vector2(backgroundImage.rectTransform.sizeDelta.x * this.progress, backgroundImage.rectTransform.sizeDelta.y);
    }
}
