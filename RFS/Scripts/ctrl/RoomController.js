
var myApp = angular.module('ResideoApp', ['ngMaterial']);

myApp.controller('RoomBookingController', ['$scope','$http', function ($scope, $http) {
    $scope.selectedLocation = '';
    $scope.locations = [''];
    $scope.stimes = ['10:00 AM', '10:30 AM'];
    $scope.etimes = ['12:00 PM', '12:30 PM'];
    $scope.rooms = ['Vidhan Soudha', 'Lal Bagh', 'Sakey Tank', 'Bangalore Palace'];
    $http.get('/api/location').
        then(function (response) {
            $scope.locations =response.data;
        });
    $scope.showRooms = false;
    $scope.findRooms = function () {
        $scope.showRooms = true;

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