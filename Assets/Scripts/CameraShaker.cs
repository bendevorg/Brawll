using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {

public static CameraShaker instance;

private Vector3 _originalPos;
private float _timeAtCurrentFrame;
private float _timeAtLastFrame;
private float _fakeDelta;

void Awake()
{
    instance = this;
}

void Update() {
    // Calculate a fake delta time, so we can Shake while game is paused.
    _timeAtCurrentFrame = Time.realtimeSinceStartup;
    _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
    _timeAtLastFrame = _timeAtCurrentFrame;
	if(Input.GetKeyDown(KeyCode.G)) {
    	Shake(0.3f, 0.3f);
    }
}

public static void Shake (float duration, float amount) {
    // instance._originalPos = instance.gameObject.transform.localPosition;
    instance.StopAllCoroutines();
    instance.StartCoroutine(instance.cShake(duration, amount));
}

public IEnumerator cShake (float duration, float amount) {
    float endTime = Time.time + duration;
    instance._originalPos = instance.gameObject.transform.localPosition;	

    while (duration > 0) {
        transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

        duration -= _fakeDelta;

        yield return null;
    }

    transform.localPosition = _originalPos;
}

}