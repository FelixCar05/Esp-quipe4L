using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] GameObject[] objetAPool;
	[SerializeField] int[] quantiteAPool;

	private List<GameObject> pool = new List<GameObject>();

	public static ObjectPool instance;
	void Start()
	{
		instance = this;

		for (int i = 0; i < Mathf.Min(objetAPool.Length, quantiteAPool.Length); i++)
		{
			for (int obj = 0; obj < quantiteAPool[i]; obj++)
			{
				GameObject objetTemp = Instantiate(objetAPool[i]);
				objetTemp.name = objetAPool[i].name;
				objetTemp.SetActive(false);
				pool.Add(objetTemp);
			}
		}
	}

	public GameObject GetPooledObject(GameObject typeObjet)
	{
		for (int i = 0; i < pool.Count; i++)
		{
			if (pool[i].name == typeObjet.name && !pool[i].activeInHierarchy)
			{
				return pool[i];
			}
		}


		return null;
	}
}
