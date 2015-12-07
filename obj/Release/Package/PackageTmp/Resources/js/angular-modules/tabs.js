deps = ['angularBootstrapNavTree'];
deps.push('ui.bootstrap');
deps.push('ngAnimate');
deps.push('ReporteApp');
var app = angular.module('CatalogoApp', deps);

app.controller('TabsController', ['$scope', '$location', function ($scope, $location) {
    $scope.tabs = [];
    console.log($location);

    this.newTab = function () {
        $scope.tabs.push({
            title: "Nueva Pesta\u00f1a",
            active: true
        });
    }

    this.addTab = function (title, templateUrl) {
        if (templateUrl === undefined) {
            return;
        }
        var exists = false;

        for (var i = 0; i < $scope.tabs.length; ++i) {
            if ($scope.tabs[i].title === title) {
                exists = true;
                console.log($scope.tabs[i]);
                $scope.tabs[i].templateUrl = templateUrl;
                $scope.tabs[i].active = true;
            }
        }


        if (!exists) {
            $scope.tabs.push({
                title: title,
                templateUrl: templateUrl,
                active: true,
                pila: []
            });
        }


    };

    this.removeTab = function (index) {
        $scope.tabs.splice(index, 1);
    };

    this.updateTab = function (el, url) {
        el.templateUrl = url;
    };
} ])
.directive('tabHighlight', [function () {

    return {
        restrict: 'A',
        link: function (scope, element) {
            // Here is the major jQuery usage where we add the event
            // listeners mousemove and mouseout on the tabs to initalize
            // the moving highlight for the inactive tabs
            var x, y, initial_background = '#c3d5e6';

            element
	      .removeAttr('style')
	      .mousemove(function (e) {
	          // Add highlight effect on inactive tabs
	          if (!element.hasClass('active')) {
	              x = e.pageX - this.offsetLeft;
	              y = e.pageY - this.offsetTop;

	              // Set the background when mouse moves over inactive tabs
	              element
	            .css({ background: '-moz-radial-gradient(circle at ' + x + 'px ' + y + 'px, rgba(255,255,255,0.4) 0px, rgba(255,255,255,0.0) 45px), ' + initial_background })
	            .css({ background: '-webkit-radial-gradient(circle at ' + x + 'px ' + y + 'px, rgba(255,255,255,0.4) 0px, rgba(255,255,255,0.0) 45px), ' + initial_background })
	            .css({ background: 'radial-gradient(circle at ' + x + 'px ' + y + 'px, rgba(255,255,255,0.4) 0px, rgba(255,255,255,0.0) 45px), ' + initial_background });
	          }
	      })
	      .mouseout(function () {
	          // Return the inital background color of the tab
	          element.removeAttr('style');
	      });
        }
    };

} ]);