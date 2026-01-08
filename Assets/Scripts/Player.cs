using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour , IKitchenObjectParent
{
    public static Player Instance { get; private set; } 

    public EventHandler OnPickedSomething;
    public event EventHandler<OnSelectCounterChangedEventArgs> OnSelectCounterChanged;
    public class OnSelectCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;            // can be assigned in Inspector or auto-found
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter SelectCounter;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        // Singleton pattern
        if (Instance != null)
        {
            Debug.LogError("there is more then one plyer instance");
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        
        // Auto-find GameInput if not assigned in inspector
        if (gameInput == null)
            
        {
            gameInput = FindFirstObjectByType<GameInput>();
            if (gameInput == null)

            {
                Debug.LogWarning("Player: GameInput not found in scene. Movement/interaction will fallback to legacy Input.");
            }
            else
            {
                Debug.Log("Player: Auto-assigned GameInput from scene.");
            }
        }

        // Subscribe safely if we have a valid gameInput
        if (gameInput != null)
        {
            gameInput.OnInteractAction += GameInput_OnInteractAction;
            gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;



        }
    }



    private void OnDestroy()
    {
        if (gameInput != null)
        {
            gameInput.OnInteractAction -= GameInput_OnInteractAction;
            gameInput.OnInteractAlternateAction -= GameInput_OnInteractAlternateAction;
        }
    }



    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (SelectCounter != null)
        {
            SelectCounter.Interact(this);
           

        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())
        {
            return;
        }
        if (SelectCounter != null)
        {
            
            SelectCounter.InteractAlternate(this);

        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions(); // still useful for showing hints or doing look-ahead raycasts
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput != null
            ? gameInput.GetMovementVectorNormalized()
            : new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hit, interactDistance, countersLayerMask))
        {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (SelectCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter); 
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);


        }

      
    }


    private void HandleMovement()
    {
        // Get movement vector (GameInput preferred, fallback to Input axes)
        Vector2 inputVector = Vector2.zero;
        if (gameInput != null)
        {
            inputVector = gameInput.GetMovementVectorNormalized();
        }
        else
        {
            inputVector.x = Input.GetAxisRaw("Horizontal");
            inputVector.y = Input.GetAxisRaw("Vertical");
            inputVector = inputVector.normalized;
        }

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // If there's no movement, set walking false and return early (saves on casts)
        if (moveDir == Vector3.zero)
        {
            isWalking = false;
            return;
        }

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        // CapsuleCast to check collision (normalize direction)
        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDir.normalized,
            moveDistance
        );

        if (!canMove)
        {
            // Try X-only movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            if (moveDirX != Vector3.zero)
            {
                canMove = !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirX,
                    moveDistance
                );

                if (canMove)
                {
                    moveDir = moveDirX;
                }
            }

            // If still blocked, try Z-only
            if (!canMove)
            {
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                if (moveDirZ != Vector3.zero)
                {
                    canMove = !Physics.CapsuleCast(
                        transform.position,
                        transform.position + Vector3.up * playerHeight,
                        playerRadius,
                        moveDirZ,
                        moveDistance
                    );

                    if (canMove)
                    {
                        moveDir = moveDirZ;
                    }
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir.normalized * moveDistance;
        }

        // Face movement direction
        if (moveDir != Vector3.zero)
        {
            transform.forward = moveDir.normalized;
        }


        isWalking = moveDir != Vector3.zero;
    }

         private void SetSelectedCounter(BaseCounter selectedConter)
    {
        this.SelectCounter = selectedConter;

        OnSelectCounterChanged?.Invoke(this, new OnSelectCounterChangedEventArgs
        {
            selectedCounter = SelectCounter
        });
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

}
   
