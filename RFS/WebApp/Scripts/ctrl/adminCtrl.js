myApp.controller('adminCtrl', ['$scope', '$http', '$mdToast', '$mdDialog', function ($scope, $http, $mdToast, $mdDialog) {
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
        $scope.selectedRoom = null;
        $http.get(myApp.rmshost + '/api/Location/' + $scope.selectedLocation.Id+'/rooms').
            then(function (response) {
                $scope.rooms = response.data;
            });
    };
    $scope.getAllLocations();
    $scope.roompic = "Content/img/room-outline.png";
    $scope.uploadFile = function (files) {
        var fd = new FormData();
        //Take the first selected file
        fd.append("file", files[0]);

        $http.post(myApp.rmshost + "/api/rooms/" + $scope.selectedRoom.Id+"/dp", fd, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).
            then(function (response) {
                $(".roomImage").removeAttr("src").attr("src", "/api/rooms/" + $scope.selectedRoom.Id +"/dp");
            }, function (response) {
                if (response.status === 401) {
                    $window.location.href = "/";
                }
            });

    };
    $http.get(myApp.rmshost + '/api/rooms/{roomid}/pic').then(function (response) {
        $scope.roompic = myApp.rmshost + '/api/room/{roomid}/pic';
    }, function (response) { });
    $scope.updateRoom = function () {

        $http.put(myApp.rmshost + '/api/rooms/' + $scope.selectedRoom.Id, $scope.selectedRoom).
            then(function (response) {
                $mdToast.show(
                    $mdToast.simple()
                        .textContent('Room profile Updated.')
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

    $scope.commissionBackRoom = function (ev) {
        $scope.selectedRoom.decommission = false;
        $scope.updateRoom();
    };
    $scope.decommissionRoom = function (ev) {
        var confirm = $mdDialog.confirm()
            .title('Are you sure want to decommission this room?')
            .textContent('All upcoming bookings related to this room will get cancelled, so please mind consequences.')
            .ariaLabel('Decommission Room')
            .targetEvent(ev)
            .ok('Yes, decommission it')
            .cancel('No, don\'t decommission it');
        $mdDialog.show(confirm).then(function () {
            $http.delete(myApp.rmshost + '/api/rooms/' + $scope.selectedRoom.Id).
                then(function (response) {
                    //$scope.locationSelected($scope.selectedLocation);
                    $scope.selectedRoom.decommission = true;
                    $mdToast.show(
                        $mdToast.simple()
                            .textContent('Room dec Updated.')
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
        }, function () {
            // $scope.status = 'You decided to keep your debt.';
        });
       
    };


}]);





