
function Update () {
var model1 = GameObject.Find("armor");
var model2 = GameObject.Find("vest");
var model3 = GameObject.Find("Sword");
var model4 = GameObject.Find("shirt");
var model5 = GameObject.Find("Armlet");
var model6 = GameObject.Find("Shield");


if (Input.GetKeyDown ("d")) {

model1.renderer.enabled=true;

}


if (Input.GetKeyDown ("f")) {
model1.renderer.enabled=false;
}

if (Input.GetKeyDown ("q")) {

model2.renderer.enabled=true;

}


if (Input.GetKeyDown ("w")) {
model2.renderer.enabled=false;
}
if (Input.GetKeyDown ("a")) {

model3.renderer.enabled=true;

}


if (Input.GetKeyDown ("s")) {
model3.renderer.enabled=false;
}
if (Input.GetKeyDown ("e")) {

model4.renderer.enabled=true;

}


if (Input.GetKeyDown ("r")) {
model4.renderer.enabled=false;
}
if (Input.GetKeyDown ("g")) {

model5.renderer.enabled=true;

}


if (Input.GetKeyDown ("h")) {
model5.renderer.enabled=false;
}
if (Input.GetKeyDown ("z")) {

model6.renderer.enabled=true;

}


if (Input.GetKeyDown ("x")) {
model6.renderer.enabled=false;
}

}
