var myApp = angular.module('ResideoApp', ['ngMaterial', 'ngRoute']);
myApp.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "/Home/roombooking",
            controller: "RoomBookingController"
        })
        .when("/mybookings", {
            templateUrl: "/Home/mybookings",
            controller: "mybookingsCtrl"
        })
        .when("/admin", {
            templateUrl: "/Home/admin",
            controller: "adminCtrl"
        }).when("/me", {
            templateUrl: "/Home/me",
            controller: "meController"
        })

});
myApp.config(function ($mdIconProvider) {
    $mdIconProvider
        .iconSet("call", '/Content/img/done.svg', 24)
        .iconSet("social", '/Content/img/done.svg', 24);
});
function compareTo() {

    return {
        require: "ngModel",
        scope: {
            compareTolValue: "=compareTo"
        },
        link: function (scope, element, attributes, ngModel) {

            ngModel.$validators.compareTo = function (modelValue) {

                return modelValue == scope.compareTolValue;
            };

            scope.$watch("compareTolValue", function () {
                ngModel.$validate();
            });
        }
    };
}

compareTo.$inject = [];
myApp.directive('compareTo', compareTo);
myApp.rmshost = window.location.protocol + "//" + window.location.hostname + ":" + window.location.port;
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
myApp.controller('RoomBookingController', ['$scope', '$http', function ($scope, $http) {
    $scope.selectedDate = new Date();
    $scope.selectedLoc = null;
    $scope.locations = [];

    $scope.startDayChange = function () {
        $scope.stimes = $scope.sameDay(today, $scope.selectedDate) ? $scope.getStartTimeArray(hour, minutes) : $scope.getStartTimeArray(0, 33);

    };
    $scope.bookings = [];
    $scope.stime = false;
    $scope.etime = null;

    $scope.sameDay = function (d1, d2) {
        return d1.getFullYear() === d2.getFullYear() &&
            d1.getMonth() === d2.getMonth() &&
            d1.getDate() === d2.getDate();
    };


    var today = new Date();

    var hour = today.getHours();
    var minutes = today.getMinutes();
    $scope.getStartTimeArray = function (hour, minutes) {
        let list = [];

        let startTime;
        while (hour < 24) {
            if (minutes < 30) {
                minutes = 30;
            } else {
                minutes = 0;
                hour += 1;
            }
            startTime = (hour < 13 ? hour : hour - 12) + ":" + (minutes === 0 ? "00" : minutes) + (hour < 13 ? " AM" : " PM");
            list.push(startTime);
        }

        return list;
    };
    $scope.startDayChange();

    $scope.startTime = null;
    $scope.startDateChange = function () {
        let a = $scope.startTime.split(':');
        let hour = parseInt(a[0]);
        let b = a[1].split(' ');
        let minutes = parseInt(b[0]);
        hour = b[1] === "AM" ? hour : hour + 12;
        $scope.etimes = $scope.getStartTimeArray(hour, minutes);
    };
    //$scope.stimes = ['06:00 AM', '06:30 AM', '07:00 AM', '07:30 AM', '08:00 AM', '08:30 AM', '09:00 AM', '09:30 AM','10:00 AM', '10:30 AM', '11:00 AM', '11:30 AM', '12:00 PM', '12:30 PM', '01:00 PM', '01:30 PM', '02:00 PM', '02:30 PM', '03:00 PM', '03:30 PM', '04:00 PM', '04:30 PM', '05:00 PM', '05:30 PM', '06:00 PM', '06:30 PM', '07:00 PM', '07:30 PM', '08:00 PM', '08:30 PM', '09:00 PM', '09:30 PM', '10:00 PM', '10:30 PM', '11:00 PM', '11:30 PM', '12:00 AM', '12:30 AM', '01:00 AM', '01:30 AM', '02:00 AM', '02:30 AM', '03:00 AM', '03:30 AM', '04:00 AM', '04:30 AM', '05:00 AM', '05:30 AM' ];
    //$scope.etimes = ['06:00 AM', '06:30 AM', '07:00 AM', '07:30 AM', '08:00 AM', '08:30 AM', '09:00 AM', '09:30 AM', '10:00 AM', '10:30 AM', '11:00 AM', '11:30 AM', '12:00 PM', '12:30 PM', '01:00 PM', '01:30 PM', '02:00 PM', '02:30 PM', '03:00 PM', '03:30 PM', '04:00 PM', '04:30 PM', '05:00 PM', '05:30 PM', '06:00 PM', '06:30 PM', '07:00 PM', '07:30 PM', '08:00 PM', '08:30 PM', '09:00 PM', '09:30 PM', '10:00 PM', '10:30 PM', '11:00 PM', '11:30 PM', '12:00 AM', '12:30 AM', '01:00 AM', '01:30 AM', '02:00 AM', '02:30 AM', '03:00 AM', '03:30 AM', '04:00 AM', '04:30 AM', '05:00 AM', '05:30 AM'];
    $scope.rooms = [];
    $scope.availableRooms = [];
    $scope.selectedDate = today;
    $scope.projectedBooking = false;

    $http.get(myApp.rmshost + '/api/Locations').
        then(function (response) {
            $scope.locations = response.data;
            if ($scope.locations.length > 0) {
                $scope.selectedLoc = $scope.locations[0];
            }
        });
    $scope.showRooms = false;
    $scope.createProjectedBooking = function (room) {

        var currentdate = new Date();
        $scope.projectedBooking =
            {
                "RoomImage": room.image,
                "RoomName": room.RoomName,
                "Location": { "Name": $scope.selectedLoc.Name, "Country": $scope.selectedLoc.Country },
                "MonitorScreen": room.MonitorScreen,
                "Projector": room.Projector,
                "Sitting": room.Sitting,
                "VideoConferencing": room.VideoConferencing,
                "Date": $scope.selectedDate.getDate() + "/"
                    + ($scope.selectedDate.getMonth() + 1) + "/"
                    + $scope.selectedDate.getFullYear(),
                "StartTime": $scope.startTime,
                "EndTime": $scope.endTime,
                "data": {
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
                },
            };



    };
    $scope.minDate = new Date();
    $scope.FetchRoomList = function () {

        $scope.selectedstime = ($scope.selectedDate.getMonth() + 1) + '/' + $scope.selectedDate.getDate() + '/' + $scope.selectedDate.getFullYear() + ' ' + $scope.startTime;
        $scope.selectedetime = ($scope.selectedDate.getMonth() + 1) + '/' + $scope.selectedDate.getDate() + '/' + $scope.selectedDate.getFullYear() + ' ' + $scope.endTime;;
        $http.get(myApp.rmshost + '/api/location/' + $scope.selectedLoc.Id + '/searchrooms/?SdateTime=' + $scope.selectedstime + '&EdateTime=' + $scope.selectedetime).
            then(function (response) {
                $scope.availableRooms = response.data;
                if (response.data.length > 0) {
                    $("#findbutton").html("Reset");
                    $scope.showRooms = true;
                    $scope.noroom = false;
                } else {
                    $scope.noroom = true;

                }

            });

    };
    $scope.findRooms = function () {
        // DISABLE ALL CONTROLLS

        if ($scope.showRooms) {
            $("#findbutton").html("Find");
            $scope.showRooms = false;
            $scope.projectedBooking = false;
            $scope.bookingDone = false;

        } else {

            $scope.FetchRoomList();

        }
    };
    $scope.noroom = false;
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
        $http.get(myApp.rmshost + '/api/location/' + loc.Id + '/rooms').
            then(function (response) {
                $scope.rooms = response.data;
            });
    };
    $scope.roomSelected = function () {

    };
    $scope.bookingSelected = function () {

    };

    $scope.bookingActivated = false;
    $scope.bookingDone = false;
    $scope.confirmBooking = function () {
        $scope.bookingActivated = true;
        $http.post(myApp.rmshost + '/api/booking', $scope.projectedBooking.data).
            then(function (response) {
                $scope.rooms = response.data;
                $scope.bookingActivated = false;
                $scope.bookingDone = true;

            });
    };

    $scope.backToRoomList = function () {
        $scope.bookingDone = false;
        $scope.projectedBooking = false;
        $scope.FetchRoomList();

    };

}]);
$(document).ready(function () {
    $('#body').show();
    $('#msg').hide();
});