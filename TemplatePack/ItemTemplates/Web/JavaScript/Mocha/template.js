// Template based on the Mocha testing Framework from http://visionmedia.github.io/mocha/
// Syntax based on the Should.js BDD style testing from https://github.com/visionmedia/should.js

var should = require('should');

// Synchronous Code
describe('Array', function () {
    describe('#indexOf()', function () {
        it('should return -1 when the value is not present', function () {
            [1, 2, 3].indexOf(5).should.equal(-1);
            [1, 2, 3].indexOf(0).should.equal(-1);
        })
    })
})

// Asynchronous Code
describe('User', function () {
    describe('#save()', function () {
        it('should save without error', function (done) {
            var user = new User('Luna');
            user.save(done);
        })
    })
})

// Run a specific Test Case

//describe('Array', function () {
//    describe('#indexOf()', function () {
//        it.only('should return -1 unless present', function () {

//        })

//        it('should return the index when present', function () {

//        })
//    })
//})

// Skip a Specific Test case

//describe('Array', function () {
//    describe('#indexOf()', function () {
//        it.skip('should return -1 unless present', function () {

//        })

//        it('should return the index when present', function () {

//        })
//    })
//})
