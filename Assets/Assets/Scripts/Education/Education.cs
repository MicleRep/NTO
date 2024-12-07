using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class Education : MonoBehaviour
{
    [SerializeField] GameObject[] _units;
    [SerializeField] GameObject _winO,_black;
    [SerializeField] AudioClip _winC;
    [SerializeField] AudioSource _source,_main;
    bool _win;

    IEnumerator Start()
    {
        // if(YandexGame.savesData._steel!=0)
        // {
        //     // Game();
        // }
        yield return new WaitForSeconds(0.05f);
        _black.SetActive(false);
        _main.enabled=true;
        while(true)
        {
            yield return new WaitForSeconds(0.2f);
            _win=true;
            foreach(Solder s in FindObjectsOfType<Solder>())
            {
                if(s.gameObject.layer==3)
                {
                    _win=false;
                }
            }
            if(_win)
            {
                _source.PlayOneShot(_winC);
                Time.timeScale=0;
                _winO.SetActive(true);
            }
        }
    }

    public void Game()
    {
        Time.timeScale=1;
        SceneManager.LoadScene("Game");
    }
}
