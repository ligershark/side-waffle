//Following object is created just to make the sample tests pass
//Delete the following statements once you start writing your tests
var obj = {
    number: null,
    setNumber: function (num) {
        this.number = num;
    },
    getSquare: function () {
        return this.number * this.number;
    }
};

module("Checking behaviour of object", {
    //Setting
    setup: function () {
        obj.setNumber(10);
    },

    //Resetting
    teardown: function () {
        obj.number = 5;
    }
});

test("Number should be having a value", function () {
    notEqual(obj.number, 0);
});

test("getSquare should return square of number", function () {
    equal(obj.getSquare(), 100);
});