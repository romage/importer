angular.module("app", ['ngRoute', 'ngAria', 'angularFileUpload', 'ui.bootstrap']);

angular.module("app").config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "/app/views/home.html",
            controller: "homeController"
        })
});

