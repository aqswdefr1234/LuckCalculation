using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator
{
    //fillOrigin : 0은 왼쪽(수평) 또는 하단(수직)을 나타내며, 1은 오른쪽(수평) 또는 상단(수직)
    public void Fill(Image fillImage, float fillDuration, Image.FillMethod fillmethod, int fillOrigin)
    {
        CoroutineManager.Instance.RunCoroutine(FillImage(fillImage, fillDuration, fillmethod, fillOrigin));
    }
    public void FillAndCb(Image fillImage, float fillDuration, Image.FillMethod fillmethod, int fillOrigin, Action action)
    {
        CoroutineManager.Instance.CallBack(FillImage(fillImage, fillDuration, fillmethod, fillOrigin), action);
    }
    public IEnumerator FillImage(Image fillImage, float fillDuration, Image.FillMethod fillmethod, int fillOrigin)
    {
        if (fillOrigin != 0 && fillOrigin != 1) 
        {
            Debug.LogError("fillOrigin은 1 또는 0 이어야함");
            yield break;
        }
        
        float timer = 0f;
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = fillmethod;
        fillImage.fillOrigin = fillOrigin;

        while (timer < fillDuration)
        {
            timer += Time.deltaTime;
            float fillAmount = Mathf.Lerp(0f, 1f, timer / fillDuration);
            fillImage.fillAmount = fillAmount;
            yield return null;
        }
    }
}
//