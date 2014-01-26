#pragma strict

var audioToPlay : AudioSource;

function Start () {

	

}

function Update () {

	

}

function OnTriggerEnter(other : Collider){
		if(other.gameObject.tag=="Player")
		returnToL2S1();
		}
		 
		function returnToL2S1()
		{
			audioToPlay.Play();
					
			yield WaitForSeconds (4);
			
			Application.LoadLevel("L2S1");
			
			
			
		}