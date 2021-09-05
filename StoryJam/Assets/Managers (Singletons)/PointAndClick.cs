using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class PointAndClick : MonoBehaviour
{
	#region Singleton

	public static PointAndClick Instance { get; private set; } = null;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		Instance = this;
		// DontDestroyOnLoad(this.gameObject);
	}

	#endregion


	private Vector2 currentMousePosition;

	private bool isCursor;

	public Texture2D cursorTexture;
	public Texture2D pointerTexture;

	private CursorMode cursorMode = CursorMode.Auto;
	private Vector2 hotSpotCursor = new Vector2(14, 9);
	private Vector2 hotSpotPointer = new Vector2(10, 10);

	RaycastHit2D hit;

	public void OnMouseMove(InputValue value)
	{
		currentMousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
	}

	public void Start()
	{
		Cursor.SetCursor(cursorTexture, hotSpotCursor, cursorMode);
	}

	private void FixedUpdate()
	{
		hit = Physics2D.Raycast(currentMousePosition, Vector2.zero);

		if(hit.collider != null)
		{
			if(hit.collider.gameObject.tag == "Interactable" || hit.collider.gameObject.tag == "PickUp")
			{
				if (hit.collider.gameObject.GetComponent<ChangeRoom>())
				{
					ChangeRoom tempObject = hit.collider.gameObject.GetComponent<ChangeRoom>();
					for (int i = 0; i < tempObject.conditions.Count; i++)
					{
						if ((int)tempObject.conditions[i] > GlobalData.current[(int)Enum.Parse(typeof(goalIndex), tempObject.conditions[i].ToString())])
						{
							return;
						}
					}
					SetCursor(false);
				}
				else
				{
					SetCursor(false);
				}
			}
		}
		else
		{
			SetCursor(true);
		}
	}

	public void SetCursor(bool isCursor)
	{
		if(this.isCursor == isCursor)
		{
			return;
		}

		if (isCursor)
		{
			Cursor.SetCursor(cursorTexture, hotSpotCursor, cursorMode);
			this.isCursor = true;
		}
		else
		{
			Cursor.SetCursor(pointerTexture, hotSpotPointer, cursorMode);
			this.isCursor = false;
		}
	}

	public void OnLeftClick(InputValue value)
	{
		int clicked = (int)value.Get<float>();


		if (clicked == 1)
		{
			// if dialogue is open, progress?
			// else below

			if (hit.collider != null)
			{
				if (hit.collider.gameObject.tag == "PickUp")
				{
					hit.collider.GetComponent<PickUp>().RunAction();
				}

				if (hit.collider.gameObject.tag == "Interactable")
				{
					if(hit.collider.GetComponent<ChangeRoom>() != null && DialogManager.Instance.isActive)
					{
						FindObjectOfType<DialogButton>().RunAction();
					}
					else
					{
						hit.collider.GetComponent<Action>().RunAction();
					}



				}
			}

		}

	}
}
