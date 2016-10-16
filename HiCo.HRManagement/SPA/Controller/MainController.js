var MainController = function ($scope, Api) {
    $scope.year = new Date().getFullYear();
}

MainController.$inject = ['$scope', 'Api'];