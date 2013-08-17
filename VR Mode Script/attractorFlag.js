#pragma strict

enum FlagColor { Green, Red, Blue }

public var flagColor : FlagColor = FlagColor.Green;
public var speed : float = 0.0;

function Start() {
	var agents : NavMeshAgent[] = FindObjectsOfType(NavMeshAgent) as NavMeshAgent[];
	for(var agent : NavMeshAgent in agents) {

		if(flagColor == FlagColor.Red && agent.tag != "Red")
			continue;

		if(flagColor == FlagColor.Blue && agent.tag != "Blue")
			continue;

		agent.destination = transform.position;
		if(speed != 0) {
			agent.speed = speed;
		}
	}
}

function OnEnable() {
	Start();
}

function OnDrawGizmos() {
	switch(flagColor)
	{
		case FlagColor.Red:
			Gizmos.DrawIcon(transform.position+Vector3.up, "RedFlag32.png");
			break;
		case FlagColor.Blue:
			Gizmos.DrawIcon(transform.position+Vector3.up, "BlueFlag32.png");
			break;
		default:
			Gizmos.DrawIcon(transform.position+Vector3.up, "GreenFlag32.png");
			break;
	}
}

