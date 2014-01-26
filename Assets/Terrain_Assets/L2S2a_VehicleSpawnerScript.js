#pragma strict

//Inspector Variables
var vehicleMinCreationRate: float;
var vehicleMaxCreationRate: float;
var vehicleSpeed: float = 10;

var vehicleObject: Rigidbody;

var maximumUses: int = 1000;

//Private variables

private var countdown : float;



function Start () {

	countdown = Random.Range(vehicleMinCreationRate, vehicleMaxCreationRate);

}

function Update () {

	countdown -= Time.deltaTime;
	
	
	if(countdown <= 0)
	{
		
		countdown = Random.Range(vehicleMinCreationRate, vehicleMaxCreationRate);
		
		var newVehicle : Rigidbody = Instantiate(vehicleObject, transform.position, Quaternion.identity);
		
		newVehicle.velocity = transform.forward * vehicleSpeed;
		
		maximumUses -= 1;
		
		if(maximumUses<=0)
		{
			Destroy (this);
		}
	}
	
		


}