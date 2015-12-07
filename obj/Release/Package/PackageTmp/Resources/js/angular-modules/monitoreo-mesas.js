angular.module('CatalogoApp')
    .controller('MonitoreoMesasController', function ($http, $location, $compile, $timeout) {
        self = this;

        self.loading = true;

        getMonitoreoView = function (puntoId) {
            var promise = $http({
                method: 'GET',
                url: 'filavirtual/puntos/' + puntoId + '/mesas/monitorear-mesas'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getFila = function (puntoId) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos/' + puntoId + '/fila'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getServicios = function () {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/parametros/tipo-atenciones'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getFilaPorServicio = function (puntoId, servicioId) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos/' + puntoId + '/servicios/' + servicioId + '/fila'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getCurrentTicket = function (mesaId) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/mesas/' + mesaId + '/ticket'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getMesas = function (puntoId) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos/' + puntoId + '/agentes'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getMesa = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/mesas/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getPunto = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getEstadoActualAgente = function (id)
        {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/mesas/' + id +'/estado-actual'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getPuntos = function () {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getUsuarios = function () {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/usuarios'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        self.init = function (tab) {
            console.log('TEMPURL',tab.templateUrl);
            var params = tab.templateUrl.split('/');
            self.loading = true;
            self.puntoId = params[params.indexOf('puntos') + 1];
            getPunto(self.puntoId).then(function (promise) {
                self.punto = promise.data;
            })
            console.log('puntoId', self.redirect);
            getMesas(self.puntoId).then(function (promise) {
                self.mesas = promise.data;
                refreshStates();
            })

            getServicios().then(function (promise) {
                self.servicios = promise.data;
                self.filas = [];
                self.servicios.forEach(function (servicio) {
                    getFilaPorServicio(self.punto.Id, servicio.Id).then(function (promise) {
                        console.log(servicio.Nombre, promise.data);
                        self.filas.push({
                            id: servicio.Id,
                            servicio: servicio.Nombre,
                            cantidad: promise.data
                        });
                    })
                })
            })

        }

        function refreshStates() {
            if (self.mesas.length == 0 || self.mesas == undefined) {
                return;
            }

            self.mesas.forEach(function (mesa) {
                getEstadoActualAgente(mesa.Id).then(function (promise) {
                    var estado = promise.data;
                    estado.FechaInicio = convertToDate(estado.FechaInicio);
                    estado.FechaFin = convertToDate(estado.FechaFin);
                    mesa.Estado = estado;
                })

                getCurrentTicket(mesa.Id).then(function (promise) {
                    mesa.Ticket = promise.data;
                })

                getFila(self.punto.Id).then(function (promise) {
                    self.fila = promise.data;
                    self.servicios.forEach(function (servicio) {
                        getFilaPorServicio(self.punto.Id, servicio.Id).then(function (promise) {
                            var item = self.filas.filter(function (item) {
                                return item.id == servicio.Id
                            })[0];

                            item.cantidad = promise.data;
                        })
                    })
                })
            });

            $timeout(function () {
                refreshStates();
            }, 1000);
        }
        getPuntos().then(function (promise) {
            self.puntos = promise.data;
            self.puntoId = self.puntos[0].Id;
        })

        function convertToDate(fecha)
        {
            fechaStr = fecha;
            fechaUnicode = fechaStr.substring(6, fechaStr.length - 2) * 1;
            return new Date(fechaUnicode);
        }

        self.monitorear = function (tab) {
            tab.templateUrl = 'filavirtual/puntos/' + self.puntoId + '/mesas/monitorear-mesas';
            /*getMonitoreoView(self.puntoId).then(function (promise) {
                var monitoreoDiv = $('#monitoreo-div');
                monitoreoDiv.html(promise.data);
                $compile(monitoreoDiv);
            })*/
        }
    });