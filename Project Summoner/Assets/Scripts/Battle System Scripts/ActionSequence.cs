using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequence : MonoBehaviour
{
    public event EventHandler<EventArgs> OnSequenceComplete;
    public event EventHandler<EventArgs> OnSequenceStart;
    public event EventHandler<EventArgs> OnSequenceStop;

    [SerializeField] private float duration;
    [SerializeField] private bool isLoop;
    [SerializeField] private bool isPlaying;
    private float currentTime;
    private float previousTime;
    private Dictionary<Action, float> taskByTime;

    private void Awake()
    {
        taskByTime = new Dictionary<Action, float>();
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        // Previous time needs to be set to below 0 on the first call otherwise any actions
        // scheduled for time 0 will not be ran
        previousTime = currentTime == 0 ? -1 : currentTime;
        currentTime += Time.deltaTime;
        if (currentTime > duration)
            currentTime = isLoop ? currentTime % duration : duration;

        foreach(KeyValuePair<Action, float> pair in taskByTime) {
            if (pair.Value > previousTime && pair.Value <= currentTime)
                pair.Key();
        }

        if(!isLoop && currentTime >= duration) {
            Debug.Log("Sequence has completed");
            isPlaying = false;
            taskByTime.Clear();
            OnSequenceComplete?.Invoke(this, new EventArgs());
        }
    }

    public void StartSequence()
    {
        isPlaying = true;
        Debug.Log("Sequence has been started");
        OnSequenceStart?.Invoke(this, new EventArgs());
    }

    public void StopSequence()
    {
        isPlaying = false;
        Debug.Log("Sequence has been stopped");
        OnSequenceStop?.Invoke(this, new EventArgs());
    }

    public void ClearSequence()
    {
        isPlaying = false;
        duration = 0f;
        currentTime = 0f;
        previousTime = 0f;
        taskByTime.Clear();
    }

    public void AddTaskByTime(Action task, float time)
    {
        taskByTime.Add(task, time);
    }

    public float GetDuration() { return duration; }

    public void SetDuration(float duration) { this.duration = duration; }

    public bool IsLoop() { return isLoop; }

    public void SetIsLoop(bool isLoop) {  this.isLoop = isLoop; }

    public bool IsPlaying() { return isPlaying; }

    public void SetIsPlaying(bool isPlaying) { this.isPlaying = isPlaying; }
}
