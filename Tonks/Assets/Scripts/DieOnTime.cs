using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnTime : MonoBehaviour
{
	public float DeathTime = 1.0f;
	public float StartTime = 0.0f;

	private void Awake()
	{
		StartTime = Time.time;
	}
	// Update is called once per frame
	void Update()
    {

        if(Time.time >= StartTime+DeathTime)
		{
			Destroy(this.gameObject);
		}
    }
}
