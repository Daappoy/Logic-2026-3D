using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform Player;
    
    void LateUpdate()
    {
        Vector3 newPosition = Player.position;
        //ikuti rotasi player
        transform.rotation = Quaternion.Euler(90f, Player.eulerAngles.y, 0f);
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
