angular.module('CatalogoApp')
    .controller('LoginController', function ($http, $window, $location) {
        self = this;
        setLogin = function (item) {
            var promise = $http({
                method: 'POST',
                url: 'Login',
                data: item
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
        }

        self.logUser = function (item) {
            setLogin(item).then(function (promise) {
                self.response = promise.data;
                localStorage.setItem('user', item.Id);
                if (self.response.result) {
                    $window.location.href = self.response.value;
                }
            })
        }
    });