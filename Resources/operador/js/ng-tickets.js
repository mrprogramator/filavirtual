angular.module("OperadorApp")
	.controller("DumpedTicketController", function($http, $location) {
	    url = $location.$$absUrl.substr(0, $location.$$absUrl.indexOf("/operador"));

		self = this;

		self.loading = true;

		getDumpedTickets = function () {
			var promise = $http({
				method: 'POST',
				url: url + '/filavirtual/tickets/abandonados'
			})
			.success(function (data, status, headers, config) {
	                    return data;
	        })
	        .error(function (data, status, headers, config) {
	                return { "status": false };
	        });

	        return promise;
		}

		 getDumpedTicketsRange = function (inicio, fin, nroTicket) {
            var promise = $http({
                method: 'POST',
                url: url + '/filavirtual/tickets/abandonados/' + inicio + '/' + fin + '/' + nroTicket
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        self.search = function () {
            inicio = $('#dTPInicio').val();
            inicio = inicio.replace(/\//g,"-");
            inicio = inicio.replace(/\:/g,"-");
            inicio = inicio.replace(/ /g,"+");
            
            
            fin = $('#dTPFin').val();
            fin = fin.replace(/\//g,"-");
            fin = fin.replace(/\:/g,"-");
            fin = fin.replace(/ /g,"+");
            
            if (fin == "") {
            	fin = inicio;
            }

            nroTicket = $('#nroTicket').val();

            self.loading = true;
            self.items = [];
            getDumpedTicketsRange(inicio, fin, nroTicket).then(function (promise) {
                self.items = promise.data;
                self.items.forEach( function (item) {
					fechaStr = item.FechaEmision;
					fechaUnicode = fechaStr.substring(6, fechaStr.length - 2) * 1;
					item.FechaEmision = new Date(fechaUnicode);
				})
                self.loading = false;
            })
        }

		

		$('#ticketModal').on('shown.bs.modal', function () {
		  	getDumpedTickets().then(function (promise) {
				self.items = promise.data;
				self.items.forEach( function (item) {
					fechaStr = item.FechaEmision;
					fechaUnicode = fechaStr.substring(6, fechaStr.length - 2) * 1;
					item.FechaEmision = new Date(fechaUnicode);
				})
				console.log('dumped',self.items);
				self.loading = false;
			});
		})

	})