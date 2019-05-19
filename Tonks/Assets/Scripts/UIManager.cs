using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
	TMPro.TextMeshProUGUI ProjectilesFiredText = null;
	[SerializeField]
	TMPro.TextMeshProUGUI EnemiesKilledText = null;
	[SerializeField]
	TMPro.TextMeshProUGUI DamageDoneText = null;
	[SerializeField]
	TMPro.TextMeshProUGUI TimeLastedText = null;
	[SerializeField]
	TMPro.TextMeshProUGUI DamageTakenText = null;
	[SerializeField]
	TMPro.TextMeshProUGUI GameOverText = null;
	[SerializeField]
	UnityEngine.UI.Button GameOverButton = null;


	public int ProjectilesFired = 0;
	public int EnemiesKilled = 0;
	public int DamageDone = 0;
	public float TimeLasted = 0;
	public int DamageTaken = 0;

	public static UIManager inst =null;
	private void Awake()
	{
		if (inst)
		{
			Destroy(this);
		}
		else
		{
			inst = this;
		}
	}
	// Update is called once per frame
	void Update()
    {
		
		if(EntityManagementSystem.inst.GetPlayerEntity())
		{
			TimeLasted += Time.deltaTime;
			GameOverText.gameObject.SetActive(false);
			GameOverButton.gameObject.SetActive(false);
		}
		else
		{
			GameOverText.gameObject.SetActive(true);
			GameOverButton.gameObject.SetActive(true);
		}



		TimeLastedText.text = "Seconds Lasted: " + (int)TimeLasted;
		ProjectilesFiredText.text = "Projectiles Fired: " + ProjectilesFired;
		EnemiesKilledText.text = "Enemies Killed: " + EnemiesKilled;
		DamageDoneText.text = "Damage Done: " + DamageDone;
		DamageTakenText.text = "Damage Taken: " + DamageTaken;
    }

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
