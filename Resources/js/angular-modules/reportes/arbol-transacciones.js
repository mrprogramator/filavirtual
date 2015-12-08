angular.module('ReporteApp')
    .controller('ArbolTransaccionesController', function ($scope, $http, $timeout) {
        self = this;
        self.loadingTree = false;
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

        getArbolTransaccion = function (puntoId, inicio, fin) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos/' + puntoId + '/arbol-transacciones/' + inicio + '/' + fin
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
        }

        function convertToTree(transTree, tree) {
            tree = {
                id: transTree.Id,
                label: transTree.Nombre,
                cantidad: transTree.Cantidad,
                children: []
            }

            transTree.Hijos.forEach(function (branch) {
                var x = convertToTree(branch, []);
                tree.children.push(x);
            })

            return tree;
        }

        self.getArbol = function () {
            inicio = $('#dTPInicio').val();
            self.inicio = inicio;

            inicio = inicio.split("/")[2] + '-' + inicio.split("/")[1] + '-' + inicio.split("/")[0];

            fin = $('#dTPFin').val();
            self.fin = fin;

            fin = fin.split("/")[2] + '-' + fin.split("/")[1] + '-' + fin.split("/")[0];


            self.punto = self.puntos.filter(function (punto) {
                return punto.Id = self.puntoId;
            })[0];

            self.loadingTree = false;
            getArbolTransaccion(self.puntoId, inicio, fin).then(function (promise) {
                self.treeTrans = promise.data;
                console.log('data',promise.data);
                var tree = [];
                my_data = convertToTree(self.treeTrans, tree);
                //tree.push(my_data);
                my_data.children.push({ label: 'Total', cantidad: my_data.cantidad })
                tree.unshift({ label: 'PARAMÉTRICA', cantidad: 'CANTIDAD', children: my_data.children })
                self.data = tree;
                console.log('TREE', self.data);
                self.loadingTree = true;

                $timeout(function () {
                    $('.abn-tree-cantidad li').first().addClass('first');
                    $('.abn-tree-cantidad li').last().addClass('last');
                }, 1)

            })
        }

        getPuntos().then(function (promise) {
            self.puntos = promise.data;
            self.puntoId = self.puntos[0].Id;
        })

        
    });