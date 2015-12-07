angular.module('ExporterApp', [])
    .controller('ExporterController', function ($http, $timeout, $location) {
        self = this;
        url = $location.$$absUrl.substr(0, $location.$$absUrl.indexOf("/filavirtual"));

        var connection = $.hubConnection(url);
        var mainHub = connection.createHubProxy('mainHub');
        self.puntoId = "PTAB";

        self.all = true;

        connection.start().done(function () {
            console.log('connection started');
            mainHub.invoke("joinGroup", self.puntoId);
        })
        .fail(function (err) {
            console.log('cannot connect: ' + err);
        });

        mainHub.on('length', function (length) {
            setAtencionesLength(length);
        });

        mainHub.on('exportada', function (item) {
            self.exporting = true;
            self.message = "Exportando atenciones"
            increaseExportadas(item);
        });

        self.items = [];

        increaseExportadas = function (item) {
            $timeout(function () {
                if (self.exportadas >= self.atencionesLength) {
                    self.message = "Atenciones exportadas exitosamente"
                    return;
                }
                self.exportadas++;
                self.items.push(item);
                console.log('atención exportada', self.exportadas);
                if (self.exportadas >= self.atencionesLength) {
                    self.message = "Atenciones exportadas exitosamente"
                }
                
            },0)
        }

        
        exportarAtenciones = function (inicio, fin) {
            var promise = $http({
                method: 'POST',
                url: url + '/filavirtual/atenciones/exportar/' + inicio + '/' + fin
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        exportarAtencionesPorCantidad = function (inicio, fin, count) {
            var promise = $http({
                method: 'POST',
                url: url + '/filavirtual/atenciones/exportar/' + inicio + '/' + fin + '/' + count
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getAtenciones = function () {
            var promise = $http({
                method: 'POST',
                url: url + '/filavirtual/atenciones-pasadas'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        self.volver = function (tab) {
            tab.templateUrl = url + '/filavirtual/atencion/?ajax=1&_=' + Date.now;
        }

        function checkAtenciones() {
            getAtenciones().then(function (promise) {
                self.exportadas = promise.data;
                console.log('exportadas:', self.exportadas.length);
                self.timeCheck = $timeout(function () {
                    checkAtenciones();
                }, 3000);
            })
        }
        self.exportar = function () {
            var inicio = $('#dTPInicio').val();

            
            console.log(inicio);
            inicio = inicio.split("/")[2] + '-' + inicio.split("/")[1] + '-' + inicio.split("/")[0];

            var fin = $('#dTPFin').val();
            fin = fin.split("/")[2] + '-' + fin.split("/")[1] + '-' + fin.split("/")[0];

            self.info = { status: true, message: 'Exportando atenciones...' }

            self.loading = true;
            //checkAtenciones();

            if (self.cantidad == 0 || self.cantidad == undefined) {
                self.all = true;
            }

            if (self.all == true) {
                exportarAtenciones(inicio, fin).then(function (promise) {
                    self.loading = false;
                    //$timeout.cancel(self.timeCheck);
                    if (promise.data.result) {
                        self.info.message = promise.data.value;
                        console.log('atenciones exportadas con exito');
                    }
                    else {
                        self.info.message = promise.data.value;
                        console.log('error', promise.data.value);

                    }
                })
            }

            else {
                exportarAtencionesPorCantidad(inicio, fin, self.cantidad).then(function (promise) {
                    self.loading = false;
                    //$timeout.cancel(self.timeCheck);
                    if (promise.data.result) {
                        self.info.message = promise.data.value;
                        console.log('atenciones exportadas con exito');
                    }
                    else {
                        self.info.message = promise.data.value;
                        console.log('error', promise.data.value);

                    }
                })
            }
            
        }

        function convertToDate(text) {
            if (text == undefined) {
                return 'date is undefined';
            }
            var day = text.split('/')[0];
            var month = text.split('/')[1];
            var year = text.split('/')[2];

            return new Date(month + "/" + day + "/" + year);
        }

        self.getPorcentage = function() {
            if (self.exportadas == 0)
                return 0;
            return self.exportadas * 100 / self.atencionesLength;
        }
        self.exportadas = 0;
        self.atencionesLength = 100;

        testProgressBar = function () {
            self.exporting = true;
            if (self.exportadas >= self.atencionesLength) {
                self.message = "Atenciones exportadas exitosamente"
                return;
            }
            self.message = "Exportando atenciones"
            self.exportadas += 5;
            
            $timeout(function () {
                testProgressBar();
            },1000);
        }

        setAtencionesLength = function (length) {
            self.exporting = true;
            $timeout(function () {
                self.items = [];
                self.atencionesLength = length;
                self.exportadas = 0;
            }, 0);
        }
    });