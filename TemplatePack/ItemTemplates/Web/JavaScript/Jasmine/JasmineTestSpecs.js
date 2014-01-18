
//Test suite
describe('Tests functionality defined in the object obj', function () {

    //Setup
    beforeEach(function () {
        obj.setNumber(10);
        obj.getSquare = function () {
            return this.number * this.number;
        };
    });

    //Spec - 1
    it('Checks if the number is defined and value is not 5', function () {
        expect(obj.number).not.toBe(null);
        expect(obj.number).toBe(10);
    });

    //Spec - 2
    it('Should return square of the number upon calling getSquare', function () {
        expect(obj.getSquare()).toBe(100);
    });

    //Teardown
    afterEach(function () {
        obj.setNumber(null);
    });
});

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