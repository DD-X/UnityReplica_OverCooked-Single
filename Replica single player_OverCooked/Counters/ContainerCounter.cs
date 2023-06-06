using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;


    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    //[SerializeField] private Transform counterTopPoint;

    //private KitchenObject kitchenObject;

    public override void Interact(Player player) { //To override base function

        if (!player.HasKitchenObject()) {
            //Player is not carrying anything
            //just clone one kitchenObject for the specifc counter(let it to know),and give it to the player


            /* Spawn KitchenObject
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            //Clone   Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint); //
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player); //From the IKitchenObjectParent, pass it to players
            */

            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        /*else {
            //Give the object to the player
            kitchenObject.SetKitchenObjectParent(player);

        }*/

    }
}
