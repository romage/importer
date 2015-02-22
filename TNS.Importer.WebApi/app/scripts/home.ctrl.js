angular.module("app").controller("homeController", ["$scope", '$http', '$timeout', '$compile', '$upload', function ($scope, $http, $timeout, $compile, $upload) {
    
    $scope.project = {};
  
    //$scope.$watch('files', function () {
    //    $scope.upload($scope.files);
    //});

    $scope.upload = function (files) {
        if (files && files.length) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];

                $upload.upload({
                    url: 'http://localhost:62636/Values/upload',
                    fields: $scope.project,
                    file: file
                }).progress(function (evt) {
                    $scope.progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                    console.log('progress: ' + $scope.progressPercentage.progressPercentage + '% ' +
                                evt.config.file.name);
                }).success(function (data, status, headers, config) {
                    console.log('file ' + config.file.name + 'uploaded. Response: ' +
                                JSON.stringify(data));
                });
            }
        }
    };
}]);