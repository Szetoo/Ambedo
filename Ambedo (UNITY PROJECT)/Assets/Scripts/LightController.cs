using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public bool isFlickering;

    private bool isLightOn;

    private IEnumerator loopCoroutine;
    
    

    // Start is called before the first frame update
    void Start()
    {
        if (isFlickering)
        {
            loopCoroutine = lightLoop();
            StartCoroutine(loopCoroutine);
        }
    }

    private void lightOn(bool kills)
    {
        GetComponentInChildren<Light>().enabled = true;
        if (!kills)
        {
            GetComponentInChildren<Light>().intensity = 15;
        }
        else
        {
            GetComponentInChildren<Light>().intensity = 90;
            GetComponent<SpriteRenderer>().enabled = true;
        }
        GetComponent<BoxCollider2D>().enabled = kills;
        
        isLightOn = true;
        
    }

    private void lightOff()
    {
        GetComponentInChildren<Light>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        isLightOn = false;
        

    }

    private IEnumerator lightLoop()
    {
        float seed = Mathf.Abs(GetComponent<Transform>().transform.position.x); 
        float randomNum = (100000/(69 * Mathf.Exp(seed/25) + 420) % 1337) + 420/seed; //https://www.youtube.com/watch?v=dQw4w9WgXcQ
        yield return new WaitForSecondsRealtime(randomNum);

        AudioSource[] lightSounds = GetComponents<AudioSource>();
        while (true)
        {
            lightSounds[1].Play();
            lightOn(true);
            yield return new WaitForSecondsRealtime(randomNum*1.3f);
            lightOff();
            lightSounds[1].Stop();
            yield return new WaitForSecondsRealtime(randomNum*2.1f);
            lightSounds[0].Play();
            lightOn(false);
            yield return new WaitForSecondsRealtime(0.15f);
            lightOff();
            yield return new WaitForSecondsRealtime(0.3f);
            lightOn(false);
            yield return new WaitForSecondsRealtime(0.1f);
            lightOff();
            lightSounds[0].Stop();
            yield return new WaitForSecondsRealtime(1f);

            
        }
    }
}
