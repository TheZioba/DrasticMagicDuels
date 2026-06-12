using UnityEngine;

public class Defendo : MonoBehaviour
{
    public Owner owner;
    public Transform casterTransform;

    public void Init(float lifetime, Owner owner)
    {
        this.owner = owner;
        Destroy(gameObject, lifetime);
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("StrikandoStraight"))
        {
            StrikandoStraight strikando = other.GetComponent<StrikandoStraight>();

            if (strikando == null)
                return;

            if (owner == Owner.FirstLeftPlayerMage && strikando.owner == Owner.FirstRightPlayerMage)
            {
                Debug.Log("Strikando spell deviated by LeftPlayerMage!");
                strikando.owner = Owner.FirstLeftPlayerMage;
                
            }

            else if(owner == Owner.FirstRightPlayerMage && strikando.owner == Owner.FirstLeftPlayerMage)
            {
                Debug.Log("Strikando spell deviated by RightPlayerMage!");
                strikando.owner = Owner.FirstRightPlayerMage;
            }
        }
    }*/
}
