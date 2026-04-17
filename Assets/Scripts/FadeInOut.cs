using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Entire Script Authored by Hunter Cave 

public class FadeInOut : MonoBehaviour
{

    // Material Variables
    [Range(0f, 1f)]
    public Material myMaterial;
    Color myColor;

    // Timing Variables
    public float visibleTime = 10f;
    public float hiddenTime = 2f;
    Coroutine myCoroutine;

    private void Start()
    {
        myColor = myMaterial.color;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(visibleTime);

        // Decrease Material Alpha Gradually
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            myColor.a = alpha;
            myMaterial.color = myColor;
            yield return new WaitForSeconds(1/120);
            
        }

        yield return StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(hiddenTime);

        // Increase Material Alpha Gradually
        for (float alpha = 0; alpha <= 1f; alpha += 0.1f)
        {
            myColor.a = alpha;
            myMaterial.color = myColor;
            yield return new WaitForSeconds(1 / 120);

        }

        yield return StartCoroutine(FadeOut());
    }
}