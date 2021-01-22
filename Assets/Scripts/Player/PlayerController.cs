using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

/* Controls the player. Here we chose our "focus" and where to move. */

[RequireComponent(typeof(ThirdPersonMovement))]
public class PlayerController : MonoBehaviour
{

	public delegate void OnFocusChanged(Interactable newFocus);
	public OnFocusChanged onFocusChangedCallback;

	public Interactable focus;  // Our current focus: Item, Enemy etc.

	public LayerMask movementMask;      // The ground
	public LayerMask interactionMask;   // Everything we can interact with

	ThirdPersonMovement motor;      // Reference to our motor

	// Get references
	void Start()
	{
		motor = GetComponent<ThirdPersonMovement>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	// Set our focus to a new focus
	void SetFocus(Interactable newFocus)
	{
		if (onFocusChangedCallback != null)
			onFocusChangedCallback.Invoke(newFocus);

		// If our focus has changed
		if (focus != newFocus && focus != null)
		{
			// Let our previous focus know that it's no longer being focused
			focus.OnDefocused();
		}

		// Set our focus to what we hit
		// If it's not an interactable, simply set it to null
		focus = newFocus;

		if (focus != null)
		{
			// Let our focus know that it's being focused
			focus.OnFocused(transform);
		}

	}

}