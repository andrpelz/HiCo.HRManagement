var EmployeeController = function ($scope, Api) {
    // ------------------------- Daten abrufen -------------------------
    $scope.data = {
        employee: {
            totalItems: 0,
            currentPage: 1,
            itemsPerPage: 5,
            data: []
        }
    };

    function GetData() {
        $scope.data.employee.data = [];

        var request = {
            PageNumber: $scope.data.employee.currentPage,
            EntriesPerPage: $scope.data.employee.itemsPerPage
        };

        Api.PostApiCall("Employee", "GetEmployeeList", request, function (res) {
            if (res.hasErrors == true) {
                $scope.setError("Error getting data: " + res.error);
            } else {
                $scope.data.employee.data = res.result.Employees;
                $scope.data.employee.totalItems = res.result.TotalItems;
            }
        });
    }

    GetData();

    $scope.pageChanged = function () {
        GetData();
    }

    // ------------------------- Daten löschen -------------------------
    $scope.deleteEmployee = function (employeeId, name) {
        if (confirm('Do you really want to remove the employee ' + name + '?')) {
            Api.PostApiCall("Employee", "DeleteEmployee", employeeId, function (res) {
                if (res.hasErrors == true) {
                    $scope.setError("Error sending employee data: " + res.error);
                } else {
                    if (res.result == 1)
                        GetData(); // Grid Page neu abrufen
                    else
                        $scope.setError("Error at removing the employee (" + res.result + ").");
                }
            });
        }
    }
}

EmployeeController.$inject = ['$scope', 'Api'];