#pragma strict

var count : int = 20;
var radius : float = 10.0;
var angleOffset : float = 0.0;
var prefab : Transform;

function Start () {
	for (var i=0;i<count;i++) {
		var angle : float = i;
		angle /= count;
		var rot = Quaternion.Euler(0, angle * 360.0 + angleOffset, 0);
		var pos = rot * Vector3.forward * -radius;
		var instance = Instantiate(prefab, pos + transform.position, rot);

		var agent : NavMeshAgent = instance.GetComponent(NavMeshAgent);
		if (agent) {
			agent.SetDestination(transform.position - pos);
			agent.speed = 2 + 0.25*(Random.value-0.5);
		}
	}
}
