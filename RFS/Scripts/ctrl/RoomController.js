﻿
//$('.headerlink').click(function (e) {
//    $('.headerlinka').css("color", "white");
//    $(this).css("background-color", "red")
    
//});

var myApp = angular.module('ResideoApp', ['ngMaterial', 'ngRoute']);
myApp.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "/Home/roombooking",
            controller:"RoomBookingController"
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
    $scope.stimes = ['06:00 AM', '06:30 AM', '07:00 AM', '07:30 AM', '08:00 AM', '08:30 AM', '09:00 AM', '09:30 AM','10:00 AM', '10:30 AM', '11:00 AM', '11:30 AM', '12:00 PM', '12:30 PM', '01:00 PM', '01:30 PM', '02:00 PM', '02:30 PM', '03:00 PM', '03:30 PM', '04:00 PM', '04:30 PM', '05:00 PM', '05:30 PM', '06:00 PM', '06:30 PM', '07:00 PM', '07:30 PM', '08:00 PM', '08:30 PM', '09:00 PM', '09:30 PM', '10:00 PM', '10:30 PM', '11:00 PM', '11:30 PM', '12:00 AM', '12:30 AM', '01:00 AM', '01:30 AM', '02:00 AM', '02:30 AM', '03:00 AM', '03:30 AM', '04:00 AM', '04:30 AM', '05:00 AM', '05:30 AM' ];
    $scope.etimes = ['06:00 AM', '06:30 AM', '07:00 AM', '07:30 AM', '08:00 AM', '08:30 AM', '09:00 AM', '09:30 AM', '10:00 AM', '10:30 AM', '11:00 AM', '11:30 AM', '12:00 PM', '12:30 PM', '01:00 PM', '01:30 PM', '02:00 PM', '02:30 PM', '03:00 PM', '03:30 PM', '04:00 PM', '04:30 PM', '05:00 PM', '05:30 PM', '06:00 PM', '06:30 PM', '07:00 PM', '07:30 PM', '08:00 PM', '08:30 PM', '09:00 PM', '09:30 PM', '10:00 PM', '10:30 PM', '11:00 PM', '11:30 PM', '12:00 AM', '12:30 AM', '01:00 AM', '01:30 AM', '02:00 AM', '02:30 AM', '03:00 AM', '03:30 AM', '04:00 AM', '04:30 AM', '05:00 AM', '05:30 AM'];
    $scope.rooms = [];
    $scope.availableRooms = [];
        $scope.selectedDate;
        $scope.projectedBooking = false;  
    
    $http.get(myApp.rmshost +'/api/Locations').
        then(function (response) {
            $scope.locations =response.data;
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
                    + $scope.selectedDate.getFullYear() ,
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
        $scope.FetchRoomList = function () {

            $scope.selectedstime = $scope.selectedDate.getMonth() + '/' + $scope.selectedDate.getDate() + '/' + $scope.selectedDate.getFullYear() + ' ' + $scope.startTime;
            $scope.selectedetime = $scope.selectedDate.getMonth() + '/' + $scope.selectedDate.getDate() + '/' + $scope.selectedDate.getFullYear() + ' ' + $scope.endTime;;
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
        $http.get(myApp.rmshost + '/api/location/' + loc.Id+'/rooms').
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

myApp.controller('mybookingsCtrl', ['$scope','$mdDialog', '$http', function ($scope, $mdDialog, $http) {

    $scope.bookings = [];
    $scope.selectedBooking = false;
    $scope.selectedBookingRoom = '';
    
    $scope.getUpcomingBookings = function () {
        $http.get(myApp.rmshost + '/api/me/bookings').
            then(function (response) {
                $scope.bookings = response.data;
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });
    };
    $scope.getUpcomingBookings();
    $scope.bookingSelected = function (booking) {
        $http.get(myApp.rmshost + '/api/Rooms/' + booking.Room.Id).
            then(function (response) {
                $scope.selectedBooking = booking;
                $scope.selectedBookingRoom = response.data;
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });

    };

    $scope.cancelBooking = function () {
        //show dialog for final confirmation
        //make screen busy
        $http.delete(myApp.rmshost + '/api/Booking/' + $scope.selectedBooking.Id).
            then(function (response) {
                $scope.getUpcomingBookings();
                $scope.selectedBooking = false;
                $scope.selectedBookingRoom = false;
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });
    };

    $scope.showConfirm = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        var confirm = $mdDialog.confirm()
            .title('Are you sure want to cancel this booking?')
            .textContent('Good Idea to cancel, if you are not going use it. It will help someone else to utilise this room and time.')
            .ariaLabel('Lucky day')
            .targetEvent(ev)
            .ok('Yes, cancel it')
            .cancel('No, don\'t cancel it');

        $mdDialog.show(confirm).then(function () {
            $scope.cancelBooking();
        }, function () {
           // $scope.status = 'You decided to keep your debt.';
        });
    };


}]);

myApp.controller('adminCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.selectedUser = null;
    $scope.users = [];
    $scope.getListOfUsers = function () {
        $http.get(myApp.rmshost + '/api/user').
            then(function (response) {
                $scope.users = response.data;
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });
    };

    $scope.userSelected = function (user) {

        $http.get(myApp.rmshost + '/api/user/' + user.Id).
            then(function (response) {
                $scope.selectedUser = response.data;
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });
    };

    $scope.getListOfUsers();

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

myApp.controller('NavigationController', ['$window', '$scope', '$http', function ($window, $parentscope, $http, $mdDialog) {

    var originatorEv;
    $parentscope.isAdmin = false;
    $parentscope.openMenu = function ($mdMenu, ev) {
        originatorEv = ev;
        $mdMenu.open(ev);
    };

    $http.get(myApp.rmshost + '/api/isadmin').
        then(function (response) {
            $parentscope.isAdmin = response.data;
        }, function (response) {
            if (response.status === 401) {
                $window.location.href = "/";
            }
        });

    $parentscope.notificationsEnabled = true;
    $parentscope.toggleNotifications = function () {
        $parentscope.notificationsEnabled = !$parentscope.notificationsEnabled;
    };

    $parentscope.redial = function () {
        $mdDialog.show(
            $mdDialog.alert()
                .targetEvent(originatorEv)
                .clickOutsideToClose(true)
                .parent('body')
                .title('Suddenly, a redial')
                .textContent('You just called a friend; who told you the most amazing story. Have a cookie!')
                .ok('That was easy')
        );

        originatorEv = null;
    };

    $parentscope.checkVoicemail = function () {
        // This never happens.
    };

}]);

myApp.controller('meController', ['$window', '$scope', '$http','$mdToast', function ($window, $scope, $http, $mdToast ) {

    $scope.last = {
        bottom: true,
        top: false,
        left: false,
        right: true
    };
    $scope.user = { Name: "" };
    $scope.locations = [];
    $scope.getMyProfile = function () {
        $http.get(myApp.rmshost + '/api/me').
            then(function (response) {
                $scope.user = response.data;
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });
    };
    $scope.toastPosition = angular.extend({}, $scope.last);
    $scope.getToastPosition = function () {

        return Object.keys($scope.toastPosition)
            .filter(function (pos) {
                return $scope.toastPosition[pos];
            }).join(' ');
    };

    $scope.updateProfile = function () {
       


        $http.put(myApp.rmshost + '/api/me/' + $scope.user.Id, $scope.user).
            then(function (response) {
                var pinTo = $scope.getToastPosition();

                $mdToast.show(
                    $mdToast.simple()
                        .textContent('Profile Updated.')
                        .position(pinTo)
                        .hideDelay(3000))
                    .then(function () {
                        $log.log('user profile updated.');
                    }).catch(function () {
                        $log.log('Toast failed or was forced to close early by another toast.');
                    });
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });

    };
    
    $scope.getMyProfile();
    $scope.getAllLocations = function () {
        $http.get(myApp.rmshost + '/api/Locations').
            then(function (response) {
                $scope.locations = response.data;
            });
    };
    $scope.getAllLocations();

}]);