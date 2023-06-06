using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IKitchenObjectParent
{

    public static Player Instance { get; private set; }


    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs{
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake() { //Run before the Start
        if(Instance != null) {
            Debug.LogError("There is more than one Instance!");
        }
        Instance = this; //No more than one Player
    }


    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if (!GameHandler.Instance.IsGamePlaying()) return; //Stop here

        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (!GameHandler.Instance.IsGamePlaying()) return; //Stop here

        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    

    private void Update(){
        HandleMovement();
        HandleInteraction();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteraction() {
        //Input System Movement (new)
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero) {
            //have some directions to contain the last direction even if stop moving
            lastInteractDir = moveDir;
        }
        

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance,countersLayerMask)) {
            //if had some colliders detections, raycastHit return to the specifc name of the colliders
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                /*exact the same function of TryGetComponent
                ClearCounter clearCounter = raycastHit.transform.GetComponent<ClearCounter>();
                if (clearCounter != null) { }*/
                //Has clearCounter (clearer way!)
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                    //selectedCounter = clearCounter;
                }
            }
            else {
                SetSelectedCounter(null);
                //selectedCounter = null;
            }
        }
        else {
            SetSelectedCounter(null);
            //selectedCounter = null;
        }
        //Debug.Log(selectedCounter);
    }

    private void HandleMovement() {
        //1.Input System Movement (new)
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //2.Collision Detection
        float moveDistance = Time.deltaTime * moveSpeed;
        float palyerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, palyerRadius, moveDir, moveDistance);
        //Physics & Physics2D different

        /* 2.1 Raycast
        float palyerSize = 0.7f;
        bool canMove = !Physics.Raycast(transform.position, moveDir, palyerSize);
        **Raycast: detect the player's center! If the body is smaller than a half, this will not work the real detection
        */

            //2.2 Can not move towards the moveDir detection
            if (!canMove) {
            //Can not move towards the moveDir

            //Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;//speed WD = D
            //Just keyBoard: canMove = moveDir.x!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, palyerRadius, moveDirX, moveDistance);
            //IF consider the gamePad
            canMove = (moveDir.x < - .5f || moveDir.x > + .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, palyerRadius, moveDirX, moveDistance);
            if (canMove) {
                //Can move only on the X 
                moveDir = moveDirX;
            }
            else {
                //Can not move only on the X

                //Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, palyerRadius, moveDirZ, moveDistance);
                if (canMove) {
                    //Can move only on the Z
                    moveDir = moveDirZ;
                }//else{//Can not move any direction}
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;//if not plus, the player object will move really faster
        }

        //2.3 Animator Controller, turn from Idle to Walk
        isWalking = moveDir != Vector3.zero;

        //2.4 Rotation, for rotation of forward player attached with the movement
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        }) ;
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            //Player pickup something
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }

    }
    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }
    public void ClearKitchenObject() {
        kitchenObject = null;
    }
    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
