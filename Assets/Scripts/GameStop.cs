using UnityEngine;

public class GameStop : MonoBehaviour
{
    [SerializeField] AudioSource[] _sources;
    [SerializeField] AudioListener[] _listeners;
    [SerializeField] GameObject _pause;
    [SerializeField] Main _main;
    bool _paused,_pauseC;

    void OnApplicationFocus(bool focus)
    {
        if(!_paused||_pauseC)
        {
            if(focus==true)
            {
                foreach(AudioSource s in _sources)
                {
                    s.UnPause(); 
                }
                foreach(AudioListener l in _listeners)
                {
                    l.enabled=true;
                }
                Time.timeScale=1;
            }
            else
            {
                foreach(AudioSource s in _sources)
                {
                    s.Pause(); 
                }
                foreach(AudioListener l in _listeners)
                {
                    l.enabled=false;
                }
                Time.timeScale=0;
            }
            _pauseC=false;
        }
    }

    public void Pause(bool pause)
    {
        _pause.SetActive(pause);
        _paused=_pause;
        _pauseC=true;
        OnApplicationFocus(!pause);
    }
}
