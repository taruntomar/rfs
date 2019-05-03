
var myApp = angular.module('ResideoApp', ['ngMaterial']);

myApp.rmshost = "http://localhost:64486";
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
    $scope.selectedLoc = null;
    $scope.locations = [];
    $scope.bookings = [];
    $scope.stime = '';
    $scope.etime='';
    $scope.stimes = ['10:00 AM', '10:30 AM'];
    $scope.etimes = ['12:00 PM', '12:30 PM'];
    $scope.rooms = [];
    $scope.availableRooms = [];
    $scope.selectedDate;
    
    $http.get(myApp.rmshost +'/api/Locations').
        then(function (response) {
            $scope.locations =response.data;
        });
    $scope.showRooms = false;
    $scope.findRooms = function () {
        
        $scope.selectedstime = $scope.selectedDate.getMonth() + '/' + $scope.selectedDate.getDate() + '/' + $scope.selectedDate.getFullYear() + ' ' + $scope.startTime;
        $scope.selectedetime = $scope.selectedDate.getMonth() + '/' + $scope.selectedDate.getDate() + '/' + $scope.selectedDate.getFullYear() + ' ' + $scope.endTime;;
        $http.get(myApp.rmshost + '/api/location/' + $scope.selectedLoc.Id+'/searchrooms/?SdateTime=' + $scope.selectedstime + '&EdateTime=' + $scope.selectedetime).
            then(function (response) {
                $scope.availableRooms = response.data;
            });

        $scope.showRooms = true;

    };

    $scope.isShowRooms = function () {
        if ($scope.rooms.length >= 1) {
            return true;
        }
        return false;
    };
    $scope.isShowBookings = function () {
        if ($scope.bookings.length >= 1) {
            return true;
        }
        return false;
    };
    
    $scope.locationSelected = function (loc) {
        $http.get(myApp.rmshost + '/api/location/' + loc.Id+'/rooms').
            then(function (response) {
                $scope.rooms = response.data;
            });
    };
    $scope.roomSelected = function () {

    };
    $scope.bookingSelected = function () {

    };


    $scope.bookRoom = function (room) {
        var currentdate = new Date();
        $data = {
            "Id": room.Id,
            "RoomId": room.Id,
            "starttime": $scope.selectedstime,
            "endtime": $scope.selectedetime,
            "createdOn": + currentdate.getDate() + "/"
                + (currentdate.getMonth() + 1) + "/"
                + currentdate.getFullYear() + " @ "
                + currentdate.getHours() + ":"
                + currentdate.getMinutes() + ":"
                + currentdate.getSeconds(),
            "createdBy": "username"
        };
        $http.post(myApp.rmshost + '/api/booking', $data).
            then(function (response) {
                $scope.rooms = response.data;
            });
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