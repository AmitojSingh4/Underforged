using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Playables;
// https://www.youtube.com/watch?v=VBoi6BigwpQ
public static class TimelineReverse{
    
    public delegate void Callback();
    private static readonly WaitForEndOfFrame _frameWait = new WaitForEndOfFrame();

    public static IEnumerator Reverse(PlayableDirector timeline, Callback callback)
    {
        //Debug.Log("reverse");
        DirectorUpdateMode defaultUpdateMode = timeline.timeUpdateMode;
        timeline.timeUpdateMode = DirectorUpdateMode.Manual;

        if(timeline.time.ApproxEquals(timeline.duration) || timeline.time.ApproxEquals(0)){
            timeline.time = timeline.duration;
        }

        timeline.Evaluate();

        yield return _frameWait;

        float dt = (float) timeline.duration;

        while (dt > 0)
        {
            dt -= Time.unscaledDeltaTime;

            timeline.time = Mathf.Max(dt, 0);
            timeline.Evaluate();
            yield return _frameWait;
        }

        timeline.time = 0;
        timeline.Evaluate();
        timeline.timeUpdateMode = defaultUpdateMode;
        timeline.Stop();

        callback();
    }

    public static bool ApproxEquals(this double num, float other){
        return Mathf.Approximately((float) num, other);
    }
    public static bool ApproxEquals(this double num, double other){
        return Mathf.Approximately((float) num, (float)other);
    }    
}