using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SmokeHandler : MonoBehaviour
{
    public int[] emissionAmt = {20, 30, 40};
    public float[] maxSize = {.1f, .6f, .03f};

    [Tooltip("ranges from -1->1.")]
    public float hueOffset = 0f;
    public float saturation = 1f;
    public float contrast = 1f;
    ParticleSystem[] children;
    

    public bool DEBUG_alwaysOn = false;

    void Awake()
    {
        children = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < children.Length; i++) {
            var r = children[i].GetComponent<ParticleSystemRenderer>();
            r.maxParticleSize = maxSize[1] * .1f;

            string[] s = r.material.GetPropertyNames(MaterialPropertyType.Float);
            List<string> st = s.ToList();

            r.material.SetFloat("_hueOffset", hueOffset);
            r.material.SetFloat("_saturation", saturation);
            r.material.SetFloat("_contrast", contrast);

        }
    }

    public void turnOn() {
        for (int i = 0; i < children.Length; i++) {
            var e = children[i].emission;
            e.rateOverTime = emissionAmt[i];
        }
    }

    public void turnOff() {
        if (DEBUG_alwaysOn) return;

        for (int i = 0; i < children.Length; i++) {
            var e = children[i].emission;
            e.rateOverTime = 0;
        }
    }

}
