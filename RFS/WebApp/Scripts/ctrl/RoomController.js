
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

myApp.controller('mybookingsCtrl', ['$scope', '$mdDialog', '$http', '$window', function ($scope, $mdDialog, $http, $window) {

    $scope.upcomingbookings = [];
    $scope.selectedBooking = false;
    $scope.selectedBookingRoom = '';

    $scope.getUpcomingBookings = function () {
        $http.get(myApp.rmshost + '/api/me/bookings/upcoming').
            then(function (response) {
                $scope.upcomingbookings = response.data;
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });
    };
    $scope.getPastBookings = function () {
        $http.get(myApp.rmshost + '/api/me/bookings/past').
            then(function (response) {
                $scope.pastbookings = response.data;
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });

    };
    $scope.pastbookings = [];
    $scope.getUpcomingBookings();
    $scope.getPastBookings();
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

myApp.controller('adminCtrl', ['$scope', '$http', '$mdToast', function ($scope, $http, $mdToast) {
    $scope.selectedUser = null;
    $scope.users = [];
    $scope.searchPattern = "";
    $scope.TotalUsers = [];
    $scope.searchUser = function () {
        $scope.users = [];
        for (let i = 0; i < $scope.TotalUsers.length; i++) {
            if ($scope.TotalUsers[i].Name.toLowerCase().includes($scope.searchPattern.toLowerCase())) {
                $scope.users.push($scope.TotalUsers[i]);
            }
        }
    };
    $scope.getListOfUsers = function () {
        $http.get(myApp.rmshost + '/api/user').
            then(function (response) {
                $scope.users = response.data;
                $scope.TotalUsers = response.data;
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
    $scope.changeUserActivation = function () {
        $http.put(myApp.rmshost + "/api/user/" + $scope.selectedUser.Id + "/activation/", $scope.selectedUser).then(function (response) {
            $mdToast.show(
                $mdToast.simple()
                    .textContent('Profile Updated.')
                    .position("bottom right")
                    .hideDelay(3000))
                .then(function () {
                    //$log.log('user profile updated.');
                }).catch(function () {
                    //$log.log('Toast failed or was forced to close early by another toast.');
                });
        }, function (response) { });

    };
    $scope.getListOfUsers();
    //================location===========================
    $scope.locations = [];
    $scope.selectedLocation = "";
    $scope.rooms = null;
    $scope.selectedRoom = null;
    $scope.getAllLocations = function () {
        $http.get(myApp.rmshost + '/api/Locations').
            then(function (response) {
                $scope.locations = response.data;
            });
    };
    $scope.selectedRoom = null;
    $scope.roomSelected = function (room) {
        $scope.selectedRoom = room;
    };
    $scope.locationSelected = function (loc) {
        $scope.selectedLocation = loc;
        $http.get(myApp.rmshost + '/api/Location/' + $scope.selectedLocation.Id+'/rooms').
            then(function (response) {
                $scope.rooms = response.data;
            });
    };
    $scope.getAllLocations();
    $scope.roompic = "Content/img/room.jpg";
    $http.get(myApp.rmshost + '/api/room/{roomid}/pic').then(function (response) {
        $scope.roompic = myApp.rmshost + '/api/room/{roomid}/pic';
    }, function (response) { });
    $scope.updateRoom = function () {

    };
    $scope.deleteRoom = function () {

    };

}]);

myApp.controller('AccountVerificationController', ['$window', '$scope', '$http', '$mdToast', function ($window, $scope, $http, $mdToast) {
    $scope.verificationSucces = false;
    $scope.user = {
        email: "",
        name: "",
        phone: "",
        password: "",
        checkpassword: "",
        code: ""
    };
    $scope.sleep = function (ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
    $http.post('/api/VerifyAccount', $scope.user).
        then(function (response) {
            if (response.status === 200) {
                $scope.verificationSucces = true;
                $scope.sleep(6000);
                $window.location.href = '/';
            }
        });
}]);


myApp.controller('IdentityController', ['$window', '$scope', '$http', '$mdToast', function ($window, $scope, $http, $mdToast) {
    $scope.signupStart = false;
    $scope.signupSucces = false;
    $scope.user = {
        email: "",
        name: "",
        phone: "",
        password: "",
        checkpassword: "",
        code: ""
    };
    $scope.passwordResetDone = false;
    $scope.sentResetLink = false;
    $scope.passwordResetStart = false;
    $scope.startSendingResetLink = false;
    $scope.resetPassword = function () {
        $scope.startSendingResetLink = true;
        $http.get('/api/SendPasswordResetLink?email=' + encodeURIComponent($scope.user.email)).
            then(function (response) {
                if (response.status === 200) {
                    //password reset link sent successfully
                    $scope.sentResetLink = true;
                }
            });


    };
    $scope.resetPasswordForUser = function () {
        $scope.passwordResetStart = true;
        $http.post('/api/ResetPassowrd', $scope.user).
            then(function (response) {
                if (response.status === 200) {
                    $scope.passwordResetDone = true;
                    $scope.sleep(6000);
                    $window.location.href = '/';
                }
            });

    };
    $scope.loginfailed = false;
    $scope.loginStarted = false;
    $scope.login = function () {
        let credentials = {
            username: $scope.user.email,
            password: $scope.user.password
        };
        $scope.loginStarted = true;
        $http.post('/api/Login', credentials).
            then(function (response) {
                if (response.status === 200) {
                    $scope.loginStarted = false;
                    $window.location.href = '/';

                } else {
                    $scope.loginStarted = false;
                    $scope.loginfailed = true;
                }
            }, function (response) {
                $scope.loginStarted = false;
                $scope.loginfailed = true;
            }
            );
    };
    $scope.sleep = function (ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
    $scope.signup = function () {
        // check if there is no error
        if (!($scope.user.password === $scope.user.checkpassword))
            return;
        $scope.signupStart = true;
        $http.post('/api/Signup', $scope.user).
            then(function (response) {
                $scope.signupStart = false;
                $scope.signupSucces = true;
                $scope.sleep(9000);
                $scope.login();

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
    $parentscope.dplocation = "Content/img/user.svg";
    $http.get(myApp.rmshost + '/api/me/dp').then(function (response) {
        $parentscope.dplocation = myApp.rmshost + '/api/me/dp';
    }, function (response) { });
    $http.get(myApp.rmshost + '/api/isadmin').
        then(function (response) {
            $parentscope.isAdmin = response.data;
        }, function (response) {
            if (response.status === 401) {
                $window.location.href = "/";
            }
        });
    $parentscope.ResetVerificationMail = false;
    $parentscope.ResetVerificationMailStarted = false;
    $parentscope.resendVerificationLink = function () {
        $parentscope.ResetVerificationMailStarted = true;
        $http.get(myApp.rmshost + '/api/SendVerificationMail').
            then(function (response) {
                $parentscope.ResetVerificationMail = true;
                $parentscope.ResetVerificationMailStarted = false;
            }, function (response) {
            });

    };
    $parentscope.isVerified = false;
    $http.get(myApp.rmshost + '/api/isVerified').
        then(function (response) {
            $parentscope.isVerified = response.data;
            if (!$parentscope.isVerified) {
                $(".overlay").toggle();
            }
        }, function (response) {
            $(".overlay").toggle(); // show/hide the overlay
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

myApp.controller('meController', ['$window', '$scope', '$http', '$mdToast', function ($window, $scope, $http, $mdToast) {

    $scope.dplocation = "Content/img/user.svg";
    $scope.last = {
        bottom: true,
        top: false,
        left: false,
        right: true
    };
    $scope.loc = "";
    $scope.user = { Name: "" };
    $scope.locations = [];
    $scope.uploadFile = function (files) {
        var fd = new FormData();
        //Take the first selected file
        fd.append("file", files[0]);

        $http.post(myApp.rmshost + "/api/me/dp", fd, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).
            then(function (response) {
                $(".userImage").removeAttr("src").attr("src", "/api/me/dp");
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });

    };
    $scope.isemailnotified = true;
    $scope.getMyProfile = function () {
        $http.get(myApp.rmshost + '/api/me').
            then(function (response) {
                $scope.user = response.data;
                //for (let i = 0; i < $scope.locations.length; i++) {
                //    if ($scope.locations[i].Name === $scope.user.loc.Name) {
                //        $scope.user.loc = $scope.locations[i];
                //    }
                //}
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });
    };

    $http.get(myApp.rmshost + '/api/me/dp').then(function (response) {
        $scope.dplocation = '/api/me/dp'
    }, function (response) { });

    $scope.updateProfile = function () {



        $http.put(myApp.rmshost + '/api/me/' + $scope.user.Id, $scope.user).
            then(function (response) {
                $mdToast.show(
                    $mdToast.simple()
                        .textContent('Profile Updated.')
                        .position("bottom right")
                        .hideDelay(3000))
                    .then(function () {
                        //$log.log('user profile updated.');
                    }).catch(function () {
                        //$log.log('Toast failed or was forced to close early by another toast.');
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

$(document).ready(function () {
    $('#body').show();
    $('#msg').hide();
});