var EditEmployeeController = function ($scope, $routeParams, $location, Api) {
    // ------------------------- Daten abrufen -------------------------
    $scope.employee = null;

    function GetEmployee(employeeId) {
        Api.PostApiCall("Employee", "GetEmployee", employeeId, function (res) {
            if (res.hasErrors == true) {
                $scope.setError("Error getting employee: " + res.error);
            } else {
                $scope.employee = res.result;
            }
        });
    }

    GetEmployee($routeParams.employeeId);

    // ------------------------- Daten speichern -------------------------
    // save data on submit
    $scope.submitForm = function (isValid) {
        // check if the from is valid
        if (isValid) {
            Api.PostApiCall("Employee", "SaveEmployee", $scope.employee, function (res) {
                if (res.hasErrors == true) {
                    $scope.setError("Error sending employee data: " + res.error);
                } else {
                    if (res.result == 1)
                        $location.path('/employee'); // back to the list view
                    else
                        $scope.setError("Error at saving the employee data (" + res.result + ").");
                }
            });
        }
    };
}

EditEmployeeController.$inject = ['$scope', '$routeParams', '$location', 'Api'];