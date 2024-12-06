using System.Collections;
using UnityEngine;

public class Bases : Uint
{
    [SerializeField] GameObject _menu;
    

    public override IEnumerator Die()
    {
        _menu.SetActive(true);
        Time.timeScale=0;
        FindFirstObjectByType<Main>().Win();
        yield return new WaitForSeconds(0);
    }
}
