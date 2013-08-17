#pragma strict
@script RequireComponent(NavMeshAgent)

private var locoState_ : String = "Locomotion_Stand";
private var agent_ : NavMeshAgent;
private var anim_ : Animation;
private var linkStart_ : Vector3;
private var linkEnd_ : Vector3;
private var linkRot_ : Quaternion;

function Start() {
	agent_ = GetComponent.<NavMeshAgent>();
	agent_.autoTraverseOffMeshLink = false;
	AnimationSetup();

	while(Application.isPlaying) {
		yield StartCoroutine(locoState_);
	}
}

function Locomotion_Stand() {
	do {
		UpdateAnimationBlend();
		yield;
	} while(agent_.remainingDistance == 0);

	locoState_ = "Locomotion_Move";
	return;
}

function Locomotion_Move() {
	do {
		UpdateAnimationBlend();
		yield;

		if(agent_.isOnOffMeshLink) {
			locoState_ = SelectLinkAnimation();
			return;
		}
	} while(agent_.remainingDistance != 0);

	locoState_ = "Locomotion_Stand";
	return;
}

function Locomotion_JumpAnimation() {
	var linkAnimationName : String = "RunJump";

	agent_.Stop(true);
	anim_.CrossFade(linkAnimationName, 0.1, PlayMode.StopAll);
	transform.rotation = linkRot_;
	var posStartAnim : Vector3 = transform.position;
	do {
		var tlerp : float = anim_[linkAnimationName].normalizedTime;
		var newPos : Vector3 = Vector3.Lerp(posStartAnim, linkEnd_, tlerp);
		newPos.y += 0.4*Mathf.Sin(3.14159*tlerp);
		transform.position = newPos;

		yield;
	} while(anim_[linkAnimationName].normalizedTime < 1);

	anim_.Play("Idle");
	agent_.CompleteOffMeshLink();
	agent_.Resume();
	transform.position = linkEnd_;
	locoState_ = "Locomotion_Stand";
	return;
}

function Locomotion_LadderAnimation() {
/*	var linkCenter : Vector3 = 0.5*(linkEnd_ + linkStart_);
	var linkAnimationName : String;
	if(transform.position.y > linkCenter.y) {
		linkAnimationName = "Ladder Down";
	} else {
		linkAnimationName = "Ladder Up";
	}

 	agent_.Stop(true);

	var startRot : Quaternion = transform.rotation;
	var startPos : Vector3 = transform.position;
	var blendTime : float = 0.2;
	var tblend : float = 0;
	do {
		transform.position = Vector3.Lerp(startPos, linkStart_, tblend/blendTime);
		transform.rotation = Quaternion.Slerp(startRot, linkRot_, tblend/blendTime);
		yield;
		tblend += Time.deltaTime;
	} while(tblend < blendTime);
	transform.position = linkStart_;

	anim_.CrossFade(linkAnimationName, 0.1, PlayMode.StopAll);
	agent_.ActivateCurrentOffMeshLink(false);
	do {
		yield;
	} while(anim_[linkAnimationName].normalizedTime < 1);
	agent_.ActivateCurrentOffMeshLink(true);

	anim_.Play("Idle");
	transform.position = linkEnd_;
	agent_.CompleteOffMeshLink();
	agent_.Resume();

	locoState_ = "Locomotion_Stand";
	return;*/
}

private function SelectLinkAnimation() : String {
	var link : OffMeshLinkData;
	//agent_.GetCurrentOffMeshLinkData(link);
	link = agent_.currentOffMeshLinkData;
	var distS : float = (transform.position - link.startPos).magnitude;
	var distE : float = (transform.position - link.endPos).magnitude;
	if(distS < distE) {
		linkStart_ = link.startPos;
		linkEnd_ = link.endPos;
	} else {
		linkStart_ = link.endPos;
		linkEnd_ = link.startPos;
	}

	var alignDir : Vector3 = linkEnd_ - linkStart_;
	alignDir.y = 0;
	linkRot_ = Quaternion.LookRotation(alignDir);

	if(link.linkType == OffMeshLinkType.LinkTypeManual) {
		return "Locomotion_LadderAnimation";
	} else {
		return "Locomotion_JumpAnimation";
	}
}

private function AnimationSetup() {
	anim_  = GetComponent.<Animation>();

	// loop in sync
	anim_["Walk"].layer = 1;
	anim_["Run"].layer = 1;

	anim_.SyncLayer(1);

	// speed up & play once
	anim_["RunJump"].wrapMode = WrapMode.ClampForever;
	anim_["RunJump"].speed = 2;
	/*anim_["Ladder Up"].wrapMode = WrapMode.ClampForever;
	anim_["Ladder Up"].speed = 2;
	anim_["Ladder Down"].wrapMode = WrapMode.ClampForever;
	anim_["Ladder Down"].speed = 2;*/

	anim_.CrossFade("Idle", 0.1, PlayMode.StopAll);
}

private function UpdateAnimationBlend() {
	var walkAnimationSpeed : float = 0.5;
	var runAnimationSpeed : float = 1.0;
	var speedThreshold : float = 0.01;

	var velocityXZ : Vector3 = Vector3(agent_.velocity.x, 0.0f , agent_.velocity.z);
	var speed : float = velocityXZ.magnitude;
	anim_["Run"].speed = speed / runAnimationSpeed;
	anim_["Walk"].speed = speed / walkAnimationSpeed;

	if(speed > (walkAnimationSpeed+runAnimationSpeed)/2.0f) {
		anim_.CrossFade("Run");
	}
	else if(speed > speedThreshold) {
		anim_.CrossFade("Walk");
	} else {
		anim_.CrossFade("Idle", 0.3, PlayMode.StopAll);
	}
}
