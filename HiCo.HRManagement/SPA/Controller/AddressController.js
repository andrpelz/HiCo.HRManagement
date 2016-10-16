var AddressController = function ($scope, $routeParams, Api) {
    // ------------------------- Daten abrufen -------------------------
    $scope.addresses = [];
    $scope.employeeId = $routeParams.employeeId;

    function GetAddresses() {
        $scope.addresses = [];

        Api.PostApiCall("Address", "GetEmployeeAddressList", $scope.employeeId, function (res) {
            if (res.hasErrors == true) {
                $scope.setError("Error getting data: " + res.error);
            } else {
                $scope.addresses = res.result;
            }
        });
    }

    GetAddresses();

    // ------------------------- Daten löschen -------------------------
    $scope.deleteAddress = function (addressId, adr) {
        if (confirm('Do you really want to remove the addres ' + adr + '?')) {
            Api.PostApiCall("Address", "DeleteAddress", addressId, function (res) {
                if (res.hasErrors == true) {
                    $scope.setError("Error sending address data: " + res.error);
                } else {
                    if (res.result == 1)
                        GetAddresses(); // Grid Page neu abrufen
                    else
                        $scope.setError("Error removing the address (" + res.result + ").");
                }
            });
        }
    }
}

AddressController.$inject = ['$scope', '$routeParams', 'Api'];