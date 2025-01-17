﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMaanage : MonoBehaviour
{
    public RectTransform btn;
    public RectTransform btn2;
    public RectTransform btn3;
    public GameObject circleGuidanceControllerImage;
    public GameObject rectGuidanceControllerImage;
    public GameObject ovalGuidanceControllerImage;
    public CircleGuidanceController circleGuidanceController;
    public RectGuidanceController rectGuidanceController;
    public OvalGuidanceController ovalGuidanceController;
    // Start is called before the first frame update
    void Start()
    {
        circleGuidanceControllerImage.SetActive(false);
        rectGuidanceControllerImage.SetActive(false);
        ovalGuidanceControllerImage.SetActive(false);
        // circon.SetTarget(btn);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            circleGuidanceController.SetTarget(btn);
            circleGuidanceControllerImage.SetActive(true);
            rectGuidanceControllerImage.SetActive(false);
            ovalGuidanceControllerImage.SetActive(false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rectGuidanceController.SetTarget(btn2);
            rectGuidanceControllerImage.SetActive(true);
            circleGuidanceControllerImage.SetActive(false);
            ovalGuidanceControllerImage.SetActive(false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ovalGuidanceController.SetTarget(btn3);
            ovalGuidanceControllerImage.SetActive(true);
            rectGuidanceControllerImage.SetActive(false);
            circleGuidanceControllerImage.SetActive(false);
        }
    }
  
    public void Btn1()
    {
        Debug.Log("Btn1");
    }
    public void Btn2()
    {
        Debug.Log("Btn2");
    }

    public void Btn3()
    {
        Debug.Log("Btn3");
    }
}
