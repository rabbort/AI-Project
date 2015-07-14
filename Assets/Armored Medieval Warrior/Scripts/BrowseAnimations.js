#pragma strict

var changeAnimationKey : KeyCode = KeyCode.Space;
var currentAnimation : int = 0;
var forceLoop : boolean = false;

var playOrder : String[];

function Update () {
	var animationID : int = 0;
	if(Input.GetKeyDown(changeAnimationKey)){
		if(forceLoop){
			animation[playOrder[currentAnimation]].wrapMode = WrapMode.Loop;
		}
		animation.CrossFade(playOrder[currentAnimation]);
		currentAnimation ++;
		if(currentAnimation >= playOrder.Length){
			currentAnimation = 0;
		}
	}

}