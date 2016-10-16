var ApiService = function ($rootScope, $http) {
    var result;
    this.GetApiCall = function (controllerName, method, callback) {
        $rootScope.setError(null); // Fehler zurücksetzen
        result = $http.get('api/' + controllerName + '/' + method)
            .success(function (data, status) {
                var res = {
                    result: data,
                    hasErrors: false
                };
                callback(res);
            })
            .error(function (data, status) {
                var res = {
                    result: "",
                    hasErrors: true,
                    error: data.ExceptionMessage
                };
                callback(res);
            });
    }

    this.PostApiCall = function (controllerName, methodName, params, callback) {
        $rootScope.setError(null); // Fehler zurücksetzen
        result = $http.post('api/' + controllerName + '/' + methodName, params)
            .success(function (data, status) {
                var res = {
                    result: data,
                    hasErrors: false
                };
                callback(res);
            })
            .error(function (data, status) {
                var res = {
                    result: "",
                    hasErrors: true,
                    error: data.ExceptionMessage
                };
                callback(res);
            });
        return result;
    };

}