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
    $scope.selecteduserIndex = null;
    $scope.userSelected = function (index,user) {
        $scope.selecteduserIndex = index;
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
                $scope.selectedRoom = null;
                $scope.selectedLocation = "";
                $scope.rooms = null;
            });
    };
    $scope.selectedRoom = null;
    $scope.roomSelected = function (index,room) {
        $scope.selectedRoomIndex = index;
        $scope.selectedRoom = room;
    };
    $scope.addLocation = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        var confirm = $mdDialog.prompt()
            .title('Add new location')
            .textContent('{city}-{short-address} is a good way to name.')
            .placeholder('location Name')
            .ariaLabel('Location name')
            .initialValue('')
            .targetEvent(ev)
            .required(true)
            .ok('Okay!')
            .cancel('Cancel');

        $mdDialog.show(confirm).then(function (result) {
            $http.post(myApp.rmshost + '/api/locations/', { "Id": "", "Name": result, "Country": "India" }).then(function (response) {
                $scope.getAllLocations();
            });
        }, function () {
            $scope.status = 'You didn\'t name your dog.';
        });
    };
    $scope.addRoom = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        var confirm = $mdDialog.prompt()
            .title('Add new Room')
            .textContent('Any unique and pretty name will work.')
            .placeholder('room Name')
            .ariaLabel('room name')
            .initialValue('')
            .targetEvent(ev)
            .required(true)
            .ok('Okay!')
            .cancel('Cancel');

        $mdDialog.show(confirm).then(function (result) {
            $http.post(myApp.rmshost + '/api/rooms/', { "Id": "", "RoomName": result, "Projector": false, "Sitting": 0, "location": $scope.selectedLocation.Id, "VideoConferencing_": false, "MonitorScreen": false, "decommission": true, "RoomProfilePics": null }).then(function (response) {
                $scope.getAllLocations();
            });
        }, function () {
            $scope.status = 'You didn\'t name your dog.';
        });
    };
    $scope.selectedLocIndex = null;
    $scope.selectedRoomIndex = null;
    $scope.locationSelected = function (index, loc) {
        $scope.selectedLocIndex = index;
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





