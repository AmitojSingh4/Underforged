using UnityEngine;
using UnityEngine.Playables;

// CameraAnimationButton
public class CameraAnimationButton : MenuButton{
    // resume pause
    [SerializeField] PlayableDirector timeline;
    [SerializeField] bool reversed = false;
    
    public override void OnClick(){
        if(!reversed)
            timeline.Play();
        else
            StartCoroutine(TimelineReverse.Reverse(timeline, () => {}));
        // load level and camera transition
    }    
}