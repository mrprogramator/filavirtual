angular.module('ReporteApp')
    .controller('AtencionesOperadorController', function ($scope, $http, $timeout) {

        function showReport(tab, typeName, parameters) {
            console.log('parameters', parameters);
            console.log(jQuery.param(parameters));

            //tab.templateUrl = "reporte/report/reportview/?typeName=" + typeName + "&parametros=" + jQuery.param(parameters);
        }

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

        function getPuntos() {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
        }

        getPuntos().then(function (promise) {
            $scope.puntos = promise.data;
            $scope.puntoId = $scope.puntos[0].Id;
        })

        $scope.makeReport = function (tab) {
            $timeout(function () {
                var inicio = $('#dTPInicio').val();

                inicio = inicio.split("/")[2] + '-' + inicio.split("/")[1] + '-' + inicio.split("/")[0];

                var fin = $('#dTPFin').val();
                fin = fin.split("/")[2] + '-' + fin.split("/")[1] + '-' + fin.split("/")[0];

                var punto = $scope.puntos.filter(function (p) {
                    return p.Id == $scope.puntoId
                })[0];
                console.log(inicio, fin, punto.Id);

                var typeName = "SistemaDeGestionDeFilas.Reports.AtencionesOperador,SistemaDeGestionDeFilas"
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
                            Name: "puntoId",
                            Value: punto.Id
                        },
                        {
                            Name: "puntoNombre",
                            Value: punto.Descripcion.toUpperCase()
                        }
                ]

                
                getReport(typeName, parameters).then(function (promise) {
                    console.log('PROMISE', promise);
                    $('#report-atop-div').html(promise.data);
                });
                console.log(typeName, parameters);
            }, 1000);
        }
    });