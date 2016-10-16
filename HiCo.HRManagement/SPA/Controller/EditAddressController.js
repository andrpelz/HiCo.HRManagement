var EditAddressController = function ($scope, $routeParams, $location, Api) {
    // ------------------------- Daten abrufen -------------------------
    $scope.address = null;

    function GetAddress(addressId, employeeId) {
        Api.PostApiCall("Address", "GetAddress", addressId, function (res) {
            if (res.hasErrors == true) {
                $scope.setError("Error getting address: " + res.error);
            } else {
                $scope.address = res.result;
                if (addressId == 0) // Employee Id at new address
                    $scope.address.EmployeeId = employeeId;
            }
        });
    }

    GetAddress($routeParams.addressId, $routeParams.employeeId);

    // ------------------------- Daten speichern -------------------------
    // save data on submit
    $scope.submitForm = function (isValid) {
        // check if the from is valid
        if (isValid) {
            Api.PostApiCall("Address", "SaveAddress", $scope.address, function (res) {
                if (res.hasErrors == true) {
                    $scope.setError("Error sending address data: " + res.error);
                } else {
                    if (res.result == 1)
                        $location.path('/addresses/' + $routeParams.employeeId); // back to the list view
                    else
                        $scope.setError("Error saving the address data (" + res.result + ").");
                }
            });
        }
    };
}

EditAddressController.$inject = ['$scope', '$routeParams', '$location', 'Api'];