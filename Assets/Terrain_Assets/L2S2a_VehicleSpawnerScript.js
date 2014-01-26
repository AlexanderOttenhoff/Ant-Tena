#pragma strict

//Inspector Variables
var vehicleCreationRate: float = 10;
var vehicleSpeed: float = 10;

var vehicleObject: Rigidbody;

//Private variables

private var countdown : int;


function Start () {

	countdown = vehicleCreationRate;

}

function Update () {

	countdown -= Time.deltaTime;
	
	if(countdown <= 0)
	{
		
		countdown = vehicleCreationRate;
		
		var newVehicle : Rigidbody = Instantiate(vehicleObject, transform.position, transform.rotation);
		
		newVehicle.velocity = transform.forward * vehicleSpeed;
		
		
	}	


}