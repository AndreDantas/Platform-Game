using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CubePoolerDemo : MonoBehaviour 
{
	#region Fields / Properties
	[SerializeField] SetPooler pooler;
	#endregion

	#region MonoBehaviour
	void OnEnable ()
	{
		pooler.willEnqueue = OnEnqueue;
		pooler.didDequeue = OnDequeue;
	}

	void OnDisable ()
	{
		pooler.willEnqueue = null;
		pooler.didDequeue = null;
	}
	#endregion

	#region Event Handlers
	void OnEnqueue (Poolable item)
	{
		Button button = item.GetComponent<Button>();
		button.onClick.RemoveAllListeners();
	}

	void OnDequeue (Poolable item)
	{
		float xPos = UnityEngine.Random.Range(-6f, 6f);
		float yPos = UnityEngine.Random.Range(-4f, 4f);
		float zPos = UnityEngine.Random.Range(-5f, 5f);
		item.transform.localPosition = new Vector3( xPos, yPos, zPos );
		item.gameObject.SetActive(true);

		Button button = item.GetComponent<Button>();
		button.onClick.AddListener( ()=>{ pooler.Enqueue(item); } );
	}

	public void OnAddButton ()
	{
		pooler.Dequeue();
	}
	#endregion
}