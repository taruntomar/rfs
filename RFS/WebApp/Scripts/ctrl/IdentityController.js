
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

