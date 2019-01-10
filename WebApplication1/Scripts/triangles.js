var app = angular.module('myApp', []);
app.controller('myCtrl', function ($scope) {

    $.extend($scope, { rowText: "A", columnText: 1, lastQueryRow: "", lastQueryColumn: 0, x1: 0, y1: 0, x2: 0, y2: 0, x3: 0, y3: 0 });

    // Takes an object with parameters x1, y1, x2, y2, x3, y3 and draws a triangle using the three pairs of coordinates
    $scope.drawTriangle = function (data) {
        var canvas = document.getElementById('canvas-box');
        if (canvas.getContext) {

            var objctx = canvas.getContext('2d');
            objctx.beginPath();
            objctx.moveTo(data.x1, data.y1);
            objctx.lineTo(data.x2, data.y2);
            objctx.lineTo(data.x3, data.y3);
            objctx.closePath();
            objctx.fillStyle = "rgb(200,0,0)";
            objctx.fill();

        } else {
            alert("html5 browser needed");
        }
    };

    // Submits the data in the top section to the controller, displays the input and coordinates if it succeeds.  Also draws the triangle in the canvas.
    $scope.submitData = function () {
        $.ajax({
            url: "../api/trianglesapi/" + $scope.rowText + "/" + $scope.columnText,
            type: "POST",
            dataType: 'json',
            data: {},
            success: function (data) {
                $scope.lastQueryRow = $scope.rowText;
                $scope.lastQueryColumn = $scope.columnText;
                $scope.drawTriangle(data);
                $.extend($scope, data);
                $(".last-results").show();
                $scope.$apply();
            },
            error: function () {
                $(".last-results").hide();
            }
        });
    };

    // Submits the data in the bottom section to the API and displays the results if it succeeds.
    $scope.submitCoords = function () {
        $.ajax({
            url: "../api/trianglesapi/" + $scope.v1x + "/" + $scope.v1y + "/" + $scope.v2x + "/" + $scope.v2y + "/" + $scope.v3x + "/" + $scope.v3y + "/",
            type: "POST",
            dataType: 'json',
            data: {},
            success: function (data) {
                $scope.triangleResults = data;
                $(".results").show();
                $scope.$apply();
            },
            error: function () {
                $(".results").hide();
            }
        });
    };

    // Copies the results from the top section to the inputs in the bottom section (to make testing easier)
    $scope.copyCoords = function () {
        $scope.v1x = $scope.x1;
        $scope.v1y = $scope.y1;
        $scope.v2x = $scope.x2;
        $scope.v2y = $scope.y2;
        $scope.v3x = $scope.x3;
        $scope.v3y = $scope.y3;
        $scope.$apply();
    }
});

// Don't even allow a user to type something besides a-f into the row textbox:
app.directive('rowValidation', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {

            modelCtrl.$parsers.push(function (inputValue) {

                var transformedInput = inputValue.slice(-1).toUpperCase();
                if (transformedInput.charCodeAt(0) < "A".charCodeAt(0)
                    || transformedInput.charCodeAt(0) > "F".charCodeAt(0)) {
                    transformedInput = "";
                }

                if (transformedInput != inputValue) {
                    modelCtrl.$setViewValue(transformedInput);
                    modelCtrl.$render();
                }

                return transformedInput;
            });
        }
    };
});

// Set the canvas size
$(function () {
    var canvas = document.getElementById('canvas-box');
    canvas.width = 60;
    canvas.height = 60;
});