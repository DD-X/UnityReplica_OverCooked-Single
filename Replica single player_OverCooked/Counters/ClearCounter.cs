using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    


    //[SerializeField] private ClearCounter secondClearCounter;
    //[SerializeField] private bool testing;

    //private KitchenObject kitchenObject;

    //For testing to move the kitchenObject from one counter to another
    /*private void Update() {
        if(testing && Input.GetKeyDown(KeyCode.T)) {
            if (kitchenObject != null) {
                kitchenObject.SetKitchenObjectParent(secondClearCounter);
                //Debug.Log(kitchenObject.GetClearCounter());
            }
        }
    }*/

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //There is no kitchenObject here
            if (player.HasKitchenObject()) {
                //Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);//Set to this object
            }
            else {
                //Player not carrying anything
            }
        }
        else {
            //There is kitchenObject here
            if (player.HasKitchenObject()) {
                //Player is carrying something 
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    //Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else {
                    //Player is not carrying plate but something else
                    if (GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject01)) {
                        //Counter is holding a plate
                        if (plateKitchenObject01.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
                
            }
            else {
                //Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    //Not aim to spawn anything, just to serve up and provide items
    /*public override void Interact(Player player) { //To override base function
        //Debug.Log("Interact!");

        if (kitchenObject == null) {
            //just clone one kitchenObject for the specifc counter(let it to know)

            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);       //Clone
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);


            /*kitchenObjectTransform.localPosition = Vector3.zero;           //LocalPosition vs globalPosition
            
            kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();   //Get kitchenObject references.
            kitchenObject.SetClearCounter(this); //Get clearCounter references.*/

    //Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName);
    //¡ü Output the kitchenObject name from OS

    /*else {
        //Give the object to the player
        kitchenObject.SetKitchenObjectParent(player);

        //Debug.Log(kitchenObject.GetClearCounter());
    }

}
*/

}
