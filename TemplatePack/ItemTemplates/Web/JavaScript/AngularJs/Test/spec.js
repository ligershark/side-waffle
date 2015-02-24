describe('$safeitemname$', function () {
    var $controller,
        $httpBackend,
        $rootScope;

    beforeEach(function () {
        module('app');
        inject(function (_$controller_, _$httpBackend_, _$rootScope_) {
            $controller = _$controller_;
            $httpBackend = _$httpBackend_;
            $rootScope = _$rootScope_;
        });
    });

    it('should exist', function () {
        //TODO: expectation goes here
    });
});
