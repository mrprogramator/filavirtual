angular.module("OperadorApp")
	.controller("TransaccionController", function($http, $scope, $location) {
	    self = this;
	    url = $location.$$absUrl.substr(0, $location.$$absUrl.indexOf("/operador"));
		self.loading = true;

		function isLeafOf(leaf) {
		    return (leaf.GrupoId == this);
		}

		function fillTree(jsonResponse, tree, root) {
		    jsonResponse.filter(isLeafOf, root.id).forEach(function (elem) {
		        root.children.push({
		            id: elem.Id,
		            label: elem.Nombre,
		            children: []
		        });
		    });

		    if (root.children == 0) {
		        tree.push(root);
		        return root;
		    }

		    root.children.forEach(function (branch) {
		        branch = fillTree(jsonResponse, [], branch);
		    });
		    tree.push(root);
		}
		var my_tree = [];
		
		getTransacciones = function () {
		    var promise = $http({
		        method: 'POST',
		        url: url + '/catalogo/parametros'
		    })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

		    return promise;
		}

		self.handleClick = function () {
		    var self = this;
		    $timeout(function () {
		        self.selectedItem = self.items
                    .filter(function (item) {
                        return item.Id == self.selectedBranch.id;
                    })[0];
		        console.log('selected', self.selectedBranch);
		    }, 0);
		}

		self.init = function (servicioId) {
		    getTransacciones().then(function (promise) {
		        self.items = promise.data;
		        console.log('transacciones', self.items);
		        promise.data
                    .filter(function (param) {
                        return param.GrupoId == servicioId;
                    })
                    .forEach(function (padre) {
                        fillTree(
                                promise.data,
                                my_tree, {
                                    id: padre.Id,
                                    label: padre.Nombre,
                                    children: []
                                })
                    });

		        self.data = my_tree;
		        console.log('Cómo que no hay tree data? :', my_tree);
		        console.log('ITEMS', self.items);

		        self.loading = false;
		        console.log('LOADING......', self.loading);
		    });
		}
	})