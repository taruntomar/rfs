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