using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpherePoolerDemo : MonoBehaviour 
{
	#region Fields / Properties
	[SerializeField] StringKeyedPooler pooler;
	[SerializeField] InputField keyInput;
	#endregion

	#region MonoBehaviour
	void OnEnable ()
	{
		pooler.didDequeueForKey = DidDequeueForKey;
	}

	void OnDisable ()
	{
		pooler.didDequeueForKey = null;
	}
	#endregion

	#region Event Handlers
	void DidDequeueForKey (Poolable item, string key)
	{
		float xPos = UnityEngine.Random.Range(-6f, 6f);
		float yPos = UnityEngine.Random.Range(-4f, 4f);
		float zPos = UnityEngine.Random.Range(-5f, 5f);
		item.transform.localPosition = new Vector3( xPos, yPos, zPos );
		item.gameObject.SetActive(true);
		item.name = key;
	}

	public void OnAddButton ()
	{
		if (!string.IsNullOrEmpty(keyInput.text))
			pooler.DequeueByKey(keyInput.text);
	}

	public void OnRemoveButton ()
	{
		pooler.EnqueueByKey(keyInput.text);
	}
	#endregion
}
