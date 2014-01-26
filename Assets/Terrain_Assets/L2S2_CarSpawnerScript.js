#pragma strict

//Inspector Variables
var carCreationRate: float = 10;
var countdown : int;
var carObject: Transform;

function Start () {

	countdown = carCreationRate;

}

function Update () {

	timer -= Time.deltaTime;
	
	if(timer <= 0)
	{
		
		countdown = carCreationRate;
		v
		Rigidbody newCar = (Rigidbody) Instantiate(carObject, transform.position, transform.rotation);
		
		newCar.
		
		
	}	


}