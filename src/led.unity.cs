using System;
using UnityEngine;

public class led : MonoBehaviour
{
    private int NUM_LEDS = 300;
    public GameObject p;
    
    private SpriteRenderer[] goLed ;
    private int[] leds;
    
    // Start is called before the first frame update
    void Start()
    {
        goLed = new SpriteRenderer[NUM_LEDS];
        leds = new int[NUM_LEDS];
        for (int i = 0; i < NUM_LEDS; i++)
        {
            GameObject go = GameObject.Instantiate(p);
            goLed[i] = go.GetComponent<SpriteRenderer>();
            go.transform.position = new Vector3(i, 0);
        }
    }

    private int ledCutoff = 154;
    int tTotal = -100;

    private int thunderStride = 1000;
    private int bubbleStr = 0xFF;

    private void FixedUpdate()
    {
        tTotal++;
        int t = tTotal % thunderStride;
        
        float reGen = Math.Clamp((t - 150) / 20.0f, 0, 1);
        
        if (reGen == 0)
            for (int i = 0; i < ledCutoff; i++)
                leds[i] = 0;
        
        lightning(t, 0, 3, 8, 12, 0, ledCutoff, 255);
        lightning(t, 30, 31, 40, 50, 40, ledCutoff, 255);
        lightning(t, 59, 60, 67, 75, 0, 85, 255);
        
        lightning(t,  80, 83, 85, 88, 20, 45, 25);
        lightning(t, 88, 90, 93, 96, 90, 114, 50);
        lightning(t, 96, 100, 102, 105, 50, 65, 120);
        lightning(t, 105, 110, 111, 120, 110, 120, 20);

        for (int i = ledCutoff; i < NUM_LEDS; i++)
            leds[i] = CRGB(255 << 16 | bubbleStr << 8);
        
        if (reGen > 0)
            for (int i = 0; i < ledCutoff; i++)
                leds[i] = CRGB((int)(255 * reGen) << 16 | (int)(bubbleStr * reGen) << 8);

        bubble(reGen, 10, 30, 240, 120);
        bubble(reGen, 160, 175, 240, 120);

        show();
    }

    private void lightning(int t, int start, int middle, int end, int endEnd, int fromPos, int toPos, int brightness)
    {
        if (t >= start && t < endEnd)
        {
            int color = 0;
            if (t >= start && t < middle)
                color = gray((int)((t - start) / (float)(middle - start) * brightness));
            else if (t >= middle && t < end)
                color = gray(brightness);
            else if (t >= end && t < endEnd)
                color = gray((int)((1 - (t - end) / (float)(endEnd - end)) * brightness));

            for (int i = fromPos; i < toPos; i++)
                leds[i] = color;
        }
    }

    private void bubble(float reGen, int fromPos, int toPos, int freqTime, int duration)
    {
        if (tTotal % freqTime >= 0 && tTotal % freqTime <= duration)
        {
            for (int i = fromPos; i < toPos; i++)
            {
                float r = i >= ledCutoff ? 1 : reGen;
                if (r > 0)
                    leds[i] = CRGB((int)(255 * r) << 16 |
                               (int)((1 - Math.Sin((i - fromPos) / (float)(toPos - fromPos) * 3.14) *
                                         Math.Sin(tTotal % freqTime / (float)duration * 3.14)) *
                                     bubbleStr * r) << 8);
            }
        }
    }
    
    private static int gray(int color)
    {
        return CRGB((color << 16) | (color << 8) | color);
    }

    private static int CRGB(int color)
    {
        return color;
    }
    
    private void show()
    {
        for (int i = 0; i < NUM_LEDS; i++)
        {
            goLed[i].color = new Color((leds[i] >> 16) / 255.0f, ((leds[i] & 0xff00) >> 8) / 255.0f, (leds[i] & 0xff) / 255.0f);
        }
    }
}
