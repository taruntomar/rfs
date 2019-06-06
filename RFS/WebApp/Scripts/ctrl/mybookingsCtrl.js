
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
