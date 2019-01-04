using Assets.Scripts.Models.GameObjects;
using UnityEngine;

public class GameObjectScript : MonoBehaviour
{
    public GameObjectType Type;

    public int GameId { get; set; }
	public bool IsUsed { get; set; }

	public void SetColor(Color color)
	{
		var spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer != null)
		{
			spriteRenderer.color = color;
		}
		else
		{
			var meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer != null)
			{
				meshRenderer.material.SetColor("_SpecColor", color);
			}
		}
	}
}
