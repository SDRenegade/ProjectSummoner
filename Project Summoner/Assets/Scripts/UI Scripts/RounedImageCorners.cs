using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO Come back to this class when you have time to look into Editor Scripts
public class RounedImageCorners : MonoBehaviour
{
    [SerializeField] private float topLeftCornerRadius;
    [SerializeField] private float topRightCornerRadius;
    [SerializeField] private float bottomLeftCornerRadius;
    [SerializeField] private float bottomRightCornerRadius;

    private Image image;

    public void Start()
    {
        image = GetComponent<Image>();

        //image
    }

    public void Update()
    {
        
    }
}
