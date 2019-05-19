using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
	[SerializeField]
	Transform Player;
	[SerializeField]
	float MoveSpeed;
	[SerializeField]
	GameObject MinimapLight;
	[SerializeField]
	bool EnableMinimapLight;
	[SerializeField]
	Vector3 FollowOffset;
    // Update is called once per frame
    void Update()
    {
		EntityComponent playerEnt = EntityManagementSystem.inst.GetPlayerEntity();
		if(playerEnt)
		{
			Player = playerEnt.transform;
			if (Player)
			{
				Vector3 position = Camera.main.transform.position;
				Vector3 dif = Player.position - FollowOffset - position;
				position.x = position.x + (dif.normalized.x * MoveSpeed * Time.deltaTime);
				position.z = position.z + (dif.normalized.z * MoveSpeed * Time.deltaTime);
				Camera.main.transform.position = position;
			}
		}
		
    }

	void OnPreCull()
	{
		if (MinimapLight != null)
			MinimapLight.SetActive(EnableMinimapLight);
	}
}
