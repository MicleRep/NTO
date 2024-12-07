using UnityEngine;

public class Helicopter : Uint
{
    [SerializeField] float _speed;

    public override void Mowe()
    {
        if(_target!=null)
        {
            if(!_attacking)
            {
                transform.position=Vector3.MoveTowards(transform.position,new Vector3(_target.position.x,transform.position.y,_target.position.z),_speed);
                transform.LookAt(_target);
            }
        }
    }
}
