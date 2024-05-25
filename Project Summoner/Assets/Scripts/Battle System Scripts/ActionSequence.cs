using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequence : MonoBehaviour
{
    public event EventHandler<EventArgs> OnSequenceComplete;
    public event EventHandler<EventArgs> OnSequenceStart;
    public event EventHandler<EventArgs> OnSequenceStop;

    private Dictionary<Action, float> taskByTime;
    private float duration;
    private bool isLoop;
    private bool isPlaying;
    private float currentTime;
    private float previousTime;

    private void Awake()
    {
        taskByTime = new Dictionary<Action, float>();
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        // Previous time needs to be set to below 0 one the first call otherwise any actions
        // scheduled for time 0 will not be ran
        previousTime = currentTime == 0 ? -1 : currentTime;
        currentTime += Time.deltaTime;
        if (currentTime > duration)
            currentTime = duration;

        foreach(KeyValuePair<Action, float> pair in taskByTime) {
            if (pair.Value > previousTime && pair.Value <= currentTime)
                pair.Key();
        }

        if(!isLoop && currentTime >= duration) {
            OnSequenceComplete?.Invoke(this, new EventArgs());
            isPlaying = false;
            taskByTime.Clear();
            Debug.Log("AnimationSequence has completed");
        }
    }

    public void StartSequence()
    {
        isPlaying = true;
        Debug.Log("AnimationSequence has been started");
    }

    public void StopSequence()
    {
        isPlaying = false;
        Debug.Log("AnimationSequence has been stopped");
    }

    public void AddTaskByTime(Action task, float time)
    {
        taskByTime.Add(task, time);
    }

    public float GetDuration() { return duration; }

    public void SetDuration(float duration) { this.duration = duration; }

    public bool IsPlaying() { return isPlaying; }

    public void SetIsPlaying(bool isPlaying) { this.isPlaying = isPlaying; }
}
