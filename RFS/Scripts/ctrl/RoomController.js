
var myApp = angular.module('ResideoApp', ['ngMaterial']);

myApp.rmshost = "https://rms-restapi.azurewebsites.net:443";
myApp.directive('mdInputContainer', function ($timeout) {
    return function ($scope, element) {
        var ua = navigator.userAgent;
        if (ua.match(/chrome/i) && !ua.match(/edge/i)) {
            $timeout(function () {
                if (element[0].querySelector('input[type=password]:-webkit-autofill')) {
                    element.addClass('md-input-has-value');
                }
                 if (element[0].querySelector('input[type=email]:-webkit-autofill')) {
                    element.addClass('md-input-has-value');
                }
            }, 100);
        }
    };
});

myApp.controller('RoomBookingController', ['$scope','$http', function ($scope, $http) {
    $scope.selectedLocation = '';
    $scope.locations = [''];
    $scope.bookings = [''];
    $scope.stimes = ['10:00 AM', '10:30 AM'];
    $scope.etimes = ['12:00 PM', '12:30 PM'];
    $scope.rooms = [''];
    $http.get(myApp.rmshost +'/api/Locations').
        then(function (response) {
            $scope.locations =response.data;
        });
    $scope.showRooms = false;
    $scope.findRooms = function () {
        $scope.showRooms = true;

    };

    $isShowRooms = function () {
        if ($scope.rooms.length >= 1) {
            return true;
        }
        return false;
    };
    $isShowBookings = function () {
        if ($scope.bookings.length >= 1) {
            return true;
        }
        return false;
    };
    
    $locationSelected = function (loc) {
        $http.get(myApp.rmshost + '/api/Rooms?locationId='+loc.Id).
            then(function (response) {
                $scope.rooms = response.data;
            });
    };
    $roomSelected = function () {

    };
    $bookingSelected = function () {

    };

}]);



myApp.controller('IdentityController', ['$window', '$scope', '$http', function ($window,$scope, $http) {

   
    $scope.login = function () {
        let credentials = {
            username: $scope.username,
            password: $scope.password
        };
        $http.post('/api/Login', credentials ).
            then(function (response) {
                if (response.status === 200) {
                    $window.location.href = '/';

                }
            });        
    };

    $scope.signup = function () {
        let userdata = {
            username: $scope.username,
            password: $scope.password
        };
        $http.post('/api/Signup', userdata).
            then(function (response) {
                $scope.locations = response.data;
            });     
        

    };


}]);