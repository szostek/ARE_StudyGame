using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace BrainCheck {

	public class UnityReceiveMessages : MonoBehaviour
	{
	    public static UnityReceiveMessages Instance;
	    public TextMesh tMesh;
		string currentStatus;
		string duration;
		void Awake(){
			Instance = this;	
		}

		// Use this for initialization
		void Start () {
		}

		// Update is called once per frame
		void Update () {
		}

		public void statusMessage(string message) {
			GetComponent<TextMesh>().text = message + "\n" +GetComponent<TextMesh>().text;
		}

		public void clearStatus() {
			GetComponent<TextMesh>().text = "";
		}
	}
}
