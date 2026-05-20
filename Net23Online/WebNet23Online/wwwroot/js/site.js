var a1 = 1;     // BAD
let a2 = 1;     // GOOD
const a3 = 1;   // BEST

let age = 10;           // number
let name = "Ivan";      // string
let nameAlso = 'Ivan';  // string
let nameAlsoV3 = `Ivan`;// string
let isGood = true;      // bool
let array = [1];        // array
let user = {            // object
    name: "qwe",
    age: 50,
    sayHi: function () { console.log('hi') }
};
user.isGood = true;
user.sayHi();

function Do1() {
    console.log(1);
}

const Do2 = function () {
    console.log(1);
}

const Do3 = () => {
    console.log(1);
}

if (user.isGood) {
    console.log('good');
} else {
    console.log('bad');
}

user = null;
if (user) {
    console.log('exist');
} else {
    console.log("Doesn't exist");
}

let adress; // adress == undefined

let index = 1;
while(true){
    index++;
    if (index > 10){
        break;
    }
    console.log(`index: ${index}`);
    // console.log(`index: ` + index);
}

for (let i = 0; i < array.length; i++) {
    const element = array[i];
}


