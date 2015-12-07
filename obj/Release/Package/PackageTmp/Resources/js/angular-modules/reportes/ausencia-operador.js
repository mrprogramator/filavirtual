angular.module('ReporteApp')
    .controller('AusenciaOperadorController', function ($scope, $http, $timeout) {
        function getReport(typeName, parameters) {
            var promise = $http({
                method: 'POST',
                url: 'reporte/report/reportview',
                data: {
                    TypeName: typeName,
                    Parameters : parameters
                }
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
        }

        function getOperadores() {
            var promise = $http({
                method: 'POST',
                url: 'operadores'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
        }

        getOperadores().then(function (promise) {
            $scope.operadores = promise.data;
            $scope.operadorId = $scope.operadores[0].Id;
        })

        $scope.makeReport = function (tab) {
            $timeout(function () {
                var inicio = $('#dTPInicio').val();

                inicio = inicio.split("/")[2] + '-' + inicio.split("/")[1] + '-' + inicio.split("/")[0];

                var fin = $('#dTPFin').val();
                fin = fin.split("/")[2] + '-' + fin.split("/")[1] + '-' + fin.split("/")[0];

                var operador = $scope.operadores.filter(function (o) {
                    return o.Id == $scope.operadorId
                })[0];


                console.log(inicio, fin, operador);

                var typeName = "SistemaDeGestionDeFilas.Reports.AusenciaOperador,SistemaDeGestionDeFilas"
                var parameters = [
                        {
                            Name: "fechaInicio",
                            Value: new Date(inicio)
                        },
                        {
                            Name: "fechaFin",
                            Value: new Date(fin)
                        },
                        {
                            Name: "operadorId",
                            Value: operador.Id
                        },
                        {
                            Name: "operadorNombre",
                            Value: operador.Nombre
                        }
                ]
                
                getReport(typeName, parameters).then(function (promise) {
                    console.log('PROMISE', promise);
                    $('#report-taop-div').html(promise.data);
                });
                console.log(typeName, parameters);
            }, 1000);
        }
    });