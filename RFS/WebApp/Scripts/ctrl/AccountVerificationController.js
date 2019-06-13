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
