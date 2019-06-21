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