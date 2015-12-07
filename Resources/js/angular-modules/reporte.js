angular.module('CatalogoApp')
    .controller('ReporteController', function ($scope, $http, $timeout) {
        $scope.loading = true;

        getAtencionesHora = function () {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/atenciones/group-by/hora'
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

        getPuntos().then(function (promise) {
            $scope.puntos = promise.data;
        });

        $scope.handleChange = function () {
            
        }

        $scope.generarReporte = function () {
            var fecini = convertToDate($('#dTPInicio').val());
            var fecfin = convertToDate($('#dTPFin').val());

            console.log('FECHA INICIO', fecini);
            console.log('FECHA FIN', fecfin);

            getAtencionesHora().then(function (promise) {
                $scope.data = promise.data;
                $scope.response = promise.data;
                if ($scope.response.result == false) {
                    $scope.loading = false;
                    return;
                }
                console.log($scope.data);
                //my = MG.convert.date(my, 'date');
                MG.data_graphic({
                    title: "Atenciones por hora",
                    description: "Cantidad de atenciones promedio agrupadas por hora",
                    data: promise.data,
                    area: true,
                    interpolate: 'basic',
                    x_accessor: 'Hora',
                    y_accessor: 'Atenciones',
                    width: 1000,
                    y_label: 'Cantidad de atenciones',
                    x_label: 'Horas',
                    max_x: 23,
                    yax_count: 50,
                    xax_count: 24,
                    height: 500,
                    target: '#test'
                });
                $scope.loading = false;
            })
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

        var my =
        [
            {
                "date": "2014-01-08",
                "value": 500
            },
            {
                "date": "2014-01-12",
                "value": 500
            },
            {
                "date": "2014-01-19",
                "value": 2000
            },
            {
                "date": "2014-01-20",
                "value": 2200
            },
            {
                "date": "2014-01-21",
                "value": 2300
            },
            {
                "date": "2014-04-23",
                "value": 500
            },
            {
                "date": "2014-04-24",
                "value": 1500
            },
            {
                "date": "2014-04-25",
                "value": 3000
            }
        ];
        x = [
            {
                "Hora": 0,
                "value": 0
            },
            {
                "Hora": 1,
                "value": 1
            },
            {
                "Hora": 2,
                "value": 2
            },
            {
                "Hora": 3,
                "value": 3
            },
            {
                "Hora": 4,
                "value": 4
            },
            {
                "Hora": 5,
                "value": 5
            },
            {
                "Hora": 6,
                "value": 6
            }
        ]
        
        $timeout(function () {


            console.log('YA');
        }, 2000)




    });