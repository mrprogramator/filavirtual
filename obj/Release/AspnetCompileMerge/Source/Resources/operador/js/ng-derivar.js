angular.module("OperadorApp")
	.controller("DerivarController", function($http, $scope, $location) {
	    url = $location.$$absUrl.substr(0, $location.$$absUrl.indexOf("/operador"));
		$scope.loading = true;

		getTiposAtenciones = function () {
			var promise = $http({
				method: 'POST',
				url: url + '/catalogo/parametros/tipo-atenciones'
			})
			.success(function (data, status, headers, config) {
	                    return data;
	        })
	        .error(function (data, status, headers, config) {
	                return { "status": false };
	        });

	        return promise;
		}

		getTiposAtencionByMesa = function (id) {
	        var promise = $http({
	            method: "POST",
	            url: url + "/filavirtual/mesas/" + id + "/tipo-atenciones",
	        })
	        .success(function (data, stauts, headers, config) {
	            return data;
	        })
	        .error(function (data, status, headers, config) {
	            return {"status": false};
	        });

	        return promise;
	    }

		derivarTicket = function (puntoId, nroTicket, servicioId) {
			var promise = $http({
				method: 'POST',
				url: url + "/filavirtual/puntos/" + puntoId + "/ticket/" + nroTicket + "/derivar/servicio/" + servicioId
			})
			.success(function (data, status, headers, config) {
	                    return data;
	        })
	        .error(function (data, status, headers, config) {
	                return { "status": false };
	        });

	        return promise;	
		}

		$scope.x = {Id: 'EMPTY', Nombre: 'EMPTY'}

		$scope.realizarDerivacion = function (puntoId, nroTicket, servicioSelectedId, callback) {
			callback();
			derivarTicket(puntoId, 
				nroTicket, 
				servicioSelectedId)
			.then( function (promise) {
				$scope.response = promise.data;
				$('#derivarModal').modal('toggle');
			})
		}

		$scope.init = function (nroAgente) {
			console.log("DERIVAR NROAGE:", nroAgente);
			getTiposAtenciones().then(function (promise) {
				$scope.items = promise.data;
				getTiposAtencionByMesa(nroAgente).then(function (promise2) {
                    $scope.servicios = promise2.data;
					console.log('servicios',$scope.servicios);

                    $scope.servicios.forEach(function (servicio) {
                        var transToRemove = $scope.items.filter( function (transaccion) {
                            return transaccion.Id == servicio.TipoId;
                        })[0];

                        removeItemFromArray(transToRemove, $scope.items);
                    })
					console.log('tipos de atencion',$scope.items);
                })
				$scope.loading = false;
			});	
		}
		

	    function removeItemFromArray(item, array) {
	        var index = array.indexOf(item);
	        console.log('index', index);
	        if (index >= 0) {
	            array.splice(index, 1);
	        }
	    }
	})