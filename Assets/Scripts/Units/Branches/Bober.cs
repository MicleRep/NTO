using UnityEngine;

public class Bober : MonoBehaviour
{
    [SerializeField] GameObject _bomba,_target;
    void FixedUpdate()
    {   
        transform.Translate(_target.transform.position);
    }
   // IEnumerator BombSpawn()
 //   {
  //      Instantiate(_bomba,transform.position,Quaternion.identity);
   //     yield return new WaitForSeconds(3);
//       BombSpawn();
 //   }
}
