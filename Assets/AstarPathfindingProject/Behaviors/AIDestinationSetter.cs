using UnityEngine;
using System.Collections;
using System;

namespace Pathfinding
{
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour
	{
		/// <summary>The object that the AI should move to</summary>
		public Transform player, obelisk, target;
		IAstarAI ai;
		private Vector3 lastPosition;
		private Vector3 movementDirection;

		void OnEnable()
		{
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
		}

		void Start()
		{
			if (GameObject.Find("Player") != null)
				player = GameObject.Find("Player").transform;
			if (GameObject.Find("Obelisk") != null)
				obelisk = GameObject.Find("Obelisk").transform;
			lastPosition = transform.position;
		}

		void OnDisable()
		{
			if (ai != null) ai.onSearchPath -= Update;
		}

		/// <summary>Updates the AI's destination every frame</summary>
		void Update()
		{
			if (player != null && ai != null && obelisk != null)
			{
				float distanceToPlayer = Vector3.Distance(transform.position, player.position);
				float distanceToObelisk = Vector3.Distance(transform.position, obelisk.position);

				if (distanceToPlayer > distanceToObelisk)
				{
					target = obelisk;
				}
				else
				{
					target = player;
				}

				ai.destination = target.position;

				// Calculate the movement direction from the last frame
				if ((transform.position - lastPosition).normalized != Vector3.zero)
					movementDirection = (transform.position - lastPosition).normalized;
				transform.GetComponent<Animator>().SetFloat("x", movementDirection.x);
				transform.GetComponent<Animator>().SetFloat("y", movementDirection.y);
				lastPosition = transform.position;
			}
			else if (player != null && ai != null)
			{
				target = player;

				ai.destination = target.position;

				// Calculate the movement direction from the last frame
				if ((transform.position - lastPosition).normalized != Vector3.zero)
					movementDirection = (transform.position - lastPosition).normalized;
				transform.GetComponent<Animator>().SetFloat("x", movementDirection.x);
				transform.GetComponent<Animator>().SetFloat("y", movementDirection.y);
				lastPosition = transform.position;
			}
		}
	}
}
