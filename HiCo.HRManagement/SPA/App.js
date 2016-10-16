var app = angular.module('App', ['ngRoute', 'ui.bootstrap']);

app.service('Api', ['$rootScope', '$http', ApiService]);

app.controller('MainController', MainController);
app.controller('EmployeeController', EmployeeController);
app.controller('EditEmployeeController', EditEmployeeController);
app.controller('AddressController', AddressController);
app.controller('EditAddressController', EditAddressController);

app.config(['$routeProvider', function ($routeProvider, $routeParams) {
    $routeProvider
        .when('/employee', {
            title: 'Employee List',
            templateUrl: '/SPA/View/Employee.html',
            controller: EmployeeController
        })
        .when('/editemployee/:employeeId', {
            title: 'Employee Edit',
            templateUrl: '/SPA/View/EditEmployee.html',
            controller: EditEmployeeController
        })
        .when('/addresses/:employeeId', {
            title: 'Employee Address List',
            templateUrl: '/SPA/View/Addresses.html',
            controller: AddressController
        })
        .when('/editaddress/:addressId/:employeeId', {
            title: 'Address Edit',
            templateUrl: '/SPA/View/EditAddress.html',
            controller: EditAddressController
        })
        .otherwise({
            redirectTo: function () {
                return '/employee';
            }
        });
}]);


app.run(['$rootScope', function ($rootScope) {
    // Error anzeigen
    $rootScope.errorMessage = "";
    $rootScope.hasError = false;
    $rootScope.setError = function (errorMessage) {
        $rootScope.errorMessage = errorMessage;
        if (errorMessage != null && errorMessage.length > 0)
            $rootScope.hasError = true;
        else
            $rootScope.hasError = false;
    };

    // Page Title setzen wenn eine Route geladen wird
    $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
        $rootScope.title = current.$$route.title;
    });
}]);