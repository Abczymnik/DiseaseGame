using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [field:SerializeField] public Transform InteractionPoint { get; private set; }
    [SerializeField] private LayerMask interactionLayer;
    private float InteractionPointRadius = 1f;
    public bool IsInteracting { get; private set; }

    private InputAction interactionInput;

    private void OnValidate()
    {
        InteractionPoint = transform.Find("InteractionPoint");
        interactionLayer = LayerMask.GetMask("Interactable");
    }

    private void OnEnable()
    {
        interactionInput = PlayerUI.Instance.InputActions.Gameplay.Use;
        interactionInput.performed += OnInteractionWish;
    }

    private void OnInteractionWish(InputAction.CallbackContext _)
    {
        Collider[] colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, interactionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out IInteractable interactable))
            {
                StartInteraction(interactable);
            }
        }
    }

    private void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccesfull);
        IsInteracting = true;
    }

    private void EndInteraction()
    {
        IsInteracting = false;
    }

    private void OnDisable()
    {
        interactionInput.performed -= OnInteractionWish;
    }
}
