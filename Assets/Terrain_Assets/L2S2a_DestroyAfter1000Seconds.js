#pragma strict

//Private vars

private var autodestructCountdown: int = 1000;

function Start () {

}

function Update () {

	autodestructCountdown -= Time.deltaTime;
	
	if(autodestructCountdown <= 0)
	{
		Destroy(this);
	}

}