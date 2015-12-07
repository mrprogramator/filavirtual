angular.module('LoginApp', [])
    .controller('LoginController', function ($http, $cacheFactory, $window, $location) {
        url = $location.$$absUrl.substr(0, $location.$$absUrl.indexOf("/operador"));
        self = this;
        console.log(url);
        getAgente = function (log) {
            var promise = $http({
                method: 'POST',
                url: url + '/filavirtual/Agente/GetAgenteByLogUsr/?log=' + log
            })
            .success(function (data, status, headers, config) {
                    return data;
            })
            .error(function (data, status, headers, config) {
                    return { "status": false };
            });

            return promise;
        }

        setLogin = function (item) {
            var promise = $http({
                method: 'POST',
                url: url + '/catalogo/Usuario/Login',
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
            setUser(item);
            setLogin(item).then(function (promise) {
                self.response = promise.data;

                if (self.response.result) {
                    getAgente(item.Id).then(function (promise) {
                        var agente = promise.data;
                        if (agente.Id == undefined){
                            self.response = {
                                result: false, 
                                value: 'No existe un agente relacionado con el usuario ingresado'
                            }
                            return;
                        }
                        localStorage.setItem('user',agente.Id);
                        localStorage.setItem('log',agente.LogUsr);
                        var landingUrl = '../operador';
                        $window.location.href = landingUrl;
                    });
                    
                }
            })
        }
    });