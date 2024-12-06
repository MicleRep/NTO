using System.Collections;
using UnityEngine;

public class EMain : Main
{
    [SerializeField] Animator _cursourA;

    public override void SelectUnit(int i)
    {
        base.SelectUnit(i);
        Debug.Log("qqq");
        if(_cursourA!=null)
        {
            _cursourA.Play("2");
        }
    }
    protected override IEnumerator OnEducation()
    {
        yield return new WaitForSeconds(0.7f);
        if(_cursourA!=null)
        {
            Destroy(_cursourA.gameObject);
        }
    }
    public override void Load()
    {
        
    }
    public override IEnumerator BomberCooldown(){yield return null;}
}
