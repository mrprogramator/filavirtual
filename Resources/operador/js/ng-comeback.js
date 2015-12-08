angular.module('OperadorApp')
    .controller('ComebackController', function ($http, $cacheFactory, $window, $location, $timeout) {
        url = $location.$$absUrl.substr(0, $location.$$absUrl.indexOf("/operador"));
        self = this;
        self.loading = true;
        getAgente = function (log) {
            var promise = $http({
                method: 'POST',
                url: url + '/Agente/GetAgenteByLogUsr/?log=' + log
            })
            .success(function (data, status, headers, config) {
                    return data;
            })
            .error(function (data, status, headers, config) {
                    return { "status": false };
            });

            return promise;
        }

        getAgenteById = function (nroAge) {
            var promise = $http({
                method: 'POST',
                url: url + '/Agente/GetAgente/?id=' + nroAge
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
                url: url + '/Usuario/Login',
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
        self.loadUser = function (user) {
            self.loading = true;
            var user = {Id:'empty'}
            var id = localStorage.getItem('user');
            getAgenteById(id).then(function (promise) {
                console.log('promise',promise.data);
                user.Id = promise.data.LogUsr;
                self.loading = false;
                console.log('loading finished!');
            });
        }
        $('#comebackModasdl').on('shown.bs.modal', function () {
            self.user = {Id:'empty'}
            var id = localStorage.getItem('user');
            getAgenteById(id).then(function (promise) {
                console.log('promise',promise.data);
                self.user.Id = promise.data.LogUsr;
                self.loading = false;
                console.log('loading finished!');
            });
        })
        self.logUser = function (item, callback) {
            console.log('item', item);
            setLogin(item).then(function (promise) {
                $timeout(function () {
                    self.response = promise.data;
                    console.log('response', self.response);
                    if (!self.response.result) {
                        $('#wrongPasswordModal').modal('show');
                    }
                    if (self.response.result) {
                        console.log('hola');
                        getAgente(item.Id).then(function (promise) {
                            var agente = promise.data;
                            console.log('AGENTE::::::', agente);
                            if (agente.Id == undefined) {
                                self.response = {
                                    result: false,
                                    value: 'No existe un agente relacionado con el usuario ingresado'
                                }
                                return;
                            }
                            var currentUser = localStorage.getItem('user');
                            console.log("currentUser:::", currentUser);
                            if (currentUser == agente.Id) {

                                console.log('hola');
                                $('#comebackModal').modal('toggle');
                                item.Password = "";
                                callback();
                            }
                            else {
                                self.response = {
                                    result: false,
                                    value: 'El usuario y/o la contraseña ingresados no coinciden con el usuario actual'
                                }
                                return;

                            }
                        });

                    }
                }, 0);
                
            })
        }

    });