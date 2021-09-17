using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchUI : MonoBehaviour
{
    public Image circleSlider;
    public Text coolDownSec;

    public bool IsFinish;
    public float coolDownTime;
    private bool canSlider;
    private float updateTime;
    private float coolTimeFloat;


    private void OnEnable()
    {
        IsFinish = false;
        canSlider = true;
        updateTime = 0.0f;
        coolTimeFloat = coolDownTime;
    }



    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        updateUI();
    }
 
    void updateUI()
    {
        if(canSlider)
        {
            updateTime = updateTime + Time.deltaTime;
            coolDownSec.text = string.Format("{0:0.0}", coolTimeFloat - updateTime);

            circleSlider.fillAmount = 1.0f - (Mathf.SmoothStep(0, 100, updateTime / coolDownTime) / 100);

            if(updateTime > coolDownTime)
            {
                circleSlider.fillAmount = 0.0f;
                updateTime = 0.0f;
                canSlider = false;
                IsFinish = true;
                updateTime = 0.0f;
                this.gameObject.SetActive(false);
            }
        }
    }





}
