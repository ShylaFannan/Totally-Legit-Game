using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DizzyCam : MonoBehaviour
{
    private float _shakeLength = 0.3f;
    private float _shakeTime;

    public void TakeDamage()
    {
        StartCoroutine(CameraShakeRoutine());
    }

    public IEnumerator CameraShakeRoutine()
    {
        Vector3 defaultPosition = this.transform.position; //what position our camera starts/stops in
        float _shakeTime = Time.time + _shakeLength; //time the camera shakes = real time + shakelength variable

        while(Time.time < _shakeTime) //while time game has been running is less than shaketime variable do this
        {
            float xPosition = Random.Range(-1f, 1f); //random position on x
            float yPosition = Random.Range(-1f, 1f); //random position on y between -1 and 1 
            this.transform.position = new Vector3(xPosition, yPosition, -10f); //camera will move to random x and y and -10 along z
            yield return null;
        }
        this.transform.position = defaultPosition; //move camera back to default position
    }
}
