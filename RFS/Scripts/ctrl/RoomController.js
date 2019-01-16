
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



myApp.controller('IdentityController', ['$scope', '$http', function ($scope, $http) {
    $scope.username = "";
    $scope.password = "";

}]);