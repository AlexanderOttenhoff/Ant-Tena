#pragma strict

//Inspector Variables
var carCreationRate: float = 10;
var countdown : int;


function Start () {

	countdown = carCreationRate;

}

function Update () {

	timer -= Time.deltaTime;
	
	if(timer <= 0)
	{
		
		countdown = carCreationRate;
		Instantiate(brick, Vector3 (x, y, 0), Quaternion.identity);
		
	}	


}