using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using PrimeTween;
using UnityEngine;

public static class CommonUtil
{
    public static void StopAllTweens(List<Tween> tweens) {
        
    }
    
    public static void OnHitColorEffect(
        Renderer meshRenderer,
        MaterialPropertyBlock materialPropertyBlock,
        Color startColor,
        Color endColor,
        float duration,
        List<Tween> tweens
    )
    {
        Tween tween = Tween.Custom(
            startColor, endColor,
            duration: duration,
            onValueChange: newVal =>
            {
                materialPropertyBlock.SetColor("_Albedo", newVal);
                meshRenderer.SetPropertyBlock(materialPropertyBlock);
            },
            cycles: 2,
            cycleMode: CycleMode.Yoyo
            );

        tweens.Add(tween);
    }

    public static bool IsNull(object testObject)
    {
        return testObject == null;
    }

    public static bool IsNotNull(object testObject)
    {
        return testObject != null;
    }

    public static GameObject GetParentGameObject(GameObject child)
    {
        return child.transform.parent.gameObject;
    }

    #region CONSOLE
    public static void ClearLog()
    {
#if UNITY_EDITOR
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
#endif
    }
    #endregion

    #region COLOR
    public static Color ChangeAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    #endregion

    private static IEnumerator WaitFor(float amount, Action onCompletedAction)
    {
        float deltaTime = Time.deltaTime;

        WaitForSeconds waitForSeconds = new WaitForSeconds(deltaTime);

        float time = 0;

        while (time < amount)
        {
            time += deltaTime;

            yield return waitForSeconds;
        }

        onCompletedAction?.Invoke();
    }
}
