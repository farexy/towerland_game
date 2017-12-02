using Assets.Scripts.Models.GameObjects;
using UnityEngine;

public class GameObjectScript : MonoBehaviour
{
    public GameObjectType Type;

    public int GameId { get; set; }
	public bool IsUsed { get; set; }

	public void SetColor(Color color)
	{
		GetComponent<SpriteRenderer>().color = color;
	}
}
