using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator
{
    //fillOrigin : 0�� ����(����) �Ǵ� �ϴ�(����)�� ��Ÿ����, 1�� ������(����) �Ǵ� ���(����)
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
            Debug.LogError("fillOrigin�� 1 �Ǵ� 0 �̾����");
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